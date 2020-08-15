using Diaspora.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diaspora
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void registerBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool isUsernameEmpty = string.IsNullOrWhiteSpace(Useremail.Text);
                bool isPswdEmpty = string.IsNullOrEmpty(pswd.Text);
                bool isMatches = pswd.Text == pswd1.Text;

                if (isUsernameEmpty || isPswdEmpty == true)
                {
                    await DisplayAlert("Alert", "Username or Password Cannot be empty", "Ok");
                }
                if(isMatches == false)
                {
                    await DisplayAlert("Alert", "Passwords must match", "Ok");
                }
                else
                {
                    WebClient client = new WebClient();
                    Uri uri = new Uri("https://apis.paxol.cloud/register.php");
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("username", Useremail.Text);
                    parameters.Add("password", pswd.Text);
                    parameters.Add("confirm_password", pswd1.Text);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                    client.UploadValuesAsync(uri, parameters);
                }

            }
            catch (Exception ex)
            {
               // Unable to send details
                await DisplayAlert("Error", "Unable to register, try agains after sometime", "Ok");
            }
        }

        private async void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var r = Encoding.UTF8.GetString(e.Result);    //.UTF8.GetString(e.Result);

            var answr = r.ToString();

            bool axo = string.IsNullOrWhiteSpace(r);

            if (answr == "Success" || axo == false)
            {
                Post post = new Post()
                {
                    user_email = Useremail.Text
                    
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.Databaselocation))
                {
                    conn.CreateTable<Post>();

                    int row = conn.Insert(post);

                    conn.Close();

                    if (row > 0)
                    {
                        await Navigation.PushAsync(new main());
                    }
                    else
                    {
                        await DisplayAlert("Alert", "There is a server Error please try again later", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Alert", answr, "Ok");
            }
            
        }
    }
}