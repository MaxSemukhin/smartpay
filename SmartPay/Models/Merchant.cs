namespace SmartPay.Models;

public class Merchant
{
    public int Id { get; set; }
    
    public MerchantCategory? Category { get; set; }
    public List<Product> Products { get; set; }
    
    public string Name { get; set; }
}