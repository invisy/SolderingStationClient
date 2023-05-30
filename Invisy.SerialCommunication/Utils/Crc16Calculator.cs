namespace Invisy.SerialCommunication.Utils;

public class Crc16Calculator : ICrc16Calculator
{
    public ushort Calculate(ReadOnlySpan<byte> data, int index, int count)
    {
        ushort crc = 0xFFFF;

        for (var i = index; i < count; i++)
        {
            crc = (ushort)(crc ^ data[i]);

            for (var j = 0; j < 8; j++)
            {
                var lsb = (char)(crc & 0x0001);
                crc = (ushort)((crc >> 1) & 0x7fff);

                if (lsb == 1)
                    crc = (ushort)(crc ^ 0xa001);
            }
        }

        return crc;
    }
}