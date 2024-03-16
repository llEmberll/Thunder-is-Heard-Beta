using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsProcessor : MonoBehaviour
{
    public void PutSelectedObjectToInventory()
    {
        ObjectPreview selectedObj = FindObjectOfType<ObjectPreview>();
        if (selectedObj == null) return;

        Debug.Log("selected object putted to inv");
    }
}
