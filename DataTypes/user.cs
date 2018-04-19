using System;
using System.Collections.Generic;
using System.Text;

public class User
{
    public enum Roles { Unapproved, Student, Professor, SiteAdmin };
    public Roles Role { get; set; } = Roles.Unapproved;
    public string Name { get; set; } = "";
    public int Id { get; set; } = -1;
    public string PasswordHash { get; set; } = "";
    public string Salt { get; set; } = "";
    public string Email { get; set; } = "";
}