using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
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
        }

        //public async void getLocation()
        //{
            

        //}

       

        private void profile_Clicked(object sender, EventArgs e)
        {

        }

        private async void home_Clicked(object sender, EventArgs e)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var Location = await Geolocation.GetLocationAsync(request);

                if (Location != null)
                {
                    mylat.Text = Location.Latitude.ToString();
                    mylong.Text = Location.Longitude.ToString();
                    var ab = Location.Latitude;
                    var bc = Location.Longitude;
                    Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map
                    {
                        // ...
                    };
                    Pin pin = new Pin
                    {
                        Label = "User name",
                        Address = "City & Occupation",
                        Type = PinType.Place,
                        Position = new Position(ab, bc)
                    };
                    Position position = new Position(ab, bc);
                    //MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
                    //Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(mapSpan);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.444));
                    map.MoveToRegion(mapSpan);
                    map.Pins.Add(pin);

                    //insert into database
                    WebClient client = new WebClient();
                    Uri uri = new Uri("http://www.akyinvestmentsltd.com/diaspora/index.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("user_lat", mylat.Text);
                    parameters.Add("user_long", mylong.Text);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(uri, parameters);

                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Error", "Location Not supported", "Ok");
                }
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

        private void emergency_Clicked(object sender, EventArgs e)
        {

        }
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            string r = Encoding.UTF8.GetString(e.Result);
            if (r == "login")
            {
                //exeption to handle if not logged in
            }
            else
            {
                DisplayAlert("Alert", r, "OK");
            }

        }

    }
}
