using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace FunctionAppHeartSoundsML.DataModels
{
    public class HeartPrediction: HeartData
    {
        [ColumnName("PredictedLabel")]
        public String Prediction { get; set; }
        public float[] Score { get; set; }

    }
}
