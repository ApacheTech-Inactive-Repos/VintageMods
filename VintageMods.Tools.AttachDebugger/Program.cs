using System;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace VintageMods.Tools.AttachDebugger
{
    internal static class Program
    {
        private static void AttachToProcess(string processName, int timeout)
        {
            try
            {
                var dte2 = (DTE2) Win32.GetActiveObject("VisualStudio.DTE.16.0");
                foreach (var proc in dte2.Debugger.LocalProcesses.Cast<Process>()
                    .Where(proc => proc.Name.EndsWith(processName)))
                {
                    proc.Attach();
                    System.Threading.Thread.Sleep(timeout);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Unable to Attach to Debugger : " + ex.Message);
            }
        }

        public static void Main()
        {
            // to call w/ Command Line arguments follow this syntax
            // AttachProcess <<ProcessName>> <<TimeOut>>
            // AttachProcess app.exe 2000
            var appName = "Vintagestory.exe";
            var ttl = 20000; // 20 Seconds
            try
            {
                if (Environment.GetCommandLineArgs().Length >= 1)
                    appName = Environment.GetCommandLineArgs()[0];

                if (Environment.GetCommandLineArgs().Length >= 2)
                {
                    int.TryParse(Environment.GetCommandLineArgs()[2], out ttl);
                }
                Environment.GetCommandLineArgs();
                AttachToProcess(appName, ttl);
                Console.WriteLine("Attached!!");
            }
            catch (Exception ex)
            {
                Console.Write("Unable to Attach to Debugger : " + ex.Message);
            }
        }
    }
}