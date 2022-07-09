using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPay.Models;

public class Check
{
    [Key] public int Uid { get; set; }

    public int Id { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
    public List<Product> Products { get; set; }
}

public class CheckViewModel
{
    public int Uid { get; set; }

    public int Id { get; set; }

    public List<Product> Products { get; set; }
}