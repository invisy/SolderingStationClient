using System.IO.Ports;
using SolderingStation.DAL.Abstractions;
using SolderingStation.Entities;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.BLL.Implementation.Specifications;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class SerialPortsSettingsService : ISerialPortsSettingsService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserProfileService _userProfileService;
    
    public SerialPortsSettingsService(IUnitOfWork uow, IUserProfileService userProfileService)
    {
        _uow = uow;
        _userProfileService = userProfileService;
    }

    public async Task<SerialPortSettings?> GetByPortName(string portName)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var spec = new SerialConnectionParametersByPortNameSpecification(userProfileId, portName);
        var serialConnectionParameters = await _uow.SerialConnectionParametersRepository.GetBySpecAsync(spec);

        return serialConnectionParameters != null ? Map(serialConnectionParameters) : null;
    }

    public async Task Add(SerialPortSettings portSettings)
    {
        _uow.SerialConnectionParametersRepository.Add(Map(portSettings));
        await _uow.SaveChanges();
    }

    public async Task Remove(string portName)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var spec = new SerialConnectionParametersByPortNameSpecification(userProfileId, portName);
        var serialConnectionParameters = await _uow.SerialConnectionParametersRepository.GetBySpecAsync(spec);
        if (serialConnectionParameters != null)
        {
            _uow.SerialConnectionParametersRepository.Delete(serialConnectionParameters.Id);
            await _uow.SaveChanges();
        }
    }

    public async Task Update(SerialPortSettings portSettings)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var spec = new SerialConnectionParametersByPortNameSpecification(userProfileId, portSettings.PortName);
        var serialConnectionParameters = await _uow.SerialConnectionParametersRepository.GetBySpecAsync(spec);
        if (serialConnectionParameters == null)
            return;
        
        serialConnectionParameters.PortName = portSettings.PortName;
        serialConnectionParameters.Parity = (int)portSettings.Parity;
        serialConnectionParameters.BaudRate = portSettings.BaudRate;
        serialConnectionParameters.StopBits = (int)portSettings.StopBits;
        serialConnectionParameters.DataBits = portSettings.DataBits;
        _uow.SerialConnectionParametersRepository.Update(serialConnectionParameters);
        await _uow.SaveChanges();
    }
    
    private SerialPortSettings Map(SerialConnectionParametersEntity parameters)
    {
        return new SerialPortSettings(
            parameters.PortName, parameters.BaudRate, (Parity)parameters.Parity, parameters.DataBits, (StopBits)parameters.StopBits);
    }
    
    private SerialConnectionParametersEntity Map(SerialPortSettings parameters)
    {
        var profileId = _userProfileService.GetProfileId();
        return new SerialConnectionParametersEntity(
            profileId, parameters.PortName, parameters.BaudRate, 
            (int)parameters.Parity, parameters.DataBits, (int)parameters.StopBits);
    }
}