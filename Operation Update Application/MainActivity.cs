using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Util;
using Android.Content;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

namespace Operation_Update_Application
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{



        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            Data dataBase = new Data();

            CheckPermissions();
            SqliteConnection dbConn = dataBase.SetUpDatabase();

			SetContentView(Resource.Layout.activity_main);

           var loginButton = FindViewById<Button>(Resource.Id.loginButton);

           loginButton.Click += (object sender, System.EventArgs e) =>
           {
               var password = FindViewById<EditText>(Resource.Id.passWord);
               var user = FindViewById<EditText>(Resource.Id.userName);

               SqliteCommand command = new SqliteCommand(dbConn);
               command.CommandText = "SELECT COUNT(USERNAME) FROM USER WHERE PASSWORD = '" + password.Text + "' and USERNAME = '" + user.Text + "'";

               if (Convert.ToInt32(command.ExecuteScalar()) == 1)
               {
                   var newActivity = new Intent(this, typeof(OperationListActivity));
                   newActivity.PutExtra("user", user.Text);
                   StartActivity(newActivity);
               }
               else
               {
                   Toast.MakeText(this, "Failed login, try john and john or recreate database",ToastLength.Long).Show();                 
               }
           };

            var recreateButton = FindViewById<Button>(Resource.Id.recreateDatabase);

            recreateButton.Click += (object sender, System.EventArgs e) =>
            {
                dataBase.RecreateDatabase();
            };

        }


        protected void CheckPermissions()
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
            {
                // Permission has never been accepted
                // So, I ask the user for permission
                RequestPermissions(new String[] { Android.Manifest.Permission.WriteExternalStorage },0);
            }
            else
            {
                // Permission has already been accepted previously
            }

            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) != (int)Android.Content.PM.Permission.Granted)
            {
                // Permission has never been accepted
                // So, I ask the user for permission
                RequestPermissions(new String[] { Android.Manifest.Permission.ReadExternalStorage }, 0);
            }
            else
            {
                // Permission has already been accepted previously
            }
        }
    }
}

