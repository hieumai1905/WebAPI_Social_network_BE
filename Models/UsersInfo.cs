using System;
using System.Collections.Generic;

namespace Web_Social_network_BE.Models;

public partial class UsersInfo
{
    public string UserInfoId { get; set; } = "";

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string Status { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string? AboutMe { get; set; }

    public string? CoverImage { get; set; }
}
