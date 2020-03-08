using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.BluetoothLE;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using CarAnalyzer.Infrastructure;

namespace CarAnalyzer
{
    public class DeviceViewModel : ViewModel
    {
        IDevice device;


        public DeviceViewModel(IUserDialogs dialogs)
        {
            this.PairToDevice = ReactiveCommand.Create(() =>
            {
                if (!this.device.Features.HasFlag(DeviceFeatures.PairingRequests))
                {
                    dialogs.Toast("Pairing is not supported on this platform");
                }
                else if (this.device.PairingStatus == PairingStatus.Paired)
                {
                    dialogs.Toast("Device is already paired");
                }
                else
                {
                    this.device
                        .PairingRequest()
                        .Subscribe(x =>
                        {
                            var txt = x ? "Device Paired Successfully" : "Device Pairing Failed";
                            dialogs.Toast(txt);
                            this.RaisePropertyChanged(nameof(this.PairingText));
                        });
                }
            });
        }

        public ICommand ConnectionToggle { get; }
        public ICommand PairToDevice { get; }
        public ICommand RequestMtu { get; }

        [Reactive] public string Name { get; private set; }
        [Reactive] public Guid Uuid { get; private set; }
        [Reactive] public string PairingText { get; private set; }

        [Reactive] public string ConnectText { get; private set; } = "Connect";
        [Reactive] public int Rssi { get; private set; }
    }
}