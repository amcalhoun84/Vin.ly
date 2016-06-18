using System;
using System.Text;
using System.Linq;
using Android.Content;
using System.Collections.Generic;
using Android.App;
using System.Threading.Tasks;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Util;

namespace Vin.ly
{
    [Activity(Label = "Vin.ly", MainLauncher = false, Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            // Get our button from the layout resource,
            // and attach an event to it

            TextView userGet = FindViewById<TextView>(Resource.Id.VinlyUser);
            TextView _locationText = FindViewById<TextView>(Resource.Id.VinlyLocale);

            Button wineList = FindViewById<Button>(Resource.Id.ListButton);
            // Button namedWine = FindViewById<Button>(Resource.Id.NameButton);
            Button typedWine = FindViewById<Button>(Resource.Id.TypeButton);
            Button regionWine = FindViewById<Button>(Resource.Id.RegionButton);
            Button yearWine = FindViewById<Button>(Resource.Id.YearButton);
            Button editWine = FindViewById<Button>(Resource.Id.EditButton);
            Button postWine = FindViewById<Button>(Resource.Id.AddButton);
            Button getLocation = FindViewById<Button>(Resource.Id.GPSButton);
            Button getPairing = FindViewById<Button>(Resource.Id.getPairing);
            Button logOutBtn = FindViewById<Button>(Resource.Id.LogOutButton);

            wineList.Click += GoToWineList;
            // namedWine.Click += GoToWineName;
            typedWine.Click += GoToWineType;
            regionWine.Click += GoToWineRegion;
            yearWine.Click += GoToWineYear;
            editWine.Click += GoToEditWine;
            postWine.Click += GoToPostWine;
            getLocation.Click += GoToLocationGPS;
            getPairing.Click += GoToPairing;
            logOutBtn.Click += LogOff;


            // Console.WriteLine("User name check; {0}", DataStoreLogIn.getGlobalUser());

            userGet.Text = "Welcome " + DataStoreLogIn.getGlobalUser() + "!";

        }

        void GoToWineList(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(WineList)));
        }

        void GoToWineYear(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(WineYear)));
        }

        void GoToWineType(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(WineType)));
        }

        void GoToWineRegion(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(WineRegion)));
        }

        void GoToEditWine(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(EditWine)));
        }

        void GoToPostWine(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(PostActivity)));
        }

        void GoToLocationGPS(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(VinlyGPS)));
        }

        void GoToPairing(object sender, EventArgs eventArgs)
        {
            StartActivity(new Intent(Application.Context, typeof(GetFood)));
        }

        void LogOff(object sender, EventArgs eventArgs)
        {
            DataStoreLogIn.setGlobalUser(null);

            StartActivity(new Intent(Application.Context, typeof(LogInActivity)));
            
        }

    }
}

