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
public class ChecksController: ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ChecksController(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CheckViewModel>>> GetChecks()
    {
        return await _mapper.ProjectTo<CheckViewModel>(_db.Checks.Where(c => c.UserId == HttpContext.GetUser().Id).Include("Products.Product")).ToListAsync();
    }
}