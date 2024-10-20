using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;

[Route("api/controller")]
[ApiController]

public class ClusteringController : ControllerBase
{
  private readonly PredictionEngine<CustomerData, ClusterPrediction> _predictionEngine;

  public ClusteringController()
  {
    var mlContext = new MLContext();
    // Load model
    ITransformer trainedModel = mlContext.Model.Load("Models/customerClusteringModel.Zip", out var modelSchema);
  }
  
  //Prediction Engine
  _predictionEngine = MLContext.Model.CreatePredictionEngine<CustomerData, ClusterPrediction>(trainedModel);

  [HttpPost]
  public ActionResult<string> PredictCluster([FromBody] CustomerData customerData)
  {
    var prediction = _predictionEngine.Predict(customerData);
    return Ok($"Customer belongs to cluster: {prediction.PredictedClusterId}");
  }
} 
