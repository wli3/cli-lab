using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.DotNet.Tools.Uninstall.Shared.BundleInfo;
using Microsoft.DotNet.Tools.Uninstall.Shared.Exceptions;

namespace Microsoft.DotNet.Tools.Uninstall.Windows
{
    public static class ProcessHandler
    {


        internal static void ExecuteUninstallCommand()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

            foreach (var bundle in new[] {
                @"C:\ProgramData\Package Cache\{0ecb2aeb-69f6-4c91-99ca-3da8fc126200}\dotnet-sdk-2.1.507-win-x64.exe",
                @"C:\ProgramData\Package Cache\{d198c231-829e-4f4b-832d-0571aa77244a}\dotnet-sdk-2.1.604-win-x64.exe" } )
            {

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = bundle,
                        Arguments = "/uninstall /quiet",
                        UseShellExecute = true,
                        CreateNoWindow = true,
                    }
                };

                
                process.Start();
                Console.WriteLine($" process.Start(); {process.ToString()}");
                process.WaitForExit();
                Console.WriteLine($" process.WaitForExit();");

            }
        }

        internal static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
          //  Console.WriteLine(sender.ToString());

            Console.WriteLine("\nThe read operation has been interrupted.");

            Console.WriteLine("  Key pressed: {0}", args.SpecialKey);

            Console.WriteLine("  Cancel property: {0}", args.Cancel);

            // Set the Cancel property to true to prevent the process from terminating.
            Console.WriteLine("Setting the Cancel property to true...");
            args.Cancel = true;

            // Announce the new value of the Cancel property.
            Console.WriteLine("  Cancel property: {0}", args.Cancel);
            Console.WriteLine("The read operation will resume...\n");
        }
    }
}
