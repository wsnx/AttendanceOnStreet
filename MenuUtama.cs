using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AttendanceOnStreet
{
    [Activity(Label = "Attendance On The Street", Icon = "@drawable/Agility")]
    public class MenuUtama : Activity
    {
        ListView MyListvw;
        string strNIK;
        string strNama;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuUtama);
            strNama = Intent.GetStringExtra("Nama").ToString();
            strNIK = Intent.GetStringExtra("NIK").ToString();

            MyListvw = FindViewById<ListView>(Resource.Id.myListView);

            // Create your application here
            List<string> itemsNew;
            itemsNew = new List<string>();
            
            itemsNew.Add("Absen");
            itemsNew.Add("List Absensi");
            itemsNew.Add("Standing Meeting");
            itemsNew.Add(" ");
            itemsNew.Add("Logout");
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, itemsNew);
            MyListvw.Adapter = adapter;

            MyListvw.ItemClick += MyListvw_ItemClick;
        }

        private void MyListvw_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            string strItem = MyListvw.GetItemAtPosition(e.Position).ToString();
            Intent intent;
            switch (strItem)
            {
                case "Absen":                    
                    intent = new Intent(this, typeof(CaptureImage));
                    intent.PutExtra("NIK", strNIK);
                    intent.PutExtra("Nama", strNama);
                    StartActivity(intent);
                    break;
                case "List Absensi":
                    intent = new Intent(this, typeof(ListAbsensi));
                    intent.PutExtra("NIK", strNIK);
                    intent.PutExtra("Nama", strNama);
                    StartActivity(intent);
                    break;
                case "Standing Meeting":
                    intent = new Intent(this, typeof(eSeal));
                    StartActivity(intent);
                    break;
                case "Logout":
                    intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    break;
            }

            

            //AlertDialog.Builder dlg = new AlertDialog.Builder(this);
            //AlertDialog alert = dlg.Create();
            //alert.SetTitle("Testing");
            //alert.SetMessage(strSONo);
            //alert.Show();

            
        }
    }
}