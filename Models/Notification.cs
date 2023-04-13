using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Notification
{
    public string NotificationId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime NotificationsAt { get; set; } = DateTime.Now;

    public string Status { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string UserTargetId { get; set; } = null!;

    
}
