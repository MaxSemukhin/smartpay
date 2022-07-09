using System.Linq.Expressions;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecomendationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<RecomendationsController> _logger;

    public RecomendationsController(ApplicationDbContext db, ILogger<RecomendationsController> logger)
    {
        _db = db;
        _logger = logger;
    }

    private async Task<List<Recommendation>> ContentBased()
    {
        var checks = _db.Checks.Where(c => c.UserId == HttpContext.GetUser().Id);
        var allProducts = checks.SelectMany(c => c.Products);
        var counted = await allProducts
            .GroupBy(p => p.Id)
            .Select(y => new
            {
                Element = y.Key,
                Counter = y.Count(),
            }).OrderByDescending(y => y.Counter).ToListAsync();;

        var countedWithSimilar = counted.Select(c => new
        {
            Element = c.Element,
            Counter = c.Counter,
            Similar = _db.Products
                .Where(p => p.CategoryId == _db.Products.First(pr => pr.Id == c.Element).CategoryId)
                .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Contains(p)).Count() } )
                
                .Concat(_db.Products
                    .Where(p => p.Category.CategoryId == _db.Products.First(pr => pr.Id == c.Element).Category.CategoryId)
                    .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Contains(p)).Count() / 2 } )
                )
                
                .Concat(_db.Products
                    .Where(p => p.MerchantId == _db.Products.First(pr => pr.Id == c.Element).MerchantId)
                    .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Contains(p)).Count() / 4 } )
                )
                
                .OrderByDescending(p => p.Popularity)
        });

        var unOrdered = countedWithSimilar.SelectMany(c => c.Similar);

        var maxCounter = unOrdered.MaxBy(c => c.Counter).Counter;
        var minCounter = unOrdered.MinBy(c => c.Counter).Counter;
        var cm = maxCounter - minCounter;
        
        var maxPopularity = unOrdered.MaxBy(c => c.Popularity).Popularity;
        var minPopularity = unOrdered.MinBy(c => c.Popularity).Popularity;
        var pm = maxPopularity - minPopularity;
        
        var results = unOrdered
            .Select(c => new { Product = c.Product, Score = cm != 0 ? c.Counter / cm : 0 + pm > 0 ? c.Popularity / pm : 0 })
            .DistinctBy(c => c.Product.Id)
            .OrderByDescending(c => c.Score).Take(10).ToList();

        var recommendationList = results.Select(r => new Recommendation() { Product = r.Product, Score = r.Score }).ToList();

        return recommendationList;
    }

    private async Task<List<Recommendation>> CollaborationBased()
    {
        var checks = _db.Checks.Include(c => c.Products);
        var userChecks = _db.Checks.Where(c => c.UserId == HttpContext.GetUser().Id);

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
        _logger.LogInformation("Clusters Count: {c}", clustersCount);

        string featuresColumnName = "Features";
        var pipeline = mlContext.Transforms
            .Concatenate(featuresColumnName, "Features")
            .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: clustersCount));

        var model = pipeline.Fit(dataView);
        
        var predictions = model.Transform(dataView);
        var metrics = mlContext.Clustering.Evaluate(predictions);
        
        _logger.LogInformation("Metrics: {0}", metrics.AverageDistance);

        var predictor = mlContext.Model.CreatePredictionEngine<IrisData, ClusterPrediction>(model, inputSchemaDefinition: schemaDef);
        
        var prediction = predictor.Predict(new IrisData() {Features = new []{1f, 0f, 0f, 0f}});
        
         _logger.LogInformation("Classter: {c}", prediction.PredictedClusterId);
         
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
                     _logger.LogInformation("{i} Found in {n}", i, allKeys[f]);

                     for (var index = 0; index < allKeys.Count; index++)
                     {
                         if (allKeys[index] == allKeys[f])
                         {
                             _logger.LogInformation("{a} Similar to {b}", userChecksPrepared[i], allFeatures[index]);
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
                                     _logger.LogInformation("I can offer product {p} ({c}%)", products[j].Name, percent * 100);
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

    private async Task<List<Recommendation>> FavoriteCategoriesBased()
    {
        var user = HttpContext.GetUser();
        await _db.Entry(user).Collection(u => u.FavoriteCategories).LoadAsync();
        
        var checks = _db.Checks.Where(c => c.UserId == HttpContext.GetUser().Id);
        var products = _db.Products.Where(p => user.FavoriteCategories.Contains(p.Category.Category));
        
        var counted = await products
            .GroupBy(p => p.Id)
            .Select(y => new
            {
                Element = y.Key,
                Counter = y.Count(),
                Product = y.First()
            }).ToListAsync();

        var unOrdered = counted.Select(p => new
        {
            Element = p.Element,
            Counter = p.Counter,
            Product = p.Product,
            Popularity = _db.Checks.Where(c => c.Products.Contains(p.Product)).Count()
        });
        
        var maxCounter = unOrdered.MaxBy(c => c.Counter).Counter;
        var minCounter = unOrdered.MinBy(c => c.Counter).Counter;
        var cm = maxCounter - minCounter;
        
        var maxPopularity = unOrdered.MaxBy(c => c.Popularity).Popularity;
        var minPopularity = unOrdered.MinBy(c => c.Popularity).Popularity;
        var pm = maxPopularity - minPopularity;
        
        var results = unOrdered
            .Select(c => new { Product = c.Product, Score = (cm != 0 ? c.Counter / cm : 1) + (pm > 0 ? c.Popularity / pm : 0) })
            .DistinctBy(c => c.Product.Id)
            .OrderByDescending(c => c.Score).Take(10).ToList();

        var recommendationList = results.Select(r => new Recommendation() { Product = r.Product, Score = r.Score }).ToList();

        return recommendationList;
    }

    private Product ProductWithoutChecks(Product product)
    {
        product.Checks = null;
        return product;
    }

    [HttpGet]
    public async Task<ActionResult<List<Recommendation>>> GetRecomendations()
    {
        var recommendationList = new List<Recommendation>();
        
        var contendBased = ContentBased();
        var collaborationBased = CollaborationBased();
        var favoriteCategoriesBased = FavoriteCategoriesBased();

        await Task.WhenAll(contendBased, collaborationBased, favoriteCategoriesBased); // ToDo Перенести в интерфейсы

        recommendationList = recommendationList.Concat(contendBased.Result).ToList();
        recommendationList = recommendationList.Concat(collaborationBased.Result).ToList();
        recommendationList = recommendationList.Concat(favoriteCategoriesBased.Result).ToList();

        recommendationList = recommendationList.GroupBy(l => l.Product.Id).Select(g => new Recommendation()
        {
            Product = ProductWithoutChecks(g.First().Product),
            Score = g.Select(s => s.Score).Sum()
        }).ToList();
        
        recommendationList.Sort((r1, r2) => (int) Math.Round(r2.Score - r1.Score));

        return recommendationList;
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