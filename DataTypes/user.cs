using System;
using System.Collections.Generic;
using System.Text;

public class User
{
    public enum role {Default, Student, Professor}; 
    public role r;
    public string name {get;set;}
    public int id {get;set;}
    public string passwordHash {get;set;}
    public string salt {get;set;}
    public string email {get; set;}
}