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
            if (!IsUserExists())
            {
                Registration();

                Clear();
            }
            else
            {
                Debug.Log("Such user already exists");
            }
            
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

    private bool IsUserExists()
    {
        //TODO ������� � ���� � ������� �������� �� ������� ������ �� ������
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

        CreateUser();

        Debug.Log("success");

    }

    private void Authorization()
    {
        Debug.Log("auth!");

        if (IsUserExist())
        {
            Debug.Log("Exist!");

            if (IsDataCorrect())
            {
                Debug.Log("success");

                Clear();
                //TODO ��������� �������� �����
            }

            else
            {
                Debug.Log("Not correct data");
            }
            
        }
        else
        {
            Debug.Log("Not exist");
        }
    }

    private bool IsDataCorrect()
    {
        //TODO �� ���������� ����� ������ ������������ ������� ������������ ��� ������ � ���������� ������

        return true;
    }

    private bool IsUserExist()
    {
        //TODO ���������� � ��� ����������� EF � �������� ������

        return true;
    }

    private void CreateUser()
    {
        //TODO ������� ������������ � ���� ����������� EF � ��������� ������� � �������
    }

    private void Clear()
    {
        regData = new RegData();

        baseNameFromInput.text = null;
        passwordFromInput.text = null;
    }
    
}