using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Users")]
public class User
{
    public User (string name, string password)
    {
        Name = name;
        Password = password;
    }

    public int Id { get; set; }
    [Required]
    [StringLength(12)]
    public string? Name { get; set; }
    [Required]
    [StringLength(12)]
    public string? Password { get; set; }
    [StringLength(8)]
    [DefaultValue("user")]
    public string? Role{ get; set; }
}
