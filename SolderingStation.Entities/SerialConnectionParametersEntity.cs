namespace SolderingStation.Entities;

public class SerialConnectionParametersEntity : BaseEntity<uint>
{
    public uint ProfileId { get; set; }
    public string PortName { get; set; } = string.Empty;
    public int BaudRate { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }

    //For EF
    public SerialConnectionParametersEntity()
    {
        
    }
    
    public SerialConnectionParametersEntity(uint profileId, string portName, int baudRate, int parity, int dataBits, int stopBits)
    {
        ProfileId = profileId;
        PortName = portName;
        BaudRate = baudRate;
        Parity = parity;
        DataBits = dataBits;
        StopBits = stopBits;
    }
}