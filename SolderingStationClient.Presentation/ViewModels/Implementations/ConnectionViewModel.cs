﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Avalonia.Collections;
using ReactiveUI;
using SolderingStationClient.BLL.Abstractions.Services;
using SolderingStationClient.Models;
using SolderingStationClient.Models.Dto;
using SolderingStationClient.Presentation.Models;
using SolderingStationClient.Presentation.Services;
using SolderingStationClient.Presentation.ViewModels.Interfaces;

namespace SolderingStationClient.Presentation.ViewModels.Implementations;

public class ConnectionViewModel : ViewModelBase, IConnectionViewModel
{
    private readonly IApplicationDispatcher _applicationDispatcher;
    private readonly IViewModelCreator _viewModelCreator;
    private readonly ISerialPortsService _serialPortService;
    private readonly IMessageBoxService _messageBoxService;

    private bool _isActive;

    private SerialPortInfoViewModel? _selectedPort;

    public ConnectionViewModel(
        IApplicationDispatcher applicationDispatcher,
        IViewModelCreator viewModelCreator,
        ISerialPortsService serialPortService,
        IMessageBoxService messageBoxService)
    {
        _applicationDispatcher = Guard.Against.Null(applicationDispatcher);
        _viewModelCreator = Guard.Against.Null(viewModelCreator);
        _serialPortService = Guard.Against.Null(serialPortService);
        _messageBoxService = Guard.Against.Null(messageBoxService);

        Init();
    }

    public IAvaloniaList<SerialPortInfoViewModel> AvailablePorts { get; } = new AvaloniaList<SerialPortInfoViewModel>();

    public SerialPortInfoViewModel? SelectedPort
    {
        get => _selectedPort;
        set
        {
            if (value == null)
            {
                IsActive = false;
            }
            else
            {
                this.RaiseAndSetIfChanged(ref _selectedPort, value);
                IsActive = true;
            }
        }
    }

    public override bool IsActive
    {
        get => _isActive && AvailablePorts.Count > 0;
        set => this.RaiseAndSetIfChanged(ref _isActive, value);
    }

    public Interaction<ISerialPortAdvancedSettingsWindowViewModel, Unit> ShowSerialPortAdvancedSettingsWindow { get; } =
        new();

    public async Task Toggle()
    {
        try
        {
            IsActive = false;
            if (!(bool)_selectedPort?.IsConnected)
                await _serialPortService.Connect(_selectedPort.SerialPortName);
            else
                _serialPortService.Disconnect(_selectedPort.SerialPortName);
        }
        catch (Exception)
        {
            await _messageBoxService.ShowMessageBoxWithLocalizedMessage("Localization.DeviceIsNotSupported", MessageBoxType.Error);
        }
        finally
        {
            IsActive = true;
        }
    }

    public async Task OpenAdvancedSettings()
    {
        if (_selectedPort == null)
            return;

        var serialPortAdvancedSettingsWindowViewModel = _viewModelCreator.Create<ISerialPortAdvancedSettingsWindowViewModel>();
        await serialPortAdvancedSettingsWindowViewModel.Init(_selectedPort.SerialPortName);
        await ShowSerialPortAdvancedSettingsWindow.Handle(serialPortAdvancedSettingsWindowViewModel);
    }

    private void Init()
    {
        var serialPortInfoVms = _serialPortService.SerialPorts.Select(portInfo => CreateFromDto(portInfo));
        AvailablePorts.AddRange(serialPortInfoVms);
        SelectedPort = AvailablePorts.LastOrDefault();

        _serialPortService.PortAdded += OnPortAdded;
        _serialPortService.PortRemoved += OnPortRemoved;
        _serialPortService.PortInfoUpdateEvent += OnPortInfoUpdated;
    }

    private void OnPortAdded(object? sender, SerialPortInfoEventArgs args)
    {
        var newPortVm = CreateFromDto(args.PortInfoDto);
        _applicationDispatcher.Dispatch(() =>
        {
            AvailablePorts.Add(newPortVm);
            SelectedPort = newPortVm;
        });
    }

    private void OnPortRemoved(object? sender, SerialPortRemovedEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            SelectedPort = null;
            var viewModel = AvailablePorts.First(port => port.SerialPortName == args.PortName);
            AvailablePorts.Remove(viewModel);
            SelectedPort = AvailablePorts.LastOrDefault();
        });
    }

    private void OnPortInfoUpdated(object? sender, SerialPortInfoEventArgs args)
    {
        _applicationDispatcher.Dispatch(() =>
        {
            var viewModel = AvailablePorts.First(port => port.SerialPortName == args.PortInfoDto.SerialPortName);
            viewModel.IsConnected = args.PortInfoDto.IsConnected;
        });
    }

    private SerialPortInfoViewModel CreateFromDto(SerialPortInfoDto dto)
    {
        return new SerialPortInfoViewModel(dto.SerialPortName, dto.IsConnected);
    }
}