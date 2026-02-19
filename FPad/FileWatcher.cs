using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad;

/// <summary>
/// Emits events when the file is changed
/// </summary>
public class FileWatcher : IDisposable
{
    #region Fields

    private const int DEBOUNCE_DELAY_MS = 200;

    private object eventLock = new();
    private object savingLock = new();
    private object disposeLock = new();

    private FileSystemWatcher fileSystemWatcher;
    private ManualResetEvent fileChangedEvent;
    private DateTime lastWriteTime;

    private Thread eventThread;
    private CancellationTokenSource eventThreadCanceledTs;

    #endregion

    public string PathToFile { get; private set; }

    /// <summary>
    /// Called on random thread when it's detected that the file has been modified by another program
    /// </summary>
    public event EventHandler FileModified;

    public FileWatcher(string pathToFile)
    {
        string dirPath = Path.GetDirectoryName(pathToFile);
        string fileName = Path.GetFileName(pathToFile);
        if (Directory.Exists(dirPath))
        {
            FileInfo fi = new(pathToFile);
            if (fi.Exists)
            {
                // Proper init
                PathToFile = pathToFile;

                fileChangedEvent = new ManualResetEvent(false);

                eventThreadCanceledTs = new CancellationTokenSource();
                eventThread = new Thread(() => EventThreadProc(eventThreadCanceledTs.Token));
                eventThread.Start();

                lastWriteTime = fi.LastWriteTime;

                fileSystemWatcher = new FileSystemWatcher(dirPath, fileName);
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                fileSystemWatcher.IncludeSubdirectories = false;
                fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                fileSystemWatcher.EnableRaisingEvents = true;
            }
        }
    }

    /// <summary>
    /// Wrap saving file into this method's call when our app itself saves the file which it monitors.
    /// </summary>
    /// <param name="saveAction"></param>
    public void SaveWrapper(Action saveAction)
    {
        lock (savingLock)
        {
            saveAction();
            lastWriteTime = File.GetLastWriteTime(PathToFile);
        }
    }

    public void Dispose()
    {
        FileSystemWatcher fileSystemWatcherLocal = null;
        lock (disposeLock)
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcherLocal = fileSystemWatcher;
                fileSystemWatcher = null;
            }
        }

        if (fileSystemWatcherLocal != null) // Means that we do the dispose
        {
            fileSystemWatcherLocal.Dispose();

            eventThreadCanceledTs.Cancel();
            fileChangedEvent.Set(); // Wake up the thread
            eventThread.Join();

            fileChangedEvent.Dispose();
            fileChangedEvent = null;
        }
    }

    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        lock (eventLock)
        {
            fileChangedEvent.Set();
        }
    }

    private void EventThreadProc(CancellationToken ct)
    {
        while (true)
        {
            fileChangedEvent.WaitOne();
            if (ct.IsCancellationRequested)
                return;

            // Debounce
            ThreadingUtils.Sleep(DEBOUNCE_DELAY_MS, ct);
            if (ct.IsCancellationRequested)
                return;

            bool raiseEvent = false;
            lock (savingLock) // wait for unfinished save
            {
                lock (eventLock)
                {
                    DateTime newLastWriteTime = File.GetLastWriteTime(PathToFile);
                    raiseEvent = newLastWriteTime != lastWriteTime;
                    lastWriteTime = newLastWriteTime;

                    fileChangedEvent.Reset();
                }
            }

            if (ct.IsCancellationRequested)
                return;

            if (raiseEvent)
                FileModified?.Invoke(this, EventArgs.Empty);
        }
    }
}
