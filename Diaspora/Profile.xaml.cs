using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Diaspora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
            LoadData();
        }

        private async void profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        private async void home_Clicked(object sender, EventArgs e)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var Location = await Geolocation.GetLocationAsync(request);

                if (Location != null)
                {
                    var mylat = Location.Latitude.ToString();
                    var mylong = Location.Longitude.ToString();
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

                    string upspt = "3";
                    //insert into database
                    WebClient client = new WebClient();
                    Uri uri = new Uri("http://www.akyinvestmentsltd.com/diaspora/index.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("user_lat", mylat);
                    parameters.Add("user_long", mylong);
                    parameters.Add("user_id", upspt);
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
        public class ItemClass
        {
            public string user_id { get; set; }
            public string u_name { get; set; }
            public string u_position { get; set; }
            public string u_pic { get; set; }
            public string u_company { get; set; }
            }
        public async void LoadData()
        {
            var content = "";
            HttpClient client = new HttpClient();
            var RestURL = "http://www.akyinvestmentsltd.com/diaspora/results.php";
            client.BaseAddress = new Uri(RestURL);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(RestURL);
            content = await response.Content.ReadAsStringAsync();
            var Items = JsonConvert.DeserializeObject<List<ItemClass>>(content);
            u_profile.ItemsSource = Items;
        }
    }
}