namespace SmartPay.Models;

public class Category
{
    public int Id { get; set; }
    
    public int MCC { get; set; }
    public string Name { get; set; }
    
    public List<User> Users { get; set; }
    public List<SubCategory> SubCategories { get; set; }
}

public class CategoryViewModel
{
    public int Id { get; set; }
    
    public int MCC { get; set; }
    public string Name { get; set; }
    
    public List<SubCategoryViewModel> SubCategories { get; set; }
}

public class FavoriteCategoryPost
{
    public int Id { get; set; }
}