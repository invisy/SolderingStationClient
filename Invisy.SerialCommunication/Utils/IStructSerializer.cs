namespace Invisy.SerialCommunication.Utils;

public interface IStructSerializer
{
    byte[] Serialize<T>(T data) where T : struct;
    T? Deserialize<T>(byte[] data) where T : struct;
}