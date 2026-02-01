using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPad.Interaction;

internal class FPadRec
{
    public int Pid { get; set; }
    public string AcceptMessageEventName { get; set; }
    public string CurrentDocumentFullPath { get; set; }

    public void Write(BinaryWriter wr)
    {
        wr.Write(Pid);
        wr.Write(AcceptMessageEventName ?? string.Empty);
        wr.Write(CurrentDocumentFullPath ?? string.Empty);
    }

    public static FPadRec Read(BinaryReader rr)
    {
        int pid = rr.ReadInt32();
        string acceptMessageEventName = rr.ReadString();
        string currentDocumentFullPath = rr.ReadString();
        return new FPadRec()
        {
            Pid = pid,
            AcceptMessageEventName = acceptMessageEventName,
            CurrentDocumentFullPath = currentDocumentFullPath
        };
    }
}
