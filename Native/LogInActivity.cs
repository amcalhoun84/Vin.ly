using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vin.ly
{

    [Activity(Label = "Vin.ly", MainLauncher = true, NoHistory = true, Icon = "@drawable/Icon")]
    public class LogInActivity : Activity
    {
 
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Login);


            EditText userName = FindViewById<EditText>(Resource.Id.UserName);
            EditText password = FindViewById<EditText>(Resource.Id.Password);

            TextView error = FindViewById<TextView>(Resource.Id.LogInError);
            Button login = FindViewById<Button>(Resource.Id.Login);
            Button clear = FindViewById<Button>(Resource.Id.Clear);
            Button newUserSignUp = FindViewById<Button>(Resource.Id.NewUser);
                      


            login.Click += async (IntentSender, e) =>
            {
                if (userName.Text == null || userName.Text == "" || password.Text == "" || password.Text == null)
                {
                    error.Text = "User name and password cannot be blank.";
                }
                else
                {

                    try
                    {


                        string url = "http://52.27.116.225:8000/api/user/";

                        JsonValue json = await FetchLogin(url);
                        int jCount = json.Count;

                        for (int i = 0; i < jCount; i++)
                        {
                            if (json[i]["username"] == userName.Text && json[i]["password"] == password.Text)
                            {
                                try
                                {
                                    DataStoreLogIn.setGlobalUser(userName.Text);
                                    Console.WriteLine("User name check; {0}", DataStoreLogIn.getGlobalUser());
                                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                                }

                                catch (WebException w)
                                {
                                    error.Text = "Network Connection Issue. Please try again shortly.";
                                    Console.WriteLine("Network Error, General", w);
                                }
                                catch (Exception f)
                                {
                                    error.Text = "System Error, please contact developer.";
                                    Console.Write("Internal System Error is preventing log-in", f);
                                }
                                finally
                                {
                                    Console.Write("Unspecified Error. Please report error and process to developer at heliosprod@me.com.");

                                }
                            }
                            else if (json[i]["username"] == userName.Text && json[i]["password"] != password.Text)
                            {
                                error.Text = "Incorrect password.";
                            }
                            else
                            {
                            }

                        }

                        
                    }
                    catch (WebException w)
                    {
                        error.Text = "Cannot connect to API";
                        Console.WriteLine("Web Connection Error: {0}", w);
                    }
                    catch (Exception d)
                    {
                        error.Text = "Other error, please contact heliosprod@me.com";
                        Console.WriteLine("Internal Error: {0}", d);
                    }
                }
            };

            newUserSignUp.Click += NewUserActivity;
            clear.Click += ClearActivity;

        }

        private async Task<JsonValue> FetchLogin(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "applicaiton/json";
            request.Method = "GET";

            Console.Out.WriteLine("Request: {0}", request);

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Resonse: {0}", json.ToString());
                    return json;
                }
            }
        }

        void ClearActivity(object sender, EventArgs eventArgs)
        {
                EditText userName = FindViewById<EditText>(Resource.Id.UserName);
                EditText password = FindViewById<EditText>(Resource.Id.Password);

                userName.Text = "";
                password.Text = "";

        }

        void NewUserActivity(object sender, EventArgs eventArgs)
        {
                StartActivity(new Intent(Application.Context, typeof(SignUpActivity)));
        }


    }
}