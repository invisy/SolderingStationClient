namespace Invisy.SerialCommunication.Models;

public enum CommandResponse
{
    Ok = 1,
    WrongCommand,
    PacketIsCorrupted,
    WrongPacketLength,
    WrongArgumentValue,
}