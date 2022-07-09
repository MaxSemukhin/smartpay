using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using SmartPay.Controllers;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.RecommendationServices;

class CollaborationBasedRecommendations: IRecommendationService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CollaborationBasedRecommendations> _logger;

    public CollaborationBasedRecommendations(ApplicationDbContext db, ILogger<CollaborationBasedRecommendations> logger)
    {
        _db = db;
        _logger = logger;
    }

    
    public async Task<List<Recommendation>> GetRecommendations(HttpContext context)
    {
        var checks = _db.Checks.Include(c => c.Products);
        var userChecks = _db.Checks.Where(c => c.UserId == context.GetUser().Id);

        var products = await _db.Products.ToListAsync();
        
        var mlContext = new MLContext(seed: 0);
        
        var schemaDef = SchemaDefinition.Create(typeof(IrisData));
        schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, products.Count);

        var data = new List<IrisData>();

        foreach (var check in checks)
        {
            var f = new float[products.Count];
            for (var i = 0; i < products.Count; i++)
            {
                var product = products[i];
                if (check.Products.Contains(product)) f[i] = 1f;
                else f[i] = 0f;
            }

            data.Add(new IrisData() { Features = f});
        }
    
        IDataView dataView = mlContext.Data.LoadFromEnumerable<IrisData>(data,schemaDef);
        
        var clustersCount = (int)Math.Round(products.Count * 0.5f);  // Magic number
        _logger.LogDebug("Clusters Count: {c}", clustersCount);

        string featuresColumnName = "Features";
        var pipeline = mlContext.Transforms
            .Concatenate(featuresColumnName, "Features")
            .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: clustersCount));

        var model = pipeline.Fit(dataView);
        
        var predictions = model.Transform(dataView);
        var metrics = mlContext.Clustering.Evaluate(predictions);
        
        _logger.LogDebug("Metrics: {0}", metrics.AverageDistance);

        var predictor = mlContext.Model.CreatePredictionEngine<IrisData, ClusterPrediction>(model, inputSchemaDefinition: schemaDef);
        
        var prediction = predictor.Predict(new IrisData() {Features = new []{1f, 0f, 0f, 0f}});
        
         _logger.LogDebug("Classter: {c}", prediction.PredictedClusterId);
         
         var userChecksPrepared = new List<float[]>();
         var userChecksIds = new List<int>();

         foreach (var check in userChecks)
         {
             var f = new float[products.Count];
             for (var i = 0; i < products.Count; i++)
             {
                 var product = products[i];
                 if (check.Products.Contains(product)) f[i] = 1f;
                 else f[i] = 0f;
             }

             userChecksPrepared.Add(f);
             userChecksIds.Add(check.Id);
         }

         var allFeatures = new List<float[]>();
         var allKeys = new List<float>();

         // Get DataViewSchema of IDataView
         DataViewSchema columns = predictions.Schema;

         // Define variables where extracted values will be stored to
         VBuffer<float> features = default;
         System.UInt32 key = default;
         VBuffer<float> score = default;

         using (DataViewRowCursor cursor = predictions.GetRowCursor(columns))
         {
             // Define delegates for extracting values from columns
             ValueGetter<VBuffer<float>> featuresDelegate = cursor.GetGetter<VBuffer<float>>(columns[0]);
             ValueGetter<System.UInt32> keyDelegate = cursor.GetGetter<System.UInt32>(columns[2]);
             ValueGetter<VBuffer<float>> scoreDelegate = cursor.GetGetter<VBuffer<float>>(columns[3]);

             // Iterate over each row
             while (cursor.MoveNext())
             {
                 //Get values from respective columns
                 featuresDelegate.Invoke(ref features);
                 keyDelegate.Invoke(ref key);
                 scoreDelegate.Invoke(ref score);

                 allFeatures.Add(features.GetValues().ToArray());
                 allKeys.Add(key);
             }
         }

         var recommendations = new List<Recommendation>();

         for (int f = 0; f < allFeatures.Count; f++)
         {
             for (int i = 0; i < userChecksPrepared.Count; i++)
             {
                 if (userChecksPrepared[i].SequenceEqual(allFeatures[f]))
                 {
                     _logger.LogDebug("{i} Found in {n}", i, allKeys[f]);

                     for (var index = 0; index < allKeys.Count; index++)
                     {
                         if (allKeys[index] == allKeys[f])
                         {
                             _logger.LogDebug("{a} Similar to {b}", userChecksPrepared[i], allFeatures[index]);
                             int eqCouter = 0;
                             for (int j = 0; j < userChecksPrepared[i].Length; j++)
                             {
                                 if (userChecksPrepared[i][j] == allFeatures[index][j])
                                 {
                                     eqCouter++;
                                 }
                             }
                             
                             for (int j = 0; j < userChecksPrepared[i].Length; j++)
                             {
                                 if (userChecksPrepared[i][j] == 0 && allFeatures[index][j] > 0)
                                 {
                                     var percent = (float) eqCouter / userChecksPrepared[i].Length;
                                     _logger.LogDebug("I can offer product {p} ({c}%)", products[j].Name, percent * 100);
                                     recommendations.Add(new Recommendation() {Product = products[j], Score = percent});
                                 }
                             }
                         }
                     }

                     break;
                 }
             }
         }

         return recommendations;
    }
}


public class IrisData
{
    [ColumnName("Features")]
    [VectorType(3)]
    public float[] Features;
}

public class ClusterPrediction
{
    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId;

    [ColumnName("Score")]
    public float[] Distances;
}

