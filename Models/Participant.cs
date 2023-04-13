using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Participant
{
    public string UserId { get; set; } = null!;

    public string GroupChatId { get; set; } = null!;

    public string UserRole { get; set; } = null!;

   
}
