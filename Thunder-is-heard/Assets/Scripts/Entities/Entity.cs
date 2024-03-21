using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Interactable
{
    public string id;
    public Vector2Int originalSize;
    public Vector2Int currentSize;
    public List<Vector2Int> occypiedPoses;
    public int rotation;
    public Vector2Int center;
    public abstract override string Type { get; }
    public string Id { get { return id; } }

    public Transform model;
    public Map map;


    public void Awake()
    {
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
    }

    public void SetOriginalSize(Vector2Int newSize)
    {
        originalSize = newSize;
        currentSize = originalSize;
    }

    public void SetRotation(int newRotation)
    {
        rotation = newRotation;
        currentSize = GetCorrectSizeByRotation(originalSize, rotation);
    }

    public void SetModel(Transform newModel)
    {
        model = newModel;
        SetRotation(GetDeterminedRotationByModel(model));
    }

    public void SetOccypation(List<Vector2Int> position)
    {
        occypiedPoses = position;
    }

    public static int GetDeterminedRotationByModel(Transform modelForCheck)
    {
        return (int)modelForCheck.eulerAngles.y;
    }

    public static Vector2Int GetCorrectSizeByRotation(Vector2Int size, int currentRotation)
    {
        if (size.x == size.y)
        {
            return size;
        }
        if (currentRotation == 90 || currentRotation == 270)
        {
            size = GetSwappedSize(size);
        }

        return size;
    }

    public static Vector2Int GetSwappedSize(Vector2Int currentSize)
    {
        currentSize.x = currentSize.y + currentSize.x;
        currentSize.y = currentSize.x - currentSize.y;
        currentSize.x -= currentSize.y;

        return currentSize;
    }

    public void OnDestroy()
    {
        Debug.Log("On destroy");

        map.Free(occypiedPoses);
    }
}