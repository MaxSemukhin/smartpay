using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartPay.Models;
using SmartPay.Controllers;
using SmartPay.Data;

namespace SmartPay.RecommendationServices;

public class FavoriteCategoriesBasedRecommendations: IRecommendationService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FavoriteCategoriesBasedRecommendations(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<List<Recommendation>> GetRecommendations(HttpContext context)
    {
        var user = context.GetUser();
        await _db.Entry(user).Collection(u => u.FavoriteCategories).LoadAsync();
        
        var checks = _db.Checks.Where(c => c.UserId == context.GetUser().Id);
        var products = _db.Products
            .Where(p => user.FavoriteCategories.Contains(p.Category.Category))
            .Include(p => p.Category)
            .Include(p => p.Merchant);
        
        var counted = await products
            .GroupBy(p => p.Id)
            .Select(y => new
            {
                Element = y.Key,
                Counter = y.Count(),
                Product = y.First()
            }).ToListAsync();

        _db.Checks.Where(c => c.Products.Any(cp => cp.ProductId == 1)).Count();

        var unOrdered = counted.Select(p => new
        {
            Element = p.Element,
            Counter = p.Counter,
            Product = p.Product,
            Popularity = _db.Checks.Where(c => c.Products.Any(cp => cp.ProductId == p.Element)).Count()
        });
        
        var maxCounter = unOrdered.MaxBy(c => c.Counter)?.Counter;
        var minCounter = unOrdered.MinBy(c => c.Counter)?.Counter;
        var cm = maxCounter != null && minCounter != null ? maxCounter - minCounter : 0;
        
        var maxPopularity = unOrdered.MaxBy(c => c.Popularity)?.Popularity;
        var minPopularity = unOrdered.MinBy(c => c.Popularity)?.Popularity;
        var pm = maxPopularity != null && maxPopularity != null ? maxPopularity - minPopularity : 0;
        
        var results = unOrdered
            .Select(c => new { Product = c.Product, Score = (cm != null && cm != 0 ? (float)c.Counter / cm : 1f) + (pm != null && pm > 0 ? c.Popularity / pm : 0f) })
            .DistinctBy(c => c.Product.Id)
            .OrderByDescending(c => c.Score).Take(10).ToList();

        var recommendationList = results.Select(r => new Recommendation() { Product =  _mapper.Map<ProductViewModel>(r.Product), Score = (float)r.Score }).ToList();

        return recommendationList;
    }
}