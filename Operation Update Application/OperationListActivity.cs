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
        List<OperationData> opData = new List<OperationData>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_operation_list);

            string username = Intent.GetStringExtra("user");
            int i = 0;

            Data db = new Data();
            SqliteConnection dbConn = db.SetUpDatabase();
            SqliteCommand command = new SqliteCommand(dbConn);

            command.CommandText = "SELECT * FROM OPERATION WHERE RESPONSIBLE_USER ='" + username +"'";

            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
        
            foreach(DataRow row in dt.Rows)
            {
                OperationData newItem = new OperationData();
                newItem.JobNo = Convert.ToInt32(row[0]);
                newItem.StartDate = Convert.ToDateTime(row[1]);
                newItem.OperationType = Convert.ToString(row[2]);
                newItem.OperationStatus = Convert.ToString(row[3]);
                newItem.ResponsibleUser = Convert.ToString(row[4]);
                newItem.CompletedPercent = Convert.ToInt32(row[5]);
                i++;
                opData.Add(newItem);
            }


            ListView oplistView = (ListView)FindViewById(Android.Resource.Id.List);    
            
            ArrayAdapter adapter = new ArrayAdapter<OperationData>(this, Android.Resource.Layout.SimpleListItem1, opData);

            oplistView.Adapter = adapter;

            oplistView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var newActivity = new Intent(this, typeof(OperationDetailsActivity));
                newActivity.PutExtra("selectedItem",JsonConvert.SerializeObject(opData[e.Position]));
                StartActivity(newActivity);
            };
        }
    }
}