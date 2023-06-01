namespace SolderingStation.Entities;

public class SerialConnectionParametersEntity : BaseEntity<int>
{
    public string PortName { get; set; } = string.Empty;
    public int BaudRate { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }
}