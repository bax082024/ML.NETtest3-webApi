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
        var mlContext = new MLContext();  

        
        ITransformer trainedModel = mlContext.Model.Load("Models/customerClusteringModel.zip", out var modelSchema);

        _predictionEngine = mlContext.Model.CreatePredictionEngine<CustomerData, ClusterPrediction>(trainedModel);
    }

    
    [HttpPost]
    public ActionResult<string> PredictCluster([FromBody] CustomerData customerData)
    {
       
        var prediction = _predictionEngine.Predict(customerData);
        return Ok($"Customer belongs to cluster: {prediction.PredictedClusterId}");
    }
}


public class CustomerData
{
    public float Age { get; set; }
    public float Salary { get; set; }
}


public class ClusterPrediction
{
    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId { get; set; }
}
