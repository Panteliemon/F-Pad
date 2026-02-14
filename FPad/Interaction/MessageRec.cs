using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Interaction;

internal class MessageRec
{
    public int TargetPid { get; set; }
    public MessageType MessageType { get; set; }

    // Payload (message-type dependent):
    public int Param1 { get; set; }
    public int Param2 { get; set; }

    public void Write(BinaryWriter wr)
    {
        wr.Write(TargetPid);
        wr.Write((byte)MessageType);
        
        if (MessageType == MessageType.ActivateSetCaret)
        {
            wr.Write((short)(2 * sizeof(int))); // payload length
            wr.Write(Param1);
            wr.Write(Param2);
        }
        else
        {
            wr.Write((short)0); // payload length
        }
    }

    public static MessageRec Read(BinaryReader rr)
    {
        int targetPid = rr.ReadInt32();
        MessageType messageType = (MessageType)rr.ReadByte();
        int payloadLengthLeft = rr.ReadInt16();

        int param1 = 0;
        int param2 = 0;
        if ((messageType == MessageType.ActivateSetCaret) && (payloadLengthLeft >= 2 * sizeof(int)))
        {
            param1 = rr.ReadInt32();
            param2 = rr.ReadInt32();
            payloadLengthLeft -= 2 * sizeof(int);
        }

        while (payloadLengthLeft > 0)
        {
            rr.ReadByte();
            payloadLengthLeft--;
        }

        return new MessageRec()
        {
            TargetPid = targetPid,
            MessageType = (MessageType)messageType,
            Param1 = param1,
            Param2 = param2
        };
    }
}
