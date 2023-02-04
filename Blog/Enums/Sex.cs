namespace Blog.Enums;

[Flags]
public enum Sex
{
    Male = 0b_0000_0000,
    Female = 0b_0000_0001,
    Unknown = 0b_000_0010
}