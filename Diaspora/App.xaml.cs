using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diaspora
{
    public partial class App : Application
    {
        public static string Databaselocation = string.Empty;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new main());
        }
        public App(string databseLocation)
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new main());
            
            Databaselocation = databseLocation;
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
    }
}
