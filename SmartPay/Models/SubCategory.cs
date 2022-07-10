namespace SmartPay.Models;

public class SubCategory
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public List<Product> Products { get; set; }
}

public class SubCategoryViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}


public class SubCategoryProductViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    
    public CategoryViewModel Category { get; set; }
}