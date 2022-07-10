using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FavoriteController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FavoriteController(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryViewModel>>> GetCategories()
    {
        return await _mapper
            .ProjectTo<CategoryViewModel>(_db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser())))
            .ToListAsync();
    }

    [HttpGet("merchants")]
    public async Task<ActionResult<List<MerchantViewModel>>> GetMerchants()
    {
        return await _mapper
            .ProjectTo<MerchantViewModel>(_db.Merchants.Where(c => c.Users.Contains(HttpContext.GetUser())))
            .ToListAsync();
    }

    [HttpPost("categories")]
    public async Task<ActionResult<List<CategoryViewModel>>> PostCategories(
        [FromBody] FavoriteCategoryPost[] categories)
    {
        var user = HttpContext.GetUser();
        _db.Entry(user).Collection(u => u.FavoriteCategories).Load();
        user.FavoriteCategories.Clear();

        foreach (var category in categories.Take(3))
        {
            var dbCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (dbCategory != null)
                user.FavoriteCategories.Add(dbCategory);
        }

        await _db.SaveChangesAsync();

        return await _mapper
            .ProjectTo<CategoryViewModel>(_db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser())))
            .ToListAsync();
    }
    
    [HttpPost("merchants")]
    public async Task<ActionResult<List<CategoryViewModel>>> PostMerchants([FromBody] MerchantViewModel[] merchants)
    {
        var user = HttpContext.GetUser();
        _db.Entry(user).Collection(u => u.FavoriteMerchants).Load();
        user.FavoriteMerchants.Clear();

        foreach (var merchant in merchants.Take(6))
        {
            var dbMerchants = await _db.Merchants.FirstOrDefaultAsync(m => m.Id == merchant.Id);
            if (dbMerchants != null)
                user.FavoriteMerchants.Add(dbMerchants);
        }

        await _db.SaveChangesAsync();

        return await _mapper
            .ProjectTo<CategoryViewModel>(_db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser())))
            .ToListAsync();
    }


    [HttpGet("categories/all")]
    public async Task<ActionResult<List<CategoryViewModel>>> ListAllCategories()
    {
        return await _mapper.ProjectTo<CategoryViewModel>(_db.Categories).ToListAsync();
    }

    [HttpGet("merchants/all")]
    public async Task<ActionResult<List<CategoryWithMerhands>>> ListAll()
    {
        var cat = _db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser()));

        var products = await _db.Products
            .Where(p => cat.Contains(p.Category.Category))
            .Select(p => new { Category = p.Category.Category, Merchant = p.Merchant })
            .ToListAsync();

        return products
            .GroupBy(p => p.Category)
            .Select(g => new CategoryWithMerhands()
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                Merchants = _mapper.Map<List<MerchantViewModel>>(g.Select(p => p.Merchant)).DistinctBy(m => m.Id).ToList()
            }).ToList();
    }
}