using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;

public class Book
{
    public int ID { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Language { get; set; }
    public int Pages { get; set; }
    public virtual Publisher Publisher { get; set; }
}

public class Publisher
{
    public int ID { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}

public class Base
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int BaseId { get; set; }
    public Base Base { get; set; } = null!;
}

