namespace SolderingStation.Entities;

public class SerialConnectionParametersEntity : BaseEntity<uint>
{
    public string PortName { get; set; } = string.Empty;
    public int BaudRate { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }

    //For EF
    public SerialConnectionParametersEntity()
    {
        
    }
    
    public SerialConnectionParametersEntity(string portName, int baudRate, int parity, int dataBits, int stopBits)
    {
        PortName = portName;
        BaudRate = baudRate;
        Parity = parity;
        DataBits = dataBits;
        StopBits = stopBits;
    }
}