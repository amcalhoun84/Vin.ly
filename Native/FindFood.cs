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
    [Activity(Label = "Find Wine by Food")]
    public class FindFood : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.WineName);

            TextView vname = FindViewById<TextView>(Resource.Id.NameDisplay);
            TextView vtype = FindViewById<TextView>(Resource.Id.TypeDisplay);
            TextView vvarietal = FindViewById<TextView>(Resource.Id.YearDisplay);
            TextView valccon = FindViewById<TextView>(Resource.Id.AlcDisplay);
            TextView vcomments = FindViewById<TextView>(Resource.Id.CmtDisplay);

            EditText wineSearch = FindViewById<EditText>(Resource.Id.SearchWine);
            Button button = FindViewById<Button>(Resource.Id.findWine);
            TextView Error = FindViewById<TextView>(Resource.Id.WineError);
           
            button.Click += async (IntentSender, e) =>
            {
                try
                {
                    string url = "http://52.27.116.225:8000/api/wine/";

                    Error.Text = "";
                    vname.Text = "";
                    vtype.Text = "";
                    vvarietal.Text = "";
                    valccon.Text = "";
                    vcomments.Text = "";

                    JsonValue json = await FetchWineAsync(url);

                    int jCount = json.Count;

                    for (int i = 0; i < jCount; i++)
                    {
                        if (json[i]["name"] == wineSearch.Text)
                        {
                            vname.Text += json[i]["name"] + "\n";
                            vtype.Text += json[i]["type"] + "\n";
                            vvarietal.Text += json[i]["varietal"] + "\n";
                            valccon.Text += json[i]["alccontent"] + "\n";
                            vcomments.Text += json[i]["comments"] + "\n";
                        }
                    }

                }
                catch(WebException)
                {
                    vname.Text = "";
                    vtype.Text = "";
                    vvarietal.Text = "";
                    valccon.Text = "";
                    vcomments.Text = "";
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