#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Account;

public class ChangeEmailViewModel
{
    [EmailAddress] public string NewEmail { get; set; }
}