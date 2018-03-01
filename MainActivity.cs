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
using Android.Telephony;
using Android.Support.V4.Content;
using Android;

namespace AttendanceOnStreet
{
    [Activity(Label = "Attendance On The Street", MainLauncher = true, Icon = "@drawable/Agility", NoHistory =true)]
    public class MainActivity : Activity
    {
        string mPhoneNumber = "";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LogIn);
            try
            {
                const string permission = Manifest.Permission.ReadPhoneState;
                if (CheckSelfPermission(permission) == (int)Permission.Granted)
                {
                    Android.Telephony.TelephonyManager tm = (Android.Telephony.TelephonyManager)this.GetSystemService(Android.Content.Context.TelephonyService);
                    mPhoneNumber = tm.Line1Number.ToString();
                    TextView tv = FindViewById<TextView>(Resource.Id.txvMobileNo);
                    tv.Text = "Mobile No : " + mPhoneNumber;

                    //AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                    //AlertDialog alert = dlg.Create();
                    //alert.SetTitle("Login Gagal");
                    //alert.SetMessage(mPhoneNumber);
                    //alert.Show();

                    if (mPhoneNumber != null)
                    {
                        com.rahmat_faisal.www.Service ws = new com.rahmat_faisal.www.Service();
                        string strNIK;
                        strNIK = ws.G_EmployeeID(mPhoneNumber);
                        if (strNIK != "")
                        {
                            EditText txtNIP = FindViewById<EditText>(Resource.Id.edTxNIP);
                            txtNIP.Text = strNIK;

                            EditText txtPwd = FindViewById<EditText>(Resource.Id.txtPWD);
                            txtPwd.RequestFocus();
                        }
                        else
                        {
                            EditText txtNIP = FindViewById<EditText>(Resource.Id.edTxNIP);
                            txtNIP.RequestFocus();
                        }
                    }
                    else
                    {
                        EditText txtNIP = FindViewById<EditText>(Resource.Id.edTxNIP);
                        txtNIP.RequestFocus();
                    }
                }

                
            }
            catch(Exception ex)
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                AlertDialog alert = dlg.Create();
                alert.SetTitle("Login Gagal");
                alert.SetMessage(ex.Message.ToString());
                alert.Show();

                //Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                EditText txtNIP = FindViewById<EditText>(Resource.Id.edTxNIP);
                txtNIP.RequestFocus();
            }
            
            
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            com.rahmat_faisal.www.Service ws = new com.rahmat_faisal.www.Service();
            string hasil;
            
            EditText txtUserID = FindViewById<EditText>(Resource.Id.edTxNIP);
            EditText txtPwd = FindViewById<EditText>(Resource.Id.txtPWD);
            //hasil = ws.CheckLogin(txtUserID.Text.ToString(), txtPwd.Text.ToString());
            hasil = ws.CheckEmployeeID(txtUserID.Text.ToString(), txtPwd.Text.ToString());

            //AlertDialog.Builder dlg = new AlertDialog.Builder(this);
            //AlertDialog alert = dlg.Create();
            //alert.SetTitle("User Login");
            //alert.SetIcon(Resource.Drawable.userLogin);
            //alert.SetMessage(hasil);
            if (hasil.ToLower() != "xxxxxxxxxx")
            {
                //StartActivity(typeof(ListPickList));
                Intent intent = new Intent(this, typeof(MenuUtama));
                intent.PutExtra("NIK", txtUserID.Text.ToString());
                intent.PutExtra("Nama", hasil);
                StartActivity(intent);
            }
            else
            {
                Notification.Builder builder = new Notification.Builder(this)
                            .SetContentTitle("Login Gagal")
                            .SetContentText("NIK atau Password salah")
                            .SetDefaults(NotificationDefaults.Sound)
                            .SetSmallIcon(Resource.Drawable.Notification);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager =
                    GetSystemService(Context.NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);

                AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                AlertDialog alert = dlg.Create();
                alert.SetTitle("Login Gagal");
                alert.SetMessage("NIK atau Password salah");
                alert.Show();
            }
            //alert.Show();
        }        

    }
}

