using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Message
{
    public long MessageId { get; set; }

    public DateTime SendAt { get; set; } = DateTime.Now;

    public string Content { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string ConversationId { get; set; } = null!;

    
}
