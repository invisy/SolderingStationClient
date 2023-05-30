namespace SolderingStationClient.Models.Dto;

public class SerialPortInfoDto
{
    public SerialPortInfoDto(string serialPortName, bool isConnected)
    {
        SerialPortName = serialPortName;
        IsConnected = isConnected;
    }

    public string SerialPortName { get; }
    public bool IsConnected { get; }
}