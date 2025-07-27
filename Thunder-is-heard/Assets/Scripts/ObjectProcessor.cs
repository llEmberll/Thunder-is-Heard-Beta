using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectProcessor : MonoBehaviour
{
    public Map map;


    public void Awake()
    {
        map = GameObject.FindGameObjectWithTag(Tags.map).GetComponent<Map>();
        
    }

    public void Start()
    {
        EventMaster.current.ObjectOnBaseWorkStatusChanged += ChangeObjectOnBaseWorkStatus;
    }

    public void PutSelectedObjectOnBattleToInventory()
    {
        ObjectPreview preview = FindObjectOfType<ObjectPreview>();
        if (IsSomeObjectSelected(preview))
        {
            Debug.Log("selected object putted to inv");

            AddGameObjectToInventory(preview.buildedObjectOnScene);
            RemoveObjectFromBattle(FightSceneLoader.parameters._battleId, preview.buildedObjectOnScene);
            preview.buildedObjectOnScene = null;
            preview.Cancel();
        }
    }

    public static void PutSelectedObjectOnBaseToInventory()
    {
        ObjectPreview preview = FindObjectOfType<ObjectPreview>();
        if (IsSomeObjectSelected(preview))
        {
            if (preview.buildedObjectOnScene.name == "����")
            {
                Debug.Log("������ ������ ����");
                return;
            }

            Debug.Log("selected object putted to inv");

            AddGameObjectToInventory(preview.buildedObjectOnScene);
            RemoveObjectFromBase(preview.buildedObjectOnScene);
            preview.buildedObjectOnScene = null;
            preview.Cancel();
        }
    }

    public static bool IsSomeObjectSelected(ObjectPreview preview)
    {
        if (preview == null) 
        {
            Debug.Log("No preview => INVALID INPUT");
            return false;
        }

        if (preview.buildedObjectOnScene == null)
        {
            Debug.Log("No buildedObjectOnScene => INVALID INPUT");
            return false;
        }

        return true;
    }

    public static void AddToInventory(InventoryCacheItem newItem)
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();

        CacheItem[] itemsForAdd = new CacheItem[1] { newItem };
        inventory.Add(itemsForAdd);
        Cache.Save(inventory);

        EventMaster.current.OnAddInventoryItem(newItem);
        EventMaster.current.OnChangeInventory();
    }

    public static void AddGameObjectToInventory(GameObject obj)
    {
        CacheItem coreObjectData = Cache.GetObjectCoreData(obj);
        if (coreObjectData == null)
        {
            Debug.Log("No coreObjectData => INVALID INPUT");
            return;
        }

        Entity entity = obj.GetComponent<Entity>();

        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", coreObjectData.GetExternalId() },
            { "type", entity.Type },
            { "count", 1 }
        }
        );

        AddToInventory(newItem);
    }

    public static void AddUnitToInventory(string unitId, int count = 1)
    {
        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", unitId },
            { "type", "Unit" },
            { "count", count }
        }
        );

        AddToInventory(newItem);
    }

    public void RemoveObjectFromBattle(string battleId, GameObject obj)
    {
        Entity entity = obj.GetComponent<Entity>();

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(cacheItem.Fields);

        if (entity.Type.Contains("Build"))
        {
            BuildOnBattle[] builds = battleData.GetBuilds();
            int indexToRemove = Array.FindIndex(builds, build => build.idOnBattle == entity.ChildId);
            if (indexToRemove == -1)
            {
                return;
            }

            builds = builds.Where((val, idx) => idx != indexToRemove).ToArray();
            battleData.SetBuilds(builds);
        }
        else if (entity.Type.Contains("Unit"))
        {
            UnitOnBattle[] units = battleData.GetUnits();
            int indexToRemove = Array.FindIndex(units, unit => unit.idOnBattle == entity.ChildId);
            if (indexToRemove == -1)
            {
                return;
            }

            units = units.Where((val, idx) => idx != indexToRemove).ToArray();
            battleData.SetUnits(units);
        }
        else
        {
            return;
        }

        Cache.Save(battleTable);

        map.Free(entity.occypiedPoses);

        EventMaster.current.OnRemoveBattleObject(entity);
        EventMaster.current.OnChangeBattleObjects();
    }

    public static  void RemoveObjectFromBase(GameObject obj)
    {
        CacheItem objectData = Cache.GetBasePlayerObjectData(obj);
        if (objectData == null)
        {
            Debug.Log("No BaseObjectData => INVALID INPUT");
            return;
        }

        Entity entity = obj.GetComponent<Entity>();

        CacheTable baseObjectsTable = Cache.LoadByName("Player" + entity.Type);
        baseObjectsTable.DeleteById(objectData.GetExternalId());
        Cache.Save(baseObjectsTable);
        Destroy(obj);

        ProductsNotificationCacheTable notificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem notificationByObject = notificationsTable.FindBySourceObjectId(entity.ChildId);
        if (notificationByObject != null)
        {
            DeleteProductsNotificationByItemId(notificationByObject.GetExternalId());
        }

        EventMaster.current.OnRemoveBaseObject(entity);
        EventMaster.current.OnChangeBaseObjects();
    }

    public void CreateObjectsOnBattleFromSpawnData(UnitOnBattleSpawnData[] unitsSpawnData, BuildOnBattleSpawnData[] buildsSpawnData)
    {
        if (unitsSpawnData != null && unitsSpawnData.Count() > 0)
        {
            foreach (UnitOnBattleSpawnData unitSpawnData in unitsSpawnData)
            {
                UnitOnBattle currentUnit = unitSpawnData.UnitOnBattle;
                RectangleBector2Int occypationAsBector2Int = map.FindNearestFreeRectangle(new RectangleBector2Int(currentUnit.Position), unitSpawnData.PossibleSpawnPositions);
                if (occypationAsBector2Int == null)
                {
                    continue;
                }

                currentUnit.position = occypationAsBector2Int.GetPositions();
                CreateUnitOnBattle(currentUnit);
            }
        }

        if (buildsSpawnData != null && buildsSpawnData.Count() > 0)
        {
            foreach (BuildOnBattleSpawnData buildSpawnData in buildsSpawnData)
            {
                BuildOnBattle currentBuild = buildSpawnData.BuildOnBattle;
                RectangleBector2Int occypationAsBector2Int = map.FindNearestFreeRectangle(new RectangleBector2Int(currentBuild.Position), buildSpawnData.PossibleSpawnPositions);
                if (occypationAsBector2Int == null)
                {
                    continue;
                }

                currentBuild.position = occypationAsBector2Int.GetPositions();
                CreateBuildOnBattle(currentBuild);
            }
        }
    }

    public void CreateObjectsOnBattle(UnitOnBattle[] unitDatas, BuildOnBattle[] buildDatas)
    {
        if (unitDatas != null && unitDatas.Length > 0)
        {
            CreateUnitsOnBattle(unitDatas);
        }
        
        if (buildDatas != null && buildDatas.Length > 0)
        {
            CreateBuildsOnBattle(buildDatas);
        }
    }

    public void CreateUnitsOnBattle(UnitOnBattle[] units)
    {
        foreach (UnitOnBattle unit in units)
        {
            CreateUnitOnBattle(unit);
        }
    }

    public void CreateUnitOnBattle(UnitOnBattle unit)
    {
        CacheItem currentCacheItem = Cache.LoadByType<UnitCacheTable>().GetById(unit.coreId);
        if (currentCacheItem == null) return;
        UnitCacheItem currentCoreUnitData = new UnitCacheItem(currentCacheItem.Fields);
        Transform model = CreateModel(currentCoreUnitData.GetModelPath() + "/" + unit.side, unit.rotation).transform;
        Vector3 unitObjPosition = new Vector3(unit.position.First()._x, 0, unit.position.First()._y);
        model.transform.position += unitObjPosition;

        CreateObjectOnBattle(
            battleId: FightSceneLoader.parameters._battleId,
            coreId: unit.coreId,
            type: "Unit",
            model: model,
            objName: currentCoreUnitData.GetName(),
            size: currentCoreUnitData.GetSize().ToVector2Int(),
            occypation: new List<Vector2Int>() { unit.position.First().ToVector2Int() },
            unit.health,
            unit.side,
            unit.skillsData
            );
    }

    public void CreateBuildsOnBattle(BuildOnBattle[] builds)
    {
        foreach (BuildOnBattle build in builds)
        {
            CreateBuildOnBattle(build);
        }
    }

    public void CreateBuildOnBattle(BuildOnBattle build)
    {
        CacheItem currentCacheItem = Cache.LoadByType<BuildCacheTable>().GetById(build.coreId);
        if (currentCacheItem == null) return;
        BuildCacheItem currentCoreBuildData = new BuildCacheItem(currentCacheItem.Fields);

        Transform model = CreateModel(currentCoreBuildData.GetModelPath() + "/" + build.side, build.rotation).transform;
        Vector3 buildObjPosition = new Vector3(build.position.First()._x, model.transform.position.y, build.position.First()._y);
        model.transform.position += buildObjPosition;

        CreateObjectOnBattle(
            battleId: FightSceneLoader.parameters._battleId,
            coreId: build.coreId,
            type: "Build",
            model: model,
            objName: currentCoreBuildData.GetName(),
            size: currentCoreBuildData.GetSize().ToVector2Int(),
            occypation: Bector2Int.MassiveToVector2Int(build.position).ToList(),
            build.health,
            build.side
            );
    }

    public void CreateObjectOnBattle(
        string battleId, 
        string coreId,
        string type, 
        Transform model,
        string objName, 
        Vector2Int size,
        List<Vector2Int> occypation, 
        int? currentHealth = null, 
        string side = null,
        SkillOnBattle[] skillDatas = null
        )
    {
        if (battleId == null) return;
        if (side == null) side = Sides.federation;

        GameObject entity = null;

        if (type.Contains("Build"))
        {
            BuildCacheTable coreBuildDatas = Cache.LoadByType<BuildCacheTable>();
            CacheItem coreItemData = coreBuildDatas.GetById(coreId);
            BuildCacheItem coreBuildData = new BuildCacheItem(coreItemData.Fields);
            int maxHealth = coreBuildData.GetHealth();
            if (currentHealth == null) currentHealth = maxHealth;
            int damage = coreBuildData.GetDamage();
            int distance = coreBuildData.GetDistance();
            string doctrine = coreBuildData.GetDoctrine();
            string interactionComponentName = "Inaction";
            string interactionComponentType = "";

            BuildOnBattle build = AddNewBuildOnBattleToCache(
                battleId, 
                coreId, 
                Bector2Int.GetVector2IntListAsBector(occypation), 
                (int)model.rotation.eulerAngles.y, 
                maxHealth,
                currentHealth.Value,
                damage,
                distance,
                side
                );

            entity = CreateObject(coreId, type, model, objName, size, occypation);

            AddAndPrepareBuildComponent(
                entity,
                model,
                coreId,
                build.idOnBattle,
                objName,
                size,
                occypation.ToArray(),
                maxHealth,
                currentHealth.Value,
                damage,
                distance,
                doctrine,
                side,
                interactionComponentName,
                interactionComponentType,
                WorkStatuses.idle
                );
        }

        else if (type.Contains("Unit"))
        {
            UnitCacheTable coreUnitDatas = Cache.LoadByType<UnitCacheTable>();
            CacheItem coreItemData = coreUnitDatas.GetById(coreId);
            UnitCacheItem coreUnitData = new UnitCacheItem(coreItemData.Fields);
            int maxHealth = coreUnitData.GetHealth();
            if (currentHealth == null) currentHealth = maxHealth;
            int damage = coreUnitData.GetDamage();
            int distance = coreUnitData.GetDistance();
            int mobility = coreUnitData.GetMobility();
            string unitType = coreUnitData.GetUnitType();
            string doctrine = coreUnitData.GetDoctrine();
            float movementSpeed = coreUnitData.GetMovementSpeed();

            UnitOnBattle unit = AddNewUnitOnBattleToCache(
                battleId, 
                coreId, 
                new Bector2Int(occypation.First()), 
                (int)model.rotation.eulerAngles.y, 
                maxHealth,
                currentHealth.Value,
                damage,
                distance,
                mobility,
                side
                );

            entity = CreateObject(coreId, type, model, objName, size, occypation);

            string[] skillIds = new string[] {};
            if (skillDatas == null)
            {
                skillIds = coreUnitData.GetSkillIds();
            }
            else
            {
                skillIds = SkillOnBattle.GetSkillIdsBySkillOnBattleDatas(skillDatas);
            }

            AddAndPrepareUnitComponent(
                entity,
                model,
                coreId,
                unit.idOnBattle,
                objName,
                size,
                occypation.ToArray(),
                maxHealth,
                currentHealth.Value,
                damage,
                distance,
                mobility,
                side,
                unitType,
                doctrine,
                movementSpeed,
                skillIds
                );
            ConfigureSkills(entity.GetComponent<Unit>(), skillDatas);
        }

        EventMaster.current.ExposeObject(entity.GetComponent<Entity>());
        EventMaster.current.OnChangeBattleObjects();
    }

    public void CreateObjectOnBase(string coreId, string type, Transform model, string objName, Vector2Int size, List<Vector2Int> occypation, string side = null)
    {
        if (side == null) side = Sides.federation;

        GameObject entity = null;
        if (type.Contains("Build"))
        {
            BuildCacheTable coreBuildDatas = Cache.LoadByType<BuildCacheTable>();
            CacheItem coreItemData = coreBuildDatas.GetById(coreId);
            BuildCacheItem coreBuildData = new BuildCacheItem(coreItemData.Fields);
            int health = coreBuildData.GetHealth();
            int currentHealth = health;
            int damage = coreBuildData.GetDamage();
            int distance = coreBuildData.GetDistance();
            string doctrine = coreBuildData.GetDoctrine();
            string interactionComponentName = coreBuildData.GetInteractionComponentName();
            string interactionComponentType = coreBuildData.GetInteractionComponentType();

            PlayerBuildCacheItem playerBuildCacheItem = AddNewBuildOnBaseToCache(
            coreId,
            objName,
            Bector2Int.GetVector2IntListAsBector(occypation),
            Entity.GetDeterminedRotationByModel(model),
            WorkStatuses.idle
            );

            entity = CreateObject(coreId, type, model, objName, size, occypation);

            AddAndPrepareBuildComponent(
                entity,
                model,
                coreId,
                playerBuildCacheItem.GetExternalId(),
                objName,
                size,
                occypation.ToArray(),
                health,
                currentHealth,
                damage,
                distance,
                doctrine,
                side,
                interactionComponentName,
                interactionComponentType,
                WorkStatuses.idle
                );
        }

        else if (type.Contains("Unit"))
        {
            UnitCacheTable coreUnitDatas = Cache.LoadByType<UnitCacheTable>();
            CacheItem coreItemData = coreUnitDatas.GetById(coreId);
            UnitCacheItem coreUnitData = new UnitCacheItem(coreItemData.Fields);
            int health = coreUnitData.GetHealth();
            int currentHealth = health;
            int damage = coreUnitData.GetDamage();
            int distance = coreUnitData.GetDistance();
            int mobility = coreUnitData.GetMobility();
            string unitType = coreUnitData.GetUnitType();
            string doctrine = coreUnitData.GetDoctrine();
            float movementSpeed = coreUnitData.GetMovementSpeed();
            string[] skillIds = coreUnitData.GetSkillIds();

            PlayerUnitCacheItem playerUnitCacheItem = AddNewUnitOnBaseToCache(
            coreId,
            objName,
            Bector2Int.GetVector2IntListAsBector(occypation),
            Entity.GetDeterminedRotationByModel(model)
            );

            entity = CreateObject(coreId, type, model, objName, size, occypation);

            AddAndPrepareUnitComponent(
                entity, 
                model, 
                coreId, 
                playerUnitCacheItem.GetExternalId(), 
                objName, 
                size,
                occypation.ToArray(), 
                health, 
                currentHealth,
                damage, 
                distance, 
                mobility, 
                side,
                unitType,
                doctrine,
                movementSpeed,
                skillIds
                );
        }

        else
        {
            throw new System.Exception("����������� ��� �������: " + type);
        }

        EventMaster.current.ExposeObject(entity.GetComponent<Entity>());
        EventMaster.current.OnChangeBaseObjects();

    }

    public GameObject CreateObject(string coreId, string type, Transform model, string objName, Vector2Int size, List<Vector2Int> occypation)
    {
        Vector2Int rootPoint = occypation.First();
        Transform objectsPool = GameObject.FindWithTag(Config.exposableObjectsTypeToObjectsOnSceneTag[type]).transform;

        GameObject entity = null;
        if (type.Contains("Build"))
        {
            entity = CreateEntityObject(rootPoint, objName, objectsPool);
        }

        else if (type.Contains("Unit"))
        {
            entity = CreateEntityObject(rootPoint, objName, objectsPool);
        }

        else
        {
            throw new System.Exception("����������� ��� �������: " + type);
        }

        //model.transform.position += entity.transform.position;
        model.SetParent(entity.transform);

        map.Occypy(occypation);
        return entity;
    }

    public static GameObject CreateEntityObject(Vector2Int position, string name, Transform parent)
    {
        var prefab = Resources.Load<GameObject>(Config.resources["emptyPrefab"]);
        GameObject obj = Instantiate(
            prefab,
            new Vector3(position.x, 0, position.y),
            Quaternion.identity,
            parent
            );
        obj.name = name;
        return obj;
    }

    public static GameObject CreateModel(string modelPath, int rotation, Transform parent = null)
    {
        GameObject modelPrefab = Resources.Load<GameObject>(modelPath);

        Vector3 position = modelPrefab.transform.position;
        if (parent != null)
        {
            position += parent.transform.position;
        }

        GameObject model = Instantiate(
            modelPrefab, position,
            Quaternion.Euler(new Vector3(0, rotation, 0)),
            parent
            );

        model.name = "Model";
        return model;
    }

    public static GameObject CreateProductsNotificationObject(Vector2Int position, string name, Transform parent)
    {
        var productsNotificationPrefab = Resources.Load<GameObject>(Config.resources["UIProductsNotificationPrefab"]);
        GameObject obj = Instantiate(
            productsNotificationPrefab,
            new Vector3(position.x, productsNotificationPrefab.transform.position.y, position.y),
            productsNotificationPrefab.transform.rotation,
            parent
            );
        obj.name = name;
        return obj;
    }

    public void ReplaceObjectOnBattle(string battleId, GameObject obj, List<Vector2Int> newPosition, int newRotation)
    {
        Entity entity = obj.GetComponent<Entity>();
        obj.transform.position = new Vector3(newPosition.First().x, 0, newPosition.First().y);
        entity.model.SetParent(entity.transform);

        map.Free(entity.occypiedPoses);
        entity.SetOccypation(newPosition);
        map.Occypy(newPosition);

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(cacheItem.Fields);

        if (entity.Type.Contains("Build"))
        {
            BuildOnBattle[] builds = battleData.GetBuilds();
            int indexToChange = Array.FindIndex(builds, build => build.idOnBattle == entity.ChildId);
            if (indexToChange == -1)
            {
                return;
            }

            builds[indexToChange].position = Bector2Int.GetVector2IntListAsBector(newPosition);
            builds[indexToChange].rotation = newRotation;
            battleData.SetBuilds(builds);
        }
        else if (entity.Type.Contains("Unit"))
        {
            UnitOnBattle[] units = battleData.GetUnits();
            int indexToChange = Array.FindIndex(units, unit => unit.idOnBattle == entity.ChildId);
            if (indexToChange == -1)
            {
                return;
            }

            units[indexToChange].position = new Bector2Int[] { new Bector2Int(newPosition.First()) };
            units[indexToChange].rotation = newRotation;
            battleData.SetUnits(units);
        }
        else
        {
            return;
        }

        Cache.Save(battleTable);
        EventMaster.current.OnReplaceBattleObject(entity);
    }

    public void ReplaceObjectOnBase(GameObject obj, List<Vector2Int> newPosition, int newRotation)
    {
        Entity entity = obj.GetComponent<Entity>();

        obj.transform.position = new Vector3(newPosition.First().x, 0, newPosition.First().y);
        entity.model.SetParent(entity.transform);

        map.Free(entity.occypiedPoses);
        entity.SetOccypation(newPosition);
        map.Occypy(newPosition);

        CacheItem baseObjectData = Cache.GetBasePlayerObjectData(obj);

        CacheTable baseObjectsTable = Cache.LoadByName("Player" + entity.Type);
        baseObjectData.SetField("position", Bector2Int.GetVector2IntListAsBector(newPosition));
        baseObjectData.SetField("rotation", newRotation);
        baseObjectsTable.Items[baseObjectData.GetExternalId()] = baseObjectData;
        Cache.Save(baseObjectsTable);

        EventMaster.current.OnReplaceBaseObject(entity);
    }

    public static void AddAndPrepareBuildComponent(
        GameObject buildObj, 
        Transform model, 
        string coreId, 
        string childId,
        string objName, 
        Vector2Int size, 
        Vector2Int[] occypation,
        int health, 
        int currentHealth,
        int damage, 
        int distance, 
        string doctrine,
        string side, 
        string interactionComponentName,
        string interactionComponentType,
        string workStatus
        )
    {
        Build component = buildObj.AddComponent<Build>();
        component.SetName(objName);
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.SetAttributes(health, currentHealth, damage, distance, 0);
        component.SetDoctrine(doctrine);
        component.SetSide(side);

        component.ChangeBehaviour();
            
        component.coreId = coreId;
        component.childId = childId;

        component.interactionComponent = InteractionComponentFactory.GetComponentById(interactionComponentName);
        component.interactionComponent.Init(childId, interactionComponentType);
        component.ChangeWorkStatus(workStatus);

        if (interactionComponentType != "" && IsObjectIdleAndNotHaveProductsNotification(component))
        {
            CreateProductsNotification(component.ChildId, ProductsNotificationTypes.idle);
        }
    }

    public static void AddAndPrepareUnitComponent(
        GameObject unitObj, 
        Transform model, 
        string coreId, 
        string childId,
        string objName, 
        Vector2Int size, 
        Vector2Int[] occypation,
        int health, 
        int currentHealth,
        int damage,
        int distance,
        int mobility, 
        string side,
        string unitType,
        string doctrine,
        float movementSpeed,
        string[] skillIds
        )
    {
        Unit component = unitObj.AddComponent<Unit>();
        component.coreId = coreId;
        component.childId = childId;

        component.SetName(objName);
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.SetAttributes(health, currentHealth, damage, distance, mobility);
        component.SetSide(side);
        component.SetUnitType(unitType);
        component.SetDoctrine(doctrine);
        component.SetMovementSpeed(movementSpeed);

        if (skillIds == null || skillIds.Length < 1) return;
        Skill[] skillComponents = new Skill[skillIds.Length];

        int index = 0;
        foreach (string skillId in skillIds)
        {
            Skill currentSkillComponent = SkillFactory.GetSkillById(skillId);
            skillComponents[index] = currentSkillComponent;
            index++;
        }

        component.SetSkills(skillComponents);
    }

    public static void ConfigureSkills(Unit unit, SkillOnBattle[] skillDatas)
    {
        if (skillDatas == null || skillDatas.Length == 0) return;

        Skill[] skillComponents = unit._skills;

        foreach (Skill skillComponent in skillComponents)
        {
            string currentSkillCoreId = skillComponent.CoreId;
            SkillOnBattle currentSkillOnBattle = null;
            foreach (SkillOnBattle skillOnBattle in skillDatas)
            {
                if (skillOnBattle.coreId == currentSkillCoreId)
                {
                    currentSkillOnBattle = skillOnBattle;
                }
            }

            if (currentSkillOnBattle == null) continue;

            skillComponent.Configure(currentSkillOnBattle);
        }
    }

    public static void AddAndPrepareProductsNotificationComponent(
        GameObject productsNotificationObj,
        string sourceObjectId,
        string type,
        Sprite icon, 
        int productionCount,
        ResourcesData gives
        )
    {
        ProductsNotifcation component = productsNotificationObj.GetComponent<ProductsNotifcation>();
        component.Init(sourceObjectId, type, icon, productionCount, gives);
    }

    public static void AddAndPrepareObstacleComponent(
        GameObject buildObj,
        Transform model,
        string coreId,
        string childId,
        string objName,
        Vector2Int size,
        Vector2Int[] occypation,
        string side
        )
    {
        Obstacle component = buildObj.AddComponent<Obstacle>();
        component.SetName(objName);
        component.SetOriginalSize(size);
        component.SetModel(model);
        component.SetOccypation(new List<Vector2Int>(occypation));
        component.SetSide(side);

        component.ChangeBehaviour();

        component.coreId = coreId;
        component.childId = childId;
    }


    public static PlayerBuildCacheItem AddNewBuildOnBaseToCache(string buildId, string name, Bector2Int[] occypation, int rotation, string workStatus)
    {
        PlayerBuildCacheItem exposedBuildData = new PlayerBuildCacheItem(new Dictionary<string, object>());
        exposedBuildData.SetCoreId(buildId);
        exposedBuildData.SetName(name);
        exposedBuildData.SetPosition(occypation);
        exposedBuildData.SetRotation(rotation);
        exposedBuildData.SetWorkStatus(workStatus);

        PlayerBuildCacheTable exposedBuilds = Cache.LoadByType<PlayerBuildCacheTable>();
        exposedBuilds.Add(new CacheItem[1] { exposedBuildData });
        Cache.Save(exposedBuilds);

        return exposedBuildData;
    }

    public static PlayerUnitCacheItem AddNewUnitOnBaseToCache(string unitId, string name, Bector2Int[] occypation, int rotation)
    {
        PlayerUnitCacheItem exposedUnitData = new PlayerUnitCacheItem(new Dictionary<string, object>());
        exposedUnitData.SetCoreId(unitId);
        exposedUnitData.SetName(name);
        exposedUnitData.SetPosition(occypation);
        exposedUnitData.SetRotation(rotation);

        PlayerUnitCacheTable exposedUnits = Cache.LoadByType<PlayerUnitCacheTable>();
        exposedUnits.Add(new CacheItem[1] { exposedUnitData });
        Cache.Save(exposedUnits);

        return exposedUnitData;
    }

    public static BuildOnBattle AddNewBuildOnBattleToCache(
        string battleId,
        string buildId, 
        Bector2Int[] occypation, 
        int rotation,
        int maxHealth,
        int health,
        int damage,
        int distance,
        string side = null, 
        string workStatus = null
        )
    {
        if (side == null) side = Sides.federation;
        if (workStatus == null) workStatus = WorkStatuses.idle;

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem cacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(cacheItem.Fields);

        BuildOnBattle[] battleBuilds = battleData.GetBuilds();
        BuildOnBattle[] updatedBattleBuilds = new BuildOnBattle[battleBuilds.Length + 1];

        for (int i = 0; i < battleBuilds.Length; i++)
        {
            updatedBattleBuilds[i] = battleBuilds[i];
        }

        BuildCacheTable buildTable = Cache.LoadByType<BuildCacheTable>();
        CacheItem baseBuildCacheItem = buildTable.GetById(buildId);
        BuildCacheItem baseBuildData = new BuildCacheItem(baseBuildCacheItem.Fields);
        string doctrine = baseBuildData.GetDoctrine();

        BuildOnBattle newBuild = new BuildOnBattle(
            buildId,
            occypation,
            rotation,
            maxHealth,
            health,
            damage,
            distance,
            doctrine,
            side,
            workStatus
            );
        updatedBattleBuilds[battleBuilds.Length] = newBuild;

        battleData.SetBuilds(updatedBattleBuilds);

        battleTable.Items[battleData.GetExternalId()] = battleData;
        Cache.Save(battleTable);
        return newBuild;
    }

    public static UnitOnBattle AddNewUnitOnBattleToCache(
        string battleId,
        string unitId,
        Bector2Int occypation,
        int rotation,
        int maxHealth,
        int health,
        int damage,
        int distance,
        int mobility,
        string side = null,
        SkillOnBattle[] skillsData = null
        )
    {
        if (side == null) side = Sides.federation;

        BattleCacheTable battleTable = Cache.LoadByType<BattleCacheTable>();
        CacheItem battleCacheItem = battleTable.GetById(battleId);
        BattleCacheItem battleData = new BattleCacheItem(battleCacheItem.Fields);

        UnitOnBattle[] battleUnits = battleData.GetUnits();
        UnitOnBattle[] updatedBattleUnits = new UnitOnBattle[battleUnits.Length + 1];

        for (int i = 0; i < battleUnits.Length; i++)
        {
            updatedBattleUnits[i] = battleUnits[i];
        }

        UnitCacheTable unitTable = Cache.LoadByType<UnitCacheTable>();
        CacheItem baseUnitsCacheItem = unitTable.GetById(unitId);
        UnitCacheItem unitCacheItem = new UnitCacheItem(baseUnitsCacheItem.Fields);
        string unitType = unitCacheItem.GetUnitType();
        string doctrine = unitCacheItem.GetDoctrine();

        UnitOnBattle newUnit = new UnitOnBattle(
            unitId,
            new Bector2Int[] { occypation },
            rotation,
            maxHealth,
            health,
            damage,
            distance,
            mobility,
            unitType,
            doctrine,
            side,
            unitSkillsData: skillsData
            );
        updatedBattleUnits[battleUnits.Length] = newUnit;

        battleData.SetUnits(updatedBattleUnits);

        battleTable.Items[battleData.GetExternalId()] = battleData;
        Cache.Save(battleTable);
        return newUnit;
    }

    public static void OnBuyMaterial(string materialId)
    {
        InventoryCacheTable inventory = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem newItem = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", materialId },
            { "type", "Material" },
            { "count", 1 }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[1] { newItem };
        inventory.Add(itemsForAdd);
        Cache.Save(inventory);

        EventMaster.current.OnChangeInventory();
    }

    public void ChangeObjectOnBaseWorkStatus(string objectOnBaseId, string newStatus)
    {
        PlayerBuildCacheTable buildsOnBaseTable = Cache.LoadByType<PlayerBuildCacheTable>();
        CacheItem buildOnBaseCacheItem = buildsOnBaseTable.GetById(objectOnBaseId);
        PlayerBuildCacheItem buildOnBase = new PlayerBuildCacheItem(buildOnBaseCacheItem.Fields);

        buildOnBase.SetWorkStatus(newStatus);
        buildsOnBaseTable.AddOne(buildOnBase);

        Cache.Save(buildsOnBaseTable);
    }

    public static bool IsObjectIdleAndNotHaveProductsNotification(Build build)
    {
        if (build.workStatus != WorkStatuses.idle)
        {
            return false;
        }

        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        return productsNotificationsTable.FindBySourceObjectId(build.ChildId) == null;
    }

    public static void CreateProductsNotification(
        string sourceObjectId,
        string notificationType, 
        string iconSection = "",
        string iconName = "",
        int count = 1, 
        ResourcesData gives = null, 
        string unitId = null
        )
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();

        ProductsNotificationCacheItem newProductsNotification = new ProductsNotificationCacheItem(new Dictionary<string, object>());
        newProductsNotification.SetSourceObjectId(sourceObjectId);
        newProductsNotification.SetType(notificationType);
        newProductsNotification.SetIconSection(iconSection);
        newProductsNotification.SetIconName(iconName);
        newProductsNotification.SetCount(count);
        if (gives != null) newProductsNotification.SetGives(gives);
        newProductsNotification.SetUnitId(unitId);

        productsNotificationsTable.AddOne(newProductsNotification);
        Cache.Save(productsNotificationsTable);

        EventMaster.current.OnCreateProductsNotification(newProductsNotification);
    }

    public static void DeleteProductsNotificationBySourceObjectId(string sourceObjectId)
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        ProductsNotificationCacheItem itemForDeletion = productsNotificationsTable.FindBySourceObjectId(sourceObjectId);
        if ( itemForDeletion == null ) 
        {
            return;
        }

        productsNotificationsTable.DeleteById(itemForDeletion.GetExternalId());
        Cache.Save(productsNotificationsTable);

        EventMaster.current.OnDeleteProductsNotification(itemForDeletion);
    }

    public static void DeleteProductsNotificationByItemId(string itemId)
    {
        ProductsNotificationCacheTable productsNotificationsTable = Cache.LoadByType<ProductsNotificationCacheTable>();
        CacheItem item = productsNotificationsTable.GetById(itemId);
        if ( item == null ) { return; }

        ProductsNotificationCacheItem productsNotificationForDeletion = new ProductsNotificationCacheItem(item.Fields);
        productsNotificationsTable.DeleteById(itemId);
        Cache.Save(productsNotificationsTable);
        EventMaster.current.OnDeleteProductsNotification(productsNotificationForDeletion);
    }

    public void ThrowSystemException()
    {
        throw new System.Exception("Искусственно вызванная системная ошибка из ObjectProcessor");
    }
}
