using System.IO.Ports;
using SolderingStation.DAL.Implementation;
using SolderingStation.Entities;
using SolderingStationClient.BLL.Abstractions;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;

namespace SolderingStationClient.BLL.Implementation.Services;

public class SerialPortsSettingsService : ISerialPortsSettingsService
{
    private readonly SolderingStationDbContext _context;
    private readonly IUserProfileService _userProfileService;
    
    public SerialPortsSettingsService(SolderingStationDbContext context, IUserProfileService userProfileService)
    {
        _context = context;
        _userProfileService = userProfileService;
    }

    public SerialPortSettings? GetByPortName(string portName)
    {
        var userProfileId = _userProfileService.GetProfileId();

        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(p => p.Id == userProfileId);

        if (profile.SerialConnectionsParameters != null)
        {
            var serialConnectionParameters =
                profile.SerialConnectionsParameters.FirstOrDefault(sp => sp.PortName == portName);
            return serialConnectionParameters != null ? Map(serialConnectionParameters) : null;
        }

        return null;
    }

    public void Add(SerialPortSettings portSettings)
    {
        var userProfileId = _userProfileService.GetProfileId();
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(p => p.Id == userProfileId);
        if (profile.SerialConnectionsParameters != null)
        {
            profile.SerialConnectionsParameters.Add(Map(portSettings));
            profiles.Update(profile);
        }
    }

    public void Remove(string portName)
    {
        var userProfileId = _userProfileService.GetProfileId();
        
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(p => p.Id == userProfileId);

        var serialConnectionParameters = profile.SerialConnectionsParameters.FirstOrDefault(x => x.PortName == portName);
        
        if (serialConnectionParameters != null)
        {
            profile.SerialConnectionsParameters.Remove(serialConnectionParameters);
            profiles.Update(profile);
        }
    }

    public void Update(SerialPortSettings portSettings)
    {
        var userProfileId = _userProfileService.GetProfileId();
        
        var profiles = _context.GetCollection<ProfileEntity>();
        var profile = profiles.FindOne(p => p.Id == userProfileId);

        var serialConnectionParameters = profile.SerialConnectionsParameters.FirstOrDefault(x => x.PortName == portSettings.PortName);
        if (serialConnectionParameters == null)
            return;
        
        serialConnectionParameters.PortName = portSettings.PortName;
        serialConnectionParameters.Parity = (int)portSettings.Parity;
        serialConnectionParameters.BaudRate = portSettings.BaudRate;
        serialConnectionParameters.StopBits = (int)portSettings.StopBits;
        serialConnectionParameters.DataBits = portSettings.DataBits;
        
        profile.SerialConnectionsParameters.Remove(serialConnectionParameters);
        profiles.Update(profile);
    }
    
    private SerialPortSettings Map(SerialConnectionParametersEntity parameters)
    {
        return new SerialPortSettings(
            parameters.PortName, parameters.BaudRate, (Parity)parameters.Parity, parameters.DataBits, (StopBits)parameters.StopBits);
    }
    
    private SerialConnectionParametersEntity Map(SerialPortSettings parameters)
    {
        var profileId = _userProfileService.GetProfileId();
        return new SerialConnectionParametersEntity(parameters.PortName, parameters.BaudRate, 
            (int)parameters.Parity, parameters.DataBits, (int)parameters.StopBits);
    }
}