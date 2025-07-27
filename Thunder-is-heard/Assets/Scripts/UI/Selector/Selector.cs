using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Selector : MonoBehaviour
{
    public Entity _selectedObject = null;
    public Build selectedBuild = null;

    public GameObject selector;
    public List<GameObject> selectorsForAttackers = new List<GameObject>();
    public Sprite friendlySelectorSprite, enemySelectorSprite, neutralSelectorSprite, attackableSelectorSprite;

    public Canvas attackRadiusCanvas;
    public int attackRadiusSizePerCell = 800;

    public Canvas objectInfoCanvas;
    public Vector3 objectInfoCanvasOffset = new Vector3(-1, 2, -1);
    public TMP_Text name;

    public Slider healthSlider;
    public Image healthPanel;
    public TMP_Text healthCount;

    public Image damagePanel;
    public TMP_Text damageCount;

    public GameObject skillsPanel;
    public Transform skillsContent;
    public GameObject _skillPrefab;


    public Canvas productsInfoCanvas;
    public Vector3 productsInfoCanvasOffset = new Vector3(-0.408f, 2.83f, -0.408f);
    public DateTime productsEndTime;
    public bool isSelectedObjectProducts = false;
    public TMP_Text productInfoText;
    public Image productionImage;
    public TMP_Text productionCount;

    public UnitsOnFight _unitsOnFight;
    public BuildsOnFight _buildsOnFight;
    public BattleEngine _battleEngine;
    public bool isFight;


    void Start()
    {
        InitState();
        InitSelectorSprites();
        InitSkillElementPrefab();
        TurnOff();

        EventMaster.current.EnteredOnObject += OnEnterObject;
        EventMaster.current.ExitedOnObject += OnExitObject;
        EventMaster.current.DamagedObject += OnDamageObject;
        EventMaster.current.DestroyedObject += OnDestroyObject;
        EventMaster.current.UnitMoveFinished += OnFinishSomeUnitMove;
    }

    public void InitState()
    {
        SceneState sceneState = GameObject.FindWithTag("State").GetComponent<SceneState>();
        isFight = sceneState.currentState.stateName == Scenes.fight;
        if (isFight )
        {
            _battleEngine = GameObject.FindGameObjectWithTag(Tags.battleEngine).GetComponent<BattleEngine>();
            _unitsOnFight = GameObject.FindGameObjectWithTag(Tags.unitsOnScene).GetComponent<UnitsOnFight>();
            _buildsOnFight = GameObject.FindGameObjectWithTag(Tags.buildsOnScene).GetComponent<BuildsOnFight>();
        }
    }

    public void InitSelectorSprites()
    {
        friendlySelectorSprite = ResourcesUtils.LoadIcon(Config.resources["allySelector"]);
        enemySelectorSprite = ResourcesUtils.LoadIcon(Config.resources["enemySelector"]);
        neutralSelectorSprite = ResourcesUtils.LoadIcon(Config.resources["neutralSelector"]);
        attackableSelectorSprite = ResourcesUtils.LoadIcon(Config.resources["attackableSelector"]);
    }

    public void InitSkillElementPrefab()
    {
        _skillPrefab = Resources.Load<GameObject>(Config.resources["UISkillItemPrefab"]);
    }

    private void Update()
    {
        if (isSelectedObjectProducts && selectedBuild != null)
        {
            if (selectedBuild.workStatus == WorkStatuses.working) 
            {
                UpdateProductsLeftTimeText();
            }
            else
            {
                OffProductsInfo(selectedBuild);
            }
        }
    }

    public void OnFinishSomeUnitMove(Unit unit)
    {
        if (_selectedObject != null && _selectedObject.ChildId == unit.ChildId)
        {
            OnEnterObject(unit);
        }
    }

    public void OnEnterObject(Entity obj)
    {
        _selectedObject = obj;
        ConfigureName(obj);
        ConfigureSelector(obj, selector);
        ConfigureInfoPanel(obj);
        ConfigureRadius(obj);
        ConfigureHealthSlider(obj);
        ConfigureHealth(obj);
        ConfigureDamage(obj);

        bool mayHaveAttackers = false;
        if (obj is Build)
        {
            mayHaveAttackers = true;
            Build build = (Build)obj;
            selectedBuild = build;
            if (build.workStatus == WorkStatuses.working)
            {
                ConfigureProductsInfo(build);
            }
        }

        else if (obj is Unit unit)
        {
            if (unit._onMove == true)
            {
                TurnOff(false);
                return;
            }
            mayHaveAttackers = true;
            if (unit._skills != null && unit._skills.Length > 0)
            {
                ConfigureSkillsInfo(unit);
            }
        }

        if (isFight && mayHaveAttackers)
        {
            ConfigureAttackersDisplay(obj);
        }
    }

    public void OnExitObject(Entity obj)
    {
        TurnOff();
    }

    public void OnDamageObject(Entity obj)
    {
        if (_selectedObject == null) return;
        if (_selectedObject.ChildId != obj.ChildId) return;

        OnEnterObject(obj);
    }

    public void OnDestroyObject(Entity obj)
    {
        if (_selectedObject == null) return;
        if (_selectedObject.ChildId != obj.ChildId) return;

        TurnOff();
    }

    public void TurnOff(bool forgetSelectedObject = true)
    {
        if (forgetSelectedObject == true) _selectedObject = null;
        skillsPanel.SetActive(false);
        objectInfoCanvas.enabled = attackRadiusCanvas.enabled =  productsInfoCanvas.enabled = false;
        selector.SetActive(false);
        if (selectorsForAttackers != null && selectorsForAttackers.Count > 0)
        {
            foreach (var selector in selectorsForAttackers)
            {
                Destroy(selector.gameObject);
            }
            selectorsForAttackers = new List<GameObject>();
        }

        isSelectedObjectProducts = false;
    }

    public void ConfigureName(Entity obj)
    {
        name.text = obj.name;
    }

    public void ConfigureInfoPanel(Entity obj)
    {
        objectInfoCanvas.enabled = true;
        ConfigureInfoPanelPosition(obj);
    }

    public void ConfigureInfoPanelPosition(Entity obj)
    {
        objectInfoCanvas.transform.position = new Vector3(
            obj.model.transform.position.x + objectInfoCanvasOffset.x,
            objectInfoCanvasOffset.y,
            obj.model.transform.position.z + objectInfoCanvasOffset.z
        );
    }

    public void ConfigureSelector(Entity obj, GameObject targetSelector)
    {
        targetSelector.SetActive(true);
        ConfigureSelectorSprite(obj, targetSelector);
        ConfigureSelectorPosition(obj, targetSelector);
        ConfigureSelectorSize(obj, targetSelector);

    }

    public void ConfigureSelectorForAttacker(Entity attacker)
    {
        GameObject targetSelector = Instantiate(selector, selector.transform.position, Quaternion.identity, transform);
        selectorsForAttackers.Add(targetSelector);
        ConfigureSelector(attacker, targetSelector);
    }

    public void ConfigureSelectorSprite(Entity obj, GameObject targetSelector)
    {
        Dictionary<string, Sprite> selectorSpriteBySide = new Dictionary<string, Sprite>()
        {
            { Sides.federation, friendlySelectorSprite },
            { Sides.empire, enemySelectorSprite },
            { Sides.neutral, neutralSelectorSprite },
        };

        SpriteRenderer selectorImage = targetSelector.GetComponent<SpriteRenderer>();

        string side = obj.side;
        Sprite spriteForSelector;
        if (isFight && side == Sides.empire && _battleEngine.currentBattleSituation.GetAttackersByTargetId(obj.ChildId).Count > 0)
        {
            spriteForSelector = attackableSelectorSprite;
        }
        else
        {
            spriteForSelector = selectorSpriteBySide[side];
        }

        selectorImage.sprite = spriteForSelector;
    }

    public void ConfigureSelectorPosition(Entity obj, GameObject targetSelector)
    {
        targetSelector.transform.position = new Vector3(
            obj.model.transform.position.x, 
            targetSelector.transform.position.y, 
            obj.model.transform.position.z
        );
    }

    public void ConfigureSelectorSize(Entity obj, GameObject targetSelector)
    {
        int maxSize  =obj.currentSize.x;
        if (obj.currentSize.y > maxSize) {
            maxSize = obj.currentSize.y;
        }

        targetSelector.transform.localScale = new Vector3(
                maxSize,
                maxSize,
                maxSize
        );
    }


    public void ConfigureRadius(Entity obj)
    {
        if (obj.distance < 1 || obj is Obstacle)
        {
            attackRadiusCanvas.enabled = false;
            return;
        }

        ConfigureRadiusPosition(obj);
        int rangeSize = obj.distance * 1600 + attackRadiusSizePerCell;

        attackRadiusCanvas.enabled = true;
        attackRadiusCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(rangeSize, rangeSize);
    }

    public void ConfigureRadiusPosition(Entity obj)
    {
        attackRadiusCanvas.transform.position = new Vector3(
            obj.model.transform.position.x,
            attackRadiusCanvas.transform.position.y,
            obj.model.transform.position.z
        );
    }


    public void ConfigureHealthSlider(Entity obj)
    {
        if (obj is Obstacle)
        {
            healthSlider.gameObject.SetActive(false);
            return;
        }

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = obj.maxHealth;
        healthSlider.value = obj.currentHealth;
    }

    public void ConfigureHealth(Entity obj)
    {
        if (obj is Obstacle)
        {
            healthPanel.gameObject.SetActive(false);
            return;
        }

        healthPanel.gameObject.SetActive(true);
        healthCount.text = obj.maxHealth.ToString();
    }

    public void ConfigureDamage(Entity obj)
    {
        if (obj.damage < 1 || obj is Obstacle)
        {
            damagePanel.gameObject.SetActive(false);
            return;
        }

        damagePanel.gameObject.SetActive(true);
        damageCount.text = obj.damage.ToString();
    }


    public void OffProductsInfo(Build build)
    {
        productsInfoCanvas.enabled = false;
        isSelectedObjectProducts = false;
    }

    public void ConfigureProductsInfo(Build build)
    {
        productsInfoCanvas.enabled = true;
        ConfigureProductsInfoCanvasPosition(build);

        isSelectedObjectProducts = true;

        ProcessOnBaseCacheTable processesOnBaseTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        ProcessOnBaseCacheItem productsProcessData = processesOnBaseTable.FindBySourceObjectId(build.ChildId);
        if (productsProcessData == null)
        {
            return;
        }

        productsEndTime = productsProcessData.GetEndTime();
        UpdateProductsLeftTimeText();

        ProcessSource processSource = productsProcessData.GetSource();

        switch (processSource.type)
        {
            case "Contract":
                ConfigureProductionForContract(processSource.id);
                break;
            case "Improvement":
                //TODO �����������
                break;
            case "UnitProduction":
                ConfigureProductionForUnitProduction(processSource.id);
                break;
        }
    }

    public void ConfigureSkillsInfo(Unit unit)
    {
        skillsPanel.SetActive(true);
        ClearSkills();

        foreach (Skill skillOnUnit in unit._skills)
        {
            CreateSkill(skillOnUnit.CoreId);
        }
    }

    public void ClearSkills()
    {
        foreach (Transform child in skillsContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateSkill(string skillId)
    {
        SkillCacheTable skillsTable = Cache.LoadByType<SkillCacheTable>();
        CacheItem cacheItem = skillsTable.GetById(skillId);
        SkillCacheItem coreSkillData = new SkillCacheItem(cacheItem.Fields);
        string iconSection = Config.resources[coreSkillData.GetIconSection()];
        string iconName = coreSkillData.GetIconName();
        Sprite icon = ResourcesUtils.LoadIcon(iconSection, iconName);

        string descriptionText = coreSkillData.GetName();

        GameObject skillObject = Instantiate(_skillPrefab);
        skillObject.transform.SetParent(skillsContent, false);

        Image skillImage = skillObject.transform.Find("Icon").GetComponent<Image>();
        skillImage.sprite = icon;

        GameObject skillDescription = skillObject.transform.Find("Description").gameObject;
        TMP_Text skillDescriptionTextComponent = skillDescription.GetComponent<TMP_Text>();
        skillDescriptionTextComponent.text = descriptionText;
    }

    public void ConfigureProductsInfoCanvasPosition(Build build)
    {
        productsInfoCanvas.transform.position = new Vector3(
            build.model.transform.position.x + productsInfoCanvasOffset.x,
            productsInfoCanvasOffset.y,
            build.model.transform.position.z + productsInfoCanvasOffset.z
        );
    }

    public Sprite FindSpriteByName(string name, Sprite[] sprites)
    {
        if (sprites == null) return null;

        name = name.ToLower();
        foreach (Sprite icon in sprites)
        {
            if (name.Contains(icon.name))
            {
                return icon;
            }
        }

        return null;
    }

    public void ConfigureProductionForContract(string contractId)
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();
        CacheItem itemData = contractsTable.GetById(contractId);
        ContractCacheItem contractItemData = new ContractCacheItem(itemData.Fields);

        ResourcesData gives = contractItemData.GetGives();
        Dictionary<string, string> resourceData = ResourcesProcessor.GetFirstNotEmptyResourceData(gives);
        ConfigureProductionCountText(Convert.ToInt32(resourceData["count"]));
        productionImage.sprite = ResourcesProcessor.GetResourceSpriteByName(resourceData["name"]);
    }

    public void ConfigureProductionForUnitProduction(string unitProductionId)
    {
        UnitProductionCacheTable unitProductionsTable = Cache.LoadByType<UnitProductionCacheTable>();
        CacheItem itemData = unitProductionsTable.GetById(unitProductionId);
        UnitProductionCacheItem unitProductionItemData = new UnitProductionCacheItem(itemData.Fields);

        ConfigureProductionCountText(1);
        productionImage.sprite = ResourcesUtils.LoadIcon(unitProductionItemData.GetIconSection(), unitProductionItemData.GetIconName());
    }

    public void ConfigureProductionCountText(int count)
    {
        productionCount.text = count.ToString();
        productionCount.gameObject.SetActive(count > 1);
    }

    public void UpdateProductsLeftTimeText()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan remainingTime = productsEndTime - currentTime;
        int remainingSeconds = (int)remainingTime.TotalSeconds;
        productInfoText.text = "after " + TimeUtils.GetDHMSTimeAsStringBySeconds(remainingSeconds);
    }

    public void ConfigureAttackersDisplay(Entity obj)
    {
        List<ObjectOnBattle> attackersOnBattle = _battleEngine.currentBattleSituation.GetAttackersByTargetId(obj.ChildId);
        foreach (ObjectOnBattle attackerOnBattle in attackersOnBattle)
        {
            Entity foundedAttackerEntity = _unitsOnFight.FindObjectByChildId(attackerOnBattle.IdOnBattle);
            if (foundedAttackerEntity == null)
            {
                foundedAttackerEntity = _buildsOnFight.FindObjectByChildId(attackerOnBattle.IdOnBattle);
            }
            if (foundedAttackerEntity == null) continue;

            ConfigureSelectorForAttacker(foundedAttackerEntity);
        }
    }
}
