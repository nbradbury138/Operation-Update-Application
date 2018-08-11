using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace Operation_Update_Application
{
    [Activity(Label = "OperationDetailsActivity")]
    public class OperationDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set content view to the operation details activity
            SetContentView(Resource.Layout.activity_operation_details);

            //deserialise the json object into an OperationData object
            OperationData operation = JsonConvert.DeserializeObject<OperationData>(Intent.GetStringExtra("selectedItem"));

            //grab all the activitities views to populate data
            var opNumber = FindViewById<TextView>(Resource.Id.opNumber);
            var opType = FindViewById<Spinner>(Resource.Id.opTypeSpinner);
            var startDate = FindViewById<EditText>(Resource.Id.startDateText);
            var opStatus = FindViewById<Spinner>(Resource.Id.opStatusSpinner);
            var percentComplete = FindViewById<EditText>(Resource.Id.percentCompleteText);


            //use the operation objects to populate the listview with an array adapter.
            string[] statusList = new string[3];
            statusList[0] = "Planned";
            statusList[1] = "Current";
            statusList[2] = "Complete";

            string[] typeList = new string[3];
            typeList[0] = "Burning";
            typeList[1] = "Seeding";
            typeList[2] = "Harvesting";

            //set the adapters for the status and type spinners
            ArrayAdapter adapterStatus = new ArrayAdapter<String>(this, Resource.Layout.spinner_text_layout,statusList);
            opStatus.Adapter = adapterStatus;

            ArrayAdapter adapterType = new ArrayAdapter<String>(this, Resource.Layout.spinner_text_layout, typeList);
            opType.Adapter = adapterType;


            //populate data with the operation data
            opNumber.Text = operation.JobNo.ToString();
            opType.SetSelection(GetIndexType(operation.OperationType));
            opStatus.SetSelection(GetIndexStatus(operation.OperationStatus));
            startDate.Text = operation.StartDate.ToString("dd/MM/yyyy");
            percentComplete.Text = operation.CompletedPercent.ToString();

     
            //placeholder to finish the activity on click, need to change this to save changes to the data row.
            var saveButton = FindViewById<Button>(Resource.Id.saveButton);

            saveButton.Click += (object sender, System.EventArgs e) =>
            {
                //add code to save to database before finishing.
                Data db = new Data();
                SqliteConnection dbConn = db.SetUpDatabase();
                SqliteCommand command = new SqliteCommand(dbConn);

                //command used to update the database
               command.CommandText = "update OPERATION set START_DATE = '"+ DateTime.ParseExact(startDate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd") + "', OPERATION_TYPE = (SELECT OPERATION_TYPE_CODE FROM LU_OPERATION_TYPE WHERE OPERATION_TYPE_DESCRIPTION ='" + opType.SelectedItem.ToString() + "')," +
                "OPERATION_STATUS = (SELECT OPERATION_STATUS_CODE FROM LU_OPERATION_STATUS WHERE OPERATION_STATUS_DESCRIPTION = '" + opStatus.SelectedItem.ToString() + "'), OPERATION_COMPLETION_PERCENTAGE = " + Convert.ToInt32(percentComplete.Text) + 
                " WHERE JOB_NO = '" + opNumber.Text + "'";

                command.ExecuteNonQuery();

                //indicate with a toast message that it has been successful
                Toast.MakeText(this, "Data Saved Successfully", ToastLength.Long).Show();
                //finish the activity.
                Finish();
            };

        }

        /// <summary>
        /// Method used to return an index number depending on the status code
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int GetIndexStatus(string value)
        {
            if (value.Equals("PL"))
            {
                return 0;
            }
            else if (value.Equals("CU"))
            {
                return 1;
            }
            else if (value.Equals("CO"))
            {
                return 2;
            }
            else
                return 0;
        }

        /// <summary>
        /// Method used to return an index number depending on the type code.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int GetIndexType(string value)
        {
            if (value.Equals("BURN"))
            {
                return 0;
            }
            else if (value.Equals("SEED"))
            {
                return 1;
            }
            else if (value.Equals("HARV"))
            {
                return 2;
            }
            else
                return 0;
        }
    }
    
}