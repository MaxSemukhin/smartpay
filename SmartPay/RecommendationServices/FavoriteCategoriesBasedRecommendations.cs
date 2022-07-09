using Microsoft.EntityFrameworkCore;
using SmartPay.Models;
using SmartPay.Controllers;
using SmartPay.Data;

namespace SmartPay.RecommendationServices;

public class FavoriteCategoriesBasedRecommendations: IRecommendationService
{
    private readonly ApplicationDbContext _db;

    public FavoriteCategoriesBasedRecommendations(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<List<Recommendation>> GetRecommendations(HttpContext context)
    {
        var user = context.GetUser();
        await _db.Entry(user).Collection(u => u.FavoriteCategories).LoadAsync();
        
        var checks = _db.Checks.Where(c => c.UserId == context.GetUser().Id);
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
}