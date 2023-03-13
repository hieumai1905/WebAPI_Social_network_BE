using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Like
{
    public long LikeId { get; set; }

    public DateTime LikeAt { get; set; }

    public string UserId { get; set; } = null!;

    public string PostId { get; set; } = null!;

    public long CommentId { get; set; }
}
