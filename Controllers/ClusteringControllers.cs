using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;

[Route("api/clustering")]
[ApiController]
public class ClusteringController : ControllerBase
{
    private readonly PredictionEngine<CustomerData, ClusterPrediction> _predictionEngine;

    public ClusteringController()
    {
        var mlContext = new MLContext();  // Initialize MLContext

        // Load the trained model
        ITransformer trainedModel = mlContext.Model.Load("Models/customerClusteringModel.zip", out var modelSchema);

        // Create a prediction engine from the trained model
        _predictionEngine = mlContext.Model.CreatePredictionEngine<CustomerData, ClusterPrediction>(trainedModel);
    }

    // POST: api/clustering
    [HttpPost]
    public ActionResult<string> PredictCluster([FromBody] CustomerData customerData)
    {
        // Make a prediction using the prediction engine
        var prediction = _predictionEngine.Predict(customerData);
        return Ok($"Customer belongs to cluster: {prediction.PredictedClusterId}");
    }
}

// Define the input data class
public class CustomerData
{
    public float Age { get; set; }
    public float Salary { get; set; }
}

// Define the prediction output class
public class ClusterPrediction
{
    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId { get; set; }
}
