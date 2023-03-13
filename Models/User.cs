namespace backend.Models;

public partial class User
{
    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = "user";

    public virtual ICollection<Message> Messages { get; } = new List<Message>();
}
