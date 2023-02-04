#nullable disable

namespace Blog.Configuration;

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}