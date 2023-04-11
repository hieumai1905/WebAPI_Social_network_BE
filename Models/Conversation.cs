using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Conversation
{
    public string ConversationId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string UserTargetId { get; set; } = null!;

   
}
