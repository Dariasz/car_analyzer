using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BluetoothLE;
using Xamarin.Forms;

namespace CarAnalyzer
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void NavigateToDeviseListPage_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeviseListPage());
        }
    }
}
