using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Plugin.Geolocator;
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
            // getLocation();
            Findme();
        }

        public async void Findme()
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync();
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.5)));
                    //insert into database
                    WebClient client = new WebClient();
                    Uri uri = new Uri("http://www.akyinvestmentsltd.com/diaspora/index.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("user_lat", position.Latitude.ToString());
                    parameters.Add("user_long", position.Longitude.ToString());
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(uri, parameters);
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    await DisplayAlert("Error", "Location Not supported", "Ok");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                    await DisplayAlert("Alert!", "Please Turn on Location", "Ok");
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception
                    await DisplayAlert("Error", "Location permission denied", "Ok");
                }
                catch (Exception ex)
                {
                    // Unable to get location
                    await DisplayAlert("Error", "Reload the APP", "Ok");
                }
        }
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);
            //if (r == "login")
            //{
            //    //exeption to handle if not logged in
            //}
            //else
            //{
                DisplayAlert("Alert", r, "OK");
          //  }

        }

    }
}
