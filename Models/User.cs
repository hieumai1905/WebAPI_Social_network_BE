using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class User
{
    public string UserId { get; set; } = ""!;
    public string FullName { get; set; } = null!;

    public string? Avatar { get; set; }


    public string UserInfoId { get; set; } = ""!;

    public virtual UsersInfo UserInfo { get; set; } = null!;
}
