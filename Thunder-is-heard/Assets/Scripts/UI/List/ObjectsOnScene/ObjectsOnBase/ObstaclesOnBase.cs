using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesOnBase : ObjectsOnBase
{
    [SerializeField] public Dictionary<string, Obstacle> items = new Dictionary<string, Obstacle>();


    public override void Awake()
    {
        
    }

    public override void Start()
    {
        this.content = this.transform;
        base.Start();
    }

    public override void EnableListeners()
    {
        EventMaster.current.BaseObjectRemoved += OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed += OnBaseObjectExposed;
    }

    public override void DisableListeners()
    {
        EventMaster.current.BaseObjectRemoved -= OnBaseObjectRemoved;
        EventMaster.current.ObjectExposed -= OnBaseObjectExposed;
    }

    public void OnBaseObjectRemoved(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (!items.ContainsKey(obj.ChildId)) return;
        items.Remove(obj.ChildId);
        Destroy(obj.gameObject);
    }

    public void OnBaseObjectExposed(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (items.ContainsKey(obj.ChildId)) return;

        if (obj == null) return;
        items.Add(obj.ChildId, obj.gameObject.GetComponent<Obstacle>());
    }

    public override bool IsProperType(string type)
    {
        return type.Contains("Obstacle");
    }

    public void UpdateObjects()
    {
        Debug.Log("Update obstacle objects");

        FillContent();
    }

    public override void FillContent()
    {
        FillObstacles();
    }

    public void FillObstacles()
    {
        ClearItems();
        items = new Dictionary<string, Obstacle>();

        PlayerObstacleCacheTable playerObstaclesTable = Cache.LoadByType<PlayerObstacleCacheTable>();
        foreach (var pair in playerObstaclesTable.Items)
        {
            PlayerObstacleCacheItem currentPlayerObstacle = new PlayerObstacleCacheItem(pair.Value.Fields);
            MappingObstacle(currentPlayerObstacle);
        }
    }

    private void MappingObstacle(PlayerObstacleCacheItem playerObstacleData)
    {
        string side = Sides.neutral;

        ObstacleCacheTable obstaclesTable = Cache.LoadByType<ObstacleCacheTable>();
        CacheItem obstacleAsCacheItem = obstaclesTable.GetById(playerObstacleData.GetCoreId());
        ObstacleCacheItem coreObstacleData = new ObstacleCacheItem(obstacleAsCacheItem.Fields);
        string modelPath = coreObstacleData.GetModelPath() + "/" + side;
        Bector2Int size = coreObstacleData.GetSize();

        string name = coreObstacleData.GetName();
        Bector2Int[] position = playerObstacleData.GetPosition();
        int rotation = playerObstacleData.GetRotation();
        string coreId = playerObstacleData.GetCoreId();
        string childId = playerObstacleData.GetExternalId();

        GameObject obstacleObj = ObjectProcessor.CreateEntityObject(position[0].ToVector2Int(), name, this.transform);
        GameObject obstacleModel = ObjectProcessor.CreateModel(modelPath, rotation, obstacleObj.transform);

        map.Occypy(Bector2Int.MassiveToVector2Int(position).ToList());
        SetModelOffsetByRotation(obstacleModel.transform, size, rotation);

        ObjectProcessor.AddAndPrepareObstacleComponent(
            obstacleObj, 
            obstacleModel.transform, 
            coreId, 
            childId, 
            name, 
            size.ToVector2Int(), 
            Bector2Int.MassiveToVector2Int(position),
            side
            );

        items.Add(childId, obstacleObj.GetComponent<Obstacle>());
    }

    public void SetModelOffsetByRotation(Transform model, Bector2Int size, int rotation)
    {
        if (rotation == 0 || rotation == 360 || rotation == 180) return;

        Bector2Int swappedSizeB = new Bector2Int(GetSwappedSize(size._x, size._y));
        float sizeDiff = ((float)swappedSizeB._x - (float)swappedSizeB._y) / 2;

        Vector3 offset = new Vector3(sizeDiff, 0, -1 * sizeDiff);
        model.position += offset;
    }

    public Vector2Int GetSwappedSize(int x, int y)
    {
        x = y + x;
        y = x - y;
        x -= y;

        return new Vector2Int(x, y);
    }

    public override Entity FindObjectByCoreId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Obstacle childEntity = transform.GetChild(i).GetComponent<Obstacle>();
            if (childEntity != null && childEntity.CoreId == id)
            {
                return childEntity;
            }
        }
        return null;
    }

    public override List<Entity> FindAllObjectsByCoreId(string id)
    {
        List<Entity> objs = new List<Entity>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Obstacle childEntity = transform.GetChild(i).GetComponent<Obstacle>();
            if (childEntity != null && childEntity.CoreId == id)
            {
                objs.Add(childEntity);
            }
        }
        return objs;
    }

    public override Entity FindObjectByChildId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Obstacle childEntity = transform.GetChild(i).GetComponent<Obstacle>();
            if (childEntity != null && childEntity.ChildId == id)
            {
                return childEntity;
            }
        }
        return null;
    }
}
