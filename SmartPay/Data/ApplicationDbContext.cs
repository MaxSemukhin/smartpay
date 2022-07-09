using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartPay.Models;

namespace SmartPay.Data;

public class ApplicationDbContext: DbContext
{
    public DbSet<MerchantCategory> MerchantCategories { get; set; } = null!;
    public DbSet<Merchant> Merchants { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<SubCategory> SubCategories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Check> Checks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}