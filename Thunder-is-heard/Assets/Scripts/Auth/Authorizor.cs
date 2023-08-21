using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;

public class RegData
{
    public string baseName = "";
    public string password = "";
}

public class Authorizor: MonoBehaviour
{
    private RegData regData;
    public TMP_InputField baseNameFromInput;
    public TMP_InputField passwordFromInput;

    public void Start()
    {
        using (var context = new LibraryContext())
        {
            // Creates the database if not exists
            context.Database.EnsureCreated();

            // Adds a publisher
            var publisher = new Publisher
            {
                Name = "Mariner Books"
            };
            context.Publisher.Add(publisher);

            // Adds some books
            context.Book.Add(new Book
            {
                ISBN = "978-0544003415",
                Title = "The Lord of the Rings",
                Author = "J.R.R. Tolkien",
                Language = "English",
                Pages = 1216,
                Publisher = publisher
            });
            context.Book.Add(new Book
            {
                ISBN = "978-0547247762",
                Title = "The Sealed Letter",
                Author = "Emma Donoghue",
                Language = "English",
                Pages = 416,
                Publisher = publisher
            });

            var user = new User
            {
                Name = "Volter August",
                Password = "916"
            };

            context.User.Add(user);

            // Saves changes
            context.SaveChanges();
        }

        regData = new RegData();

        Debug.Log("authorizer starts!");
    }

    public void UpdateBaseName()
    {
        Debug.Log("update name value");

        regData.baseName = baseNameFromInput.text;

        Debug.Log("now: " + regData.baseName);
    }

    public void UpdatePassword()
    {
        Debug.Log("update pass value");

        regData.password = passwordFromInput.text;

        Debug.Log("now: " + regData.password);
    }

    public void OnRegistrationButton()
    {
        if (IsValid())
        {
            Registration();
        }
        else
        {
            Debug.Log("invalid!");
        }
    }

    private bool IsValid()
    {
        if (regData.baseName.Length < 3)
        {
            return false;
        }
        if (regData.password.Length < 3) {
            return false; 
        }
        return true;
    }

    public void OnAuthorizationButton()
    {
        if (IsValid())
        {
            Authorization();
        }
        else
        {
            Debug.Log("invalid!");
        }
    }

    private void Registration()
    {
        Debug.Log("registration!");

        if (IsUserExist())
        {
            Debug.Log("This user already exist");
        }
        else
        {
            CreateUser();
        }
    }

    private void Authorization()
    {
        Debug.Log("auth!");

        if (IsUserExist())
        {
            Debug.Log("Exist!");
        }
        else
        {
            Debug.Log("Not exist");
        }
    }

    private bool IsUserExist()
    {
        using (var context = new LibraryContext())
        {

            context.Database.EnsureCreated();

            var baseEntity = context.Base;

            foreach (var entity in baseEntity)
            {
                var data = new StringBuilder();
                data.AppendLine($"ID: {entity.ID}");
                if (entity != null)
                {
                    return true;
                }
            }
                return false;
        }
    }

    private void CreateUser()
    {
        using (var context = new LibraryContext())
        {
            context.Database.EnsureCreated();

            var user = new User
            {
                Name = regData.baseName,
            };

            context.Add(user);

            var b = new Base
            {
                Name = regData.baseName
            };
            
            context.Add(b);

            // Saves changes
            context.SaveChanges();
        }
    }
    
}