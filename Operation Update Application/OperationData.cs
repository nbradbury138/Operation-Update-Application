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
        public int OperationId { get; set; }
        public string OperationType { get; set; }
        public int CompletedPercent { get; set; }

        public override string ToString()
        {
            return OperationId.ToString();
        }
    }
}