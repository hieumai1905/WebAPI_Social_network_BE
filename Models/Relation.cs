using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class Relation
{
    public string RelationId { get; set; } = null!;

    public DateTime ChangeAt { get; set; }

    public string TypeRelation { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string UserTargetIduserId { get; set; } = null!;

    
}
