using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposedItem : Item
{
    public override void Interact()
    {
        CreatePreview();
        
        EventMaster.current.OnBuildMode();
        EventMaster.current.ToggledOffBuildMode += OnCancelExposing;
        EventMaster.current.ObjectExposed += OnObjectExposed;
    }


    public void CreatePreview()
    {
        ITable objsTable = LocalDatabase.GetTableByName(objectType);
        if (objsTable == null)
        {
            Debug.Log("Undefined table by item type: " + objectType);
            Finish();
            return;
        }

        Dictionary<string, object>  objData = LocalDatabase.GetFieldsByTableAndTableItemIndex(objsTable, objectId);
        if (objData == null)
        {
            Finish();
            return;
        }

        Transform previewPrefab  = Resources.Load(Config.resources["prefabPreview"], typeof(Transform)) as Transform;
        var previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ObjectPreview preview = previewObject.GetComponent<ObjectPreview>();
        preview.Init(objData, objectType, objectId);
    }

    public void OnCancelExposing()
    {
        UnsubscribeAll();
    }

    public void Finish()
    {
        UnsubscribeAll();

        EventMaster.current.OnExitBuildMode();
    }

    public void UnsubscribeAll()
    {
        EventMaster.current.ToggledOffBuildMode -= OnCancelExposing;
        EventMaster.current.ObjectExposed -= OnObjectExposed;
    }

    public void Continue()
    {
    }

    public void OnObjectExposed(int objId, string objType, Vector2Int[] occypation, int rotation)
    {
        if (objType == objectType && objId == objectId)
        {
            SaveExpose(occypation, rotation);

            if (itemCount < 2)
            {
                Finish();
            }

            Substract();
        }

        else
        {
            Continue();
        }
    }

    public void SaveExpose(Vector2Int[] occypation, int rotation)
    {
        if (objectType == "Build")
        {
            SaveBuildExpose(occypation, rotation);
        }

        if (objectType == "Unit")
        {
            SaveUnitExpose(occypation, rotation);
        }
    }

    public void SaveBuildExpose(Vector2Int[] occypation, int rotation)
    {
        BuildsTable builsTable = (BuildsTable)LocalDatabase.GetTableByName("Build");
        BuildData build = builsTable.Items[objectId];

        PlayerBuildsTable playerTable = (PlayerBuildsTable)LocalDatabase.GetTableByName("Player" + objectType);
        BuildData playerBuild = (BuildData)build.Clone();
        playerBuild.position = occypation;
        playerBuild.rotation = rotation;

        playerTable.currentItem = playerBuild;
        playerTable.AddElement();

    }

    public void SaveUnitExpose(Vector2Int[] occypation, int rotation)
    {

    }
}
