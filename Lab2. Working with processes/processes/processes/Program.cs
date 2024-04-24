using processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

class Program
{
    static void Main()
    {
        ProcessManager processManager = new ProcessManager();
        processManager.Run();
    }
}