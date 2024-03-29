﻿using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Abstractions.Factories;

public interface IThermalProfileProcessingJobFactory
{
    public IThermalProfileProcessingJob Create(IEnumerable<ThermalProfileControllerBinding> bindings);
}