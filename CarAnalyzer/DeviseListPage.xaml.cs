using System;
using Xamarin.Forms;

namespace CarAnalyzer
{
    public partial class DeviseListPage : ContentPage
    {
        public DeviseListPage()
        {
            InitializeComponent();
            BindingContext = new DeviseListViewModel();
        }

        void OnListViewDeviceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            iDevice selectedDevise = e.SelectedItem as iDevice;
            selectedDevise.pair();
            selectedDevise.connect();
            Navigation.PopToRootAsync();
        }
    }
}
