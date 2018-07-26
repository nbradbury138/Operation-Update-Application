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

namespace Operation_Update_Application
{

    [Activity(Label = "OperationListActivity")]
    public class OperationListActivity : ListActivity
    {
        OperationData[] opData = new OperationData[2];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            

            opData[0] = new OperationData();
            opData[1] = new OperationData();

            opData[0].OperationId = 479105;
            opData[0].CompletedPercent = 75;
            opData[0].OperationType = "Jones";
            opData[1].OperationId = 489753;
            opData[1].CompletedPercent = 60;
            opData[1].OperationType = "Henry";

            ListAdapter = new ArrayAdapter<OperationData>(this, Resource.Layout.activity_operation_list, Resource.Id.opList, opData);          

        }
    }
}