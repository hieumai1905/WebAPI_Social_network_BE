using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Post
{
    public string PostId { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public string Content { get; set; } = null!;

    public string AccessModifier { get; set; } = null!;

    public string PostType { get; set; } = null!;

    public string UserId { get; set; } = null!;

   

   
}
