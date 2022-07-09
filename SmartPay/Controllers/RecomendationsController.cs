using System.Linq.Expressions;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using SmartPay.Data;
using SmartPay.Models;
using SmartPay.RecommendationServices;

namespace SmartPay.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecomendationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<RecomendationsController> _logger;
    private readonly IServiceProvider _provider;

    public RecomendationsController(ApplicationDbContext db, ILogger<RecomendationsController> logger, IServiceProvider provider)
    {
        _db = db;
        _logger = logger;
        _provider = provider;
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
        
        var services = _provider.GetServices<IRecommendationService>();
        
        _logger.LogInformation("Staring collect recommendations for user {userId}", HttpContext.GetUser().Id);
        _logger.LogInformation("Services: {services}", services.Select(s => s.GetType().Name));
        
        var tasks = services.Select(s => s.GetRecommendations(HttpContext));
        await Task.WhenAll(tasks);

        foreach (var task in tasks)
        {
            recommendationList = recommendationList.Concat(task.Result).ToList();
        }
        
        recommendationList = recommendationList.GroupBy(l => l.Product.Id).Select(g => new Recommendation()
        {
            Product = ProductWithoutChecks(g.First().Product),
            Score = g.Select(s => s.Score).Sum()
        }).ToList();
        recommendationList.Sort((r1, r2) => (int) Math.Round(r2.Score - r1.Score));

        return recommendationList; // ToDo Кэширование
    }
}
