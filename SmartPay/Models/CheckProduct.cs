using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPay.Models;

public class CheckProduct
{
    public int CheckUid { get; set; }
    public Check Check { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int Cost { get; set; }
    public int Amount { get; set; }
}