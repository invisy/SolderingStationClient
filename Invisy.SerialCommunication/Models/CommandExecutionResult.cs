namespace Invisy.SerialCommunication.Models;

public enum CommandExecutionStatus
{
    Ok = 1,
    WrongCommand,
    PackageIsCorrupted,
    WrongPacketLength,
    WrongArgumentValue,
    PortError = 253,
    NoResponse = 254,
    UnexpectedResponse = 255
}

public class CommandExecutionResult
{
    public CommandExecutionResult(CommandExecutionStatus status)
    {
        Status = status;
    }

    public CommandExecutionStatus Status { get; }
}

public class CommandExecutionResult<TData> : CommandExecutionResult
{
    public CommandExecutionResult(CommandExecutionStatus status, TData data) : base(status)
    {
        Data = data;
    }

    public TData Data { get; }
}