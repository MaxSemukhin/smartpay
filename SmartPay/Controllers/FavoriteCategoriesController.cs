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
public class FavoriteCategoriesController: ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public FavoriteCategoriesController(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryViewModel>>> GetCategories()
    {
        return await _mapper.ProjectTo<CategoryViewModel>(_db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser()))).ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<List<CategoryViewModel>>> PostCategories([FromBody] FavoriteCategoryPost[] categories)
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
        
        return await _mapper.ProjectTo<CategoryViewModel>(_db.Categories.Where(c => c.Users.Contains(HttpContext.GetUser()))).ToListAsync();
    }
    
    
    [HttpGet("all")]
    public async  Task<ActionResult<List<CategoryViewModel>>> ListAll()
    {
        return await _mapper.ProjectTo<CategoryViewModel>(_db.Categories).ToListAsync();
    }
}