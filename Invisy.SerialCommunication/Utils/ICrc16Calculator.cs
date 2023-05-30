namespace Invisy.SerialCommunication.Utils;

public interface ICrc16Calculator
{
    ushort Calculate(ReadOnlySpan<byte> data, int index, int count);
}