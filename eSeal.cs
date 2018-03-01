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
using System.Net;
using System.IO;

namespace AttendanceOnStreet
{
    [Activity(Label = "eSeal")]
    public class eSeal : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.eSeal);
            // Create your application here
            Button btn = FindViewById<Button>(Resource.Id.btnEseal);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("http://server.m2fleet.co.id/webservicescript/16/WS02?user=wstester&pass=wstester&id={0}&Shipment1={1}&Vehicle={2}&Driver={3}&DONumber={4}&JobNumber={5}&AjuNumber={6}&PickUpFrom={7}&Container={8}&Delivery={9}", this.DropDownList1.SelectedValue, this.txtShipment1.Text, this.txtNoMobil.Text, this.txtSupir.Text, this.txtNoDO.Text, this.txtNoJon.Text, this.txtNoAJU.Text, this.txtPickUpDari.Text, this.txtNoContainer.Text, DateTime.Now);
            sb.AppendFormat("http://server.m2fleet.co.id/webservicescript/16/WS02?user=wstester&pass=wstester&id={0}&Shipment1={1}&Vehicle={2}&Driver={3}&DONumber={4}&JobNumber={5}&AjuNumber={6}&PickUpFrom={7}&Container={8}&Delivery={9}", "7560503902", "Shipment Wisnu", "NoMobil Wisnu", "SupirWisnu", "NoDO Wisnu", "No JOB Wisnu", "No AJU Wisnu", "Pick Up Dari Wisnu", "NoContainer Wisnu", DateTime.Now);
            HttpWebRequest request = WebRequest.Create(sb.ToString()) as HttpWebRequest;
            //optional
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
        }
    }
}