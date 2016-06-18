using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace Vin.ly
{
    [Activity(Label = "Wine List", MainLauncher = false)]
    public class WineList : Activity
    {


        bool _loggedIn = DataStoreLogIn.getGlobalBoolLoggedIn();
        string _username = DataStoreLogIn.getGlobalUser();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WineList);

            TextView wineList = FindViewById<TextView>(Resource.Id.WineList);
            TextView wineType = FindViewById<TextView>(Resource.Id.WineType);
            TextView wineRegion = FindViewById<TextView>(Resource.Id.WineRegion);
            TextView wineYear = FindViewById<TextView>(Resource.Id.WineYear);
            TextView WineError = FindViewById<TextView>(Resource.Id.ListingError);


            string url = "http://52.27.116.225:8000/api/user/" + _username + "/wine/";
            Console.Out.WriteLine("String: {0}", url);
            try
            {
                JsonValue json = await FetchWineAsync(url);

                int jCount = json.Count;
                Console.Out.WriteLine("String: {0}", jCount);

         

                for (int i = 0; i < jCount; i++)
                {
                    if(jCount == 0)
                    {
                        WineError.Text = "Please Add Some Wines";
                        break;
                    }

                    if (json[i]["username"] == _username)
                    {
                        int count = 0;

                        count++;

                        if (count <= 0)
                        {
                            wineList.Text = "Looks like you need to add some wines!";
                            break;
                        }
                        //Console.Out.WriteLine("Listings: {0}", json[i]["name"]);
                        wineList.Text += json[i]["name"] + "\n\n";
                        wineType.Text += json[i]["type"] + "\n\n";
                        wineRegion.Text += json[i]["region"] + "\n\n";
                        wineYear.Text += json[i]["year"] + "\n\n";
                    }

                }


            }
            catch (WebException)
            {
                wineList.Text = "System Web Error";
            }
            catch(NullReferenceException)
            {
                WineError.Text = "Ooops! Looks like you need to add some wines";
            }

        }
        private async Task<JsonValue> FetchWineAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            Console.Out.WriteLine("Request: {0}", request);

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
               

    }
}