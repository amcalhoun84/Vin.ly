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
    [Activity(Label = "Edit Wine")]
    public class EditWine : Activity
    {

        bool _loggedIn = DataStoreLogIn.getGlobalBoolLoggedIn();
        string _username = DataStoreLogIn.getGlobalUser();
        // this class will be used to edit and delete wines.

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditWine);

            TextView vError = FindViewById<TextView>(Resource.Id.WineError);

            EditText vFindName = FindViewById<EditText>(Resource.Id.SearchWine);
            EditText editName = FindViewById<EditText>(Resource.Id.editWine);
            EditText editType = FindViewById<EditText>(Resource.Id.editType);
            EditText editRegion = FindViewById<EditText>(Resource.Id.editRegion);
            EditText edYear = FindViewById<EditText>(Resource.Id.editYear);
            TextView vUpdate = FindViewById<TextView>(Resource.Id.UpdateText);

            Button vFindBtn = FindViewById<Button>(Resource.Id.findWine);
            Button vUpdateBtn = FindViewById<Button>(Resource.Id.UpdateButton);
            Button vDeleteBtn = FindViewById<Button>(Resource.Id.DeleteButton);


            string url = "http://52.27.116.225:8000/api/user/" + _username + "/wine/";
            Console.Out.WriteLine("String: {0}", url);


            vFindBtn.Click += async (sender, e) =>
            {
                if (vFindName.Text == "")
                {
                    vError.Text = "Search Field cannot be blank";
                }
                else
                {

                    editName.Text = "";
                    editType.Text = "";
                    editRegion.Text = "";
                    edYear.Text = "";

                    try
                    {
                        JsonValue json = await FetchWineAsync(url);

                        int jCount = json.Count;

                        for (int i = 0; i < jCount; i++)
                        {
                            if (json[i]["name"] == vFindName.Text && json[i]["username"] == _username)
                            {
                                int count = 0;

                                count++;

                                try
                                {
                                    vError.Text = "Matching Wine Found";
                                    //Console.Out.WriteLine("Listings: {0}", json[i]["name"]);
                                    editName.Text = json[i]["name"];
                                    editType.Text = json[i]["type"];
                                    editRegion.Text = json[i]["region"];
                                    edYear.Text = json[i]["year"];
                                    break;

                                }

                                catch (System.Collections.Generic.KeyNotFoundException k)
                                {
                                    if (editName.Text == null)
                                        editName.Text = "";
                                    if (editType.Text == null)
                                        editType.Text = "";
                                    if (editRegion.Text == null)
                                        editRegion.Text = "";
                                    if (edYear.Text == null)
                                        edYear.Text = "";

                                    Console.WriteLine("Key not found. {0}", k);

                                }

                            }
                            else
                            {
                                vError.Text = "A wine matching that name and year was not found.";
                            }

                        }


                    }

                    catch (WebException)
                    {
                        vUpdate.Text = "System Web Error. Please try again later.";
                    }
                    catch (NullReferenceException)
                    {
                        vUpdate.Text = "Ooops! Looks like you need to add some wines";
                    }
                }
            };
            

            vUpdateBtn.Click += async (sender, e) =>
            {
               
                if (editName.Text == "" || editType.Text == "" || editRegion.Text == "" || edYear.Text == "")
                {
                    vError.Text = "No fields can be blank.";
                }
                else
                {
                    try
                    {
                        string jsonString = "{ \"name\" : \"" + editName.Text + "\", \"type\" : \"" + editType.Text + "\", \"region\" : \"" + editRegion.Text + "\", \"year\" : \"" + edYear.Text + "\", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("Json String: {0}", jsonString);
                        Console.WriteLine("URL: {0}, Name: {1}, Type: {2}, Region: {3}, Year: {4}", url, editName.Text, editType.Text, editRegion.Text, edYear.Text);

                        JsonValue json = await PutWineAsync(url, editName.Text, editType.Text, editRegion.Text, edYear.Text);


                        vUpdate.Text = "Wine Updated.";
                    }
                    catch (WebException w)
                    {
                        string jsonString = "{ \"name\" : \"" + editName.Text + "\", \"type\" : \"" + editType.Text + "\", \"region\" : \"" + editRegion.Text + "\", \"year\" : \"" + edYear.Text + "\", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("URL: {0}, Name: {1}, Type: {2}, Region: {3}, Year: {4}", url, editName.Text, editType.Text, editRegion.Text, edYear.Text);

                        Console.WriteLine("Error on the Web, {0}", w);
                        vUpdate.Text = "Issue communicating with the server, please try again later.";
                    }
                    catch (Exception d)
                    {
                        string jsonString = "{ \"name\" : \"" + editName.Text + "\", \"type\" : \"" + editType.Text + "\", \"region\" : \"" + editRegion.Text + "\", \"year\" : \"" + edYear.Text + "\", \"username\" : \"" + _username + "\" }";
                        Console.WriteLine("URL: {0}, Name: {1}, Type: {2}, Region: {3}, Year: {4}", url, editName.Text, editType.Text, editRegion.Text, edYear.Text);

                        Console.WriteLine("Error in the program, {0}", d);
                        vUpdate.Text = "Error found in input or interpreter, please contact the developer at heliosprod@me.com.";
                    }



                }

            };


            vDeleteBtn.Click += async (sender, e) =>
            {
                try
                {
                    JsonValue json = await DeleteWineAsync(url, vFindName.Text);
                    vUpdate.Text = "Wine deleted.";
                }

                catch(WebException w)
                {
                    vError.Text = "Web Error or API Connection Issue, please try again later.";
                    Console.WriteLine("Error: {0}", w);
                }

                catch(Exception d)
                {
                    vError.Text = "Internal system error. Please notify the developer at heliosprod@me.com";
                    Console.WriteLine("Error: {0}", d);
                }
            };
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

        private async Task<JsonValue> PutWineAsync(string url, string editName, string editType, string editRegion, string editYear)
        {
            EditText _vfind = FindViewById<EditText>(Resource.Id.SearchWine);

            var connectionManager = (ConnectivityManager)(Application.Context.ApplicationContext).GetSystemService(ConnectivityService);
            var activeConnection = connectionManager.ActiveNetworkInfo;
            var isConnected = activeConnection != null && activeConnection.IsConnected;

            if(isConnected)
            {
                url += _vfind.Text;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "PUT";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{ \"name\" : \"" + editName + "\", \"type\" : \"" + editType + "\", \"region\" : \"" + editRegion + "\", \"year\" : \"" + editYear + "\", \"username\" : \"" + _username + "\" }";
                    Console.WriteLine("JSON Request: {0}, {1}", url, json);
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


        private async Task<JsonValue> DeleteWineAsync(string url, string deleteName)
        {
            string newUrl = url + deleteName;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(newUrl));
            request.ContentType = "application/json";
            request.Method = "DELETE";

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

