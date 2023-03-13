using System;
using System.Collections;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chat
{
    public int Id { get; set; }

    public BitArray Name { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; } = new List<Message>();
}
