using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using FunctionAppHeartSoundsML.DataModels;

namespace FunctionAppHeartSoundsML
{
    public class HeartSounds
    {
        [FunctionName("HeartSounds")]
        
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Parse HTTP Request Body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            HeartData data = JsonConvert.DeserializeObject<HeartData>(requestBody);

            //Make Prediction
            //var predictionResult = ConsumerModel.Predict(data);

            HeartPrediction prediction = _predictionEnginePool.Predict(modelName: "HeartModel", example: data);

            Debug.WriteLine("WTF");
    

            //Convert prediction to string
            //TODO udkommenteres fordi vores prediction er en string og ikke en bool.
            //string sentiment = Convert.ToBoolean(prediction.Prediction) ? "Healthy heart" : "Unhealthy Heart";
            string sentiment = prediction.Prediction;

            //Return Prediction
            return (ActionResult)new OkObjectResult(sentiment);
        }

        private readonly PredictionEnginePool<HeartData, HeartPrediction> _predictionEnginePool;

        // AnalyzeSentiment class constructor
        public HeartSounds(PredictionEnginePool<HeartData, HeartPrediction> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
    }
    
}
