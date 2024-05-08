
using UnityEngine;

public class ItemList : UIElement, IFillable
{
    public virtual void Start()
    {
        OnBuildModeEnable();
        FillContent();
    }

    public virtual void FillContent()
    {
        
    }

    public static GameObject CreateObject(string prefabPath, Transform parent = null)
    {
        GameObject itemPrefab = Resources.Load<GameObject>(prefabPath);
        GameObject itemObject = Instantiate(itemPrefab);
        itemObject.transform.SetParent(parent,  false);
        return itemObject;
    }

    public virtual void OnBuildModeEnable()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
    }
}
