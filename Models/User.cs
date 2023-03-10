using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Users")]
public class User
{
    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public int Id { get; set; }
    [Required]
    [StringLength(16)]
    public string? Name { get; set; }
    [Required]
    [StringLength(64)]
    public string? Password { get; set; }
    [Required]
    [StringLength(8)]
    [DefaultValue("user")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Role { get; set; } = "user";
}
