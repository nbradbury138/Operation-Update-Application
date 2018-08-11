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
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using Android.Runtime;

namespace Operation_Update_Application
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        SqliteConnection dbConn;
        Data dataBase = new Data();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //check permissions if the permissions are not grnated call the permission method to request access
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted ||
                    ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                CheckPermissions();
            }

            //set content view to the main activity
            SetContentView(Resource.Layout.activity_main);


            var loginButton = FindViewById<Button>(Resource.Id.loginButton);
            // create click method for login button
            loginButton.Click += (object sender, System.EventArgs e) =>
            {
                //return the connection via the setup database method, sets up data if required.
                dbConn = dataBase.SetUpDatabase();

                //get the items associated with the login and user fields
                var password = FindViewById<EditText>(Resource.Id.passWord);
                var user = FindViewById<EditText>(Resource.Id.userName);

                //use a commmand to execute a scalar query. If the result is not 1 then create a Toast Message advising unable to login
                SqliteCommand command = new SqliteCommand(dbConn);
                command.CommandText = "SELECT COUNT(USERNAME) FROM USER WHERE PASSWORD = '" + password.Text + "' and USERNAME = '" + user.Text + "'";

                if (Convert.ToInt32(command.ExecuteScalar()) == 1)
                {
                    var newActivity = new Intent(this, typeof(OperationListActivity));
                    //pass the user details to the next activity
                    newActivity.PutExtra("user", user.Text);
                    StartActivity(newActivity);
                }
                else
                {
                    Toast.MakeText(this, "Failed login, try john and john or recreate database", ToastLength.Long).Show();
                }
            };

            //set up the recreate button and click method to recreate the database with default values.
            var recreateButton = FindViewById<Button>(Resource.Id.recreateDatabase);

            recreateButton.Click += (object sender, System.EventArgs e) =>
            {
                dataBase.RecreateDatabase();
            };

        }

        /// <summary>
        /// check permission method, checks agains the required permissions and calls the request permission method if required.
        /// </summary>
        protected void CheckPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Android.Content.PM.Permission.Granted)
            {
                //ask for permission
                RequestPermissions(new String[] { Android.Manifest.Permission.WriteExternalStorage }, 100);
            }
            else
            {
                //permission already granted
            }

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Android.Content.PM.Permission.Granted)
            {
                //ask for permission
                RequestPermissions(new String[] { Manifest.Permission.ReadExternalStorage }, 100);
            }
            else
            {
                //permission already granted
            }
        }

        /// <summary>
        /// Override the on request permission method to handle the results
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            
            switch (requestCode)
            {
                //if the request code is 100 
                case 100:
                    //for each result
                foreach (int i in grantResults)
                {
                        //check if granted, if not alert the user with a dialog.
                    if (i != (int)Permission.Granted)
                    {
                        Android.App.AlertDialog.Builder permDialog = new Android.App.AlertDialog.Builder(this);
                        permDialog.SetCancelable(false);
                        permDialog.SetMessage("This application requires permissions to be allowed before running. " +
                                "Please re-open the application and select allow.\n" +
                                "If you have previously selected 'Don't Ask Again' you need to go into Settings and accept the permissions manually");
                        permDialog.SetTitle("Permissions not set");

                            //do not allow the user to quit the message and when they hit ok close the app.
                        permDialog.SetPositiveButton("Ok", new EventHandler<DialogClickEventArgs>((s, args) => Finish()));
                        permDialog.Create();
                        permDialog.Show();
                    }
                }

                break;
            }

        }
    }
}

