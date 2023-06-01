using Invisy.SerialCommunication.Models;

namespace Invisy.SerialCommunication.Exceptions;

public class CommandResponseException : CommandException
{
    public CommandResponse ResponseCode { get; }

    public CommandResponseException(CommandResponse response) : 
        base("Command was not proceed successfully by device. Check response code")
    {
        ResponseCode = response;
    }
}