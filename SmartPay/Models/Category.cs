namespace SmartPay.Models;

public class Category
{
    public int Id { get; set; }
    
    public int MCC { get; set; }
    public string Name { get; set; }
    
    public List<SubCategory> SubCategories { get; set; }
}