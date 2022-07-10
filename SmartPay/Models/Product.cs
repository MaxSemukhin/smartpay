namespace SmartPay.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public int Price { get; set; }
    
    public List<CheckProduct> Checks { get; set; }
    
    public int MerchantId { get; set; }
    public Merchant Merchant { get; set; }
    public int CategoryId { get; set; }
    public SubCategory? Category { get; set; }
}

public class ProductViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public int Price { get; set; }
    
    public MerchantViewModel Merchant { get; set; }
    public SubCategoryProductViewModel? Category { get; set; }
}