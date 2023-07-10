using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IFillable
{
    public List<Item> inventory;


    public void Awake()
    {
        FillContent();
    }
    
    public void FillContent()
    {
        
    }

    
}
