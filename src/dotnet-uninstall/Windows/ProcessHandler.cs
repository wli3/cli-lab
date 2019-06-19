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
        private const int PROCESS_TIMEOUT = 5 * 60 * 1000;

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        internal static void ExecuteUninstallCommand(IEnumerable<Bundle> bundles)
        {
            if (!IsAdmin())
            {
                RunAsAdmin();
                return;
            }



            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

            AppDomain.CurrentDomain.ProcessExit += (_1, _2) => Console.WriteLine("!!!???");

            foreach (var bundle in bundles.ToList().AsReadOnly())
            {
                var args = ParseCommand(bundle.UninstallCommand, out var argc);

                /*

                var sInfo = new PInvoke.STARTUPINFO();
                var pSec = new PInvoke.SECURITY_ATTRIBUTES();
                var tSec = new PInvoke.SECURITY_ATTRIBUTES();
                pSec.nLength = Marshal.SizeOf(pSec);
                tSec.nLength = Marshal.SizeOf(tSec);

                Console.WriteLine(args.First());

                var retValue = PInvoke.CreateProcess(
                    args.First(),
                    bundle.UninstallCommand, //string.Join(" ", args.Skip(1)),
                    ref pSec,
                    ref tSec,
                    false,
                    PInvoke.CREATE_NEW_CONSOLE,
                    IntPtr.Zero,
                    null,
                    ref sInfo,
                    out var pInfo);

               //Debugger.Launch();

                if (!retValue ||
                    PInvoke.WaitForSingleObject(pInfo.hProcess, PROCESS_TIMEOUT) != PInvoke.WAIT_OBJECT_0 ||
                    !PInvoke.GetExitCodeProcess(pInfo.hProcess, out var exitCode))
                {
                    throw new UninstallationFailedException(bundle.UninstallCommand);
                }

                if (exitCode != 0)
                {
                    throw new UninstallationFailedException(bundle.UninstallCommand, (int)exitCode);
                }*/

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = args.First(),
                        Arguments = "/uninstall /quiet",
                        UseShellExecute = true,
                        CreateNoWindow = true,
                    }
                };

                process.Start();
                process.WaitForExit();

                Thread.Sleep(30 * 1000);
            }
        }

        private static IEnumerable<string> ParseCommand(string command, out int argc)
        {
            var argv = CommandLineToArgvW(command, out argc);

            if (argv == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            string[] args;
            try
            {
                args = new string[argc];

                for (var i = 0; i < argc; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }

            return args;
        }

        private static bool IsAdmin()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static void RunAsAdmin()
        {
            var location = Assembly.GetEntryAssembly().Location;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"{location} {string.Join(" ", Environment.GetCommandLineArgs().Skip(1))}",
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            process.WaitForExit();
        }

        internal static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine(sender.ToString());

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
