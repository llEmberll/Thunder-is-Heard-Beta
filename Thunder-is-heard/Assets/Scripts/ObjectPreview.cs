using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectPreview: MonoBehaviour
{
    private string id;
    private string name;
    private string type;
    private Transform model;

    public ObjectsOnBase objectsPool;

    public MeshRenderer meshRenderer;
    public Material materialBasic, materialAvailable, materialUnvailable, materialModel;
    public Vector2Int size;

    public bool exposableStatus;
    public List<Vector2Int> occypation;
	public Vector2Int rootPoint;
    public int rotation = 0;

    public GameObject buildedObjectOnScene = null;

    public Map map;


    public void Awake()
    {
        materialBasic = Resources.Load(Config.resources["materialPreview"] + "Basic", typeof(Material)) as Material;
        materialAvailable = Resources.Load(Config.resources["materialPreview"] + "Available", typeof(Material)) as Material;
        materialUnvailable = Resources.Load(Config.resources["materialPreview"] + "Unvailable", typeof(Material)) as Material;
    }

    public void Start()
    {
        EventMaster.current.ToggledOffBuildMode += OnExitBuildMode;
        EventMaster.current.PreviewRotated += Rotate;

        EventMaster.current.OnCreatePreview(this);
    }

    public static ObjectPreview Create()
    {
        Transform previewPrefab = Resources.Load(Config.resources["prefabPreview"], typeof(Transform)) as Transform;
        var previewObject = Instantiate(previewPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ObjectPreview preview = previewObject.GetComponent<ObjectPreview>();
        preview.map = GameObject.FindWithTag("Map").GetComponent<Map>();

        return preview;
    }

    public void Init(string objName, string objType, string objId, Vector2Int objSize, Transform objModel)
    {
        rotation = (int)objModel.eulerAngles.y;
        name = objName;
        id = objId;
        type = objType;
        size = objSize;
        model = objModel;
        InitModel();

        objectsPool = GameObject.FindWithTag(Config.exposableObjectsTypeToObjectsOnSceneTag[type]).GetComponent<ObjectsOnBase>();
    }

    public void InitModel()
    {
        buildedObjectOnScene = null;

        if (model.parent != null) 
        {
            buildedObjectOnScene = model.parent.gameObject;
            transform.position = model.parent.transform.position;
        }
        
        model.SetParent(transform);

        meshRenderer = model.GetComponent<MeshRenderer>();
        materialModel = meshRenderer.material;
        meshRenderer.material = materialBasic;
    }

    public void Rotate()
    {
        rotation += 90;
        if (rotation == 360) rotation = 0;

        RotateModel(rotation);

        if (size.x == size.y)
        {
            return;
        }

        size = Entity.GetSwappedSize(size);
        occypation = map.GetOccypationPositionForObj(rootPoint, size);
        UpdateExposableStatus();
        SetPositiveModelOffsetByRotation();
    }

    public void RotateModel(int newRotation)
    {
        Vector3 currentRotate = model.transform.eulerAngles;
        model.transform.rotation = Quaternion.Euler(currentRotate.x, newRotation, currentRotate.z);
    }
    
    public Vector3 GetModelOffsetByRotation()
    {
       float sizeDiff = ((float)size.x - (float)size.y)/2;
       return new Vector3(sizeDiff, 0, -1 * sizeDiff);
    }

    public void SetPositiveModelOffsetByRotation()
    {
        model.transform.position += GetModelOffsetByRotation();
    }

    public void SetNegativeModelOffsetByRotation()
    {
        model.transform.position -= GetModelOffsetByRotation();
    }

    public void UpdateExposableStatus()
    {
        List<Vector2Int> positionForCheckOnFree = occypation;
        if (buildedObjectOnScene != null)
        {
            positionForCheckOnFree = occypation.Except(buildedObjectOnScene.GetComponent<Entity>().occypiedPoses).ToList();
        }
        exposableStatus = map.isPositionFree(positionForCheckOnFree);
        meshRenderer.material = exposableStatus ? materialAvailable : materialUnvailable;
    }
    
    public void Move(Vector2Int newRoot)
    {
        rootPoint = newRoot;
        occypation = map.GetOccypationPositionForObj(rootPoint, size);
        UpdateExposableStatus();
        transform.position = new Vector3(newRoot.x, 0, newRoot.y);
    }


    public void Expose()
    {
        if (!exposableStatus)
        {
            return;
        }

        if (buildedObjectOnScene == null)
        {
            BuildObject();
        }

        else
        {
            ReplaceObject();
        }
    }

    public void BuildObject()
    {
        if (type.Contains("Build"))
        {
            Transform entity = CreateBuildObject();
            prepareModelToExposing(entity);
            BuildsOnBase.AddAndPrepareBuildComponent(entity.gameObject, model, id, size, occypation.ToArray());
        }
        
        else if (type.Contains("Unit"))
        {
            Transform entity = CreateUnitObject();
            prepareModelToExposing(entity);
            UnitsOnBase.AddAndPrepareUnitComponent(entity.gameObject, model, id, size, occypation.ToArray());
        }

        else
        {
            throw new System.Exception("Неожиданный тип объекта: " + type);
        }

        map.Occypy(occypation);
        AfterExpose();
        EventMaster.current.ExposeObject(id, type, GetOccypationPositionsAsBector(), rotation);
    }

    public void ReplaceObject()
    {
        Entity objectOnSceneAsEntity = buildedObjectOnScene.GetComponent<Entity>();
        objectOnSceneAsEntity.transform.position = transform.position;
        objectOnSceneAsEntity.SetModel(model);
        prepareModelToExposing(objectOnSceneAsEntity.transform);
        SaveReplace();
        CompleteReplace();
    }

    public void SaveReplace()
    {
        Entity parentObjectAsEntity = buildedObjectOnScene.GetComponent<Entity>();
        map.Free(parentObjectAsEntity.occypiedPoses);
        parentObjectAsEntity.SetOccypation(occypation);
        map.Occypy(occypation);

        CacheTable objectsTable = Cache.LoadByName(type);
        CacheItem currentObject = objectsTable.GetById(id);

        currentObject.SetField("position", GetOccypationPositionsAsBector());
        currentObject.SetField("rotation", rotation);
        Cache.Save(objectsTable);
    }

    public Bector2Int[] GetOccypationPositionsAsBector()
    {
        Bector2Int[] positions = new Bector2Int[occypation.Count];
        foreach (Vector2Int pos in occypation)
        {
            positions[occypation.IndexOf(pos)] = new Bector2Int(pos);
        }

        return positions;
    }

    public void AfterExpose()
    {
        Transform newBody = Instantiate(model.gameObject, model.position, model.rotation, null).transform;
        model = newBody;
        model.name = "Model";
        InitModel();
    }

    public Transform CreateBuildObject()
    {
        return BuildsOnBase.CreateBuildObject(rootPoint, name, objectsPool.transform).transform;
    }

    public Transform CreateUnitObject()
    {
        return UnitsOnBase.CreateUnitObject(rootPoint, name, objectsPool.transform).transform;
    }

    public Transform prepareModelToExposing(Transform parent)
    {
        meshRenderer.material = materialModel;
        model.SetParent(parent);
        return model;
    }

    public void CompleteReplace()
    {
        buildedObjectOnScene.transform.position = transform.position;
        buildedObjectOnScene = null;
        EventMaster.current.OnExitBuildMode();
    }

    public void BackModelToStartState()
    {
        transform.position = buildedObjectOnScene.transform.position;

        int oldModelRotation = (int)model.transform.rotation.y;
        RotateModel(buildedObjectOnScene.GetComponent<Entity>().rotation);
        if (oldModelRotation != (int)model.transform.rotation.y) 
        {
            SetNegativeModelOffsetByRotation();
        }

        model.transform.parent = buildedObjectOnScene.transform;
        meshRenderer.material = materialModel;
    }

    public void Cancel()
    {
        EventMaster.current.OnExitBuildMode();
    }

    public void OnExitBuildMode()
    {
        if (buildedObjectOnScene != null)
        {
            BackModelToStartState();
        }

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
