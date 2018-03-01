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
    class ListAbsensiAdapter : BaseAdapter
    {
        public List<clsListAbsensi> mItems;
        Context mcontext;

        public ListAbsensiAdapter(Context context, List<clsListAbsensi> items)
        {
            mItems = items;
            mcontext = context;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public clsListAbsensi this [int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            //SKUListViewAdapterViewHolder holder = null;

            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.absensi_row, null, false);
            }

            //fill in your items
            //holder.Title.Text = "new text here";
            TextView tvTgl = row.FindViewById<TextView>(Resource.Id.tvTgl);
            tvTgl.Text = mItems[position].Tanggal;

            TextView tvJamMasuk = row.FindViewById<TextView>(Resource.Id.tvJamMasuk);
            tvJamMasuk.Text = mItems[position].Jam_Masuk;

            TextView tvJamKeluar = row.FindViewById<TextView>(Resource.Id.tvJamKeluar);
            tvJamKeluar.Text = mItems[position].Jam_Keluar;            

            return row;
        }

    }

    class ListAbsensiAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}