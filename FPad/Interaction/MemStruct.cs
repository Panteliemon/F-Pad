using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Interaction;

internal class MemStruct
{
    private const int SIGNATURE = 0x06060600;
    private const int MAX_INSTANCES = 1024;
    private const int MAX_MESSAGES = 2048;

    public List<FPadRec> Instances { get; set; }
    public List<MessageRec> Messages { get; set; }

    /// <summary>
    /// </summary>
    /// <returns>True if new msg was added; false if such msg already exists.</returns>
    public bool AddSimpleMessage(int targetPid, MessageType messageType)
    {
        // Validate
        if (messageType == MessageType.Activate)
        {
            Messages ??= new List<MessageRec>();
            MessageRec existing = Messages.FirstOrDefault(x => x.TargetPid == targetPid
                && x.MessageType == messageType);
            if (existing == null)
            {
                Messages.Add(new MessageRec()
                {
                    TargetPid = targetPid,
                    MessageType = messageType
                });
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <returns>True if new msg was added; false if such msg already exists.</returns>
    public bool Add2ParamMessage(int targetPid, MessageType messageType, int param1, int param2)
    {
        // Validate
        if (messageType == MessageType.ActivateSetCaret)
        {
            Messages ??= new List<MessageRec>();
            MessageRec existing = Messages.FirstOrDefault(x => x.TargetPid == targetPid
                && x.MessageType == messageType
                && x.Param1 == param1
                && x.Param2 == param2);
            if (existing == null)
            {
                Messages.Add(new MessageRec()
                {
                    TargetPid = targetPid,
                    MessageType = messageType,
                    Param1 = param1,
                    Param2 = param2
                });
                return true;
            }
        }

        return false;
    }

    public void Write(BinaryWriter wr)
    {
        wr.Write(SIGNATURE);

        int instancesCount = (Instances == null) ? 0 : (Instances.Count > MAX_INSTANCES ? MAX_INSTANCES : Instances.Count);
        wr.Write(instancesCount);
        for (int i = 0; i < instancesCount; i++)
            Instances[i].Write(wr);

        int messagesCount = (Messages == null) ? 0 : (Messages.Count > MAX_MESSAGES ? MAX_MESSAGES : Messages.Count);
        wr.Write(messagesCount);
        for (int i = 0; i < messagesCount; i++)
            Messages[i].Write(wr);
    }

    public static MemStruct Read(BinaryReader rr)
    {
        int signature = rr.ReadInt32();
        if (signature != SIGNATURE)
            throw new ApplicationException();

        int instancesCount = rr.ReadInt32();
        if ((instancesCount < 0) || (instancesCount > MAX_INSTANCES))
            throw new ApplicationException();

        List<FPadRec> instances = new();
        for (int i = 0; i < instancesCount; i++)
            instances.Add(FPadRec.Read(rr));

        int messagesCount = rr.ReadInt32();
        if ((messagesCount < 0) || (messagesCount > MAX_MESSAGES))
            throw new ApplicationException();

        List<MessageRec> messages = new();
        for (int i = 0; i < messagesCount; i++)
            messages.Add(MessageRec.Read(rr));

        return new MemStruct()
        {
            Instances = instances,
            Messages = messages
        };
    }
}
