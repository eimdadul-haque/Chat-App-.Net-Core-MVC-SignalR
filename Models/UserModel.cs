using Microsoft.AspNetCore.Identity;

public class UserModel : IdentityUser 
{
    public string? name { get; set; }
}