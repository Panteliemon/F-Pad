using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPad;

internal record PrintTask(Printer Printer, Task PrintingTask, CancellationTokenSource Cts);
