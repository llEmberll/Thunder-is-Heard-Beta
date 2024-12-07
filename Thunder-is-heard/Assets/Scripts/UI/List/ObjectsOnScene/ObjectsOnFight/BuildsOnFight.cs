using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildsOnFight : ObjectsOnFight, IObjectsOnScene
{
    [SerializeField] public Dictionary<string, Build> items = new Dictionary<string, Build>();

    public string _battleId;

    public override void Awake()
    {
        
    }

    public override void Start()
    {
        this.content = this.transform;

        _battleId = FightSceneLoader.parameters._battleId;
        base.Start();
    }

    public override void EnableListeners()
    {
        EventMaster.current.BattleObjectRemoved += OnBattleObjectRemoved;
        EventMaster.current.ObjectExposed += OnBattleObjectExposed;
        EventMaster.current.DestroyedObject += OnBattleObjectRemoved;
    }

    public override void DisableListeners()
    {
        EventMaster.current.BattleObjectRemoved -= OnBattleObjectRemoved;
        EventMaster.current.ObjectExposed -= OnBattleObjectExposed;
        EventMaster.current.DestroyedObject -= OnBattleObjectRemoved;
    }

    public void OnBattleObjectRemoved(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (!items.ContainsKey(obj.ChildId)) return;
        items.Remove(obj.ChildId);
        Destroy(obj.gameObject);

    }

    public void OnBattleObjectExposed(Entity obj)
    {
        if (!IsProperType(obj.Type)) return;
        if (items.ContainsKey(obj.ChildId)) return;
        items.Add(obj.ChildId, obj.gameObject.GetComponent<Build>());
    }

    public override bool IsProperType(string type)
    {
        return type.Contains("Build");
    }

    public void UpdateObjects()
    {
        FillContent();
    }

    public override void FillContent()
    {
        FillBuilds();
    }

    public void FillBuilds()
    {
        ClearItems();
        items = new Dictionary<string, Build>();

        CacheItem battleCacheItem = Cache.LoadByType<BattleCacheTable>().GetById(_battleId);
        BattleCacheItem battleData = new BattleCacheItem(battleCacheItem.Fields);
        BuildOnBattle[] builds = battleData.GetBuilds();
        foreach (BuildOnBattle build in builds)
        {
            MappingBuild(build);
        }
    }

    public void MappingBuild(BuildOnBattle battleBuildData)
    {
        string side = battleBuildData.side;

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem buildAsCacheItem = buildsTable.GetById(battleBuildData.coreId);
        BuildCacheItem coreBuildData = new BuildCacheItem(buildAsCacheItem.Fields);
        string modelPath = coreBuildData.GetModelPath() + "/" + side;
        Bector2Int size = coreBuildData.GetSize();
        int maxHealth = battleBuildData.maxHealth;
        int currentHealth = battleBuildData.health;
        int damage = battleBuildData.damage;
        int distance = battleBuildData.distance;
        string doctrine = coreBuildData.GetDoctrine();
        string interactionComponentName = "Inaction";
        string interactionComponentType = "";

        string name = coreBuildData.GetName();
        Bector2Int[] position = battleBuildData.position;
        int rotation = battleBuildData.rotation;
        string coreId = battleBuildData.coreId;
        string childId = battleBuildData.idOnBattle;
        string workStatus = battleBuildData.workStatus;

        GameObject buildObj = ObjectProcessor.CreateEntityObject(position[0].ToVector2Int(), name, this.transform);
        GameObject buildModel = ObjectProcessor.CreateModel(modelPath, rotation, buildObj.transform);

        map.Occypy(Bector2Int.MassiveToVector2Int(position).ToList());
        SetModelOffsetByRotation(buildModel.transform, size, rotation);

        ObjectProcessor.AddAndPrepareBuildComponent(
            buildObj,
            buildModel.transform,
            coreId,
            childId,
            name,
            size.ToVector2Int(),
            Bector2Int.MassiveToVector2Int(position),
            maxHealth,
            currentHealth,
            damage,
            distance,
            doctrine,
            side,
            interactionComponentName,
            interactionComponentType,
            workStatus
            );

        items.Add(childId, buildObj.GetComponent<Build>());
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
            Build childEntity = transform.GetChild(i).GetComponent<Build>();
            if (childEntity != null && childEntity.CoreId == id)
            {
                return childEntity;
            }
        }
        return null;
    }

    public override Entity FindObjectByChildId(string id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Build childEntity = transform.GetChild(i).GetComponent<Build>();
            if (childEntity != null && childEntity.ChildId == id)
            {
                return childEntity;
            }
        }
        return null;
    }
}
