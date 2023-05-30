using SolderingStationClient.Models.Dto;

namespace SolderingStationClient.Models;

public class SerialPortInfoEventArgs : EventArgs
{
    public SerialPortInfoEventArgs(SerialPortInfoDto portInfo)
    {
        PortInfoDto = portInfo;
    }

    public SerialPortInfoDto PortInfoDto { get; }
}