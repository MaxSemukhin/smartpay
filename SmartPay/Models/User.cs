using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPay.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int Id { get; set; }
    
    public List<Check> Checks { get; set; }
}