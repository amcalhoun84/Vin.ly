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
    [Activity(Label = "Get Food Pairing", MainLauncher = false)]
    public class GetFood : Activity
    {
        bool _loggedIn = DataStoreLogIn.getGlobalBoolLoggedIn();
        string _userName = DataStoreLogIn.getGlobalUser();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GetFood);

            TextView vname = FindViewById<TextView>(Resource.Id.NameDisplay);
            TextView vfood = FindViewById<TextView>(Resource.Id.FoodDisplay);

            EditText wineSearch = FindViewById<EditText>(Resource.Id.SearchWine);
            Button button = FindViewById<Button>(Resource.Id.findWine);
            TextView Error = FindViewById<TextView>(Resource.Id.WineError);

            button.Click += async (sender, e) =>
            {
                try
                {
                    string url1 = "http://52.27.116.225:8000/api/food/";
                    string url2 = "http://52.27.116.225:8000/api/user/" + _userName + "/wine/type/" + wineSearch.Text;

                    Console.WriteLine("URL2: {0}", url2);

                    Error.Text = "";
                    vfood.Text = "";

                    JsonValue json1 = await FetchFoodAsync(url1);
                    JsonValue json2 = await FetchWineAsync(url2);

                    int jCount1 = json1.Count;
                    int jCount2 = json2.Count;
                    int jCount3;
                    int realCount = 0;

                    try
                    {

                        for (int i = 0; i < jCount1; i++)
                        {
                            jCount3 = json1[i]["winetype"].Count;
                            for (int j = 0; j < jCount3; j++)
                            {
                                Console.WriteLine("Compatible Foods for Wine Type {0}: {1}", wineSearch.Text, json1[i]["winetype"][j]);
                                try
                                {

                                    if (json1[i]["winetype"][j] == wineSearch.Text)
                                    {
                                        vfood.Text += json1[i]["name"] + ", ";
                                        realCount++;
                                    }
                                }
                                catch(NullReferenceException)
                                {
                                    Error.Text = "There is no wine that matches that type in your inventory.";
                                    vfood.Text = "";
                                }
                                catch (Exception)
                                {
                                    Error.Text = "There is no wine that matches that type in your inventory.";
                                    vfood.Text = "";
                                }



                            }
                        }
                        if (realCount == 0)
                        {
                            Error.Text = "There is no wine that matches that type in your inventory.";
                            vfood.Text = "";
                        }
                    }
                    catch(WebException)
                    {
                        Error.Text = "There was a problem connecting with the API or the network. Try again later.";
                    }
                    catch (NullReferenceException)
                    {
                        Error.Text = "There is no wine that matches that type in your inventory.";
                        vfood.Text = "";
                    }
                }

                //if ((json1[i]["winetype"[j]] == wineSearch.Text && json2[i]["type"] == wineSearch.Text) && json2[i]["username"] == _userName || json2[j]["username"] == _userName)
                //{
                //  Console.WriteLine("Wine Type: {0}, Type Wanted: {1}", json1["winetype"[i]], json2[i]["type"]);
                // vfood.Text += json1[i]["name"] + ", ";

                //}
                catch (NullReferenceException)
                {
                    Error.Text = "There is no wine that matches that type in your inventory or it doesn't have a match yet.";
                    vfood.Text = "";
                }

                catch (WebException)
                {
                    vname.Text = "";
                    vfood.Text = "";
                    Error.Text = "Issue with Connectivity, Try again Later";
                    Console.WriteLine("{0} Caught Exception.", e);
                }

                catch (Exception d)
                {
                    vname.Text = "";
                    vfood.Text = "";
                    Error.Text = "System Error";
                    Console.WriteLine("{0} Caught Exception.", d);
                }

            };
        }

        private async Task<JsonValue> FetchFoodAsync(string url)
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
