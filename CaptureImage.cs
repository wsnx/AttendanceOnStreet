using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using System;
using System.Collections.Generic;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Views;
using Android.Locations;
using System.Xml.Linq;
using System.Linq;
using Android.Util;
using System.Threading.Tasks;
using System.Text;

public static class App
{
    public static File _file;
    public static File _dir;
    public static Bitmap bitmap;
}
//tes
namespace AttendanceOnStreet
{
    [Activity(Label = "Attendance On The Street", Icon = "@drawable/Agility")]
    public class CaptureImage : Activity, ILocationListener
    {
        // removed code for clarity
        //public void OnLocationChanged(Location location) { }
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        private ImageView _imageView;
        TextView txtNIK;
        TextView txtNama;
        string strNama;
        string strNIK;

        static readonly string TAG = "X:" + typeof(CaptureImage).Name;
        TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;
        TextView _locationText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _locationText = FindViewById<TextView>(Resource.Id.location_text);
            _addressText= FindViewById<TextView>(Resource.Id.address_text);

            txtNIK = FindViewById<TextView>(Resource.Id.txtNIP);
            txtNama = FindViewById<TextView>(Resource.Id.txtNama);

            //<----Untuk Jenis Absen Start
            Spinner spinner = FindViewById<Spinner>(Resource.Id.JenisAbsen);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.jenis_absen, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            //---> Untuk Jenis Absen End
            strNama = Intent.GetStringExtra("Nama").ToString();
            strNIK = Intent.GetStringExtra("NIK").ToString();
            txtNIK.Text = strNIK;
            txtNama.Text = strNama;
            Button btnLogout = FindViewById<Button>(Resource.Id.btnLogout);
            btnLogout.Click += BtnLogout_Click;
            InitializeLocationManager();

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                Button button = FindViewById<Button>(Resource.Id.myButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                button.Click += TakeAPicture;
            }
            // Create your application here
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // Make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }
            // Dispose of the Java side bitmap.
            GC.Collect();
            Spinner spinner = FindViewById<Spinner>(Resource.Id.JenisAbsen);
            string jenisabs = spinner.SelectedItem.ToString();

            com.rahmat_faisal.www.Service ws = new com.rahmat_faisal.www.Service();
            ws.updateImagedata(txtNIK.Text.Trim(), jenisabs, ImageToByteArray());
            TextView tvJamCapture = FindViewById<TextView>(Resource.Id.tvJamPhoto);
            tvJamCapture.Text = ws.G_JamPhoto(txtNIK.Text.Trim(), jenisabs).ToString();
        }

        protected void UploadImage(ImageView IVtest)
        {

        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
            Environment.GetExternalStoragePublicDirectory(
            Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
            PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }

        public byte[] ImageToByteArray()
        {
            ImageView view = FindViewById<ImageView>(Resource.Id.imageView1);
            view.DrawingCacheEnabled = true;
            view.BuildDrawingCache();

            Bitmap bm = view.GetDrawingCache(true);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bm.Compress(Bitmap.CompressFormat.Png, 100, stream);
            byte[] byteArray = stream.ToArray();
            return byteArray;
        }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
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
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                _locationText.Text = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
            }
        }

        protected async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

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
                // Remove the last comma from the end of the address.
                _addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
            }
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Jenis Absen {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
            
        }
    }
}