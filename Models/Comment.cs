using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Comment
{
    public long CommentId { get; set; }

    public long? LikeCount { get; set; }

    public DateTime CommentAt { get; set; }

    public string Content { get; set; } = null!;

    public string PostId { get; set; } = null!;

    public string UserId { get; set; } = null!;

   
}
