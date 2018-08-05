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
    class OperationData
    {
        public int JobNo { get; set; }
        public string OperationType { get; set; }
        public string OperationStatus { get; set; }
        public string ResponsibleUser { get; set; }
        public DateTime StartDate { get; set; }
        public int CompletedPercent { get; set; }

        public override string ToString()
        {
            return JobNo.ToString();
        }
    }
}