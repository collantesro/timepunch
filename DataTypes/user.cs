using System;
using System.Collections.Generic;
using System.Text;

public class User
{
    public enum Roles { Default, Student, Professor };
    public Roles Role { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public string Email { get; set; }
}