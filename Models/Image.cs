using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Image
{
    public string ImageId { get; set; } = ""!;

    public string Url { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string PostId { get; set; } = null!;
}
