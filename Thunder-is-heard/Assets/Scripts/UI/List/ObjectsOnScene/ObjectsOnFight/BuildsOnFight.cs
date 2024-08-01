using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildsOnFight : ObjectsOnFight, IObjectsOnScene
{
    public Dictionary<string, Build> items;

    public FightProcessor fightProcessor;

    public override void Start()
    {
        fightProcessor = GameObject.FindGameObjectWithTag(Tags.fightProcessor).GetComponent<FightProcessor>();
        map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        base.Start();
    }

    public override void OnBuildModeEnable()
    {
    }

    public override void FillContent()
    {
        base.FillContent();

        FillBuilds();
    }

    public void FillBuilds()
    {
        items = new Dictionary<string, Build>();

        BuildOnBattle[] builds = fightProcessor._battleData.GetBuilds();
        foreach (BuildOnBattle build in builds)
        {
            MappingBuild(build);
        }
    }

    private void MappingBuild(BuildOnBattle battleBuildData)
    {
        string side = battleBuildData.side;

        BuildCacheTable buildsTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem buildAsCacheItem = buildsTable.GetById(battleBuildData.coreId);
        BuildCacheItem coreBuildData = new BuildCacheItem(buildAsCacheItem.Fields);
        string modelPath = coreBuildData.GetModelPath() + "/" + side;
        Bector2Int size = coreBuildData.GetSize();
        int maxHealth = coreBuildData.GetHealth();
        int currentHealth = battleBuildData.health;
        int damage = coreBuildData.GetDamage();
        int distance = coreBuildData.GetDistance();
        string interactionComponentName = coreBuildData.GetInteractionComponentName();
        string interactionComponentType = coreBuildData.GetInteractionComponentType();

        string name = coreBuildData.GetName();
        Bector2Int[] position = battleBuildData.position;
        int rotation = battleBuildData.rotation;
        string coreId = battleBuildData.coreId;
        string childId = battleBuildData.idOnBattle;
        string workStatus = battleBuildData.workStatus;

        GameObject buildObj = ObjectProcessor.CreateBuildObject(position[0].ToVector2Int(), name, this.transform);
        GameObject buildModel = ObjectProcessor.CreateBuildModel(modelPath, rotation, buildObj.transform);

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
            side,
            interactionComponentName,
            interactionComponentType,
            workStatus
            );
    }

    public void SetModelOffsetByRotation(Transform model, Bector2Int size, int rotation)
    {
        if (rotation == 0 || rotation == 360 || rotation == 180) return;

        Bector2Int swappedSizeB = new Bector2Int(GetSwappedSize(size.x, size.y));
        float sizeDiff = ((float)swappedSizeB.x - (float)swappedSizeB.y) / 2;

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

    public override Entity FindObjectById(string id)
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
}
