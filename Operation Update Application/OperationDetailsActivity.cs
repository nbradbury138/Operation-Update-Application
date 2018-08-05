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
using Newtonsoft.Json;

namespace Operation_Update_Application
{
    [Activity(Label = "OperationDetailsActivity")]
    public class OperationDetailsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_operation_details);

            OperationData operation = JsonConvert.DeserializeObject<OperationData>(Intent.GetStringExtra("selectedItem"));

            var opNumber = FindViewById<TextView>(Resource.Id.opNumber);
            var opType = FindViewById<Spinner>(Resource.Id.opTypeSpinner);
            var startDate = FindViewById<EditText>(Resource.Id.startDateText);
            var opStatus = FindViewById<Spinner>(Resource.Id.opStatusSpinner);
            var percentComplete = FindViewById<EditText>(Resource.Id.percentCompleteText);

            opNumber.Text = operation.JobNo.ToString();
            opType.SetSelection(GetIndexType(operation.OperationType));
            opStatus.SetSelection(GetIndexStatus(operation.OperationStatus));
            startDate.Text = operation.StartDate.ToString("dd/MM/yy");
            percentComplete.Text = operation.CompletedPercent.ToString();


            var saveButton = FindViewById<Button>(Resource.Id.saveButton);

            saveButton.Click += (object sender, System.EventArgs e) =>
            {
                //add code to save to database before finishing.

                Finish();
            };

        }

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