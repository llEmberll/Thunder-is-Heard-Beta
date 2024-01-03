using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposedItem : Item
{
    public override void Interact()
    {
        
        CreatePreview();
        
        EventMaster.current.OnBuildMode();

        EventMaster.current.ToggledOffBuildMode += Finish;
        EventMaster.current.ObjectExposed += FinishWithExpose;
    }


    public void CreatePreview()
    {
        Transform previewPrefab  = Resources.Load(Config.resources["prefabPreview"], typeof(Transform)) as Transform;
        var previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ObjectPreview preview = previewObject.GetComponent<ObjectPreview>();
        preview.Init(objectId, objectType);
    }

    public void Finish()
    {
        EventMaster.current.ToggledOffBuildMode -= Finish;
        EventMaster.current.ObjectExposed -= FinishWithExpose;
    }

    public void FinishWithExpose(int objId, string objType)
    {
        Finish();

        if (objType == objectType && objId == objectId)
        {
            Substract();
        }
    }
}
