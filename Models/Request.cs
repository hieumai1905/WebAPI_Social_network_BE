using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Request
{
    public long RegisterId { get; set; }

    public DateTime RegisterAt { get; set; }

    public string CodeType { get; set; } = null!;

    public int RequestCode { get; set; }

    public string Email { get; set; } = null!;
}
