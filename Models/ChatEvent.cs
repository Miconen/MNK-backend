namespace backend.Models;

public class ChatEvent {

    public DateTime Date { get; set; }

    public string Content { get; set; }

    public string ContentType { get; set; }

    public string Username { get; set; }

    public string Roomname { get; set; }

    public string JWT { get; set; } = "";
}
