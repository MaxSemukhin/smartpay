namespace SmartPay.Models;

public class CategoryWithMerhands
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public List<MerchantViewModel> Merchants { get; set; }
}