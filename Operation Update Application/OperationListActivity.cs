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

    [Activity(Label = "OperationListActivity")]
    public class OperationListActivity : ListActivity
    {
        List<OperationData> opData;
        string username;
        string opStatusString;
        ListView oplistView;
        ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set content view as the operation list
            SetContentView(Resource.Layout.activity_operation_list);

            //get username from intent
            username = Intent.GetStringExtra("user");

            //set the status list options
            string[] statusList = new string[3];
            statusList[0] = "Planned";
            statusList[1] = "Current";
            statusList[2] = "Complete";

            opStatusString = statusList[0];

            //load the list of operations
            LoadList();
            
            //set the filter spinner according to the status list created earlier
            Spinner opStatus = (Spinner)FindViewById(Resource.Id.filter);
            ArrayAdapter adapterFilter = new ArrayAdapter<string>(this, Resource.Layout.spinner_text_layout, statusList);
            opStatus.Adapter = adapterFilter;

            //if the filter is changed set the vlaue and reload the list according to the filter value
            opStatus.ItemSelected += (object sender, Spinner.ItemSelectedEventArgs e) =>
            {
                opStatusString = statusList[e.Position];
                LoadList();
            };


            //when an item is selected, serialise the current operation object and send through to next activity via intent.
            oplistView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var newActivity = new Intent(this, typeof(OperationDetailsActivity));
                newActivity.PutExtra("selectedItem", JsonConvert.SerializeObject(opData[e.Position]));
                StartActivity(newActivity);
            };
        }

        protected override void OnResume()
        {
            //on resume activity reload the list of data in case there have been changes
            base.OnResume();
            LoadList();
        }

        /// <summary>
        /// loads the list of operations given the user and the current filter status.
        /// </summary>
        private void LoadList()
        {
            //use the operation objects to populate the listview with an array adapter.
            opData = new List<OperationData>();
            //connect to database
            Data db = new Data();
            SqliteConnection dbConn = db.SetUpDatabase();
            SqliteCommand command = new SqliteCommand(dbConn);
            //select all oeprations for user
            command.CommandText = "SELECT * FROM OPERATION WHERE RESPONSIBLE_USER ='" + username + 
                "' AND OPERATION_STATUS = (SELECT OPERATION_STATUS_CODE FROM LU_OPERATION_STATUS WHERE OPERATION_STATUS_DESCRIPTION = '" + opStatusString + "')";
            //load into datatable
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            //loop through and add into list of Operation Data objects
            foreach (DataRow row in dt.Rows)
            {
                OperationData newItem = new OperationData();
                newItem.JobNo = Convert.ToInt32(row[0]);
                newItem.StartDate = Convert.ToDateTime(row[1]);
                newItem.OperationType = Convert.ToString(row[2]);
                newItem.OperationStatus = Convert.ToString(row[3]);
                newItem.ResponsibleUser = Convert.ToString(row[4]);
                newItem.CompletedPercent = Convert.ToInt32(row[5]);
                opData.Add(newItem);
            }

            //set the adapter to the data for the list.
            oplistView = (ListView)FindViewById(Android.Resource.Id.List);
            adapter = new ArrayAdapter<OperationData>(this, Resource.Layout.spinner_text_layout, opData);
            oplistView.Adapter = adapter;

        }

    }
}