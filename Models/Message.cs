using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Message
{
    public int Id { get; set; }

    public string Creationdate { get; set; } = null!;

    public int Chat { get; set; }

    public string Content { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Author { get; set; } = null!;

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual Chat ChatNavigation { get; set; } = null!;
}
