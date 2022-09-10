using Sharp.Imports;
using Sharp.Enums.Flags;

using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sharp.Utils
{
    public class Memory
    {
        public Process Process { get; set; }
        public IntPtr OpenHandle { get; set; }

        public Dictionary<string, IntPtr> Modules { get; set; }

        public Memory ( Process process )
        {
            Process = process;

            OpenProcess ( );
            DumpModules ( );
        }

        private void OpenProcess ( )
        {
            var flags =
                (int) ProcessAccessFlags.VirtualMemoryRead |
                (int) ProcessAccessFlags.VirtualMemoryWrite |
                (int) ProcessAccessFlags.VirtualMemoryOperation;

            OpenHandle = Kernel32.OpenProcess ( flags, false, Process.Id );
        }

        private void DumpModules ( )
        {
            Modules = new Dictionary<string, IntPtr> ( );

            foreach ( ProcessModule module in Process.Modules )
            {
                if ( module is null )
                {
                    continue;
                }

                Modules.Add ( module.ModuleName, module.BaseAddress );
            }
        }

        public T Read<T> ( IntPtr address )
        {
            var length = Marshal.SizeOf(typeof(T));

            if ( typeof ( T ) == typeof ( bool ) )
                length = 1;

            var buffer = new byte[length];
            var nBytesRead = uint.MinValue;
            Kernel32.ReadProcessMemory ( OpenHandle, address, buffer, ( uint ) length, ref nBytesRead );
            return GetStructure<T> ( buffer );
        }

        public byte [ ] ReadBytes ( IntPtr address, int length )
        {
            var buffer = new byte[length];
            var nBytesRead = uint.MinValue;
            Kernel32.ReadProcessMemory ( OpenHandle, address, buffer, ( uint ) length, ref nBytesRead );
            return buffer;
        }

        public float [ ] ReadMatrix<T> ( IntPtr Adress, int matrixSize ) where T : struct
        {
            var ByteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[ByteSize * matrixSize];
            ReadBytes ( Adress, buffer.Length );

            return ConvertToFloatArray ( buffer );
        }

        public void Write ( IntPtr address, object value )
        {
            var length = Marshal.SizeOf(value.GetType());
            var buffer = new byte[length];

            var ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr ( value, ptr, true );
            Marshal.Copy ( ptr, buffer, 0, length );
            Marshal.FreeHGlobal ( ptr );

            var nBytesRead = uint.MinValue;
            Kernel32.WriteProcessMemory ( OpenHandle, ( IntPtr ) address, buffer, ( IntPtr ) length, ref nBytesRead );
        }
            
        public void WriteString ( IntPtr handle, int address, string value )
        {
            byte[] data = Encoding.Default.GetBytes(value + "\0");

            Kernel32.WriteProcessMemory ( handle, address, data, data.Length, out _ );
        }

        public T GetStructure<T> ( byte [ ] bytes )
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free ( );
            return structure;
        }
        
        public float [ ] ConvertToFloatArray ( byte [ ] bytes )
        {
            if ( bytes.Length % 4 != 0 )
                throw new ArgumentException ( );

            float[] floats = new float[bytes.Length / 4];

            for ( var i = 0 ; i < floats.Length ; i++ )
                floats [ i ] = BitConverter.ToSingle ( bytes, i * 4 );

            return floats;
        }
    }
}
