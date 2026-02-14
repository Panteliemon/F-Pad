using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad.Interaction;

public static class Interactor
{
    private const string SHMEM_NAME = "FPAD_interaction_shared_memory";
    private const long SHMEM_SIZE = 1 << 20;

    private static MemoryMappedFile sharedMemory;
    private static Mutex sharedMemoryMutex;

    private static string acceptMessageEventName;
    private static EventWaitHandle acceptMessageEvent;
    private static Thread acceptThread;
    private static CancellationTokenSource acceptThreadCts;

    public static bool IsInitialized { get; private set; }
    public static int Pid { get; private set; }

    /// <summary>
    /// Called on message thread when we receive "Activate" message
    /// </summary>
    public static Action Activate { get; set; }
    /// <summary>
    /// Called on message thread when we receive "Activate and set caret" message
    /// </summary>
    public static Action<int, int> ActivateSetCaret { get; set; }

    public static void Startup()
    {
        if (IsInitialized)
            throw new InvalidOperationException();

        Pid = Process.GetCurrentProcess().Id;

        sharedMemoryMutex = new Mutex(false, "FPAD_interaction_mutex");

        acceptMessageEventName = $"FPAD_{Pid}_accept_message_event";
        acceptMessageEvent = new EventWaitHandle(false, EventResetMode.AutoReset, acceptMessageEventName);
        acceptThreadCts = new();
        acceptThread = new Thread(() => AcceptMessageThreadProc(acceptThreadCts.Token));
        acceptThread.Start();

        // Register self
        CriticalSection(() =>
        {
            MemStruct ms = UnsafeReadSharedMemory();
            FindOrAddSelf(ms);
            UnsafeWriteSharedMemory(ms);
        });

        IsInitialized = true;
    }

    public static void Shutdown()
    {
        if (!IsInitialized)
            throw new InvalidOperationException();

        // Remove ourselves
        CriticalSection(() =>
        {
            MemStruct ms = UnsafeReadSharedMemory();
            ms.Instances = ms.Instances.Where(x => x.Pid != Pid).ToList();
            ms.Messages = ms.Messages.Where(x => x.TargetPid != Pid).ToList();
            UnsafeWriteSharedMemory(ms);
        });

        acceptThreadCts.Cancel();
        acceptMessageEvent.Set();
        acceptThread.Join();

        sharedMemory?.Dispose();
        sharedMemory = null;

        IsInitialized = false;
    }

    public static void UpdateCurrentDocumentFullPath(string documentFullPath)
    {
        if (!IsInitialized)
            throw new InvalidOperationException();

        CriticalSection(() =>
        {
            MemStruct ms = UnsafeReadSharedMemory();
            FPadRec myRec = FindOrAddSelf(ms);
            myRec.CurrentDocumentFullPath = documentFullPath;
            UnsafeWriteSharedMemory(ms);
        });
    }

    /// <summary>
    /// </summary>
    /// <returns>True: found and activated. False: didn't find.</returns>
    public static bool FindAndActivateByCurrentDocumentPath(string documentFullPath, int? lineIndex, int? charIndex)
    {
        if (!IsInitialized)
            throw new InvalidOperationException();

        bool result = false;
        CriticalSection(() =>
        {
            MemStruct ms = UnsafeReadSharedMemory();
            FPadRec targetRec = ms.Instances.FirstOrDefault(x => string.Equals(x.CurrentDocumentFullPath, documentFullPath, StringComparison.InvariantCultureIgnoreCase));
            if (targetRec != null)
            {
                result = true;

                if (ms.Add2ParamMessage(targetRec.Pid, MessageType.ActivateSetCaret, lineIndex ?? 0, charIndex ?? 0))
                {
                    UnsafeWriteSharedMemory(ms);
                    SetMessageReadyEvent(targetRec);
                }
            }
        });

        return result;
    }

    #region Private

    private static void AcceptMessageThreadProc(CancellationToken ct)
    {
        while (true)
        {
            acceptMessageEvent.WaitOne();
            if (ct.IsCancellationRequested)
                return;

            List<MessageRec> messagesForMe = new List<MessageRec>();
            CriticalSection(() =>
            {
                MemStruct ms = UnsafeReadSharedMemory();
                for (int i = ms.Messages.Count - 1; i >= 0; i--)
                {
                    MessageRec msg = ms.Messages[i];
                    if (msg.TargetPid == Pid)
                    {
                        ms.Messages.RemoveAt(i);
                        messagesForMe.Add(msg);
                    }
                }
                UnsafeWriteSharedMemory(ms);
            });

            foreach (MessageRec msg in messagesForMe)
            {
                try
                {
                    ProcessMessage(msg);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    private static void ProcessMessage(MessageRec msg)
    {
        if (msg.MessageType == MessageType.Activate)
        {
            Activate?.Invoke();
        }
        else if (msg.MessageType == MessageType.ActivateSetCaret)
        {
            ActivateSetCaret?.Invoke(msg.Param1, msg.Param2);
        }
    }

    private static void CriticalSection(Action criticalAction)
    {
        sharedMemoryMutex.WaitOne();
        try
        {
            criticalAction.Invoke();
        }
        catch (Exception ex)
        {
            sharedMemoryMutex.ReleaseMutex();
            throw;
        }

        sharedMemoryMutex.ReleaseMutex();
    }

    private static void SetMessageReadyEvent(FPadRec targetRec)
    {
        if (EventWaitHandle.TryOpenExisting(targetRec.AcceptMessageEventName, out EventWaitHandle handle))
        {
            handle.Set();
        }
    }

    private static FPadRec FindOrAddSelf(MemStruct memStruct)
    {
        memStruct.Instances ??= new List<FPadRec>();
        FPadRec existing = memStruct.Instances.FirstOrDefault(x => x.Pid == Pid);
        if (existing == null)
        {
            FPadRec result = new()
            {
                Pid = Pid,
                AcceptMessageEventName = acceptMessageEventName,
                CurrentDocumentFullPath = string.Empty
            };
            memStruct.Instances.Add(result);
            return result;
        }
        else
        {
            return existing;
        }
    }

    private static MemStruct UnsafeReadSharedMemory()
    {
        sharedMemory ??= MemoryMappedFile.CreateOrOpen(SHMEM_NAME, SHMEM_SIZE);
        using MemoryMappedViewStream stream = sharedMemory.CreateViewStream();
        using BinaryReader rr = new(stream);

        try
        {
            MemStruct result = MemStruct.Read(rr);
            return result;
        }
        catch (Exception ex)
        {
            return new MemStruct()
            {
                Instances = new List<FPadRec>(),
                Messages = new List<MessageRec>()
            };
        }
    }

    private static void UnsafeWriteSharedMemory(MemStruct memStruct)
    {
        if (memStruct == null)
            return;

        sharedMemory ??= MemoryMappedFile.CreateOrOpen(SHMEM_NAME, SHMEM_SIZE);
        using MemoryMappedViewStream stream = sharedMemory.CreateViewStream();
        using BinaryWriter wr = new(stream);

        try
        {
            memStruct.Write(wr);
            wr.Flush();
        }
        catch (Exception ex)
        {
        }
    }

    #endregion
}
