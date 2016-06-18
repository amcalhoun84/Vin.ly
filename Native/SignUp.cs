using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Json;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Net;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vin.ly
{
    [Activity(Label = "SignUp", MainLauncher = false, Icon = "@drawable/Icon")]
    public class SignUpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SignUp);

            Button signUp = FindViewById<Button>(Resource.Id.VinlySignup);
            Button clearUp = FindViewById<Button>(Resource.Id.VinlyClear);
            Button returnToMain = FindViewById<Button>(Resource.Id.VinlyReturn);

            signUp.Click += NewUserSignUp;
            clearUp.Click += ClearEntries;
            returnToMain.Click += ReturnMainActivity;
        }

        protected async void NewUserSignUp(object sender, EventArgs eventArgs)
        {
            TextView error = FindViewById<TextView>(Resource.Id.Error);
            EditText userName = FindViewById<EditText>(Resource.Id.SignUpName);
            EditText userPass = FindViewById<EditText>(Resource.Id.SignupPassword1);
            EditText checkPass = FindViewById<EditText>(Resource.Id.SignupPassword2);

               
                if (userName.Text == null || userName.Text == "")
                {
                    error.Text = "User name cannot be blank.";
                }

                else if ((userName.Text != null || userName.Text != "") && (userPass.Text == null || userPass.Text == ""))
                {
                    error.Text = "Password cannot be blank.";
                }

                else if ((userPass.Text != null || userPass.Text != "") && (checkPass.Text == null || checkPass.Text == ""))
                {
                    error.Text = "Password Check cannot be blank.";
                }

               else if (userPass.Text != checkPass.Text)
              {
                error.Text = "Password does not match!";
                }
            else
            {
                try
                {
                    string url = "http://52.27.116.225:8000/api/user/";

                    JsonValue json = await FetchUserInfo(url);

                    int jCount = json.Count;

                    for (int i = 0; i < jCount; i++)
                    {
                        if (json[i]["username"] == userName.Text)
                        {
                            Console.WriteLine("Work: {0}", json[i]["username"]);
                            error.Text = "User name already in use, please choose another.";
                            return;
                        }
                    }

                    string jsonString = "{ \"username\" : \"" + userName.Text + "\", \"password\" : \"" + checkPass.Text + "\" }";
                    Console.WriteLine("JSON String: {0}", jsonString);

                     JsonValue jsonPost = await PostUserAsync(url, userName.Text, checkPass.Text);
                     error.Text = "User Added";
                   
                }
                catch (WebException w)
                {
                    Console.Out.WriteLine("Something went wrong with the web connection.", w);
                    error.Text = "External issue with the network. Check your internet connection and try again.";
                }
                catch (KeyNotFoundException k)
                {
                    Console.Out.WriteLine("Bad Request, please correct the key or remove the offending entry.", k);
                    error.Text = "Bad Entry found in database.";

                }

                catch (Exception e)
                {
                    Console.Out.WriteLine("Something went wrong with the web connection.", e);
                    error.Text = "Internal issue with the App. Call the dev.";
                }
            }

        }

        void ReturnMainActivity(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(LogInActivity)));
        }

        protected void ClearEntries(object sender, EventArgs eventArgs)
        {
            EditText userName = FindViewById<EditText>(Resource.Id.SignUpName);
            EditText userPass = FindViewById<EditText>(Resource.Id.SignupPassword1);
            EditText checkPass = FindViewById<EditText>(Resource.Id.SignupPassword2);

            userName.Text = "";
            userPass.Text = "";
            checkPass.Text = "";

        }

        private async Task<JsonValue> FetchUserInfo(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", json.ToString());

                    return json;
                }
            }
        }

        private async Task<JsonValue> PostUserAsync(string url, string userName, string userPass)
        {
            var connectionManager = (ConnectivityManager)(Application.Context.ApplicationContext).GetSystemService(ConnectivityService);
            var activeConnection = connectionManager.ActiveNetworkInfo;
            var isConnected = activeConnection != null && activeConnection.IsConnected;

            if(isConnected)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{ \"username\" : \"" + userName + "\", \"password\" : \"" + userPass + "\" }";
                    Console.WriteLine("Sending To Server: {0}", json);
                    streamWriter.Write(json);
                    Console.WriteLine("Written to Server");
                    streamWriter.Flush();
                    Console.WriteLine("Buffer Flushed");
                    streamWriter.Close();
                    Console.WriteLine("Server Connecting Closed.");

                }

                using (WebResponse res = await request.GetResponseAsync())
                {
                    using (Stream stream = res.GetResponseStream())
                    {
                        JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                        Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
                        return jsonDoc;
                    }
                }

            }

            return null;

        }

    

    }
}