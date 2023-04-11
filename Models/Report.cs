using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Report
{
    public string UserId { get; set; } = null!;

    public string PostId { get; set; } = null!;

    public string ReportContent { get; set; } = null!;

    
}
