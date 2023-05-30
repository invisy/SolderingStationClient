using System.Runtime.InteropServices;

namespace Invisy.SerialCommunication.Utils;

public class StructSerializer : IStructSerializer
{
    public byte[] Serialize<T>(T data) where T : struct
    {
        var size = Marshal.SizeOf(data);
        var arr = new byte[size];

        var ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return arr;
    }

    public T? Deserialize<T>(byte[] data) where T : struct
    {
        var structure = new T();

        if (data.Length != Marshal.SizeOf(structure))
            return structure;

        var size = Marshal.SizeOf(structure);
        var ptr = IntPtr.Zero;

        try
        {
            ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(data, 0, ptr, size);

            structure = (T)(Marshal.PtrToStructure(ptr, structure.GetType()) ?? structure);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return structure;
    }
}