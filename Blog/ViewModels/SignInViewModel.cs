using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class SignInViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}