using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator;
using Plugin.BluetoothLE;
using System.Reactive.Linq;
using Microsoft.AppCenter.Push;
using Plugin.LocalNotifications;

namespace CarAnalyzer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossLocalNotifications.Current.Show("Notification", "works");
            //var devices = CrossBleAdapter.Current.GetPairedDevices();
            //devices.Subscribe(deviceResult =>
            //{
            //    foreach (var device in deviceResult)
            //    {
            //        Console.WriteLine(device.Name);
            //    }
            //});

            GetLocationPermission();
        }

        private async void NavigateToCarDataPage_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarDataPage());
        }

        private async void NavigateToUserPage_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPage());
        }

        private async void NavigateToSettingsPage_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void GetLocationPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                {
                    await DisplayAlert("Need your permission", "We need to access your location", "Ok");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                if (results.ContainsKey(Permission.LocationWhenInUse))
                    status = results[Permission.LocationWhenInUse];
            }

            if (status == PermissionStatus.Granted)
            {
                CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Access to location denied", "We don't have access to your location", "Ok");
            }

        }


    }
}
