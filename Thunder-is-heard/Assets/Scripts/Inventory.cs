using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ItemList
{
    public List<Item> source;
    public GameObject itemPrefab;
    
    public override void FillContent()
    {
        foreach (var i in source)
        {
            GameObject itemObject = GameObject.Instantiate(itemPrefab);
            itemObject.transform.SetParent(this.transform, false);

            Item item = itemObject.GetComponent<Item>();
            
            item = i;
        }
    }
}
