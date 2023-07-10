using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RegData
{
    public string name, email, password, repeatPassword;

    public RegData(string username, string userEmail, string userPassword, string userRepeatPassword)
    {
        this.name = username;
        this.email = userEmail;
        this.password = userPassword;
        this.repeatPassword = userRepeatPassword;
    }
}

public class Authorizor: MonoBehaviour
{
    public HTTP http;

    private void Start()
    {
        Debug.Log("authorizer starts!");
    }
    
    public void Registration()
    {
        string url = "http://127.0.0.1/api";

        RegData regData = new RegData("Asher", "asher.ember@mail.ru", "pas1234567", "pas1234567");

        http.PostRequest(
            url, 
            regData
            );
    }
}