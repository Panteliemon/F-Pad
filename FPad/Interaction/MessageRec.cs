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

    public void Write(BinaryWriter wr)
    {
        wr.Write(TargetPid);
        wr.Write((byte)MessageType);
    }

    public static MessageRec Read(BinaryReader rr)
    {
        int targetPid = rr.ReadInt32();
        byte messageType = rr.ReadByte();
        return new MessageRec()
        {
            TargetPid = targetPid,
            MessageType = (MessageType)messageType
        };
    }
}
