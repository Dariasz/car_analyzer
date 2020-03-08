using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.BluetoothLE;
using System.Reactive.Linq;
using System.Diagnostics.Contracts;

namespace CarAnalyzer
{
    public partial class CarDataPage : ContentPage
    {
        public CarDataPage()
        {
            var sampleData = new SampleData
            {
                Name = "Speed",
                Value = 60,
            };

            //var devices = CrossBleAdapter.Current.GetConnectedDevices();
            //devices.Subscribe(deviceResult =>
            //{
            //    foreach (var device in deviceResult)
            //    {
            //        Console.WriteLine(device.Name);
            //        if (device != null)
            //        {
            //            GetNotifications(device);

            //        }
            //    }
            //});

            this.BindingContext = sampleData;
            InitializeComponent();
        }

        private void GetNotifications(IDevice device)
        {
            device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                var result = characteristic.Read(); // use result.Data to see response
                Console.WriteLine(result);
            });
        }
    }
}
