using System;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using FunctionAppHeartSoundsML;
using FunctionAppHeartSoundsML.DataModels;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionAppHeartSoundsML
{


    public class Startup : FunctionsStartup
    {
        private readonly string _environment;
        private readonly string _modelPath;
        public Startup()
        {
            _environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            if (_environment == "Development")
            {
                _modelPath = ("C:\\Users\\emils\\source\\repos\\FunctionAppHeartSoundsML\\FunctionAppHeartSoundsML\\MLModels\\MLMozip");
            }
            else
            {
                string deploymentPath = @"D:\home\site\wwwroot\";
                _modelPath = Path.Combine(deploymentPath, "MLModels", "MLModel.zip");
            }
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddPredictionEnginePool<HeartData, HeartPrediction>()
                .FromFile(modelName: "HeartModel", filePath: _modelPath, watchForChanges: true);
        }
    }
}

    

