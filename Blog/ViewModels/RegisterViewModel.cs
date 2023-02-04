using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Blog.Enums;

namespace Blog.ViewModels;

public class RegisterViewModel
{
    [Required] [DisplayName("First Name")] public string FirstName { get; set; } = string.Empty;
    [Required] [DisplayName("Last Name")] public string LastName { get; set; } = string.Empty;

    [DataType(DataType.PhoneNumber)]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.EmailAddress)]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [DisplayName("Password Confirmation")]
    public string PasswordConfirmation { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [DisplayName("Birthday")]
    public DateTime? DateOfBirth { get; set; }

    [Required] [DisplayName("Sex")] public Sex Sex { get; set; }
}