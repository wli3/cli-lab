using System;
using System.Runtime.InteropServices;

namespace Microsoft.DotNet.Tools.Uninstall.Windows
{
    internal class PInvoke
    {
        internal const uint DEBUG_PROCESS = 0x00000001;
        internal const uint DEBUG_ONLY_THIS_PROCESS = 0x00000002;
        internal const uint CREATE_SUSPENDED = 0x00000004;
        internal const uint DETACHED_PROCESS = 0x00000008;
        internal const uint CREATE_NEW_CONSOLE = 0x00000010;
        internal const uint NORMAL_PRIORITY_CLASS = 0x00000020;
        internal const uint IDLE_PRIORITY_CLASS = 0x00000040;
        internal const uint HIGH_PRIORITY_CLASS = 0x00000080;
        internal const uint REALTIME_PRIORITY_CLASS = 0x00000100;
        internal const uint CREATE_NEW_PROCESS_GROUP = 0x00000200;
        internal const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        internal const uint CREATE_SEPARATE_WOW_VDM = 0x00000800;
        internal const uint CREATE_SHARED_WOW_VDM = 0x00001000;
        internal const uint CREATE_FORCEDOS = 0x00002000;
        internal const uint BELOW_NORMAL_PRIORITY_CLASS = 0x00004000;
        internal const uint ABOVE_NORMAL_PRIORITY_CLASS = 0x00008000;
        internal const uint INHERIT_PARENT_AFFINITY = 0x00010000;
        internal const uint INHERIT_CALLER_PRIORITY = 0x00020000;
        internal const uint CREATE_PROTECTED_PROCESS = 0x00040000;
        internal const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;
        internal const uint PROCESS_MODE_BACKGROUND_BEGIN = 0x00100000;
        internal const uint PROCESS_MODE_BACKGROUND_END = 0x00200000;
        internal const uint CREATE_BREAKAWAY_FROM_JOB = 0x01000000;
        internal const uint CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000;
        internal const uint CREATE_DEFAULT_ERROR_MODE = 0x04000000;
        internal const uint CREATE_NO_WINDOW = 0x08000000;
        internal const uint PROFILE_USER = 0x10000000;
        internal const uint PROFILE_KERNEL = 0x20000000;
        internal const uint PROFILE_SERVER = 0x40000000;
        internal const uint CREATE_IGNORE_SYSTEM_DEFAULT = 0x80000000;

        internal const uint WAIT_ABANDONED = 0x00000080;
        internal const uint WAIT_OBJECT_0 = 0x00000000;
        internal const uint WAIT_TIMEOUT = 0x00000102;
        internal const uint WAIT_FAILED = 0xFFFFFFFF;

        [DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        internal static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes, 
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles, 
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo, 
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint WaitForSingleObject(
            IntPtr hHandle,
            uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetExitCodeProcess(
            IntPtr hProcess,
            out uint lpExitCode);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STARTUPINFOEX
        {
            public STARTUPINFO StartupInfo;
            public IntPtr lpAttributeList;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }
    }
}
