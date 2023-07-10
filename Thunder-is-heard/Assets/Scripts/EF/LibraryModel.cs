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

    public class Build
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public virtual ICollection<Base> Bases { get; set; }
    }
    public class Base
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Build> Builds { get; set; }

    }

