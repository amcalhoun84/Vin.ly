using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Net;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace Vin.ly
{
    [Activity(Label = "Add Wine")]
    public class PostActivity : Activity
    {
        
        bool _loggedIn = DataStoreLogIn.getGlobalBoolLoggedIn();
        string _username = DataStoreLogIn.getGlobalUser();

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PostWine);

            EditText vname = FindViewById<EditText>(Resource.Id.editWine);
            EditText vtype = FindViewById<EditText>(Resource.Id.editType);
            EditText vregion = FindViewById<EditText>(Resource.Id.editRegion);
            EditText vyear = FindViewById<EditText>(Resource.Id.editYear);

            Button button = FindViewById<Button>(Resource.Id.AddButton);
            Button clrBtn = FindViewById<Button>(Resource.Id.ClearButton);
            TextView vError = FindViewById<TextView>(Resource.Id.AddText);

            string url = "http://52.27.116.225:8000/api/user/" + _username + "/wine/";
            Console.WriteLine("{0}", url);


            button.Click += async (sender, e) =>
            {
                if (vname.Text == "" || vtype.Text == "" || vregion.Text == "" || vyear.Text == "")
                {
                    vError.Text = "No fields can be blank.";
                }
                else
                {
                    JsonValue getJson;

                    try
                    {
                        getJson = await FetchWineAsync(url);
                        int jCount = getJson.Count;

                        for (int i = 0; i < jCount; i++)
                        {
                            if (getJson[i]["name"] == vname.Text && getJson[i]["username"] == _username)
                            {
                                vError.Text = "Wine by that name exists. Try another name. If a similar named wine exists, try a descriptor, such as the year of vintage or type.";
                                return;
                            }

                        }
                    }
                    catch(NullReferenceException k)
                    {
                        Console.WriteLine("There is nothing in this user's inventtory yet, let's just move on. {0}", k);
                    }

                    


                    try
                    {

                        string jsonString = "{ \"name\" : \"" + vname.Text + "\", \"type\" : \"" + vtype.Text + "\", \"region\" : \"" + vregion.Text + "\", \"year\" : \"" + vyear.Text + "\", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Region: {3}, Comment: {4}", url, vname.Text, vtype.Text, vregion.Text, vyear.Text);

                        JsonValue json = await PostWineAsync(url, vname.Text, vtype.Text, vregion.Text, vyear.Text);
                        Console.WriteLine("JsonString: {0}", json);
                        //   Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vvarietal.Text, vregion.Text, vyear.Text);

                        vError.Text = "Wine Added";
                     

                    }
                    catch (WebException d)
                    {
                        string jsonString = "{ \"name\" : \"" + vname.Text + "\", \"type\" : \"" + vtype.Text + "\", \"region\" : \"" + vregion.Text + "\", \"year\" : \"" + vyear.Text + ", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vregion.Text, vyear.Text);

                        Console.WriteLine("JsonString: {0}", jsonString);
                        //   Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vvarietal.Text, vregion.Text, vyear.Text);
                        vError.Text = "Unable to send to API!";
                        Console.WriteLine("Unable to send to API!. Error: {0}", d);
                    }

                    catch (Exception c)
                    {
                        string jsonString = "{ \"name\" : \"" + vname.Text + "\", \"type\" : \"" + vtype.Text + "\", \"region\" : \"" + vregion.Text + "\", \"year\" : \"" + vyear.Text + ", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vregion.Text, vyear.Text);

                        Console.WriteLine("JsonString: {0}", jsonString);
                        // Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vvarietal.Text, vregion.Text, vyear.Text);
                        vError.Text = "System Error";
                        Console.WriteLine("System Error. Error: {0}", c);
                    }

                    //finally
                    //{
                    //    Console.WriteLine("Url: {0}, Name: {1}, Type: {2}, Varietal: {3}, Alcohol Content: {4}, Comment: {5}", url, vname.Text, vtype.Text, vvarietal.Text, vregion.Text, vyear.Text);
                    //    vError.Text = "Something went horribly wrong here.";
                    //    Console.WriteLine("Well, something went horribly wrong here. Error: {0}", e);
                    //}
                }
           };


            clrBtn.Click += clearFields;
        }

        void clearFields(object sender, EventArgs eventArgs)
        {
            EditText vname = FindViewById<EditText>(Resource.Id.editWine);
            EditText vtype = FindViewById<EditText>(Resource.Id.editType);
            EditText vregion = FindViewById<EditText>(Resource.Id.editRegion);
            EditText vyear = FindViewById<EditText>(Resource.Id.editYear);

            vname.Text = "";
            vtype.Text = "";
            vregion.Text = "";
            vyear.Text = "";
                        
        }

        private async Task<JsonValue> FetchWineAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(url));
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

        private async Task<JsonValue> PostWineAsync(string url, string vname, string vtype, string vregion, string vyear)
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
                    string json = "{ \"name\" : \"" + vname + "\", \"type\" : \"" + vtype + "\", \"region\" : \"" + vregion + "\", \"year\" : \"" + vyear + "\", \"username\" : \"" + _username + "\" }";
                    Console.WriteLine("JSON Request: {0}", json);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
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
