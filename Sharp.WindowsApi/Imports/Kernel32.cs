using System;
using System.Runtime.InteropServices;

namespace Sharp.Imports
{
    public static class Kernel32
    {
        [DllImport ( "kernel32.dll" )]
        public static extern IntPtr GetConsoleWindow ( );

        [DllImport ( "kernel32.dll" )]
        public static extern void AllocConsole ( );

        [DllImport ( "kernel32.dll" )]
        public static extern void FreeConsole ( );

        [DllImport ( "kernel32.dll" )]
        public static extern IntPtr OpenProcess ( int dwDesiredAccess, bool bInheritHandle, int dwProcessId );
      
        [DllImport ( "Kernel32.dll" )]
        public static extern bool ReadProcessMemory ( IntPtr hProcess, IntPtr lpBaseAddress, byte [ ] lpBuffer, uint nSize, ref uint lpNumberOfBytesRead );

        [DllImport ( "kernel32.dll" )]
        public static extern bool WriteProcessMemory ( IntPtr hProcess, IntPtr lpBaseAddress, byte [ ] lpBuffer, IntPtr nSize, ref uint lpNumberOfBytesWritten );

        [DllImport ( "Kernel32.dll" )]
        public static extern bool WriteProcessMemory ( IntPtr handle, int lpBaseAddress, byte [ ] lpBuffer, int nSize, out int lpNumberOfBytesWritten );

        [DllImport ( "kernel32.dll", SetLastError = true )]
        public static extern IntPtr CreateRemoteThread ( IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize,
           IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId );

        [DllImport ( "kernel32.dll", SetLastError = true, ExactSpelling = true )]
        public static extern bool VirtualFreeEx ( IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, int dwFreeType );

        [DllImport ( "kernel32.dll", SetLastError = true, ExactSpelling = true )]
        public static extern IntPtr VirtualAllocEx ( IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize,
           uint flAllocationType, uint flProtect );
    }
}
