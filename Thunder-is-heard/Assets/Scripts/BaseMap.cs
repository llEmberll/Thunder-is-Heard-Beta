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
        //CreateScenarioForEnemyOutpost();
    }

    public void CreateTutorialOnBase()
    {
        //// ������������� �����(������� �������)

        // �������
        Replic[] dialogueForStage1 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Командир! Пока захватчики оставили нас в покое, не будем терять время зря! Нашу базу нельзя даже «лагерем» назвать. Нужно это исправить!"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Начнём с солдат. Они нам очень пригодятся. Постройте один Тренировочный центр"
                ),
        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage1 = new Dictionary<string, object>() { { "tag", Tags.shop} };
        ConditionData conditionForPassStage1 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage1);

        // �����
        FocusData focusDataForStage1 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildTrainingCenter = new Dictionary<string, string>()
        {
            { "Shop", "OnlyTrainingCenter" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };

        //// ������������� �����(���������)
        // �������

        string trainingCenterId = "065a4f61-5b0e-450e-b89b-299651f90b4d";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage2 = new Dictionary<string, object>() { { "targetObjectId", trainingCenterId } };
        ConditionData conditionForPassStage2 = new ConditionData(type: "ExistObject", dataForConditionForPassStage2);

        // �����
        FocusData focusDataForStage2 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", trainingCenterId } });

        // ��������� �����������


        //// ������������� �����(������� ������ ������������ ������)
        // �������
        Replic[] dialogueForStage3 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Отлично! Теперь обучите новобранцев"
                ),
        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage3 = new Dictionary<string, object>() { { "tag", Tags.unitProductions } };
        ConditionData conditionForPassStage3 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage3);

        // �����
        FocusData focusDataForStage3 = new FocusData(type: "Build", data: new Dictionary<string, object>() { { "coreId", trainingCenterId } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForProductAssaulters = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "OnlyTutorialProductions" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };

        //// ������������� �����(��������� ������������ ������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage4 = new Dictionary<string, object>() { { "targetUnitProductionId", "f4b60bee-1dda-4377-9fab-5092f48b3e60" } };
        ConditionData conditionForPassStage4 = new ConditionData(type: "UnitProductionInProcess", dataForConditionForPassStage4);

        // �����
        FocusData focusDataForStage4 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", UnitProductions.ComponentType }, { "unitId", "bd1b7986-cf1a-4d76-8b14-c68bf10f363f" } });

        // ��������� �����������


        //// ������������� �����(���������� ������������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage5 = new Dictionary<string, object>() { { "targetUnitProductionId", "f4b60bee-1dda-4377-9fab-5092f48b3e60" } };
        ConditionData conditionForPassStage5 = new ConditionData(type: "UnitProductionFinished", dataForConditionForPassStage5);

        // �����

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForFinishProductAssaulters = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };

        //// ������������� �����(����)
        // �������
        Replic[] dialogueOnPassForStage6 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Новая сила, которой всегда не хватает…"
                ),
        };

        // ������� ��� �����������
        ConditionData conditionForPassStage6 = new ConditionData(type: "AllUnitsCollected", null);

        // �����
        FocusData focusDataForStage6 = new FocusData(type: "ProductsNotification", data: new Dictionary<string, object>() { { "type", ProductsNotificationTypes.waitingUnitCollection } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForCollectAssaulters = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "OnlyTutorialProductions" },
            { "Obstacle", "Disabled" },  { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };

        //// ����(�������� ��������)
        // �������
        Replic[] dialogueForStage7 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Разобравшись с солдатами, теперь мы можем приступить к нашей экономике. Построим офис"
                ),
        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage7 = new Dictionary<string, object>() { { "tag", Tags.shop } };
        ConditionData conditionForPassStage7 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage7);

        // �����
        FocusData focusDataForStage7 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildOffice = new Dictionary<string, string>()
        {
            { "Shop", "OnlyOffice" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };


        //// ����(���������)
        // �������

        string officeId = "8878498b-a4bc-4dc8-8f39-bc9e987a689f";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage8 = new Dictionary<string, object>() { { "targetObjectId", officeId } };
        ConditionData conditionForPassStage8 = new ConditionData(type: "ExistObject", dataForConditionForPassStage8);

        // �����
        FocusData focusDataForStage8 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", officeId } });

        // ��������� �����������


        //// ����(������� ������ ������������)
        // �������
        Replic[] dialogueForStage9 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Хорошо. Теперь перейдём к заработку денег!"
                ),
        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage9 = new Dictionary<string, object>() { { "tag", Tags.contracts } };
        ConditionData conditionForPassStage9 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage9);

        // �����
        FocusData focusDataForStage9 = new FocusData(type: "Build", data: new Dictionary<string, object>() { { "coreId", officeId } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForProductsInOffice = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "OnlyTutorialContracts" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };


        //// ����(��������� ������������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage10 = new Dictionary<string, object>() { { "targetContractId", "897d1863-f964-4809-8fc5-62f8ab4ecd9d" } };
        ConditionData conditionForPassStage10 = new ConditionData(type: "ContractInProcess", dataForConditionForPassStage10);

        // �����
        FocusData focusDataForStage10 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Contracts.ComponentType }, { "contractType", InteractionComponentTypes.rubContract } });

        // ��������� �����������


        //// ����(���������� ������������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage11 = new Dictionary<string, object>() { { "targetContractId", "897d1863-f964-4809-8fc5-62f8ab4ecd9d" } };
        ConditionData conditionForPassStage11 = new ConditionData(type: "ContractFinished", dataForConditionForPassStage11);

        // �����

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForFinishProductInOffice = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };


        //// ����(����)
        // �������
        Replic[] dialogueOnPassForStage12 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Отличная работа. Вернёмся к постройкам… Что у нас там дальше по плану?"
                ),
        };

        // ������� ��� �����������
        ConditionData conditionForPassStage12 = new ConditionData(type: "AllResourcesCollected", null);

        // �����
        FocusData focusDataForStage12 = new FocusData(type: "ProductsNotification", data: new Dictionary<string, object>() { { "type", ProductsNotificationTypes.waitingResourceCollection } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForCollectRub = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "OnlyTutorialContracts" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" },  { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };


        //// ������(����������)
        // ������
        MediaEventData eventOnStartStage13 = new MediaEventData(audioEventId: "MachinegunQueue", new Bector2Int(4, 3));

        // �������
        Replic[] dialogueForStage13 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Что?! Уже атакуют?!"
                ),
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Быть не может! Как так быстро?!"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Эй! Не стреляйте! Свои!"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Что это у него за форма?"
                ),
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Похоже на халат врача или учёного… Подходи с поднятыми руками!"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Только не стреляйте! Уже иду!"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Гражданин! Имя, Фамилия, Отчество, Место работы и…",
                focus: new Bector2Int(6, 5)
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Я ведущий инженер-конструктор секретного военного проекта. Хотя, назвать его секретным нынче сложно… Зовут меня Райан, а фамилию и отчество вам знать не положено. Простите что перебил"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Райан… Кажется я о вас где-то слышала. Паспорт или какой-то документ у вас есть?!"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Конечно слышали! Вся страна слышала обо мне! Жаль, что я настолько известен, что даже вражеский офицер просто мечтает меня получить в свои руки…"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Я всё ещё жду паспорт"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "У меня с собой ничего нет! Я сбежал из исследовательского комплекса, что отсюда в 10 километрах. Всё осталось там. Лучшее что я могу вам показать, это мой бейджик"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Уже его рассмотрела, необязательно. Что ж, если вы действительно являетесь целью вражеского командования, наверняка вы владеете кучей военных тайн. Ладно, приютим вас здесь пока. Дальше решим"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Слушайте, я буквально несколько часов назад сбежал из «877 Исследовательского комплекса»! Там очень много важной документации, которую противник уже изучает. Я хоть и главный конструктор, но память у меня не железная. Мне нужно хоть какое-то здание, где я бы мог восстановить потерянные данные, пока я не забыл то, что наши учёные изучали"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Вы хотите чтобы мы построили НИИ у себя на военной базе? Вы в своём уме?!"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Именно. Не переживайте, местное НИИ вам только поможет. Например бронежилеты которые носят ваши солдаты, устарели уже как несколько десятков лет назад! Мы бы могли это исправить путём совсем не хитрых манипуляций"
                ),
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Думаю в его словах есть смысл. В конце концов, если он не будет работать, мы в любой момент можем снести НИИ, ведь так Райан?"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.empire,
                text: "Хе-хе. Не переживайте! Наука лишней не бывает!"
                ),
        };

        // ������� ��� �����������
        ConditionData conditionForPassStage13 = new ConditionData(type: "AlwaysTrue", null);


        //// �����������(�������� ��������)
        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage14 = new Dictionary<string, object>() { { "tag", Tags.shop } };
        ConditionData conditionForPassStage14 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage14);

        // �����
        FocusData focusDataForStage14 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildLaboratory = new Dictionary<string, string>()
        {
            { "Shop", "OnlyLaboratory" }, 
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };

        //// �����������(�������������)
        // �������

        string laboratoryId = "f4465aab-c10e-4d7a-a1f7-78d419c50f24";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage15 = new Dictionary<string, object>() { { "targetObjectId", laboratoryId } };
        ConditionData conditionForPassStage15 = new ConditionData(type: "ExistObject", dataForConditionForPassStage15);

        // �����
        FocusData focusDataForStage15 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", laboratoryId } });

        // ��������� �����������


        //// ���������� ���������� � ������
        // �������
        Replic[] dialogueForStage16 = new Replic[]
        {
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.federation,
                text: "Вот это да! Вот это оборудование! Заходите в НИИ! Не стесняйтесь"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Райан! Я пока заберу у тебя командира. Мы с ним ещё не закончили с базой. Осваивайся пока. Вечером подробнее о себе расскажешь…"
                ),
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Если будет чай, позовите и меня!"
                ),

        };

        // ������� ��� �����������
        ConditionData conditionForPassStage16 = new ConditionData(type: "AlwaysTrue", null);


        //// �������(�������� ��������)
        // �������
        Replic[] dialogueForStage17 = new Replic[]
        {
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Командир! Все солдаты жалуются на то что в одной палатке тесновато. Зайдя к ним, я в этом убедилась. Нам нужно ещё одно здание, где мы могли бы расположить военнослужащих"
                ),

        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage17 = new Dictionary<string, object>() { { "tag", Tags.shop } };
        ConditionData conditionForPassStage17 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage17);

        // �����
        FocusData focusDataForStage17 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildTent = new Dictionary<string, string>()
        {
            { "Shop", "OnlyTent" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };


        //// �������(�������������)
        // �������
        Replic[] dialogueOnPassForStage18 = new Replic[]
        {
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Солдаты просили передать вам благодарность за новую казарму. Ну и от меня вам тоже спасибо"
                ),
        };

        string tentId = "ba290dde-968d-46ab-868b-b0f7598a7787";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage18 = new Dictionary<string, object>() { { "targetObjectId", tentId } };
        ConditionData conditionForPassStage18 = new ConditionData(type: "ExistObject", dataForConditionForPassStage18);

        // �����
        FocusData focusDataForStage18 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", tentId } });

        // ��������� �����������



        //// �����(�������� ��������)
        // �������
        Replic[] dialogueForStage19 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Ресурсы тоже надо где-то хранить, поэтому предлагаю сообразить склад"
                ),

        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage19 = new Dictionary<string, object>() { { "tag", Tags.shop } };
        ConditionData conditionForPassStage19 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage19);

        // �����
        FocusData focusDataForStage19 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildWarehouse = new Dictionary<string, string>()
        {
            { "Shop", "OnlyWarehouse" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };



        //// �����(�������������)
        // �������

        string warehouseId = "3d9f0f22-409e-40d7-8511-f4584b583dc0";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage20 = new Dictionary<string, object>() { { "targetObjectId", warehouseId } };
        ConditionData conditionForPassStage20 = new ConditionData(type: "ExistObject", dataForConditionForPassStage20);

        // �����
        FocusData focusDataForStage20 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", warehouseId } });

        // ��������� �����������



        //// ����������(�������� ��������)
        // �������
        Replic[] dialogueForStage21 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Отлично, теперь нам есть где хранить наши ресурсы. Ох точно! Совсем забыла… У нас топливо в штабной машине кончилось. Нужна нефтяная вышка. Постройте её"
                ),

        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage21 = new Dictionary<string, object>() { { "tag", Tags.shop } };
        ConditionData conditionForPassStage21 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage21);

        // �����
        FocusData focusDataForStage21 = new FocusData(type: "Button", data: new Dictionary<string, object>() { { "tag", "ToShopButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForBuildOilStation = new Dictionary<string, string>()
        {
            { "Shop", "OnlyOilStation" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };



        //// ����������(�������������)
        // �������

        string oilStationId = "64a4568c-bfaf-408e-9537-8e489ccaca56";

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage22 = new Dictionary<string, object>() { { "targetObjectId", oilStationId } };
        ConditionData conditionForPassStage22 = new ConditionData(type: "ExistObject", dataForConditionForPassStage22);

        // �����
        FocusData focusDataForStage22 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Shop.ComponentType }, { "coreId", oilStationId } });

        // ��������� �����������



        //// ����������(�������� ���� ������������)
        // �������
        Replic[] dialogueForStage23 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Я уже чувствую запах бензина. Дайте приказ рабочим чтобы накачали нам канистру топлива"
                ),
        };

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage23 = new Dictionary<string, object>() { { "tag", Tags.contracts } };
        ConditionData conditionForPassStage23 = new ConditionData(type: "PanelOpened", dataForConditionForPassStage23);

        // �����
        FocusData focusDataForStage23 = new FocusData(type: "Build", data: new Dictionary<string, object>() { { "coreId", oilStationId } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForProductsInOilStation = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "OnlyTutorialContracts" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };



        //// ����������(��������� ������������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage24 = new Dictionary<string, object>() { { "targetContractId", "453aa61c-ed38-449d-84bb-e79f96108bad" } };
        ConditionData conditionForPassStage24 = new ConditionData(type: "ContractInProcess", dataForConditionForPassStage24);

        // �����
        FocusData focusDataForStage24 = new FocusData(type: "UIItem", data: new Dictionary<string, object>() { { "UIType", Contracts.ComponentType }, { "contractType", InteractionComponentTypes.oilContract } });

        // ��������� �����������


        //// ����������(���������� ������������)
        // �������

        // ������� ��� �����������
        Dictionary<string, object> dataForConditionForPassStage25 = new Dictionary<string, object>() { { "targetContractId", "453aa61c-ed38-449d-84bb-e79f96108bad" } };
        ConditionData conditionForPassStage25 = new ConditionData(type: "ContractFinished", dataForConditionForPassStage25);

        // �����

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForFinishProductInOilStation = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" }, { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };



        //// ����������(����)
        // �������

        // ������� ��� �����������
        ConditionData conditionForPassStage26 = new ConditionData(type: "AllResourcesCollected", null);

        // �����
        FocusData focusDataForStage26 = new FocusData(type: "ProductsNotification", data: new Dictionary<string, object>() { { "type", ProductsNotificationTypes.waitingResourceCollection } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForCollectOil = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "OnlyTutorialContracts" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" },  { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Disabled" }
        };



        //// ����
        // �������
        Replic[] dialogueForStage27 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Наконец-то наш джип заведётся…"
                ),
            new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "Не надейся. Там опять что-то сломалось. Машина в ближайшее время точно не поедет"
                ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Опять это корыто сломалось…"
                ),
            new Replic(
                charName: Chars.scientist,
                charSide: Sides.federation,
                text: "Я в прошлом работал в автомастерской. Дайте-ка взглянуть на ваше «корыто»"
                ),
             new Replic(
                charName: Chars.tankGirl,
                charSide: Sides.federation,
                text: "А может и в ближайшее время поедет…"
                ),
        };

        // ������� ��� �����������
        ConditionData conditionForPassStage27 = new ConditionData(type: "AlwaysTrue", null);



        //// ������������ ����
        // �������
        Replic[] dialogueForStage28 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Вы только посмотрите как преобразилась наша база! Наконец, мы можем гордо называться базой… Кстати, как мы будем называться?"
                )
        };

        Replic[] dialogueOnPassForStage28 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Неплохо звучит, командир!"
                ),
        };

        // ������� ��� �����������
        ConditionData conditionForPassStage28 = new ConditionData(type: "BaseNameChanged", null);

        // �����
        FocusData focusDataForStage28 = new FocusData(type: "Text", data: new Dictionary<string, object>() { { "tag", "RenameBaseButton" } });

        // ��������� �����������
        Dictionary<string, string> behaviourIdByComponentNameForNamingBase = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" },  { "BuildingPanel", "OnlyRotateAndCancel" }, { "BaseSettingsPanel", "Base" }
        };

        TutorialStageData stage28 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage28,
            stageReplicsOnPass: dialogueOnPassForStage28,
            stageConditionsForPass: conditionForPassStage28,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForNamingBase,
            stageFocusData: focusDataForStage28,
            stageStageOnPass: null
        );

        TutorialStageData stage27 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage27,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage27,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForFinishProductInOilStation,
            stageFocusData: null,
            stageStageOnPass: stage28
        );

        TutorialStageData stage26 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage26,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForCollectOil,
            stageFocusData: focusDataForStage26,
            stageStageOnPass: stage27
        );

        TutorialStageData stage25 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage25,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForFinishProductInOilStation,
            stageFocusData: null,
            stageStageOnPass: stage26
        );

        TutorialStageData stage24 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage24,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductsInOilStation,
            stageFocusData: focusDataForStage24,
            stageStageOnPass: stage25
        );

        TutorialStageData stage23 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage23,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage23,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductsInOilStation,
            stageFocusData: focusDataForStage23,
            stageStageOnPass: stage24
        );

        TutorialStageData stage22 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage22,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildOilStation,
            stageFocusData: focusDataForStage22,
            stageStageOnPass: stage23
        );

        TutorialStageData stage21 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage21,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage21,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildOilStation,
            stageFocusData: focusDataForStage21,
            stageStageOnPass: stage22
        );

        TutorialStageData stage20 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage20,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildWarehouse,
            stageFocusData: focusDataForStage20,
            stageStageOnPass: stage21
        );

        TutorialStageData stage19 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage19,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage19,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildWarehouse,
            stageFocusData: focusDataForStage19,
            stageStageOnPass: stage20
        );

        TutorialStageData stage18 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: dialogueOnPassForStage18,
            stageConditionsForPass: conditionForPassStage18,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildTent,
            stageFocusData: focusDataForStage18,
            stageStageOnPass: stage19
        );

        TutorialStageData stage17 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage17,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage17,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildTent,
            stageFocusData: focusDataForStage17,
            stageStageOnPass: stage18
        );

        TutorialStageData stage16 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage16,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage16,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildLaboratory,
            stageFocusData: null,
            stageStageOnPass: stage17
        );

        TutorialStageData stage15 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage15,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildLaboratory,
            stageFocusData: focusDataForStage15,
            stageStageOnPass: stage16
        );

        TutorialStageData stage14 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage14,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildLaboratory,
            stageFocusData: focusDataForStage14,
            stageStageOnPass: stage15
        );

        TutorialStageData stage13 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage13,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage13,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForFinishProductInOffice,
            stageFocusData: null,
            stageStageOnPass: stage14,
            stageMediaEventData: eventOnStartStage13
        );

        TutorialStageData stage12 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: dialogueOnPassForStage12,
            stageConditionsForPass: conditionForPassStage12,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForCollectRub,
            stageFocusData: focusDataForStage12,
            stageStageOnPass: stage13
        );

        TutorialStageData stage11 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage11,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForFinishProductInOffice,
            stageFocusData: null,
            stageStageOnPass: stage12
        );

        TutorialStageData stage10 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage10,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductsInOffice,
            stageFocusData: focusDataForStage10,
            stageStageOnPass: stage11
        );

        TutorialStageData stage9 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage9,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage9,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductsInOffice,
            stageFocusData: focusDataForStage9,
            stageStageOnPass: stage10
        );

        TutorialStageData stage8 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage8,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildOffice,
            stageFocusData: focusDataForStage8,
            stageStageOnPass: stage9
        );

        TutorialStageData stage7 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage7,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage7,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildOffice,
            stageFocusData: focusDataForStage7,
            stageStageOnPass: stage8
        );

        TutorialStageData stage6 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: dialogueOnPassForStage6,
            stageConditionsForPass: conditionForPassStage6,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForCollectAssaulters,
            stageFocusData: focusDataForStage6,
            stageStageOnPass: stage7
        );

        TutorialStageData stage5 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage5,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForFinishProductAssaulters,
            stageFocusData: null,
            stageStageOnPass: stage6
        );

        TutorialStageData stage4 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage4,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductAssaulters,
            stageFocusData: focusDataForStage4,
            stageStageOnPass: stage5
        );

        TutorialStageData stage3 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage3,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage3,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForProductAssaulters,
            stageFocusData: focusDataForStage3,
            stageStageOnPass: stage4
        );

        TutorialStageData stage2 = new TutorialStageData(
            stageReplicsOnStart: null,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage2,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildTrainingCenter,
            stageFocusData: focusDataForStage2,
            stageStageOnPass: stage3
        );

        TutorialStageData stage1 = new TutorialStageData(
            stageReplicsOnStart: dialogueForStage1,
            stageReplicsOnPass: null,
            stageConditionsForPass: conditionForPassStage1,
            stageBehaviourIdByComponentName: behaviourIdByComponentNameForBuildTrainingCenter,
            stageFocusData: focusDataForStage1,
            stageStageOnPass: stage2
        );


        TutorialCacheTable table = Cache.LoadByType<TutorialCacheTable>();

        Dictionary<string, object> tutorialFields = new Dictionary<string, object>()
        {
            { "externalId", "18434891-a067-4c0f-96ae-3061132c13a6" },
            { "name", "Обучение на базе" },
            { "description", "" },
            { "conditionForStart", new ConditionData(type: "AlwaysTrue", null) },
            { "passed", false },
            { "firstStage", stage1 }
        };

        TutorialCacheItem tutorialData = new TutorialCacheItem(tutorialFields);
        table.AddOne(tutorialData);
        Cache.Save(table);
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


        // �������� �����
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
        // �������� �����

        // �������� ������
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
        // �������� ������

        // �������� �����������
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
        // �������� �����������

        // ��������� ������ ��������
        Replic[] scenarioStartDialogue = new Replic[]
        {
            new Replic(
                charName: Chars.officer, 
                charSide: Sides.federation, 
                text: "Командир! Согласно данным разведки, враг планирует укрепить аванпост неподалёку от нас. Похоже мы их цель. Нужно сорвать их планы и уничтожить плацдарм для наступления на нас!")
        };
        scenarioItem.SetStartDialogue(scenarioStartDialogue);
        // ��������� ������ ��������


        // �����
        //// Этап 0: высадка
        //// �������
        ConditionData defeatConditionDataForStage0 = new ConditionData(type: "AlwaysFalse", null);
        ConditionData victoryConditionDataForStage0 = new ConditionData(type: "AlwaysTrue", null);
        //// �������

        ///// ��
        AISettings AISettingsForEmpireSideInStage0 = new AISettings(
            "Frozen",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage0 = new AISettings(
            "Frozen",
            Sides.neutral,
            null,
            null
            );
        ///// ��

        StageData stage0 = new StageData(
                stageUnits: null,
                stageBuilds: null,
                stageReplicsOnStart: null,
                stageReplicsOnPass: null,
                stageReplicsOnFail: null,
                stageAISettings: new AISettings[] { AISettingsForEmpireSideInStage0, AISettingsForNeutralSideInStage0 },
                stageConditionsForFail: defeatConditionDataForStage0,
                stageConditionsForPass: victoryConditionDataForStage0,
                stageLandingData: landingData
                );
        //// Этап 0: высадка


        //// ���� 1
        ///// ��
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
        ///// ��

        //// �������
        string sideForConditionsDataStage1 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage1 = new RectangleBector2Int(new Bector2Int(0, 14), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionData1Stage1 = new Dictionary<string, object>() { { "side", sideForConditionsDataStage1 }, { "positionRectangle", rectangleForConditionDataStage1 } };
        ConditionData victoryConditionData1ForStage1 = new ConditionData(type: "SideReachPosition", dataForConditionData1Stage1);

        Dictionary<string, object> dataForConditionData2Stage1 = new Dictionary<string, object>() { { "attackerSide", sideForConditionsDataStage1 }, { "targetObjectId", "5168ce99-2415-4eb2-9cc4-530174d7ef4a" } };
        ConditionData victoryConditionData2ForStage1 = new ConditionData(type: "ReachToAttackObject", dataForConditionData2Stage1);

        Dictionary<string, object> victoryDataForConditionDataStage1 = new Dictionary<string, object>() { { "conditions", new ConditionData[] { victoryConditionData1ForStage1, victoryConditionData2ForStage1 } } };
        ConditionData victoryConditionDataForStage1 = new ConditionData(type: "Or", victoryDataForConditionDataStage1);

        ConditionData defeatConditionDataForStage1 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
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
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage1 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// ������ � ������ �������
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
        //// ���� 1

        //// ���� 2
        ///// ��
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
        ///// ��

        //// �������
        string[] targetObjectIdsToDestroyForStage1 = new string[] { "5168ce99-2415-4eb2-9cc4-530174d7ef4a" };
        Dictionary<string, object> dataForVictoryConditionDataWithDestroyHeadbuildStage2 = new Dictionary<string, object>() { { "targetObjectIds", targetObjectIdsToDestroyForStage1 } };
        ConditionData victoryConditionDataWithDestroyHeadbuildForStage2 = new ConditionData(type: "DestroyObjects", dataForVictoryConditionDataWithDestroyHeadbuildStage2);

        string sideForConditionDataStage2 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage2 = new RectangleBector2Int(new Bector2Int(0, 19), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionDataStage2 = new Dictionary<string, object>() { { "side", sideForConditionDataStage2 }, { "positionRectangle", rectangleForConditionDataStage2 } };
        ConditionData defeatConditionDataForStage2 = new ConditionData(type: "SideReachPosition", dataForConditionDataStage2);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage2 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Спешите атаковать штаб, пока они не запросили подкрепление!")
        };
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage2 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Вражеские подкрепления на месте, нужно было уничтожить штаб пока была возможность...")
        };
        //// ������ � ������ �������
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
        //// ���� 2

        //// ���� 2.1
        ///// ��
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
        ///// ��

        //// �������
        ConditionData victoryConditionDataForStage21 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage21 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage21 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation, 
                text: "Вражеские подкрепления на месте, нужно было уничтожить штаб пока была возможность...",
                focus: new Bector2Int(13, 25)
                )
        };
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage21 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// ������ � ������ �������
        
        //// ������ � ������ ������
        Replic[] passDialogueForStage21 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// ������ � ������ ������

        //// �����
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
        //// �����
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
        //// ���� 2.1

        //// ���� 3
        ///// ��
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
        ///// ��

        //// �������
        
        ConditionData victoryConditionDataForStage3 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage3 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
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
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage3 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// ������ � ������ �������
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
        //// ���� 3

        //// ���� 4
        ///// ��
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
        ///// ��

        //// �������
        string[] targetObjectIdsToDestroy = new string[] { "88b65738-bacc-4455-9adf-d1daee3ebc24" };
        Dictionary<string, object> dataForVictoryConditionDataWithDestroyTentStage4 = new Dictionary<string, object>() { { "targetObjectIds", targetObjectIdsToDestroy } };
        ConditionData victoryConditionDataWithDestroyTentForStage4 = new ConditionData(type: "DestroyObjects", dataForVictoryConditionDataWithDestroyTentStage4);

        string sideForConditionDataStage4 = Sides.federation;
        RectangleBector2Int rectangleForConditionDataStage4 = new RectangleBector2Int(new Bector2Int(0, 25), new Bector2Int(26, 35));
        Dictionary<string, object> dataForConditionDataStage4 = new Dictionary<string, object>() { { "side", sideForConditionDataStage4 }, { "positionRectangle", rectangleForConditionDataStage4 } };
        ConditionData defeatConditionDataForStage4 = new ConditionData(type: "SideReachPosition", dataForConditionDataStage4);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage4 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Прекрасно! Гарнизон разбит! Уничтожьте их казарму и с ними будет покончено!", focus: new Bector2Int(17, 22))
        };
        //// C�������� ������

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
        //// ���� 4
        
        //// ���� 4.1
        ///// ��
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
        ///// ��

        //// �������
        ConditionData victoryConditionDataForStage41 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage41 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage41 = new Replic[]
        {
            new Replic(
                charName: Chars.officer, 
                charSide: Sides.federation, 
                text: "Вражеские подкрепления на месте, нужно было уничтожить казарму и отходить пока была возможность...",
                focus: new Bector2Int(13, 25)
                )
        };
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage41 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Мы не справились с задачей, их гарнизоны были слишком сильны! Отступаем!")
        };
        //// ������ � ������ �������

        //// ������ � ������ ������
        Replic[] passDialogueForStage41 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// ������ � ������ ������

        //// �����
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
        //// �����
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
        //// ���� 4.1

        //// ���� 5
        ///// ��
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
        ///// ��

        //// �������
        ConditionData victoryConditionDataForStage5 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage5 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage5 = new Replic[]
        {
            new Replic(charName: Chars.fighter, charSide: Sides.empire, text: "Ну что вы встали, глупцы?! Атакуйте этих никчёмных бездарей!", focus: new Bector2Int(14, 25)),
        };
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage5 = new Replic[]
        {
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Их подкрепления испортили нам всю операцию. Отступаем, пока не стало ещё хуже!")
        };
        //// ������ � ������ �������

        //// �����
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
        //// �����
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
        //// ���� 5

        //// ���� 6
        ///// ��
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
        ///// ��

        //// �������
        ConditionData victoryConditionDataForStage6 = new ConditionData(type: "DestroyAllEnemies", null);

        ConditionData defeatConditionDataForStage6 = new ConditionData(type: "DestroyAllAllies", null);
        //// �������

        //// ��������� ������
        Replic[] startDialogueForStage6 = new Replic[]
        {
            new Replic(
                charName: Chars.tankGirl, 
                charSide: Sides.federation, 
                text: "Командир, аккуратней! Неизвестно, сколько ещё подкреплений они могут вызвать. Хорошенько подумайте про оборону. Старайтесь не допускать потерь…",
                focus: new Bector2Int(13, 25)
                )
        };
        //// C�������� ������

        //// ������ � ������ �������
        Replic[] failDialogueForStage6 = new Replic[]
        {
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Их подкрепления испортили нам всю операцию. Отступаем, пока не стало ещё хуже!")
        };
        //// ������ � ������ �������

        //// ������ � ������ ������
        Replic[] passDialogueForStage6 = new Replic[]
        {
            new Replic(charName: Chars.officer, charSide: Sides.federation, text: "Ура! Победа за нами! Враг разбит, и мы наконец возвращаем территории, пусть и небольшие"),
            new Replic(charName: Chars.tankGirl, charSide: Sides.federation, text: "Не забудьте трофейные винтовки! С патронами и оружием нынче очень туго")
        };
        //// ������ � ������ ������

        //// �����
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
        //// �����
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
        //// ���� 6

        stage0.stageOnPass = stage1;
        stage1.stageOnPass = stageOnPassStage1;
        stageOnPassStage1.stageOnPass = stageOnPassStage2;
        stageOnPassStage1.stageOnFail = stageOnFailStage2;
        stageOnPassStage2.stageOnPass = stageOnPassStage3;
        stageOnPassStage3.stageOnPass = stageOnPassStage4;
        stageOnPassStage3.stageOnFail = stageOnFailStage4;
        stageOnPassStage4.stageOnPass = stageOnPassStage5;
        // �����

        scenarioItem.SetStartStage(stage0);
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
        moveWithAttack.SetName("����� � ��������");
        moveWithAttack.SetRating(0);
        moveWithAttack.SetIconSection("UIAttributes");
        moveWithAttack.SetIconName("move_with_attack");

        skillsTable.AddOne(moveWithAttack);
        Cache.Save(skillsTable);
    }

    public void CreateTrainingMission()
    {
        ScenarioCacheTable table = Cache.LoadByType<ScenarioCacheTable>();
        ScenarioCacheItem scenarioItem = new ScenarioCacheItem(new Dictionary<string, object>());

        scenarioItem.SetExternalId("tab95c11f-8827-4c3a-b58a-c9948cdd18af");
        scenarioItem.SetName("Обучение");

        string terrainPath = Config.terrainsPath["mission"];
        terrainPath = terrainPath.Replace("{MissionName}", "Base");

        scenarioItem.SetTerrainPath(terrainPath);
        scenarioItem.SetMapSize(new Bector2Int(15, 15));

        UnitOnBattle[] scenarioUnits = new UnitOnBattle[]
        {
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(4, 2) },
                    unitRotation: 0,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "7f2652c0-1aeb-491f-ae10-b5e39faadfbd"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(6, 2) },
                    unitRotation: 0,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "24215d61-f986-4c07-9700-168c5afc7229"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(3, 5) },
                    unitRotation: 0,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "2eb19e52-6f54-4072-91e8-d63fe5676987"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(3, 7) },
                    unitRotation: 0,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "aaf752c1-04cb-4c6b-95b2-e4fe1ae63fab"
                    ),
            new UnitOnBattle(
                    coreUnitId: "bd1b7986-cf1a-4d76-8b14-c68bf10f363f",
                    unitPosition: new Bector2Int[] { new Bector2Int(7, 10) },
                    unitRotation: 0,
                    unitMaxHealth: 2,
                    unitHealth: 2,
                    unitDamage: 1,
                    unitDistance: 2,
                    unitMobility: 2,
                    UnitTypes.infantry,
                    Doctrines.land,
                    Sides.empire,
                    unitIdOnBattle: "d8009b36-652c-486d-85c3-d8f4ba797df5"
                    ),
        };

        scenarioItem.SetUnits(scenarioUnits);
        // �������� �����

        // �������� ������
        BuildOnBattle[] scenarioBuilds = new BuildOnBattle[]
        {
            new BuildOnBattle(
                    coreBuildId: "9b2cf240-5f63-4107-8751-eb91b95b94d9",
                    new Bector2Int[]
                    {
                        new Bector2Int(5, 5),
                        new Bector2Int(5, 6),
                        new Bector2Int(5, 7),
                        new Bector2Int(6, 5),
                        new Bector2Int(6, 6),
                        new Bector2Int(6, 7),
                        new Bector2Int(7, 5),
                        new Bector2Int(7, 6),
                        new Bector2Int(7, 7),
                    },
                    180,
                    9,
                    9,
                    0,
                    0,
                    Doctrines.land,
                    Sides.federation,
                    WorkStatuses.idle,
                    buildIdOnBattle: "85e8f427-fcf9-473f-a85d-12e058791727"
                ),
            new BuildOnBattle(
                    coreBuildId: "ba290dde-968d-46ab-868b-b0f7598a7787",
                    new Bector2Int[]
                    {
                        new Bector2Int(8, 5),
                        new Bector2Int(8, 6),
                        new Bector2Int(8, 7),
                        new Bector2Int(9, 5),
                        new Bector2Int(9, 6),
                        new Bector2Int(9, 7)
                    },
                    0,
                    6,
                    6,
                    0,
                    0,
                    Doctrines.land,
                    Sides.federation,
                    WorkStatuses.idle,
                    buildIdOnBattle: "37599242-a840-4926-96f5-e0d938f0a903"
                )
        };

        scenarioItem.SetBuilds(scenarioBuilds);

        ObstacleOnBattle[] scenarioObstacles = new ObstacleOnBattle[]
        {
            new ObstacleOnBattle(
                coreObstacleId: "fccb430b-9ab1-4073-a334-6183a40ad0f2",
                obstaclePosition: new Bector2Int[] { new Bector2Int(0, 5) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "f6g7h8i9-j0k1-2345-fghi-678901234567"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "ee8c8e9e-8b30-4687-9179-0a279badf766",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 7) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "b2c3d4e5-f6g7-8901-bcde-f23456789012"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 9) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "c3d4e5f6-g7h8-9012-cdef-345678901234"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "1dc74e29-35f1-4fc5-a3f0-d2c6c39d558b",
                obstaclePosition: new Bector2Int[] { new Bector2Int(3, 10) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "d4e5f6g7-h8i9-0123-defg-456789012345"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "429b4e2e-b6a2-465b-982f-4cc96d67a011",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 2) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "ea70fb18-58d5-4dd1-a312-b9ca30ca7234"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(2, 0) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "e5f6g7h8-i9j0-1234-efgh-567890123456"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "fccb430b-9ab1-4073-a334-6183a40ad0f2",
                obstaclePosition: new Bector2Int[] { new Bector2Int(8, 0) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "g7h8i9j0-k1l2-3456-ghij-789012345678"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "90ebae2b-7633-42c7-9868-406d47583d5a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 1) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "h8i9j0k1-l2m3-4567-hijk-890123456789"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "7b6c7782-0a71-4501-87f5-ed18b935cea1",
                obstaclePosition: new Bector2Int[] { new Bector2Int(12, 6) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "i9j0k1l2-m3n4-5678-ijkl-901234567890"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "429b4e2e-b6a2-465b-982f-4cc96d67a011",
                obstaclePosition: new Bector2Int[] { new Bector2Int(8, 9) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "j0k1l2m3-n4o5-6789-jklm-012345678901"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "48b9e917-ecfb-44ea-ade9-25cec693789e",
                obstaclePosition: new Bector2Int[] { new Bector2Int(7, 11) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "k1l2m3n4-o5p6-7890-klmn-123456789012"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(3, 13) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "l2m3n4o5-p6q7-8901-lmno-234567890123"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "fccb430b-9ab1-4073-a334-6183a40ad0f2",
                obstaclePosition: new Bector2Int[] { new Bector2Int(3, 4) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "m3n4o5p6-q7r8-9012-mnop-345678901234"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "1dc74e29-35f1-4fc5-a3f0-d2c6c39d558b",
                obstaclePosition: new Bector2Int[] { new Bector2Int(7, 1) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "n4o5p6q7-r8s9-0123-nopq-456789012345"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "bb9da51e-303d-4aed-951d-0248490f76b6",
                obstaclePosition: new Bector2Int[] { new Bector2Int(2, 13) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "o5p6q7r8-s9t0-1234-opqr-567890123456"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(13, 11) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "p6q7r8s9-t0u1-2345-pqrs-678901234567"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "ee8c8e9e-8b30-4687-9179-0a279badf766",
                obstaclePosition: new Bector2Int[] { new Bector2Int(13, 5) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "q7r8s9t0-u1v2-3456-qrst-789012345678"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "429b4e2e-b6a2-465b-982f-4cc96d67a011",
                obstaclePosition: new Bector2Int[] { new Bector2Int(6, 12) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "r8s9t0u1-v2w3-4567-rstu-890123456789"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "7b6c7782-0a71-4501-87f5-ed18b935cea1",
                obstaclePosition: new Bector2Int[] { new Bector2Int(4, 11) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "s9t0u1v2-w3x4-5678-stuv-901234567890"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "90ebae2b-7633-42c7-9868-406d47583d5a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(9, 12) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "t0u1v2w3-x4y5-6789-tuvw-012345678901"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "bb9da51e-303d-4aed-951d-0248490f76b6",
                obstaclePosition: new Bector2Int[] { new Bector2Int(5, 10) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "u1v2w3x4-y5z6-7890-uvwx-123456789012"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "48b9e917-ecfb-44ea-ade9-25cec693789e",
                obstaclePosition: new Bector2Int[] { new Bector2Int(10, 8) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "v2w3x4y5-z6a7-8901-vwxy-234567890123"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(13, 3) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "w3x4y5z6-a7b8-9012-wxyz-345678901234"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "7b6c7782-0a71-4501-87f5-ed18b935cea1",
                obstaclePosition: new Bector2Int[] { new Bector2Int(13, 2) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "x4y5z6a7-b8c9-0123-xyza-456789012345"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "ee8c8e9e-8b30-4687-9179-0a279badf766",
                obstaclePosition: new Bector2Int[] { new Bector2Int(11, 10) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "y5z6a7b8-c9d0-1234-yzab-567890123456"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "0b3a9589-3e96-45c9-8e4e-ab6f8dc8cd7a",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 11) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "z6a7b8c9-d0e1-2345-zabc-678901234567"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "48b9e917-ecfb-44ea-ade9-25cec693789e",
                obstaclePosition: new Bector2Int[] { new Bector2Int(1, 4) },
                obstacleRotation: 90,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "a7b8c9d0-e1f2-3456-abcd-789012345678"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "ee8c8e9e-8b30-4687-9179-0a279badf766",
                obstaclePosition: new Bector2Int[] { new Bector2Int(12, 0) },
                obstacleRotation: 180,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "b8c9d0e1-f2g3-4567-bcde-890123456789"
            ),
            new ObstacleOnBattle(
                coreObstacleId: "bb9da51e-303d-4aed-951d-0248490f76b6",
                obstaclePosition: new Bector2Int[] { new Bector2Int(5, 9) },
                obstacleRotation: 0,
                obstacleSide: Sides.neutral,
                obstacleIdOnBattle: "c9d0e1f2-g3h4-5678-cdef-901234567890"
            )
        };

        scenarioItem.SetObstacles(scenarioObstacles);


        // Этап 1 - Начало миссии
        Replic[] dialogueForStage1 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Командир! Рада что вы пришли в себя. Сейчас база окружена силами противника, вы нужны нам чтобы возглавить оборону!",
                focus: new Bector2Int(6, 5)
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "После контузии тяжело соображать правда? Ничего страшного, сейчас всё вспомните."
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Чтобы двигать камеру, зажмите и двигайте мышью."
            )
        };

        // Условие для перехода к следующему этапу - практика с камерой
        Dictionary<string, object> dataForConditionForPassStage1 = new Dictionary<string, object>()
        {
            { "duration", 5 } // секунды практики
        };
        ConditionData conditionForPassStage1 = new ConditionData(type: "CameraMovementPractice", dataForConditionForPassStage1);

        // Поведения компонентов для этапа 1
        Dictionary<string, string> behaviourIdByComponentNameForStage1 = new Dictionary<string, string>()
        {
            { "Cell", "Disabled" },
            { "Obstacle", "Disabled" },
            { "Unit", "Disabled" },
            { "Build", "Disabled" },
            { "BuildingPanel", "Disabled" },
            { "FightPanel", "Disabled" },            
            { "BaseSettingsPanel", "Disabled" },
        };

        // ИИ этапа 1
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

        // Этап 2 - Расстановка войск
        Replic[] dialogueForStage2 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Вспомнили? Отлично! Пора приступать к бою."
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Выставите бойцов рядом со штабом, чтобы обеспечить его защиту. Я советую расставить их по сторонам, чтобы с каждой стороны по неприятелю был открыт огонь"
            )
        };

        ConditionData defeatConditionDataForStage2 = new ConditionData(type: "AlwaysFalse", null);
        ConditionData victoryConditionDataForStage2 = new ConditionData(type: "AlwaysTrue", null);

        .// Придумать как заставить игрока высадить всех и нажать нужные кнопки, заблокировав лишние

        LandingData landingData = new LandingData(
                landingMaxStaff: 20,
                landingZone: new Bector2Int[]
                {
                    new Bector2Int(5, 4),
                    new Bector2Int(5, 3),
                    new Bector2Int(4, 4),
                    new Bector2Int(4, 5),
                    new Bector2Int(4, 6),
                    new Bector2Int(4, 7),
                    new Bector2Int(4, 8),
                    new Bector2Int(5, 8),
                    new Bector2Int(6, 8),
                    new Bector2Int(7, 8),
                    new Bector2Int(8, 8),
                    new Bector2Int(8, 7),
                    new Bector2Int(8, 6),
                    new Bector2Int(8, 5),
                    new Bector2Int(8, 4),
                    new Bector2Int(8, 3),
                    new Bector2Int(7, 4),
                    new Bector2Int(7, 3),
                    new Bector2Int(6, 4),
                    new Bector2Int(6, 3),
                }
            );

        // Поведения компонентов для этапа 2
        Dictionary<string, string> behaviourIdByComponentNameForStage2 = new Dictionary<string, string>()
        {
            { "FightPanel", "Disabled" },
            { "BaseSettingsPanel", "Disabled" },
        };

        // ИИ этапа 2
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

        // Этап 3 - Проверка дистанции огня
        Replic[] dialogueForStage3 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Хорошая работа. Теперь огляните свои силы, чтобы понять дистанцию, с который мы сможем открыть огонь по врагу"
            ),
        };

        // Условие для перехода к следующему этапу - правильная расстановка юнитов
        Dictionary<string, object> dataForConditionForPassStage3 = new Dictionary<string, object>()
        {
            { "times", "3" },
        };
        ConditionData conditionForPassStage3 = new ConditionData(type: "EnterOnFederationObjectCondition", dataForConditionForPassStage3);


        Реализовать фокус по любому дружественному юниту и установить здесь его
            Так же сделать поведение юнита активным если нет фокусов либо фокус на конкретном этом юните, аналогично про здания


        // ИИ этапа 3
        AISettings AISettingsForEmpireSideInStage3 = new AISettings(
            "Frozen",
            Sides.empire,
            null,
            null
            );
        AISettings AISettingsForNeutralSideInStage3 = new AISettings(
            "Frozen",
            Sides.neutral,
            null,
            null
            );

        // Поведения компонентов для этапа 3
        Dictionary<string, string> behaviourIdByComponentNameForStage3 = new Dictionary<string, string>()
        {
            { "Cell", "Disabled" },
            { "Obstacle", "Disabled" },
            { "Unit", "Disabled" },
            { "Build", "Disabled" },
            { "BuildingPanel", "Disabled" },
            { "FightPanel", "Disabled" },
            { "BaseSettingsPanel", "Disabled" },
        };

        // Этап 4 - Начало боя
        Replic[] dialogueForStage4 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Они уже наступают! Приготовьтесь к обороне!"
            )
        };

        // Условие для перехода к следующему этапу - новая цель для атаки
        Dictionary<string, object> dataForConditionForPassStage4 = new Dictionary<string, object>()
        {
            { "targetSide", Sides.empire },
        };
        ConditionData conditionForPassStage4 = new ConditionData(type: "NewTargetForAttackCondition", dataForConditionForPassStage4);



        В фокус контроллере сделать поддержку нахождения первой цели для атаки и здесь установить её


        // Фокус на вражеских юнитах
        FocusData focusDataForStage4 = new FocusData(type: "Units", data: new Dictionary<string, object>()
        {
            { "side", Sides.empire }
        });

        // Поведения компонентов для этапа 4
        Dictionary<string, string> behaviourIdByComponentNameForStage4 = new Dictionary<string, string>()
        {
            { "Shop", "Disabled" },
            { "Inventory", "Disabled" },
            { "Campany", "Disabled" },
            { "Contracts", "Disabled" },
            { "UnitProductions", "Disabled" },
            { "Obstacle", "Disabled" },
            { "BuildingPanel", "Disabled" },
            { "BaseSettingsPanel", "Disabled" },
            { "Units", "Combat" }
        };

        // Этап 5 - Появление танкистки
        Replic[] dialogueForStage5 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Отлично! Мы перехватили инициативу! В атаку!"
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Теперь нам нужно отвоевать нашу казарму у врага. Солдат там немного, поэтому без проблемы справитесь с ними. Но и не забудьте про штаб! Оставьте там солдат для прикрытия и выдвигайтесь на захват казармы!"
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Мне доложили, что противник не атакует. Судя по всему, у них кончились силы для атаки. А если они не уходят, это говорит о том, что они ждут подкрепления! Нужно их немедленно уничтожить!"
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Согласно разведданным, остатки вражеских сил осели в ближайших лесных массивах. Нам стоит развить успех и разбить неприятеля. Но не исключено, что он от нас этого и ждёт."
            )
        };

        // Условие для перехода к следующему этапу - достижение лесного массива
        Dictionary<string, object> dataForConditionForPassStage5 = new Dictionary<string, object>()
        {
            { "type", "ReachedForest" },
            { "unitCount", 3 }
        };
        ConditionData conditionForPassStage5 = new ConditionData(type: "CustomCondition", dataForConditionForPassStage5);

        // Фокус на лесном массиве
        FocusData focusDataForStage5 = new FocusData(type: "Area", data: new Dictionary<string, object>()
        {
            { "type", "Forest" }
        });

        // Этап 6 - Появление танкистки
        Replic[] dialogueForStage6 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Теперь понятно почему они перестали атаковать. Ладно, заканчивайте с ними поскорее! Вашу атаку будет поддерживать командир танка… Точнее…"
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Танка у нас нет, но под её командованием сейчас небольшая группа пехоты. Она вам поможет."
            ),
            new Replic(
                charName: Chars.tankist,
                charSide: Sides.federation,
                text: "Здравия желаю, командир! Ну что, прошла голова? Отлично, тогда за дело. Позвольте им отвлечься на вас и подойти, пока они будут заняты вами, я нанесу им удар во фланг и тыл!"
            )
        };

        // Условие для перехода к следующему этапу - появление подкрепления
        Dictionary<string, object> dataForConditionForPassStage6 = new Dictionary<string, object>()
        {
            { "type", "ReinforcementsArrived" },
            { "unitCount", 6 }
        };
        ConditionData conditionForPassStage6 = new ConditionData(type: "CustomCondition", dataForConditionForPassStage6);

        // Фокус на подкреплении
        FocusData focusDataForStage6 = new FocusData(type: "Units", data: new Dictionary<string, object>()
        {
            { "side", Sides.federation },
            { "isReinforcement", true }
        });

        // Добавление новых этапов
        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage1,
            conditionForPass: conditionForPassStage1,
            focus: focusDataForStage1,
            behaviourIdByComponentName: behaviourIdByComponentNameForStage1
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage2,
            conditionForPass: conditionForPassStage2,
            focus: focusDataForStage2
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage3,
            conditionForPass: conditionForPassStage3,
            focus: focusDataForStage3,
            behaviourIdByComponentName: behaviourIdByComponentNameForStage3
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage4,
            conditionForPass: conditionForPassStage4,
            focus: focusDataForStage4,
            behaviourIdByComponentName: behaviourIdByComponentNameForStage4
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage5,
            conditionForPass: conditionForPassStage5,
            focus: focusDataForStage5
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage6,
            conditionForPass: conditionForPassStage6,
            focus: focusDataForStage6
        ));

        // Этап 7 - Финальный бой
        Replic[] dialogueForStage7 = new Replic[]
        {
            new Replic(
                charName: Chars.tankist,
                charSide: Sides.federation,
                text: "Отлично! Противник уничтожен!"
            )
        };

        // Условие для перехода к следующему этапу - уничтожение всех вражеских юнитов
        Dictionary<string, object> dataForConditionForPassStage7 = new Dictionary<string, object>()
        {
            { "type", "AllEnemyUnitsDestroyed" }
        };
        ConditionData conditionForPassStage7 = new ConditionData(type: "CustomCondition", dataForConditionForPassStage7);

        // Этап 8 - Появление вражеского офицера
        Replic[] dialogueForStage8 = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Похоже моя версия с подкреплением оказалась верной. Но я ожидала увидеть более многочисленную группировку."
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Самое время их остановить! Займитесь ими!"
            ),
            new Replic(
                charName: Chars.enemyOfficer,
                charSide: Sides.empire,
                text: "Не ожидал от своих солдат услышать запрос о подкреплении. Всё ещё не угомонитесь? Ладно парни, разберитесь там с ними…"
            )
        };

        // Условие для перехода к следующему этапу - уничтожение 6 вражеских юнитов
        Dictionary<string, object> dataForConditionForPassStage8 = new Dictionary<string, object>()
        {
            { "type", "EnemyUnitsDestroyed" },
            { "count", 6 }
        };
        ConditionData conditionForPassStage8 = new ConditionData(type: "CustomCondition", dataForConditionForPassStage8);

        // Фокус на вражеских юнитах
        FocusData focusDataForStage8 = new FocusData(type: "Units", data: new Dictionary<string, object>()
        {
            { "side", Sides.empire }
        });

        // Этап 9 - Завершение миссии
        Replic[] dialogueForStage9 = new Replic[]
        {
            new Replic(
                charName: Chars.enemyOfficer,
                charSide: Sides.empire,
                text: "Я вас недооценил, но это ничего не решит. Ваш разгром лишь вопрос времени…"
            ),
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Ура! Мы отразили атаку! Враг надолго запомнит этот бой."
            ),
            new Replic(
                charName: Chars.tankist,
                charSide: Sides.federation,
                text: "Это был славный бой. Теперь главное не терять хватку и продолжать бить неприятеля!"
            )
        };

        // Условие для завершения миссии - уничтожение всех вражеских юнитов
        Dictionary<string, object> dataForConditionForPassStage9 = new Dictionary<string, object>()
        {
            { "type", "AllEnemyUnitsDestroyed" }
        };
        ConditionData conditionForPassStage9 = new ConditionData(type: "CustomCondition", dataForConditionForPassStage9);

        // Добавление финальных этапов
        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage7,
            conditionForPass: conditionForPassStage7
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage8,
            conditionForPass: conditionForPassStage8,
            focus: focusDataForStage8
        ));

        scenarioItem.AddStage(new Stage(
            dialogue: dialogueForStage9,
            conditionForPass: conditionForPassStage9
        ));

        // Обработка провала миссии
        Replic[] dialogueOnFail = new Replic[]
        {
            new Replic(
                charName: Chars.officer,
                charSide: Sides.federation,
                text: "Командир! Рада что вы пришли в себя. Вы выглядите напугано: вам снился кошмар?"
            )
        };

        // Условие провала - потеря всех юнитов
        Dictionary<string, object> dataForConditionForFail = new Dictionary<string, object>()
        {
            { "type", "AllFriendlyUnitsDestroyed" }
        };
        ConditionData conditionForFail = new ConditionData(type: "CustomCondition", dataForConditionForFail);

        // Добавление обработки провала
        scenarioItem.SetFailDialogue(dialogueOnFail);
        scenarioItem.SetFailCondition(conditionForFail);

        // Сохранение сценария
        table.Add(scenarioItem);
        Cache.Save(table);
    }
}
