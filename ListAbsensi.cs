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
using System.Data;

namespace AttendanceOnStreet
{
    [Activity(Label = "Attendance On The Street", Icon = "@drawable/Agility")]
    public class ListAbsensi : Activity
    {
        TextView txtNIK;
        TextView txtNama;
        string strNama;
        string strNIK;
        ListView myList_Absen;
        List<clsListAbsensi> mItems;
        Spinner spinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListAbsensi);
            // Create your application here
            txtNIK = FindViewById<TextView>(Resource.Id.txtNIP);
            txtNama = FindViewById<TextView>(Resource.Id.txtNama);

            //<----Untuk Jenis Absen Start
            spinner = FindViewById<Spinner>(Resource.Id.bulanke);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.bulanke, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            //---> Untuk Jenis Absen End
            strNama = Intent.GetStringExtra("Nama").ToString();
            strNIK = Intent.GetStringExtra("NIK").ToString();
            txtNIK.Text = strNIK;
            txtNama.Text = strNama;

            TampilkanData();

        }

        private void TampilkanData()
        {
            myList_Absen = FindViewById<ListView>(Resource.Id.myListAbsen);
            com.rahmat_faisal.www.Service myWebService = new com.rahmat_faisal.www.Service();

            int intbulan = 0;
            int inttahun = 0;
            int pilihan = 0;
            pilihan = spinner.SelectedItemPosition;

            switch (pilihan)
            {
                case 0:
                    intbulan = DateTime.Today.Month;
                    inttahun = DateTime.Today.Year;
                    break;
                case 1:
                    intbulan = DateTime.Now.AddMonths(-1).Month;
                    if (DateTime.Today.Year > DateTime.Now.AddMonths(-1).Year)
                    { inttahun = DateTime.Now.AddMonths(-1).Year; }
                    break;
                case 2:
                    intbulan = DateTime.Today.AddMonths(-2).Month;
                    if (DateTime.Today.Year > DateTime.Now.AddMonths(-2).Year)
                    { inttahun = DateTime.Now.AddMonths(-2).Year; }
                    break;
            }

            DataSet ds = myWebService.G_ListAbsensi(txtNIK.Text, intbulan, inttahun);

            mItems = new List<clsListAbsensi>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                mItems.Add(new clsListAbsensi() { Tanggal = ds.Tables[0].Rows[i]["tglabs"].ToString(), Jam_Masuk = ds.Tables[0].Rows[i]["jammasuk"].ToString(), Jam_Keluar = ds.Tables[0].Rows[i]["jamkeluar"].ToString() });
            }

            ListAbsensiAdapter adapListAbsensi = new ListAbsensiAdapter(this, mItems);
            myList_Absen.Adapter = adapListAbsensi;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("List Absen {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
            TampilkanData();
        }

    }
}