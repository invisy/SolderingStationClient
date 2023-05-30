using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Factories;
using SolderingStationClient.BLL.Implementation.Jobs;

namespace SolderingStationClient.BLL.Implementation.Factories;

public class ThermalProfileProcessingJobFactory : IThermalProfileProcessingJobFactory
{
    public IThermalProfileProcessingJob Create()
    {
        return new ThermalProfileProcessingJob();
    }
}