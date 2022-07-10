using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.RecommendationServices;

public class ContentBasedRecommendations: IRecommendationService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ContentBasedRecommendations(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<List<Recommendation>> GetRecommendations(HttpContext context)
    {
        var checks = _db.Checks
            .Where(c => c.UserId == context.GetUser().Id)
            .Include("Products.Product.Category")
            .Include("Products.Product.Merchant");
        var allProducts = checks.SelectMany(c => c.Products.Select(p => p.Product));
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
                .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Any(cp => cp.ProductId == p.Id)).Count() } )
                
                .Concat(_db.Products
                    .Where(p => p.Category.CategoryId == _db.Products.First(pr => pr.Id == c.Element).Category.CategoryId)
                    .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Any(cp => cp.ProductId == p.Id)).Count() / 2 } )
                )
                
                .Concat(_db.Products
                    .Where(p => p.MerchantId == _db.Products.First(pr => pr.Id == c.Element).MerchantId)
                    .Select(p => new { Counter = c.Counter, Product = p, Popularity = _db.Checks.Where(c => c.Products.Any(cp => cp.ProductId == p.Id)).Count() / 4 } )
                )
                
                .OrderByDescending(p => p.Popularity)
        });

        var unOrdered = countedWithSimilar.SelectMany(c => c.Similar).ToList();

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

        var recommendationList = results.Select(r => new Recommendation() { Product =  _mapper.Map<ProductViewModel>(r.Product), Score = r.Score }).ToList();

        return recommendationList;
    }
}