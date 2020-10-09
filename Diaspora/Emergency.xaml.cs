using System;
using System.Collections.Generic;
using Xamd.ImageCarousel.Forms.Plugin.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using SQLite;
using Diaspora.Model;
using Plugin.Geolocator;
using System.Net;
using System.Collections.Specialized;

namespace Diaspora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Emergency : ContentPage
    {
        
        public Emergency()
        {
            
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                int rows = posts.Count();
                if (rows > 0)
                {
                    var lastName = posts.LastOrDefault();
                    var LoggedIn = lastName.user_email;

                    bool NoUser = string.IsNullOrEmpty(LoggedIn);

                    if (NoUser == true)
                    {
                        DisplayAlert("ALert", "No user selected", "Ok");
                    }
                    else
                    {
                        var nemsis = lastName.user_email;
                        try
                        {
                            var network = Connectivity.NetworkAccess;
                            if (network != NetworkAccess.Internet)
                            {
                                DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Unable to get location
                            DisplayAlert("Error", "Reload the APP", "Ok");
                        }

                    }
                }
                else
                {
                    Navigation.PushAsync(new phone());
                }
            }
        }

        private async void covid_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You raised a Covid-19 alert. This will prompt the Ministry of Health officials to contact you in less than 30mins from now",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch(reaction)
            {
                case "You raised a Covid-19 alert. This will prompt the Ministry of Health officials to contact you in less than 30mins from now":
                
                break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;

                            bool NoUser = string.IsNullOrEmpty(LoggedIn);

                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        //place code to send to API here
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "covid19");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new phone());
                        }
                    }
                    break;
            }

        }

        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
            else
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }

        private async void ambulance_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You are requesting an ambulance. This will prompt any near by health facility to contact you in less than 30mins from now",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch (reaction)
            {
                case "You are requesting an ambulance. This will prompt any near by health facility to contact you in less than 30mins from now":

                    break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;
                            bool NoUser = string.IsNullOrEmpty(LoggedIn);
                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "AmbulanceRequest");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted1;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                        break;
            }
        }

        private async void Client_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            //throw api call feedback
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
            else
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }

        private async void brutality_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You are reporting an assault by a law enforcer. This will prompt concerned officers to contact for more info in less than 30mins from now. Note the officers' name on uniform or station",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch (reaction)
            {
                case "You are reporting an assault by a law enforcer. This will prompt concerned offivers to contact for more info in less than 30mins from now. Note the officers' name on uniform or station":

                    break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;
                            bool NoUser = string.IsNullOrEmpty(LoggedIn);
                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "PoliceBrutalityandAssault");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted2;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                    break;
            }
        }

        private async void Client_UploadValuesCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            //throw api call feedback
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = false;
                loader.IsVisible = false;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
            }
            else
            {
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }

        private async void fire_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You are raising an alert for an ambulance. We shall get back to you shortly",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch (reaction)
            {
                case "You are raising an alert for an ambulance. We shall get back to you shortly":

                    break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;
                            bool NoUser = string.IsNullOrEmpty(LoggedIn);
                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "Ambulance");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted3;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                    break;
            }
        }

        private async void Client_UploadValuesCompleted3(object sender, UploadValuesCompletedEventArgs e)
        {
            //throw api call feedback
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
            else
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }

        private async void rape_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You are raising an alert on rape. We shall get back to you shortly",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch (reaction)
            {
                case "You are raising an alert on rape. We shall get back to you shortly":

                    break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;
                            bool NoUser = string.IsNullOrEmpty(LoggedIn);
                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "Rape");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted4;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                    break;
            }
        }

        private async void Client_UploadValuesCompleted4(object sender, UploadValuesCompletedEventArgs e)
        {
            //throw api call feedback
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
            else
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }

        private async void sos_Clicked(object sender, EventArgs e)
        {
            loader.IsRunning = true;
            loader.IsVisible = true;
            List<string> buttons = new List<string>
            {
                "You are raising an emergency. We shall deploy in your area shortly",
                "Proceed"
            };
            string reaction = await DisplayActionSheet("Alert", null, "Cancel", buttons.ToArray());
            loader.IsRunning = false;
            loader.IsVisible = false;
            switch (reaction)
            {
                case "You are raising an emergency. We shall deploy in your area shortly":

                    break;


                case "Proceed":
                    loader.IsRunning = true;
                    loader.IsVisible = true;
                    using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                    {
                        conn.CreateTable<Post>();
                        var posts = conn.Table<Post>().ToList();
                        int rows = posts.Count();
                        if (rows > 0)
                        {
                            var lastName = posts.LastOrDefault();
                            var LoggedIn = lastName.user_email;
                            bool NoUser = string.IsNullOrEmpty(LoggedIn);
                            if (NoUser == true)
                            {
                                await DisplayAlert("ALert", "No user selected", "Ok");
                            }
                            else
                            {
                                var nemsis = lastName.user_email;
                                try
                                {
                                    var network = Connectivity.NetworkAccess;
                                    if (network != NetworkAccess.Internet)
                                    {
                                        await DisplayAlert("Alert", "You need an internet connection to continue", "Ok");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var locator = CrossGeolocator.Current;
                                            var position = await locator.GetPositionAsync();
                                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                                            //insert into database
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri("https://apis.paxol.cloud/alert.php");
                                            NameValueCollection parameters = new NameValueCollection();
                                            parameters.Add("username", nemsis);
                                            parameters.Add("AType", "GeneralEmergency");
                                            parameters.Add("ALat", position.Latitude.ToString());
                                            parameters.Add("ALong", position.Longitude.ToString());
                                            client.UploadValuesCompleted += Client_UploadValuesCompleted5;
                                            client.UploadValuesAsync(uri, parameters);
                                        }
                                        catch (FeatureNotSupportedException fnsEx)
                                        {
                                            //Handle not supported on device exception
                                            await DisplayAlert("Error", "Location Not supported", "Ok");
                                        }
                                        catch (FeatureNotEnabledException fneEx)
                                        {
                                            // Handle not enabled on device exception
                                            await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                                        }
                                        catch (PermissionException pEx)
                                        {
                                            //Handle permission exception
                                            await DisplayAlert("Error", "Location permission denied", "Ok");
                                        }
                                        catch (Exception ex)
                                        {
                                            // Unable to get location
                                            await DisplayAlert("Error", "Reload the APP", "Ok");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Unable to get location
                                    await DisplayAlert("Error", "Reload the APP", "Ok");
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                    break;
            }
        }
        private async void Client_UploadValuesCompleted5(object sender, UploadValuesCompletedEventArgs e)
        {
            //throw api call feedback
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                //handle when API called successfully
                await DisplayAlert("Alert", "Your alert has been raised kindly await feedback", "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
            else
            {
                loader.IsRunning = true;
                loader.IsVisible = true;
                await DisplayAlert("Alert", r, "OK");
                loader.IsRunning = false;
                loader.IsVisible = false;
            }
        }
    }
}
