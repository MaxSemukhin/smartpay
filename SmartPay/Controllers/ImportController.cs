using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPay.Data;
using SmartPay.Models;

namespace SmartPay.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ImportController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ImportController> _logger;

    public ImportController(ApplicationDbContext db, ILogger<ImportController> logger)
    {
        _db = db;
        _logger = logger;
    }
    
    [DisableRequestSizeLimit] 
    [HttpPost("import")]
    public async Task<ActionResult> Import(IFormFile data)
    {
        var stream = data.OpenReadStream();
        var reader = new StreamReader(stream);
        string text = await reader.ReadToEndAsync();
        stream.Close();
        reader.Close();

        ImportedLine[] lines = text.Split('\n')[1..].Select(e => new ImportedLine(Regex.Split(e, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"))).ToArray();

        int count = 0;
        var time = DateTime.Now;
        
        // foreach (var line in lines)
        // {
        //     var user = await _db.Users.GetOrCreate(u => u.Id == line.UserId, new User()
        //     {
        //         Id = line.UserId
        //     });
        //
        //     var check = await _db.Checks.GetOrCreate(c => c.Id == line.CheckId && c.UserId == line.UserId, new Check()
        //     {
        //         Id = line.CheckId,
        //         User = user
        //     });
        //
        //     var merchand = await _db.Merchants.GetOrCreate(m => m.Name == line.MerchantName, new Merchant()
        //     {
        //         Category = null,
        //         Name = line.MerchantName
        //     });
        //     
        //     var product = await _db.Products.Include(p => p.Merchant).FirstOrDefaultAsync(p => p.Name == line.ProcuctName && p.Merchant.Name == merchand.Name);
        //
        //     if (product == null)
        //     {
        //         product = new Product()
        //         {
        //             Category = null,
        //             Check = check,
        //             Merchant = merchand,
        //             Name = line.ProcuctName,
        //             Price = line.ProductCost
        //         };
        //
        //         _db.Products.Add(product);
        //     }
        //     
        //     await _db.SaveChangesAsync();
        //     count++;
        //     
        //     if (count % 500 == 0) _logger.LogInformation("Importing progress: {count}/{all} ({time})", count, lines.Length, (DateTime.Now - time).ToString());
        // }
        
        var memorizedProducts = new HashSet<Product>(await _db.Products.Include(p => p.Merchant).ToListAsync());
        var memorizedMerchants = await _db.Merchants.ToListAsync();
        
        var userIds = new HashSet<int>(lines.Select(l => l.UserId));
        
        foreach (var userId in userIds)
        {
            var user = await _db.Users.GetOrCreate(u => u.Id == userId, new User()
            {
                Id = userId
            });
            
            var checkIds = new HashSet<int>(lines.Where(l => l.UserId == userId).Select(l => l.CheckId));
        
            foreach (var checkId in checkIds)
            {
                var check = await _db.Checks.GetOrCreate(c => c.Id == checkId && c.UserId == userId, new Check()
                {
                    Id = checkId,
                    User = user
                });
                
                var products = lines.Where(l => l.UserId == userId && l.CheckId == checkId).ToArray();
        
                foreach (var line in products)
                {
                    var merchand = memorizedMerchants.FirstOrDefault(m => m.Name == line.MerchantName);
        
                    if (merchand == null)
                    {
                        merchand = new Merchant()
                        {
                            Category = null,
                            Name = line.MerchantName
                        };
        
                        _db.Merchants.Add(merchand);
                        memorizedMerchants.Add(merchand);
                    }
                    
                    var product = memorizedProducts.FirstOrDefault(p => p.Name == line.ProcuctName && p.Merchant.Name == merchand.Name);
        
                    if (product == null)
                    {
                        product = new Product()
                        {
                            Category = null,
                            Merchant = merchand,
                            Name = line.ProcuctName,
                            Price = line.ProductCost
                        };  
        
                        _db.Products.Add(product);
                        memorizedProducts.Add(product);
                    }
        
                    if (check.Products == null) check.Products = new List<Product>();
                    check.Products.Add(product); // ToDo Учитывать кол-во товара
                }
            }
        
            count++;
        
            if (count % 10 == 0 || count == 1) _logger.LogInformation("Importing progress: {count}/{all} ({time})", count, userIds.Count, (DateTime.Now - time).ToString());
        }
        
        await _db.SaveChangesAsync();

        return Ok();
    } 
}