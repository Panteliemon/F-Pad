using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad;

public static class ThreadingUtils
{
    public static void Sleep(int milliseconds, CancellationToken ct)
    {
        long startTicks = DateTime.Now.Ticks;
        long endTicks = startTicks + TimeSpan.TicksPerMillisecond * (long)milliseconds;
        long ticksNow = startTicks;
        if (endTicks < startTicks) // overflow is expected
        {
            while ((ticksNow < endTicks) || (ticksNow >= startTicks))
            {
                if (ct.IsCancellationRequested)
                    return;
                Thread.Sleep(20);
                ticksNow = DateTime.Now.Ticks;
            }
        }
        else // no overflow expected
        {
            while ((ticksNow < endTicks) && (ticksNow >= startTicks))
            {
                if (ct.IsCancellationRequested)
                    return;
                Thread.Sleep(20);
                ticksNow = DateTime.Now.Ticks;
            }
        }
    }
}
