using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Plugin.BluetoothLE;
using Acr.UserDialogs;
using System.Reactive.Linq;
using CarAnalyzer.Infrastructure;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Disposables;

namespace CarAnalyzer
{
    public class iDevice
    {
        public IDevice BtDevice { get; set; }
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public int Rssi { get; set; }
        public bool IsConnected { get; set; }
        public bool IsConnectable { get; set; }
        public int ServiceCount { get; set; }
        public string LocalName { get; set; }
        public int TxPower { get; set; }
        public void pair() {
            BtDevice.PairingRequest().Subscribe(x =>
            {
                string txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
                Console.WriteLine(txt);
            });
        }
        public void connect()
        {
            // don't cleanup connection - force user to d/c
            if (BtDevice.Status == ConnectionStatus.Disconnected)
            {
                BtDevice.Connect();
            }
            else
            {
                BtDevice.CancelConnection();
            }
        }
    }

    public class DeviseListViewModel : ViewModel
    {
        public DeviseListViewModel()
        {
            adapter = CrossBleAdapter.Current;
            Devices = devices;

            OpenSettings = ReactiveCommand.Create(() =>
            {
                if (this.adapter.Features.HasFlag(AdapterFeatures.OpenSettings))
                {
                    this.adapter.OpenSettings();
                }
                else
                {
                    this.dialogs.Alert("Cannot open bluetooth settings");
                }
            });

            ToggleAdapterState = ReactiveCommand.Create(() =>
            {
                if (adapter.CanControlAdapterState())
                {
                    var poweredOn = adapter.Status == AdapterStatus.PoweredOn;
                    adapter.SetAdapterState(!poweredOn);
                }
                else
                {
                    dialogs.Alert("Cannot change bluetooth adapter state");
                }
            });

            ScanToggle = ReactiveCommand.Create(() =>
            {
                if (IsScanning)
                {
                    scan.Dispose();
                    IsScanning = false;
                }
                else
                {
                    Devices.Clear();
                    IsScanning = true;
                    scan = adapter.Scan().Subscribe(scanResult =>
                    {
                        var ad = scanResult.AdvertisementData;
                        var record = new iDevice
                        {
                            Name = scanResult.Device.Name,
                            Rssi = scanResult.Rssi,
                            Uuid = scanResult.Device.Uuid,
                            ServiceCount = ad.ServiceUuids?.Length ?? 0,
                            IsConnectable = ad.IsConnectable,
                            LocalName = ad.LocalName,
                            TxPower = ad.TxPower,
                            BtDevice = scanResult.Device
                        };

                        if (!Devices.Any(device => device.Uuid == record.Uuid)) Devices.Add(record);
                    }).DisposeWith(this.DeactivateWith);
                }
            }
            );
        }

        IDisposable scan;
        IAdapter adapter;
        IUserDialogs dialogs;

        public ICommand ScanToggle { get; }
        public ICommand OpenSettings { get; }
        public ICommand ToggleAdapterState { get; }
        public ICommand ConnectDevice { get; }

        public IList<iDevice> Devices { get; private set; }

        [Reactive] public bool IsScanning { get; set; }

        ObservableCollection<iDevice> devices = new ObservableCollection<iDevice>();
    }
}
