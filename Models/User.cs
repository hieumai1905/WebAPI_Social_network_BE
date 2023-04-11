using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class User
{
    public string FullName { get; set; } = null!;

    public string? Avatar { get; set; }

    public string UserId { get; set; } = null!;

    public string UserInfoId { get; set; } = null!;

   

    public virtual UsersInfo UserInfo { get; set; } = null!;
}
