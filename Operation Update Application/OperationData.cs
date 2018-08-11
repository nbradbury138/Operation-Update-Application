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
    /// <summary>
    /// method for holding operation data in an object usable by the application.
    /// </summary>
    class OperationData
    {
        public int JobNo { get; set; }
        public string OperationType { get; set; }
        public string OperationStatus { get; set; }
        public string ResponsibleUser { get; set; }
        public DateTime StartDate { get; set; }
        public int CompletedPercent { get; set; }

        //to string for showing the operation number
        public override string ToString()
        {
            return JobNo.ToString();
        }
    }
}