using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SmartPay.Models;

public class User: IdentityUser<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int Id { get; set; }
    
    public List<Check> Checks { get; set; }
    
    public bool IsAdmin { get; set; }
}