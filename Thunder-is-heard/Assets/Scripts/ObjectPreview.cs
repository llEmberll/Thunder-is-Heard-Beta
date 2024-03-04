using System.Collections.Generic;
using UnityEngine;


public class ObjectPreview: MonoBehaviour
{
    private string id;
    private string name;
    private string type;
    private string pathToModel;

    public Transform body;
    public MeshRenderer meshRenderer;
    public Material materialBasic, materialAvailable, materialUnvailable, materialModel;
    public Bector2Int size;

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

        EventMaster.current.ToggledOffBuildMode += OnExitBuildMode;
        EventMaster.current.PreviewRotated += Rotate;

        EventMaster.current.OnCreatePreview(this);
    }

    public void Init(string objName, string objType, string objId, Bector2Int objSize, string objModelPath)
    {
        name = objName;
        id = objId;
        type = objType;
        size = objSize;
        pathToModel = objModelPath;
        InitModel();
    }

    public void InitModel()
    {
        GameObject modelPrefab = Resources.Load<GameObject>(pathToModel);
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
       UpdateOccypation(newRoot);

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

        Transform exposedBody = prepareModelToExposing(entity);

        EventMaster.current.ExposeObject(id, type, GetOccypationPositions(), rotation);

        AfterExpose(exposedBody);
    }

    public Bector2Int[] GetOccypationPositions()
    {
        Bector2Int[] positions = new Bector2Int[occypation.Count];
        foreach (Cell cell in occypation)
        {
            positions[occypation.IndexOf(cell)] = new Bector2Int(cell.position);
        }

        return positions;
    }

    public void AfterExpose(Transform exposedBody)
    {
        InitModel();
        body.transform.position = exposedBody.transform.position;
        body.transform.rotation = exposedBody.transform.rotation;
    }

    public Transform createObject()
    {

        GameObject parent = GameObject.FindWithTag(Config.exposableObjectsTypeToObjectsOnSceneTag[type]);
        GameObject prefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        var obj = Instantiate(prefab, new Vector3(rootPoint.x, 0, rootPoint.y), Quaternion.identity, parent.transform).transform;
        obj.name = name;
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
        EventMaster.current.OnExitBuildMode();
    }

    public void OnExitBuildMode()
    {
        UnsubscribeAll();
        EventMaster.current.OnDeletePreview();
        Destroy(this.gameObject);
    }

    public void UnsubscribeAll()
    {
        EventMaster.current.ToggledOffBuildMode -= OnExitBuildMode;
        EventMaster.current.PreviewRotated -= Rotate;
    }
}
