using System.Collections.Generic;
using UnityEngine;


public class BaseMap : Map
{
    public override void Awake()
    {
        Init(new Vector2Int(15, 15), Config.terrainsPath["Base"]);
    }

    public void Start()
    {

    }

    public void CreateScenarioForEnemyOutpost()
    {
        ScenarioCacheTable table = Cache.LoadByType<ScenarioCacheTable>();

        ScenarioCacheItem scenarioItem = new ScenarioCacheItem(new Dictionary<string, object>());

        scenarioItem.SetExternalId("aa815447-cdcd-464d-8f37-307f35a15ad6");
        scenarioItem.SetName("Аванпост врага");

        string terrainPath = Config.terrainsPath["mission"];
        terrainPath = terrainPath.Replace("{MissionName}", "EnemyOutpost");

        scenarioItem.SetTerrainPath(terrainPath);

        Bector2Int mapSize = new Bector2Int(27, 36);
        scenarioItem.SetMapSize(mapSize);

        RectangleBector2Int landingRectangle = new RectangleBector2Int(new Bector2Int(9, 0), new Bector2Int(13, 2));
        Bector2Int[] zoneForLanding = landingRectangle.GetPositions();

        LandingData landingData = new LandingData(
            zoneForLanding,
            10
            );

        scenarioItem.SetLanding(landingData);

        // Основные юниты
        UnitOnBattle[] scenarioUnits = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(15, 21) },
                    unitRotation: 180,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "ebd7d163-e87f-4663-9259-11ceef6068d8"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(17, 21) },
                    unitRotation: 180,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "f4651631-bfcd-4f45-9d42-a759b4f25e5d"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(11, 21) },
                    unitRotation: 180,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "9d5bcbe4-e655-452d-a20a-14d6d2080f80"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(11, 25) },
                    unitRotation: 180,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "bdcbab4a-0bf9-4bb6-9a11-da6589264794"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(15, 25) },
                    unitRotation: 180,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "da9b242c-8b4c-47cd-abe9-c62d640e4432"
                    ),
        };

        scenarioItem.SetUnits(scenarioUnits);
        // Основные юниты

        // Основные здания
        BuildOnBattle[] scenarioBuilds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                    coreBuildId: "ba290dde-968d-46ab-868b-b0f7598a7787",
                    new Bector2Int[]
                    {
                        new Bector2Int(15, 22),
                        new Bector2Int(16, 22),
                        new Bector2Int(17, 22),
                        new Bector2Int(15, 23),
                        new Bector2Int(16, 23),
                        new Bector2Int(17, 23)
                    },
                    -90,
                    6,
                    6,
                    0,
                    0,
                    Doctrines.land,
                    Sides.empire,
                    WorkStatuses.idle,
                    buildIdOnBattle: "88b65738-bacc-4455-9adf-d1daee3ebc24"
                ),
            new BuildOnBattle(
                    coreBuildId: "9b2cf240-5f63-4107-8751-eb91b95b94d9",
                    new Bector2Int[]
                    {
                        new Bector2Int(10, 16),
                        new Bector2Int(11, 16),
                        new Bector2Int(12, 16),
                        new Bector2Int(10, 17),
                        new Bector2Int(11, 17),
                        new Bector2Int(12, 17),
                        new Bector2Int(10, 18),
                        new Bector2Int(11, 18),
                        new Bector2Int(12, 18),
                    },
                    -90,
                    9,
                    9,
                    0,
                    0,
                    Doctrines.land,
                    Sides.empire,
                    WorkStatuses.idle,
                    buildIdOnBattle: "5168ce99-2415-4eb2-9cc4-530174d7ef4a"
                )
        };

        scenarioItem.SetBuilds(scenarioBuilds);
        // Основные здания

        // Основные препятствия
        float obstacleFillChance = 0.8f;
        string obstacleSide = Sides.neutral;
        int[] obstaclePossibleRotations = new int[] { 0, 90, -90, 180 };
        Dictionary<string, int> coreObstacleIdsWithChanceMultiplier = new Dictionary<string, int>()
        {
            { "bb9da51e-303d-4aed-951d-0248490f76b6", 2 },
            { "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a", 3 },
            { "7b6c7782-0a71-4501-87f5-ed18b935cea1", 2 },
            { "1dc74e29-35f1-4fc5-a3f0-d2c6c39d558b", 2 },
            { "90ebae2b-7633-42c7-9868-406d47583d5a", 1 },
        };
        Bector2Int mainRectangleForFillObstacles = mapSize;
        List<RectangleBector2Int> exclusionZonesForFillObstacles = new List<RectangleBector2Int>()
        {
            landingRectangle,
            new RectangleBector2Int(new Bector2Int(9, 14), new Bector2Int(15, 35)),
            new RectangleBector2Int(new Bector2Int(16, 18), new Bector2Int(18, 27)),
            new RectangleBector2Int(new Bector2Int(12, 0), new Bector2Int(14, 13)),
        };

        ObstacleOnBattle[] scenarioObstacles = ObstacleFiller.Fill(
            coreObstacleIdsWithChanceMultiplier, 
            mainRectangleForFillObstacles, 
            exclusionZonesForFillObstacles, 
            obstacleFillChance, 
            obstaclePossibleRotations, 
            obstacleSide
            );

        scenarioItem.SetObstacles(scenarioObstacles);
        // Основные препятствия

        // Начальный диалог сценария
        Replic[] scenarioStartDialogue = new Replic[]
        {
            new Replic(
                charName: Chars.officer, 
                charSide: Sides.federation, 
                text: "Командир! Согласно данным разведки, враг планирует укрепить аванпост неподалёку от нас. Похоже мы их цель. Нужно сорвать их планы и уничтожить плацдарм для наступления на нас! ")
        };
        scenarioItem.SetStartDialogue(scenarioStartDialogue);
        // Начальный диалог сценария

        // Этапы
        //// Этап 1
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage1 = new AISettings(
            "Frozen",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage1 = new AISettings(
            "Frozen",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        string sideForConditionsDataStage1 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage1 = new RectangleBector2Int(new Bector2Int(0, 14), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionData1Stage1 = new Dictionary<string, object>() { { "side", sideForConditionsDataStage1 }, { "positionRectangle", rectangleForConditionDataStage1 } };
        ConditionData victoryConditionData1ForStage1 = new ConditionData(type: "SideReachPosition", dataForConditionData1Stage1);

        Dictionary<string, object> dataForConditionData2Stage1 = new Dictionary<string, object>() { { "attackerSide", sideForConditionsDataStage1 }, { "targetObjectId", "5168ce99-2415-4eb2-9cc4-530174d7ef4a" } };
        ConditionData victoryConditionData2ForStage1 = new ConditionData(type: "ReachToAttackObject", dataForConditionData2Stage1);

        Dictionary<string, object> victoryDataForConditionDataStage1 = new Dictionary<string, object>() { { "conditions", new ConditionData[] { victoryConditionData1ForStage1, victoryConditionData2ForStage1 } } };
        ConditionData victoryConditionDataForStage1 = new ConditionData(type: "Or", victoryDataForConditionDataStage1);

        ConditionData defeatConditionDataForStage1 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage1 = new Replic[]
        {
            new Replic(
                charName: Chars.officer, 
                charSide: Sides.federation, 
                text: "Взгляните Командир! Позиции укреплены, однако малочисленны. По всей видимости, прошлое нападение исходило отсюда",
                focus: new Bector2Int(14, 25)
                ),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Обратите внимание на количество войск противника. Их немало, но, если мы их разобьём по частям, мы существенно облегчим себе задачу"),
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Снежана правду говорит. Рекомендую слушать её советы. Иногда они могут вам здорово помочь"),
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage1 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// Диалог в случае провала
        StageData stage1 = new StageData(
                stageUnits: null,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage1,
                stageReplicsOnPass: null,
                stageReplicsOnFail: failDialogueForStage1,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage1, AISettingsForNeutralSideInStage1 },
                stageConditionsForFail: defeatConditionDataForStage1,
                stageConditionsForPass: victoryConditionDataForStage1
                );
        //// Этап 1

        //// Этап 2
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage2 = new AISettings(
            "Frozen",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage2 = new AISettings(
            "Frozen",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        string[] targetObjectIdsToDestroyForStage1 = new string[] { "5168ce99-2415-4eb2-9cc4-530174d7ef4a" };
        Dictionary<string, object> dataForVictoryConditionDataWithDestroyHeadbuildStage2 = new Dictionary<string, object>() { { "targetObjectIds", targetObjectIdsToDestroyForStage1 } };
        ConditionData victoryConditionDataWithDestroyHeadbuildForStage2 = new ConditionData(type: "DestroyObjects", dataForVictoryConditionDataWithDestroyHeadbuildStage2);

        string sideForConditionDataStage2 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage2 = new RectangleBector2Int(new Bector2Int(0, 19), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionDataStage2 = new Dictionary<string, object>() { { "side", sideForConditionDataStage2 }, { "positionRectangle", rectangleForConditionDataStage2 } };
        ConditionData defeatConditionDataForStage2 = new ConditionData(type: "SideReachPosition", dataForConditionDataStage2);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage2 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Спешите атаковать штаб, пока они не запросили подкрепление!")
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage2 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Вражеские подкрепления на месте, нужно было уничтожить штаб пока была возможность! Нельзя допустить окружения, отступаем...")
        };
        //// Диалог в случае провала
        StageData stageOnPassStage1 = new StageData(
                stageUnits: null,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage2,
                stageReplicsOnPass: null,
                stageReplicsOnFail: null,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage2, AISettingsForNeutralSideInStage2 },
                stageConditionsForFail: defeatConditionDataForStage2,
                stageConditionsForPass: victoryConditionDataWithDestroyHeadbuildForStage2
                );
        //// Этап 2

        //// Этап 2.1
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage21 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage21 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        ConditionData victoryConditionDataForStage21 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage21 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage21 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation, 
                text: "Вражеские подкрепления на месте, нужно было уничтожить штаб пока была возможность...",
                focus: new Bector2Int(13, 25)
                )
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage21 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// Диалог в случае провала
        
        //// Диалог в случае успеха
        Replic[] passDialogueForStage21 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// Диалог в случае успеха

        //// Юниты
        UnitOnBattle[] stage21Units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "035655f8-a347-4057-87cc-83385fa20660",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 6,
                    unitHealth: 6,
                    unitDamage: 3,
                    unitDistance: 3,
                    unitMobility: 4,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "844b3458-b8c4-4fc5-a72b-44033ea5836c"
                    ),
            new UnitOnBattle(
                    coreUnitId: "035655f8-a347-4057-87cc-83385fa20660",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 34) },
                    unitRotation: 180,
                    unitMaxHealth: 6,
                    unitHealth: 6,
                    unitDamage: 3,
                    unitDistance: 3,
                    unitMobility: 4,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "e512b440-1c13-460d-b399-6e42a3823eec"
                    ),
            new UnitOnBattle(
                    coreUnitId: "a480c091-6b22-43f2-b26d-bbd3d2c2905b",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 5,
                    unitHealth: 5,
                    unitDamage: 2,
                    unitDistance: 1,
                    unitMobility: 5,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitSkillsData: new SkillOnBattle[] { new SkillOnBattle("a60f271d-4aa1-41af-b6d1-c89ed667dad6", 0, false) },
                    unitIdOnBattle: "44310c6b-a692-4fec-a2f6-56862f95f7c5"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(12, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "479e4109-c2f5-4b67-90f7-a79d5563bf6f"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(14, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "103f5510-8b4a-4755-a56e-ae2fab4d1259"
                    ),
        };
        //// Юниты
        StageData stageOnFailStage2 = new StageData(
                stageUnits: stage21Units,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage21,
                stageReplicsOnPass: passDialogueForStage21,
                stageReplicsOnFail: failDialogueForStage21,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage21, AISettingsForNeutralSideInStage21 },
                stageConditionsForFail: defeatConditionDataForStage21,
                stageConditionsForPass: victoryConditionDataForStage21
                );
        //// Этап 2.1

        //// Этап 3
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage3 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage3 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        
        ConditionData victoryConditionDataForStage3 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage3 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage3 = new Replic[]
        {
            new Replic(
                charName: Chars.fighter, 
                charSide: Sides.empire, 
                text: "Вы что, совсем страх потеряли?! Парни, немедленно принести мне головы тех наглецов!", 
                focus: new Bector2Int(11, 17)
                ),
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Не бойтесь! Неожиданность – наше преимущество! Подведите все войска и используйте перекрёстный огонь против наступающих солдат")
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage3 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// Диалог в случае провала
        StageData stageOnPassStage2 = new StageData(
                stageUnits: null,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage3,
                stageReplicsOnPass: null,
                stageReplicsOnFail: failDialogueForStage3,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage3, AISettingsForNeutralSideInStage3 },
                stageConditionsForFail: defeatConditionDataForStage3,
                stageConditionsForPass: victoryConditionDataForStage3
                );
        //// Этап 3

        //// Этап 4
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage4 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage4 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        string[] targetObjectIdsToDestroy = new string[] { "88b65738-bacc-4455-9adf-d1daee3ebc24" };
        Dictionary<string, object> dataForVictoryConditionDataWithDestroyTentStage4 = new Dictionary<string, object>() { { "targetObjectIds", targetObjectIdsToDestroy } };
        ConditionData victoryConditionDataWithDestroyTentForStage4 = new ConditionData(type: "DestroyObjects", dataForVictoryConditionDataWithDestroyTentStage4);

        string sideForConditionDataStage4 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage4 = new RectangleBector2Int(new Bector2Int(0, 25), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionDataStage4 = new Dictionary<string, object>() { { "side", sideForConditionDataStage4 }, { "positionRectangle", rectangleForConditionDataStage4 } };
        ConditionData defeatConditionDataForStage4 = new ConditionData(type: "SideReachPosition", dataForConditionDataStage4);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage4 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Прекрасно! Гарнизон разбит! Уничтожьте их казарму и с ними будет покончено!", focus: new Bector2Int(17, 22))
        };
        //// Cтартовый диалог

        StageData stageOnPassStage3 = new StageData(
                stageUnits: null,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage4,
                stageReplicsOnPass: null,
                stageReplicsOnFail: null,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage4, AISettingsForNeutralSideInStage4 },
                stageConditionsForFail: defeatConditionDataForStage4,
                stageConditionsForPass: victoryConditionDataWithDestroyTentForStage4
                );
        //// Этап 4
        
        //// Этап 4.1
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage41 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage41 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        ConditionData victoryConditionDataForStage41 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage41 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage41 = new Replic[]
        {
            new Replic(
                charName: Chars.officer, 
                charSide: Sides.federation, 
                text: "Вражеские подкрепления на месте, нужно было уничтожить казарму и отходить пока была возможность...",
                focus: new Bector2Int(13, 25)
                )
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage41 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// Диалог в случае провала

        //// Диалог в случае успеха
        Replic[] passDialogueForStage41 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// Диалог в случае успеха

        //// Юниты
        UnitOnBattle[] stage41Units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "035655f8-a347-4057-87cc-83385fa20660",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 6,
                    unitHealth: 6,
                    unitDamage: 3,
                    unitDistance: 3,
                    unitMobility: 4,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "844b3458-b8c4-4fc5-a72b-44033ea5836c"
                    ),
            new UnitOnBattle(
                    coreUnitId: "035655f8-a347-4057-87cc-83385fa20660",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 34) },
                    unitRotation: 180,
                    unitMaxHealth: 6,
                    unitHealth: 6,
                    unitDamage: 3,
                    unitDistance: 3,
                    unitMobility: 4,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "e512b440-1c13-460d-b399-6e42a3823eec"
                    ),
            new UnitOnBattle(
                    coreUnitId: "a480c091-6b22-43f2-b26d-bbd3d2c2905b",
                    unitPosition: new Bector2Int[] { new Bector2Int(13, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 5,
                    unitHealth: 5,
                    unitDamage: 2,
                    unitDistance: 1,
                    unitMobility: 5,
                    UnitTypes.vehicle,
                    Doctrines.land,
                    Sides.empire,
                    unitSkillsData: new SkillOnBattle[] { new SkillOnBattle("a60f271d-4aa1-41af-b6d1-c89ed667dad6", 0, false) },
                    unitIdOnBattle: "44310c6b-a692-4fec-a2f6-56862f95f7c5"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(12, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "479e4109-c2f5-4b67-90f7-a79d5563bf6f"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(14, 33) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "103f5510-8b4a-4755-a56e-ae2fab4d1259"
                    ),
        };
        //// Юниты
        StageData stageOnFailStage4 = new StageData(
                stageUnits: stage41Units,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage41,
                stageReplicsOnPass: passDialogueForStage41,
                stageReplicsOnFail: failDialogueForStage41,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage41, AISettingsForNeutralSideInStage41 },
                stageConditionsForFail: defeatConditionDataForStage41,
                stageConditionsForPass: victoryConditionDataForStage41
                );
        //// Этап 4.1

        //// Этап 5
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage5 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage5 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        ConditionData victoryConditionDataForStage5 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage5 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage5 = new Replic[]
        {
            new Replic(charName: Chars.fighter, charSide: Sides.empire, text: "Ну что вы встали, глупцы?! Атакуйте этих никчёмных бездарей!", focus: new Bector2Int(14, 25)),
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage5 = new Replic[]
        {
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Их подкрепления испортили нам всю операцию. Отступаем, пока не стало ещё хуже!")
        };
        //// Диалог в случае провала

        //// Юниты
        UnitOnBattle[] stage5Units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(11, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "4aca4a22-f735-4f57-9300-9477c9e9c98f"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(15, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "bad369e3-4b71-430c-9d04-eb9a7b3fa9e0"
                    ),
        };
        //// Юниты
        StageData stageOnPassStage4 = new StageData(
                stageUnits: stage5Units,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage5,
                stageReplicsOnPass: null,
                stageReplicsOnFail: failDialogueForStage5,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage5, AISettingsForNeutralSideInStage5 },
                stageConditionsForFail: defeatConditionDataForStage5,
                stageConditionsForPass: victoryConditionDataForStage5
                );
        //// Этап 5

        //// Этап 6
        ///// ИИ
        AISettings AISettingsForEmpireSideInStage6 = new AISettings(
            "Attacking",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage6 = new AISettings(
            "Attacking",
            Sides.neutral,
            null,
            null
            );
        ///// ИИ

        //// Условия
        ConditionData victoryConditionDataForStage6 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage6 = new ConditionData(type: "DestroyAllAllies", null);
        //// Условия

        //// Стартовый диалог
        Replic[] startDialogueForStage6 = new Replic[]
        {
            new Replic(
                charName: Chars.tankGirl, 
                charSide: Sides.federation, 
                text: "Командир, аккуратней! Неизвестно, сколько ещё подкреплений они могут вызвать. Хорошенько подумайте про оборону. Старайтесь не допускать потерь…",
                focus: new Bector2Int(13, 25)
                )
        };
        //// Cтартовый диалог

        //// Диалог в случае провала
        Replic[] failDialogueForStage6 = new Replic[]
        {
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Их подкрепления испортили нам всю операцию. Отступаем, пока не стало ещё хуже!")
        };
        //// Диалог в случае провала

        //// Диалог в случае успеха
        Replic[] passDialogueForStage6 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// Диалог в случае успеха

        //// Юниты
        UnitOnBattle[] stage6Units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(11, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "103978ed-2c8e-4c7d-8d3e-37faaf22c786"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(15, 35) },
                    unitRotation: 180,
                    unitMaxHealth: 3,
                    unitHealth: 3,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "38e2b5e4-d82f-4c77-9635-34e58fa28dec"
                    ),
        };
        //// Юниты
        StageData stageOnPassStage5 = new StageData(
                stageUnits: stage6Units,
                stageBuilds: null,
                stageReplicsOnStart: startDialogueForStage6,
                stageReplicsOnPass: passDialogueForStage6,
                stageReplicsOnFail: failDialogueForStage6,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage6, AISettingsForNeutralSideInStage6 },
                stageConditionsForFail: defeatConditionDataForStage6,
                stageConditionsForPass: victoryConditionDataForStage6
                );
        //// Этап 6

        stage1.stageOnPass = stageOnPassStage1;
        stageOnPassStage1.stageOnPass = stageOnPassStage2;
        stageOnPassStage1.stageOnFail = stageOnFailStage2;
        stageOnPassStage2.stageOnPass = stageOnPassStage3;
        stageOnPassStage3.stageOnPass = stageOnPassStage4;
        stageOnPassStage3.stageOnFail = stageOnFailStage4;
        stageOnPassStage4.stageOnPass = stageOnPassStage5;
        // Этапы

        scenarioItem.SetStartStage(stage1);
        table.AddOne(scenarioItem);
        Cache.Save(table);
    }

    public void CreateMissionForTest()
    {
        MissionCacheTable table = Cache.LoadByType<MissionCacheTable>();

        MissionCacheItem missionItem = new MissionCacheItem(new Dictionary<string, object>());

        string missionId = "589dc04e-153d-46b7-bb57-db641aaae115";
        string name = "First step";
        string description = "Description";
        Bector2Int poseOnMap = new Bector2Int(new Vector2Int(-425, -237));
        ResourcesData gives = new ResourcesData(rubCount: 650, expCount: 100);

        missionItem.SetName(name);
        missionItem.SetDescription(description);
        missionItem.SetExternalId(missionId);
        missionItem.SetPoseOnMap(poseOnMap);
        missionItem.SetGives(gives);

        table.AddOne(missionItem);
        Cache.Save(table);
    }

    public void CreateBattleForTest()
    {
        BattleCacheTable table = Cache.LoadByType<BattleCacheTable>();

        BattleCacheItem battleItem = new BattleCacheItem(new Dictionary<string, object>());
        string missionId = "123";
        string turn = Sides.empire;

        CellData[] map = new CellData[]
        {
            new CellData("land", new Bector2Int(new Vector2Int(0, 0))),
            new CellData("water", new Bector2Int(new Vector2Int(0, 1))),
        };

        UnitOnBattle[] units = new UnitOnBattle[]
        {
            new UnitOnBattle(
                coreUnitId: "124", 
                unitPosition: new Bector2Int[] { new Bector2Int(new Vector2Int(0, 0)) }, 
                unitRotation: 0, 
                unitMaxHealth:3, 
                unitHealth: 3, 
                unitDamage: 1, 
                unitDistance: 2, 
                unitMobility: 2,
                UnitTypes.infantry,
                Doctrines.land,
                unitSide: Sides.empire, 
                unitIdOnBattle: "987"
                ),
            new UnitOnBattle(
                coreUnitId: "125",
                unitPosition: new Bector2Int[] { new Bector2Int(new Vector2Int(0, 1)) },
                unitRotation: 0,
                unitMaxHealth:3,
                unitHealth: 2,
                unitDamage: 1,
                unitDistance: 2,
                unitMobility: 2,
                UnitTypes.infantry,
                Doctrines.land,
                unitSide: Sides.federation, "986" , 
                unitSkillsData: new SkillOnBattle[] { new SkillOnBattle("224", 1, false) } 
                )
        };

        Bector2Int[] positionForBuild1 = new Bector2Int[] {new Bector2Int(new Vector2Int(0, 0))};
        Bector2Int[] positionForBuild2 = new Bector2Int[] { new Bector2Int(new Vector2Int(0, 1)), new Bector2Int(new Vector2Int(1, 1)) };

        BuildOnBattle[] builds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                coreBuildId: "124", 
                buildPosition: positionForBuild1, 
                buildRotation: 0, 
                buildMaxHealth: 3,
                buildHealth: 2,
                buildDamage: 0, 
                buildDistance: 0,
                buildDoctrine: Doctrines.land,
                buildSide: Sides.empire, 
                buildWorkStatus: WorkStatuses.idle, 
                buildIdOnBattle: "876"
                ),
            new BuildOnBattle(
                coreBuildId: "125",
                buildPosition: positionForBuild2,
                buildRotation: 0,
                buildMaxHealth: 4,
                buildHealth: 4,
                buildDamage: 0,
                buildDistance: 0,
                buildDoctrine: Doctrines.land,
                buildSide: Sides.federation,
                buildWorkStatus: WorkStatuses.idle,
                buildIdOnBattle: "875"
                ),
        };

        battleItem.SetMissionId(missionId);
        battleItem.SetUnits(units);
        battleItem.SetBuilds(builds);
        battleItem.SetTurn(turn);

        table.AddOne(battleItem);
        Cache.Save(table);
    }

    public void CreateResources()
    {
        ResourcesCacheTable resourceTable = Cache.LoadByType<ResourcesCacheTable>();

        CacheItem data = resourceTable.GetById("5u5df540-5f63-3107-8781-eb91b95b84i1");

        ResourcesCacheItem resources = new ResourcesCacheItem(new Dictionary<string, object>());
        if (data == null)
        {
            resources = new ResourcesCacheItem(new Dictionary<string, object>());
            resources.SetExternalId("5u5df540-5f63-3107-8781-eb91b95b84i1");
            resources.SetResources(new ResourcesData());
        }
        else
        {
            resources = new ResourcesCacheItem(data.Fields);
            
        }

        resources.SetResources(new ResourcesData());
        resourceTable.Add(new CacheItem[1] { resources });
        Cache.Save(resourceTable);

        ResourcesCacheTable newResourcesTable = Cache.LoadByType<ResourcesCacheTable>();
        CacheItem newData = newResourcesTable.GetById("5u5df540-5f63-3107-8781-eb91b95b84i1");
        ResourcesCacheItem newResources = new ResourcesCacheItem(newData.Fields);
        ResourcesData resourcesData = newResources.GetResources();

    }

    public void CreateBuilds()
    {
        BuildCacheTable buildTable = Cache.LoadByType<BuildCacheTable>();

        ResourcesData headBuildCost = new ResourcesData();

        ResourcesData oilStationCost = new ResourcesData();

        ResourcesData mineCost = new ResourcesData();

        ResourcesData officeCost = new ResourcesData();

        ResourcesData tentCost = new ResourcesData();

        ResourcesData warehouseCost = new ResourcesData();

        ResourcesData trainingCenterCost = new ResourcesData();

        ResourcesData factoryCost = new ResourcesData();

        mineCost.rub = 750;
        mineCost.oil = 2;
        mineCost.steel = 3;

        oilStationCost.rub = 650;
        oilStationCost.oil = 1;
        oilStationCost.steel = 3;

        officeCost.rub = 1250;
        officeCost.oil = 3;
        officeCost.steel = 5;

        tentCost.rub = 600;
        tentCost.oil = 1;
        tentCost.steel = 2;

        warehouseCost.rub = 500;
        warehouseCost.steel = 3;

        trainingCenterCost.rub = 1600;
        trainingCenterCost.oil = 2;
        trainingCenterCost.steel = 6;

        factoryCost.rub = 3500;
        factoryCost.oil = 5;
        factoryCost.steel = 10;

        ResourcesData headBuildGives = new ResourcesData();
        headBuildGives.maxOil = 8;
        headBuildGives.maxSteel = 10;
        headBuildGives.maxStaff = 50;

        ResourcesData mineGives = new ResourcesData();
        mineGives.maxSteel = 2;

        ResourcesData oilStationGives = new ResourcesData();
        oilStationGives.maxOil = 2;

        ResourcesData officeGives = new ResourcesData();

        ResourcesData tentGives = new ResourcesData();
        tentGives.maxStaff = 5;

        ResourcesData warehouseGives = new ResourcesData();
        warehouseGives.maxOil = 5;
        warehouseGives.maxSteel = 8;

        ResourcesData trainingCenterGives = new ResourcesData();
        trainingCenterGives.maxStaff = 2;

        ResourcesData factoryGives = new ResourcesData();

        BuildCacheItem headbuild = new BuildCacheItem(new Dictionary<string, object>());

        headbuild.SetExternalId("9b2cf240-5f63-4107-8751-eb91b95b94d9");
        headbuild.SetName("Headbuild");
        headbuild.SetModelPath("Prefabs/Entity/Builds/Headbuild");
        headbuild.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        headbuild.SetCost(headBuildCost);
        headbuild.SetGives(headBuildGives);
        headbuild.SetCreateTime(0);
        headbuild.SetHealth(12);
        headbuild.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        headbuild.SetIconName("headbuild_card");

        BuildCacheItem mine = new BuildCacheItem(new Dictionary<string, object>());

        mine.SetExternalId("4b8a1805-3af8-4144-8bdb-62c93852b443");
        mine.SetName("Mine");
        mine.SetModelPath("Prefabs/Entity/Builds/Mine");
        mine.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        mine.SetCost(mineCost);
        mine.SetGives(mineGives);
        mine.SetCreateTime(0);
        mine.SetHealth(6);
        mine.SetDistance(0);
        mine.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        mine.SetIconName("mine_card");
        mine.SetInteractionComponentName("ContractComponent");
        mine.SetInteractionComponentType(InteractionComponentTypes.steelContract);

        BuildCacheItem oilStation = new BuildCacheItem(new Dictionary<string, object>());

        oilStation.SetExternalId("64a4568c-bfaf-408e-9537-8e489ccaca56");
        oilStation.SetName("OilStation");
        oilStation.SetModelPath("Prefabs/Entity/Builds/OilStation");
        oilStation.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        oilStation.SetCost(oilStationCost);
        oilStation.SetGives(oilStationGives);
        oilStation.SetCreateTime(0);
        oilStation.SetHealth(6);
        oilStation.SetDistance(0);
        oilStation.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        oilStation.SetIconName("oil_station_card");
        oilStation.SetInteractionComponentName("ContractComponent");
        oilStation.SetInteractionComponentType(InteractionComponentTypes.oilContract);

        BuildCacheItem office = new BuildCacheItem(new Dictionary<string, object>());

        office.SetExternalId("8878498b-a4bc-4dc8-8f39-bc9e987a689f");
        office.SetName("Office");
        office.SetModelPath("Prefabs/Entity/Builds/Office");
        office.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        office.SetCost(officeCost);
        office.SetGives(officeGives);
        office.SetCreateTime(0);
        office.SetHealth(8);
        office.SetDistance(0);
        office.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        office.SetIconName("office_card");
        office.SetInteractionComponentName("ContractComponent");
        office.SetInteractionComponentType(InteractionComponentTypes.rubContract);

        BuildCacheItem tent = new BuildCacheItem(new Dictionary<string, object>());

        tent.SetExternalId("ba290dde-968d-46ab-868b-b0f7598a7787");
        tent.SetName("Tent");
        tent.SetModelPath("Prefabs/Entity/Builds/Tent");
        tent.SetSize(new Bector2Int(new Vector2Int(2, 3)));
        tent.SetCost(tentCost);
        tent.SetGives(tentGives);
        tent.SetCreateTime(0);
        tent.SetHealth(5);
        tent.SetDistance(0);
        tent.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        tent.SetIconName("tent_card");

        BuildCacheItem warehouse = new BuildCacheItem(new Dictionary<string, object>());

        warehouse.SetExternalId("3d9f0f22-409e-40d7-8511-f4584b583dc0");
        warehouse.SetName("Warehouse");
        warehouse.SetModelPath("Prefabs/Entity/Builds/Warehouse");
        warehouse.SetSize(new Bector2Int(new Vector2Int(2, 3)));
        warehouse.SetCost(warehouseCost);
        warehouse.SetGives(warehouseGives);
        warehouse.SetCreateTime(0);
        warehouse.SetHealth(7);
        warehouse.SetDistance(0);
        warehouse.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        warehouse.SetIconName("warehouse_card");

        BuildCacheItem trainingCenter = new BuildCacheItem(new Dictionary<string, object>());

        trainingCenter.SetExternalId("065a4f61-5b0e-450e-b89b-299651f90b4d");
        trainingCenter.SetName("TrainingCenter");
        trainingCenter.SetModelPath("Prefabs/Entity/Builds/TrainingCenter");
        trainingCenter.SetSize(new Bector2Int(new Vector2Int(3, 3)));
        trainingCenter.SetCost(trainingCenterCost);
        trainingCenter.SetGives(trainingCenterGives);
        trainingCenter.SetCreateTime(0);
        trainingCenter.SetHealth(8);
        trainingCenter.SetDistance(0);
        trainingCenter.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        trainingCenter.SetIconName("training_center_card");
        trainingCenter.SetInteractionComponentName("UnitProductionComponent");
        trainingCenter.SetInteractionComponentType(InteractionComponentTypes.infantryUnitProduction);

        BuildCacheItem factory = new BuildCacheItem(new Dictionary<string, object>());

        factory.SetExternalId("7dfc9c80-ddac-45e9-bcdf-61166e068cad");
        factory.SetName("Factory");
        factory.SetModelPath("Prefabs/Entity/Builds/Factory");
        factory.SetSize(new Bector2Int(new Vector2Int(2, 2)));
        factory.SetCost(factoryCost);
        factory.SetGives(factoryGives);
        factory.SetCreateTime(0);
        factory.SetHealth(9);
        factory.SetDistance(0);
        factory.SetIconSection(Config.resources["UICards"] + "Builds/" + "cards");
        factory.SetIconName("factory_card");
        factory.SetInteractionComponentName("UnitProductionComponent");
        factory.SetInteractionComponentType(InteractionComponentTypes.vehicleUnitProduction);


        Debug.Log("build created and prepared");

        CacheItem[] itemsForAdd = new CacheItem[8] { headbuild, mine , oilStation , office, tent, warehouse , trainingCenter, factory};
        buildTable.Add(itemsForAdd);

        Debug.Log("build added to table");

        Cache.Save(buildTable);

    }

    public void CreateUnits()
    {
        UnitCacheTable unitsTable = Cache.LoadByType<UnitCacheTable>();
        UnitCacheItem assaulters = new UnitCacheItem(new Dictionary<string, object>());
        assaulters.SetExternalId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");
        assaulters.SetName("Assaulters");
        assaulters.SetModelPath("Prefabs/Entity/Units/Assaulters");
        assaulters.SetHealth(3);
        assaulters.SetDamage(1);
        assaulters.SetDistance(2);
        assaulters.SetMobility(2);

        UnitCacheItem osoka = new UnitCacheItem(new Dictionary<string, object>());
        osoka.SetExternalId("035655f8-a347-4057-87cc-83385fa20660");
        osoka.SetName("Osoka");
        osoka.SetModelPath("Prefabs/Entity/Units/Osoka");
        osoka.SetHealth(6);
        osoka.SetDamage(3);
        osoka.SetDistance(3);
        osoka.SetMobility(4);

        UnitCacheItem vortex = new UnitCacheItem(new Dictionary<string, object>());
        vortex.SetExternalId("a480c091-6b22-43f2-b26d-bbd3d2c2905b");
        vortex.SetName("Vortex");
        vortex.SetModelPath("Prefabs/Entity/Units/Vortex");
        vortex.SetHealth(5);
        vortex.SetDamage(2);
        vortex.SetDistance(2);
        vortex.SetMobility(6);

        unitsTable.Add(new CacheItem[3] { assaulters, osoka, vortex });
        Cache.Save(unitsTable);
    }

    public void CreateMaterial()
    {
        MaterialCacheTable materialsTable = Cache.LoadByType<MaterialCacheTable>();
        MaterialCacheItem megaphone = new MaterialCacheItem(new Dictionary<string, object>());
        megaphone.SetName("Megaphone");

        materialsTable.Add(new CacheItem[1] { megaphone });
        Cache.Save(materialsTable);
    }

    public void CreateInventory()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();

        InventoryCacheItem headbuild = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "9b2cf240-5f63-4107-8751-eb91b95b94d9" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        InventoryCacheItem mine = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "4b8a1805-3af8-4144-8bdb-62c93852b443" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        CacheItem[] itemsForAdd = new CacheItem[2] { mine, headbuild };
        inventoryTable.Add(itemsForAdd);

        Cache.Save(inventoryTable);
    }

    public void CreateInventoryUnit()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem assaulters = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "bd1b7986-cf1a-4d76-8b14-c68bf10f363f" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );

        inventoryTable.Add(new CacheItem[1] { assaulters });
        Cache.Save(inventoryTable);
    }

    public void CreateInventoryMaterial()
    {
        InventoryCacheTable inventoryTable = Cache.LoadByType<InventoryCacheTable>();
        InventoryCacheItem megaphone = new InventoryCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "31391161-4be3-4149-833d-7fdde496946c" },
            { "type", "Material" },
            { "count", 1 }
        }
        );

        inventoryTable.Add(new CacheItem[1] { megaphone });
        Cache.Save(inventoryTable);
    }

    public void CreateShopMaterial()
    {
        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();

        ShopCacheItem megaphone = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "31391161-4be3-4149-833d-7fdde496946c" },
            { "type", "Material" },
            { "count", 2 }
        }
        );


        CacheItem[] itemsForAdd = new CacheItem[1] { megaphone };
        shopTable.Add(itemsForAdd);

        Cache.Save(shopTable);
    }


    public void CreateShop()
    {
        ShopCacheTable shopTable = Cache.LoadByType<ShopCacheTable>();


        ShopCacheItem oilStation = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "64a4568c-bfaf-408e-9537-8e489ccaca56" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem office = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "8878498b-a4bc-4dc8-8f39-bc9e987a689f" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem tent = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "ba290dde-968d-46ab-868b-b0f7598a7787" },
            { "type", "Build" },
            { "count", 2 }
        }
        );

        ShopCacheItem warehouse = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "3d9f0f22-409e-40d7-8511-f4584b583dc0" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem trainingCenter = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "065a4f61-5b0e-450e-b89b-299651f90b4d" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem factory = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "7dfc9c80-ddac-45e9-bcdf-61166e068cad" },
            { "type", "Build" },
            { "count", 2 }
        }
       );

        ShopCacheItem osoka = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "035655f8-a347-4057-87cc-83385fa20660" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );

        ShopCacheItem vortex = new ShopCacheItem(new Dictionary<string, object>()
        {
            { "coreId", "a480c091-6b22-43f2-b26d-bbd3d2c2905b" },
            { "type", "Unit" },
            { "count", 2 }
        }
        );


        CacheItem[] itemsForAdd = new CacheItem[8] { office, oilStation, trainingCenter, tent, warehouse, factory, osoka, vortex };
        shopTable.Add(itemsForAdd);

        Cache.Save(shopTable);
    }

    public void CreateSteelContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnSteel_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_1.SetExternalId("93970dfe-6f4a-4b5c-8d6d-115dad61423e");
        ResourcesData costForContract_1 = new ResourcesData(rubCount: 500);
        contractOnSteel_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(steelCount: 2);
        contractOnSteel_1.SetGives(givesForContract_1);
        contractOnSteel_1.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_1.SetDuration(10);
        contractOnSteel_1.SetName("Steel express");
        contractOnSteel_1.SetIconSection("UIBuildCards");
        contractOnSteel_1.SetIconName("mine_card");

        ContractCacheItem contractOnSteel_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_2.SetExternalId("4766fc0c-db3e-46f8-853a-06dfa7f62d29");
        ResourcesData costForContract_2 = new ResourcesData(rubCount: 1200);
        contractOnSteel_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(steelCount: 6);
        contractOnSteel_2.SetGives(givesForContract_2);
        contractOnSteel_2.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_2.SetDuration(25);
        contractOnSteel_2.SetName("Half a shift");
        contractOnSteel_2.SetIconSection("UIBuildCards");
        contractOnSteel_2.SetIconName("mine_card");

        ContractCacheItem contractOnSteel_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnSteel_3.SetExternalId("e5215c51-f22f-4655-9d32-6f49fc0510fc");
        ResourcesData costForContract_3 = new ResourcesData(rubCount: 2150);
        contractOnSteel_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(steelCount: 15);
        contractOnSteel_3.SetGives(givesForContract_3);
        contractOnSteel_3.SetType(InteractionComponentTypes.steelContract);
        contractOnSteel_3.SetDuration(60);
        contractOnSteel_3.SetName("Full shift");
        contractOnSteel_3.SetIconSection("UIBuildCards");
        contractOnSteel_3.SetIconName("mine_card");

        contractsTable.Add(new CacheItem[3] { contractOnSteel_1, contractOnSteel_2, contractOnSteel_3 });
        Cache.Save(contractsTable);
    }

    public void CreateOilContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnOil_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_1.SetExternalId("7a837319-57a3-457f-9fab-bd6d8bf35021");
        ResourcesData costForContract_1 = new ResourcesData(rubCount: 600);
        contractOnOil_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(oilCount: 2);
        contractOnOil_1.SetGives(givesForContract_1);
        contractOnOil_1.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_1.SetDuration(10);
        contractOnOil_1.SetName("Oil express");
        contractOnOil_1.SetIconSection("UIBuildCards");
        contractOnOil_1.SetIconName("oil_station_card");


        ContractCacheItem contractOnOil_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_2.SetExternalId("6c6b3001-cfa9-4761-8e40-ae26e88f6ca5");
        ResourcesData costForContract_2 = new ResourcesData(rubCount: 1400);
        contractOnOil_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(oilCount: 5);
        contractOnOil_2.SetGives(givesForContract_2);
        contractOnOil_2.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_2.SetDuration(30);
        contractOnOil_2.SetName("Half a shift");
        contractOnOil_2.SetIconSection("UIBuildCards");
        contractOnOil_2.SetIconName("oil_station_card");

        ContractCacheItem contractOnOil_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnOil_3.SetExternalId("522a3c5c-ce58-42b5-b237-80ac15845b6a");
        ResourcesData costForContract_3 = new ResourcesData(rubCount: 2750);
        contractOnOil_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(oilCount: 12);
        contractOnOil_3.SetGives(givesForContract_3);
        contractOnOil_3.SetType(InteractionComponentTypes.oilContract);
        contractOnOil_3.SetDuration(65);
        contractOnOil_3.SetName("Full shift");
        contractOnOil_3.SetIconSection("UIBuildCards");
        contractOnOil_3.SetIconName("oil_station_card");

        contractsTable.Add(new CacheItem[3] { contractOnOil_1, contractOnOil_2, contractOnOil_3 });
        Cache.Save(contractsTable);
    }

    public void CreateRubContracts()
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();

        ContractCacheItem contractOnRub_1 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_1.SetExternalId("4a9f0fb4-51fe-452f-9159-6368e00d9b1f");
        ResourcesData costForContract_1 = new ResourcesData(oilCount: 1);
        contractOnRub_1.SetCost(costForContract_1);
        ResourcesData givesForContract_1 = new ResourcesData(rubCount: 550);
        contractOnRub_1.SetGives(givesForContract_1);
        contractOnRub_1.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_1.SetDuration(5);
        contractOnRub_1.SetName("Rub express");
        contractOnRub_1.SetIconSection("UIBuildCards");
        contractOnRub_1.SetIconName("office_card");

        ContractCacheItem contractOnRub_2 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_2.SetExternalId("fc4c6678-2349-4927-a222-3fa0a54b8459");
        ResourcesData costForContract_2 = new ResourcesData(oilCount: 3);
        contractOnRub_2.SetCost(costForContract_2);
        ResourcesData givesForContract_2 = new ResourcesData(rubCount: 1800);
        contractOnRub_2.SetGives(givesForContract_2);
        contractOnRub_2.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_2.SetDuration(25);
        contractOnRub_2.SetName("Half a shift");
        contractOnRub_2.SetIconSection("UIBuildCards");
        contractOnRub_2.SetIconName("office_card");

        ContractCacheItem contractOnRub_3 = new ContractCacheItem(new Dictionary<string, object>());
        contractOnRub_3.SetExternalId("c5f7ec2d-db3a-48a2-8845-51607d342e9c");
        ResourcesData costForContract_3 = new ResourcesData(oilCount: 5);
        contractOnRub_3.SetCost(costForContract_3);
        ResourcesData givesForContract_3 = new ResourcesData(rubCount: 3750);
        contractOnRub_3.SetGives(givesForContract_3);
        contractOnRub_3.SetType(InteractionComponentTypes.rubContract);
        contractOnRub_3.SetDuration(60);
        contractOnRub_3.SetName("Full shift");
        contractOnRub_3.SetIconSection("UIBuildCards");
        contractOnRub_3.SetIconName("office_card");

        contractsTable.Add(new CacheItem[3] { contractOnRub_1, contractOnRub_2, contractOnRub_3 });
        Cache.Save(contractsTable);
    }

    public void CreateUnitProductions()
    {
        UnitProductionCacheTable unitProductionsTable = Cache.LoadByType<UnitProductionCacheTable>();

        UnitProductionCacheItem assaulters = new UnitProductionCacheItem(new Dictionary<string, object>());
        assaulters.SetExternalId("7e940ae4-6d78-40a8-962b-0f1c154cf20d");
        assaulters.SetUnitId("bd1b7986-cf1a-4d76-8b14-c68bf10f363f");

        ResourcesData costForAssaulters = new ResourcesData(rubCount: 300);
        assaulters.SetCost(costForAssaulters);
        assaulters.SetType(InteractionComponentTypes.infantryUnitProduction);
        assaulters.SetDuration(5);
        assaulters.SetName("Train assaulters");
        assaulters.SetIconSection("UIUnitCards");
        assaulters.SetIconName("assaulters");


        UnitProductionCacheItem osoka = new UnitProductionCacheItem(new Dictionary<string, object>());
        osoka.SetExternalId("957bb263-524a-4460-8375-234b86055d7c");
        osoka.SetUnitId("035655f8-a347-4057-87cc-83385fa20660");

        ResourcesData costForOsoka = new ResourcesData(rubCount: 2000, oilCount: 2, steelCount: 4);
        osoka.SetCost(costForOsoka);
        osoka.SetType(InteractionComponentTypes.vehicleUnitProduction);
        osoka.SetDuration(30);
        osoka.SetName("Produce a BMP");
        osoka.SetIconSection("UIUnitCards");
        osoka.SetIconName("osoka");


        UnitProductionCacheItem vortex = new UnitProductionCacheItem(new Dictionary<string, object>());
        vortex.SetExternalId("51112095-826c-4436-91e8-5684dc94ca84");
        vortex.SetUnitId("a480c091-6b22-43f2-b26d-bbd3d2c2905b");

        ResourcesData costForVortex = new ResourcesData(rubCount: 1600, oilCount: 1, steelCount: 2);
        vortex.SetCost(costForVortex);
        vortex.SetType(InteractionComponentTypes.vehicleUnitProduction);
        vortex.SetDuration(20);
        vortex.SetName("Produce a light combat vehicle");
        vortex.SetIconSection("UIUnitCards");
        vortex.SetIconName("vortex");


        unitProductionsTable.Add(new CacheItem[3] { assaulters, vortex, osoka });
        Cache.Save(unitProductionsTable);
    }

    public void CreateObstacles()
    {
        ObstacleCacheTable obstaclesTable = Cache.LoadByType<ObstacleCacheTable>();

        ResourcesData bushDemolitionCost = new ResourcesData();
        bushDemolitionCost.rub = 250;

        ObstacleCacheItem bush1 = new ObstacleCacheItem(new Dictionary<string, object>());
        bush1.SetName("Bush");
        bush1.SetExternalId("ee8c8e9e-8b30-4687-9179-0a279badf766");
        bush1.SetSize(new Bector2Int(1, 1));
        bush1.SetDemolitionCost(bushDemolitionCost);
        bush1.SetModelPath("Prefabs/Entity/Obstacles/Bush1");

        ObstacleCacheItem bush2 = new ObstacleCacheItem(new Dictionary<string, object>());
        bush2.SetName("Bush");
        bush2.SetExternalId("429b4e2e-b6a2-465b-982f-4cc96d67a011");
        bush2.SetSize(new Bector2Int(1, 1));
        bush2.SetDemolitionCost(bushDemolitionCost);
        bush2.SetModelPath("Prefabs/Entity/Obstacles/Bush2");


        ResourcesData smallPalmDemolitionCost = new ResourcesData();
        smallPalmDemolitionCost.rub = 400;

        ObstacleCacheItem palm1 = new ObstacleCacheItem(new Dictionary<string, object>());
        palm1.SetName("Small palm");
        palm1.SetExternalId("90ebae2b-7633-42c7-9868-406d47583d5a");
        palm1.SetSize(new Bector2Int(1, 1));
        palm1.SetDemolitionCost(smallPalmDemolitionCost);
        palm1.SetModelPath("Prefabs/Entity/Obstacles/Palm1");

        ResourcesData palmDemolitionCost = new ResourcesData();
        palmDemolitionCost.rub = 600;

        ObstacleCacheItem palm2 = new ObstacleCacheItem(new Dictionary<string, object>());
        palm2.SetName("Palm");
        palm2.SetExternalId("bb9da51e-303d-4aed-951d-0248490f76b6");
        palm2.SetSize(new Bector2Int(1, 1));
        palm2.SetDemolitionCost(palmDemolitionCost);
        palm2.SetModelPath("Prefabs/Entity/Obstacles/Palm2");

        ObstacleCacheItem palm3 = new ObstacleCacheItem(new Dictionary<string, object>());
        palm3.SetName("Palm");
        palm3.SetExternalId("0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a");
        palm3.SetSize(new Bector2Int(1, 1));
        palm3.SetDemolitionCost(palmDemolitionCost);
        palm3.SetModelPath("Prefabs/Entity/Obstacles/Palm3");

        ObstacleCacheItem palm4 = new ObstacleCacheItem(new Dictionary<string, object>());
        palm4.SetName("Palm");
        palm4.SetExternalId("7b6c7782-0a71-4501-87f5-ed18b935cea1");
        palm4.SetSize(new Bector2Int(1, 1));
        palm4.SetDemolitionCost(palmDemolitionCost);
        palm4.SetModelPath("Prefabs/Entity/Obstacles/Palm4");

        ObstacleCacheItem palm5 = new ObstacleCacheItem(new Dictionary<string, object>());
        palm5.SetName("Palm");
        palm5.SetExternalId("1dc74e29-35f1-4fc5-a3f0-d2c6c39d558b");
        palm5.SetSize(new Bector2Int(1, 1));
        palm5.SetDemolitionCost(palmDemolitionCost);
        palm5.SetModelPath("Prefabs/Entity/Obstacles/Palm5");


        ResourcesData smallStoneDemolitionCost = new ResourcesData();
        smallStoneDemolitionCost.rub = 450;

        ObstacleCacheItem stone1 = new ObstacleCacheItem(new Dictionary<string, object>());
        stone1.SetName("Small stone");
        stone1.SetExternalId("fccb430b-9ab1-4073-a334-6183a40ad0f2");
        stone1.SetSize(new Bector2Int(1, 1));
        stone1.SetDemolitionCost(smallStoneDemolitionCost);
        stone1.SetModelPath("Prefabs/Entity/Obstacles/Stone2");


        ResourcesData stoneDemolitionCost = new ResourcesData();
        stoneDemolitionCost.rub = 700;

        ObstacleCacheItem stone2 = new ObstacleCacheItem(new Dictionary<string, object>());
        stone2.SetName("Stone");
        stone2.SetExternalId("42c227ad-ce04-458d-ba1a-5e09c9ea9efc");
        stone2.SetSize(new Bector2Int(1, 1));
        stone2.SetDemolitionCost(stoneDemolitionCost);
        stone2.SetModelPath("Prefabs/Entity/Obstacles/Stone1");

        ObstacleCacheItem stone3 = new ObstacleCacheItem(new Dictionary<string, object>());
        stone3.SetName("Stone");
        stone3.SetExternalId("48b9e917-ecfb-44ea-ade9-25cec693789e");
        stone3.SetSize(new Bector2Int(1, 1));
        stone3.SetDemolitionCost(stoneDemolitionCost);
        stone3.SetModelPath("Prefabs/Entity/Obstacles/Stone3");


        obstaclesTable.Add(new CacheItem[10] { bush1, bush2, palm1, palm2, palm3, palm4, palm5, stone1, stone2, stone3 });
        Cache.Save(obstaclesTable);
    }

    public void CreatePlayerObstacles()
    {
        PlayerObstacleCacheTable obstaclesTable = Cache.LoadByType<PlayerObstacleCacheTable>();

        PlayerObstacleCacheItem smallStone = new PlayerObstacleCacheItem(new Dictionary<string, object>());
        smallStone.SetRotation(180);
        smallStone.SetPosition(new Bector2Int[] { new Bector2Int(0, 5) });
        smallStone.SetExternalId("88199bd8-0f5e-43bb-807e-253f7b94abd3");
        smallStone.SetCoreId("fccb430b-9ab1-4073-a334-6183a40ad0f2");

        obstaclesTable.AddOne(smallStone);

        Cache.Save(obstaclesTable);
    }

    public void CreateSkills()
    {
        SkillCacheTable skillsTable = Cache.LoadByType<SkillCacheTable>();

        SkillCacheItem moveWithAttack = new SkillCacheItem(new Dictionary<string, object>());
        moveWithAttack.SetExternalId("a60f271d-4aa1-41af-b6d1-c89ed667dad6");
        moveWithAttack.SetName("Атака в движении");
        moveWithAttack.SetRating(0);
        moveWithAttack.SetIconSection("UIAttributes");
        moveWithAttack.SetIconName("move_with_attack");

        skillsTable.AddOne(moveWithAttack);
        Cache.Save(skillsTable);
    }
}
