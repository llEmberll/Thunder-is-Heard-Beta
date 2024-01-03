using System.Collections.Generic;
using UnityEngine;


public class ObjectPreview: MonoBehaviour
{
    private Dictionary<string, object> objData;
    private int objectId;
    private string objectType;

    public Transform body;
    public MeshRenderer meshRenderer;
    public Material materialBasic, materialAvailable, materialUnvailable, materialModel;
    public Vector2Int size;

    public bool exposableStatus;
    public List<Cell> occypation;
	public Vector2Int rootPoint;
    public int rotation = 0;

    public Map map;


    public void Awake()
    {
        materialBasic = Resources.Load(Config.resources["materialPreview"] + "Basic", typeof(Material)) as Material;
        materialAvailable = Resources.Load(Config.resources["materialPreview"] + "Available", typeof(Material)) as Material;
        materialUnvailable = Resources.Load(Config.resources["materialPreview"] + "Unvailable", typeof(Material)) as Material;
    }

    public void Start()
    {
        map = GameObject.FindWithTag("Map").GetComponent<Map>();

        EventMaster.current.ToggledOffBuildMode += Cancel;
        EventMaster.current.PreviewRotated += Rotate;
    }

    public void Init(int id, string type)
    {
        SetObjectIdAndType(id, type);
        SetObjData();
        SetObjSize();

        InitModel();
    }

    public void SetObjData()
    {
        ITable objsTable = LocalDatabase.GetTableByName(objectType);
        if (objsTable == null)
        {
            Debug.Log("Undefined table by item type: " + objectType);
            Cancel();
            return;
        }

        objData = LocalDatabase.GetFieldsByTableAndTableItemIndex(objsTable, objectId);
        if (objData == null) 
        {
            Cancel();
            return;
        }
    }

    public void SetObjectIdAndType(int id, string type)
    {
        objectId = id;
        objectType = type;
    }

    public void SetObjSize()
    {
        size = new Vector2Int(1, 1);
        if (objData.ContainsKey("sizeByX") && objData.ContainsKey("sizeByY"))
        {
            size = new Vector2Int((int)objData["sizeByX"], (int)objData["sizeByY"]);
        }
    }

    public void InitModel()
    {
        string modelPath = (string)objData["modelPath"];

        GameObject modelPrefab = Resources.Load<GameObject>(modelPath);
        body = Instantiate(modelPrefab, modelPrefab.transform.position, Quaternion.identity).transform;
        body.SetParent(transform);

        meshRenderer = body.GetComponent<MeshRenderer>();
        materialModel = meshRenderer.material;
        meshRenderer.material = materialBasic;
    }

    public void Rotate()
    {
        rotation += 90;
        if (rotation == 360) rotation = 0;

        RotateModel();

        if (size.x == size.y)
        {
            return;
        }

        SwapSize();
        UpdateOccypation(rootPoint);
        SetModelOffsetByRotation();
    }

    public void RotateModel()
    {
        Vector3 currentRotate = body.transform.eulerAngles;
        currentRotate.y = rotation;
        body.transform.rotation = Quaternion.Euler(currentRotate);
    }

    public void SetModelOffsetByRotation()
    {
       float sizeDiff = ((float)size.x - (float)size.y)/2;

       Vector3 offset = new Vector3(sizeDiff, 0, -1 * sizeDiff);

        body.transform.position += offset;
    }

    public void SwapSize()
    {
        size.x = size.y + size.x;
        size.y = size.x - size.y;
        size.x -= size.y;
    }

    public bool UpdateOccypation(Vector2Int rootPoint)
    {
        occypation = new List<Cell>();

        int maxX = rootPoint.x + size.x;
        int maxZ = rootPoint.y + size.y;

        exposableStatus = true;

        for (int currentX = rootPoint.x; currentX < maxX; currentX++)
        {
            for (int currentZ = rootPoint.y; currentZ < maxZ; currentZ++)
            {
                Vector2Int currentCellPosition = new Vector2Int(currentX, currentZ);
                if (!map.cells.ContainsKey(currentCellPosition))
                {
                    exposableStatus = false;
                    break;
                }
                Cell currentCell = map.cells[currentCellPosition];
                if (currentCell.occupied)
                {
                    exposableStatus = false;
                    break;
                    
                }
                occypation.Add(map.cells[currentCellPosition]);
            }
        }

        meshRenderer.material = exposableStatus ? materialAvailable : materialUnvailable;
        return true;
    }
    
    public void Move(Vector2Int newRoot)
    {
       bool success = UpdateOccypation(newRoot);

        rootPoint = newRoot;
        transform.position = new Vector3(newRoot.x, 0, newRoot.y);
    }


    public void Expose()
    {
        if (!exposableStatus)
        {
            return;
        }

        ApplyOccypation();

        Transform entity = createObject();

        prepareModelToExposing(entity);

        EventMaster.current.ExposeObject(objectId, objectType);

        Cancel();
    }

    public Transform createObject()
    {
        GameObject prefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        var obj = Instantiate(prefab, new Vector3(rootPoint.x, 0, rootPoint.y), Quaternion.identity).transform;
        obj.name = (string)objData["name"];
        return obj;
    }

    public Transform prepareModelToExposing(Transform parent)
    {
        meshRenderer.material = materialModel;
        body.SetParent(parent);
        return body;
    }

    public void ApplyOccypation()
    {
        foreach (var cell in occypation)
        {
            cell.Occupy();
        }
    }

    public void Cancel()
    {
        UnsubscribeAll();
        EventMaster.current.OnExitBuildMode();
        Destroy(this.gameObject);
    }

    public void UnsubscribeAll()
    {
        EventMaster.current.ToggledOffBuildMode -= Cancel;
        EventMaster.current.PreviewRotated -= Rotate;
    }
}
