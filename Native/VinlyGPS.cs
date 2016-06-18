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
    [Activity(Label = "Vinly GPS", MainLauncher = false, Icon = "@drawable/icon")]
    public class VinlyGPS : Activity, ILocationListener
    {

        static readonly string TAG = "Locale:" + typeof(VinlyGPS).Name;
        TextView _locationAddress;
        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;
        TextView _locationText;


        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location. Plese try again shortly.";
            }

            else
            {
                _locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
            }
        }

        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(TAG, "{0}, {1}", provider, status);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.VinGPS);

            InitializeLocationManager();
            _locationAddress = FindViewById<TextView>(Resource.Id.addyText);
            _locationText = FindViewById<TextView>(Resource.Id.locText);

            FindViewById<TextView>(Resource.Id.ThirdButton).Click += AddressButton_OnClick;

        }

    
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria { Accuracy = Accuracy.Fine };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }

            else
            {
                _locationProvider = string.Empty;
            }

            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            Log.Debug(TAG, "Listening for location updates using " + _locationProvider + ".");
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
            Log.Debug(TAG, "No longer listening for location updates.");
        }

        async void AddressButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                _locationAddress.Text = "Cannot determine current address. Try again.";
                return;
            }

            Address address = await ReverseGeocodeCurrentLocation();
            DisplayAddress(address);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList = await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }

                // remove any commas from the end of the address.
                _locationAddress.Text = deviceAddress.ToString();
            }

            else
            {
                _locationAddress.Text = "Unable to find the address. Try again later.";
            }
        }
    }
}

