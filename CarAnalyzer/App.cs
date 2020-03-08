using Xamarin.Forms;

namespace CarAnalyzer
{
    public partial class App : Application
    {
        public App()
        {
            SetResources();

             MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = (Color)Application.Current.Resources["primaryRed"],
                BarTextColor = Color.White
            };

            InitializeComponent();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void SetResources()
        {
            Resources = new ResourceDictionary
            {
                { "primaryRed", Color.FromHex("B30000") },
                { "primaryGrey", Color.FromHex("F2F2F2") }
            };
        }
    }
}
