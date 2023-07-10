using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposedItem : Item
{
    public override void Interact()
    {
        
        CreatePreview();
        
        EventMaster.current.OnBuildMode();
    }


    public void CreatePreview()
    {
        Transform previewPrefab  = Resources.Load(Config.resources["prefabPreview"], typeof(Transform)) as Transform;
        var previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ObjectPreview preview = previewObject.GetComponent<ObjectPreview>();
        preview.Init(objectId);
    }
}
