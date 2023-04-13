using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class MessageGroup
{
    public string MessageGroupId { get; set; } = null!;

    public DateTime SendAt { get; set; } = DateTime.Now;

    public string Content { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string GroupChatId { get; set; } = null!;

   
}
