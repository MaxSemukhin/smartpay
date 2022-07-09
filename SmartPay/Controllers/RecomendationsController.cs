using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecomendationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RecomendationsController(ApplicationDbContext db)
    {
        _db = db;
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
        var checks = _db.Checks.Include(c => c.Products).Where(c => c.UserId == HttpContext.GetUser().Id);

        return new List<Recommendation>();
    }

    [HttpGet]
    public async Task<ActionResult<List<Recommendation>>> GetRecomendations()
    {
        var recommendationList = new List<Recommendation>();
        
        var contendBased = ContentBased();
        var collaborationBased = CollaborationBased();

        await Task.WhenAll(contendBased, collaborationBased);

        recommendationList = recommendationList.Concat(contendBased.Result).ToList();
        recommendationList = recommendationList.Concat(collaborationBased.Result).ToList();
        recommendationList.Sort((r1, r2) => r2.Score - r1.Score);

        return recommendationList;
    }
}