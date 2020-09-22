using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Diaspora.Model;
using Plugin.Geolocator;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Diaspora
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
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
                        loadInd.IsVisible = false;
                        loadInd.IsRunning = false;
                        map.IsVisible = false;
                        await Navigation.PushAsync(new phone());
                    }
                    else
                    {
                        var Hist_username = lastName.user_email;
                        try
                        {
                            loadInd.IsVisible = true;
                            loadInd.IsRunning = true;
                            var locator = CrossGeolocator.Current;
                            var position = await locator.GetPositionAsync();
                            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                            //insert into database
                            WebClient client = new WebClient();
                            Uri uri = new Uri("https://apis.paxol.cloud/history.php");
                            NameValueCollection parameters = new NameValueCollection();
                            parameters.Add("username", Hist_username);
                            parameters.Add("histLat", position.Latitude.ToString());
                            parameters.Add("histLong", position.Longitude.ToString());
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
                else
                {
                    await Navigation.PushAsync(new phone());
                }
            }
        }
        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "success")
            {
                //handle when API called successfully
                loadInd.IsVisible = false;
                loadInd.IsRunning = false;
            }
            else
            {
                loadInd.IsVisible = false;
                loadInd.IsRunning = false;
                await DisplayAlert("Alert", r, "OK");
            }

        }
    }
}
