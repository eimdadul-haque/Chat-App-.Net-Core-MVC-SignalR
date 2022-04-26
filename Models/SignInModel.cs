using System.ComponentModel.DataAnnotations;

public class SignInModel
{
    [Required]
    public string name { get; set; }

    [Required]
    public string email { get; set; }

    [Required]
    public string password { get; set; }
}