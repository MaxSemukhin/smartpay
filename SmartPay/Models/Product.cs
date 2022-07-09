namespace SmartPay.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public int Price { get; set; }
    
    public List<Check> Checks { get; set; }
    public Merchant Merchant { get; set; }
    public SubCategory? Category { get; set; }
}