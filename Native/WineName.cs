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
    [Activity(Label = "Find Wine by Name")]
    public class WineName : Activity
    {
        bool _loggedIn = DataStoreLogIn.getGlobalBoolLoggedIn();
        string _userName = DataStoreLogIn.getGlobalUser();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            string _userName = DataStoreLogIn.getGlobalUser();
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.WineName);

            TextView vname = FindViewById<TextView>(Resource.Id.NameDisplay);
            TextView vtype = FindViewById<TextView>(Resource.Id.TypeDisplay);
            TextView vregion = FindViewById<TextView>(Resource.Id.RegionDisplay);
            TextView vyears = FindViewById<TextView>(Resource.Id.YearDisplay);

            EditText wineSearch = FindViewById<EditText>(Resource.Id.SearchWine);
            Button button = FindViewById<Button>(Resource.Id.findWine);
            TextView Error = FindViewById<TextView>(Resource.Id.WineError);
           
            button.Click += async (sender, e) =>
            {
                try
                {
                    string url = "http://52.27.116.225:8000/api/wine/";

                    Error.Text = "";
                    vname.Text = "";
                    vtype.Text = "";
                    vregion.Text = "";
                    vyears.Text = "";

                    JsonValue json = await FetchWineAsync(url);

                    int jCount = json.Count;

                    for (int i = 0; i < jCount; i++)
                    {
                        if (json[i]["name"] == wineSearch.Text && json[i]["username"] == _userName)
                        {
                            vname.Text += json[i]["name"] + "\n\n";
                            vtype.Text += json[i]["type"] + "\n\n";
                            vregion.Text += json[i]["region"] + "\n\n";
                            vyears.Text += json[i]["year"] + "\n\n";
                        }
                    }
                    
                }
                catch(WebException)
                {
                    vname.Text = "";
                    vtype.Text = "";
                    vregion.Text = "";
                    vyears.Text = "";
                    Error.Text = "WINE NOT FOUND";
                    Console.WriteLine("{0} Caught Exception.", e);
                }

                catch (Exception d)
                {
                    Error.Text = "System Error";
                    Console.WriteLine("{0} Caught Exception.", d);
                }

            };
        }


        private async Task<JsonValue> FetchWineAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    return jsonDoc;
                }
            }
        }

    }
 
}