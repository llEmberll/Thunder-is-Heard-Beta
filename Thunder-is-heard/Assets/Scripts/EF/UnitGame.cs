using UnityEngine;
using Microsoft.EntityFrameworkCore;
using System.Text; 

public class UnitGame : MonoBehaviour
{

    void Start()
    {
        InsertData();
        PrintData();
    }

    private static void InsertData()
    {
        using (var context = new LibraryContext())
        {
            // Creates the database if not exists
            context.Database.EnsureCreated();

            // Adds a publisher
            var build = new Build
            {
                Name = "Headbuild",
                SizeX = 3,
                SizeY = 3,
            };


            context.Build.Add(build);


            var base = new Base
            {
                Name = "Zone-51",
                Builds = { build }
            };

            context.Base.Add(base);
                
            );

            // Saves changes
            context.SaveChanges();
        }
    }

    private static void PrintData()
    {
        // Gets and prints all books in database
        using (var context = new LibraryContext())
        {
            var build = context.Build
              .Include(p => p.b);
            foreach (var book in books)
            {
                var data = new StringBuilder(); 
                data.AppendLine($"ISBN: {book.ISBN}");
                data.AppendLine($"Title: {book.Title}");
                data.AppendLine($"Publisher: {book.Publisher.Name}");
            }
        }
    }

}
