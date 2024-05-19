using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public GameObject selector;
    public Sprite friendlySelectorSprite, enemySelectorSprite, neutralSelectorSprite, attackableSelector;

    public Canvas attackRadiusCanvas;
    public int attackRadiusSizePerCell = 800;

    public Canvas objectInfoCanvas;
    public TMP_Text name;

    public Slider healthSlider;
    public Image healthPanel;
    public TMP_Text healthCount;

    public Image damagePanel;
    public TMP_Text damageCount;


    public Canvas productsInfoCanvas;
    public Vector3 productsInfoCanvasOffset = new Vector3(0.591f, 0, 0.591f);
    public int productsEndTime;
    public bool isSelectedObjectProducts = false;
    public TMP_Text productInfoText;
    public Image productionImage;
    public TMP_Text productionCount;


    void Start()
    {
        InitSelectorSprites();
        TurnOff();

        EventMaster.current.EnteredOnObject += OnEnterObject;
        EventMaster.current.ExitedOnObject += OnExitObject;
    }

    public void InitSelectorSprites()
    {
        friendlySelectorSprite = Resources.Load<Sprite>(Config.resources["allySelector"]);
        enemySelectorSprite = Resources.Load<Sprite>(Config.resources["enemySelector"]);
        neutralSelectorSprite = Resources.Load<Sprite>(Config.resources["neutralSelector"]);
        attackableSelector = Resources.Load<Sprite>(Config.resources["attackableSelector"]);
    }


    private void Update()
    {
        if (isSelectedObjectProducts)
        {
            UpdateProductsLeftTimeText();
        }
    }

    public void OnEnterObject(Entity obj)
    {
        objectInfoCanvas.enabled = true;
        ConfigureName(obj);
        ConfigureInfoPanel(obj);
        ConfigureRadius(obj);
        ConfigureSelector(obj);
        ConfigureHealthSlider(obj);
        ConfigureHealth(obj);
        ConfigureDamage(obj);

        if (obj is Build)
        {
            Build build = (Build)obj;
            if (build.workStatus == WorkStatuses.working)
            {
                ConfigureProductsInfo(build);
            }
        }
    }

    public void OnExitObject(Entity obj)
    {
        TurnOff();
    }

    public void TurnOff()
    {
        objectInfoCanvas.enabled = attackRadiusCanvas.enabled =  productsInfoCanvas.enabled = false;
        selector.SetActive(false);
        isSelectedObjectProducts = false;
    }

    public void ConfigureName(Entity obj)
    {
        name.text = obj.name;
    }

    public void ConfigureInfoPanel(Entity obj)
    {
        ConfigureInfoPanelPosition(obj);
    }

    public void ConfigureInfoPanelPosition(Entity obj)
    {
        objectInfoCanvas.transform.position = new Vector3(
            obj.model.transform.position.x,
            objectInfoCanvas.transform.position.y,
            obj.model.transform.position.z
        );
    }

    public void ConfigureSelector(Entity obj)
    {
        selector.SetActive(true);
        ConfigureSelectorSprite(obj);
        ConfigureSelectorPosition(obj);
        ConfigureSelectorSize(obj);

    }

    public void ConfigureSelectorSprite(Entity obj)
    {
        Dictionary<string, Sprite> selectorSpriteBySide = new Dictionary<string, Sprite>()
        {
            { Config.sides["ally"], friendlySelectorSprite },
            { Config.sides["enemy"], enemySelectorSprite },
            { Config.sides["neutral"], neutralSelectorSprite },
        };

        SpriteRenderer selectorImage = selector.GetComponent<SpriteRenderer>();

        string side = obj.side;

        Sprite spriteForSelector = selectorSpriteBySide[side];

        selectorImage.sprite = spriteForSelector;
    }

    public void ConfigureSelectorPosition(Entity obj)
    {
        selector.transform.position = new Vector3(
            obj.model.transform.position.x, 
            selector.transform.position.y, 
            obj.model.transform.position.z
        );
    }

    public void ConfigureSelectorSize(Entity obj)
    {
        int maxSize  =obj.currentSize.x;
        if (obj.currentSize.y > maxSize) {
            maxSize = obj.currentSize.y;
        }

        selector.transform.localScale = new Vector3(
                maxSize,
                maxSize,
                maxSize
        );
    }


    public void ConfigureRadius(Entity obj)
    {
        if (obj.distance < 1)
        {
            attackRadiusCanvas.enabled = false;
            return;
        }

        ConfigureRadiusPosition(obj);
        int rangeSize = (obj.distance - 1) * 1600 + attackRadiusSizePerCell;

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
        healthSlider.maxValue = obj.health;
        healthSlider.value = obj.health;
    }

    public void ConfigureHealth(Entity obj)
    {
        healthCount.text = obj.health.ToString();
    }

    public void ConfigureDamage(Entity obj)
    {
        if (obj.damage < 1)
        {
            damagePanel.gameObject.SetActive(false);
            return;
        }

        damagePanel.gameObject.SetActive(true);
        damageCount.text = obj.damage.ToString();
    }


    public void ConfigureProductsInfo(Build build)
    {
        ConfigureProductsInfoCanvasPosition(build);

        isSelectedObjectProducts = true;

        ProcessOnBaseCacheTable processesOnBaseTable = Cache.LoadByType<ProcessOnBaseCacheTable>();
        ProcessOnBaseCacheItem productsProcessData = processesOnBaseTable.FindBySourceObjectId(build.ChildId);
        if (productsProcessData != null)
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
                //TODO �����������
                break;
        }
    }

    public void ConfigureProductsInfoCanvasPosition(Build build)
    {
        productsInfoCanvas.transform.position = new Vector3(
            build.model.transform.position.x + productsInfoCanvasOffset.x,
            productsInfoCanvas.transform.position.y + productsInfoCanvasOffset.y,
            build.model.transform.position.z + productsInfoCanvasOffset.z
        );
    }

    public void ConfigureProductionForContract(string contractId)
    {
        ContractCacheTable contractsTable = Cache.LoadByType<ContractCacheTable>();
        CacheItem itemData = contractsTable.GetById(contractId);
        ContractCacheItem contractItemData = new ContractCacheItem(itemData.Fields);

        ResourcesData gives = contractItemData.GetGives();
        Dictionary<string, string> resourceData = ResourcesProcessor.GetFirstNotEmptyResourceData(gives);
        productionCount.text = resourceData["count"].ToString();
        productionImage.sprite = Resources.Load<Sprite>(resourceData["iconPath"]);
    }

    public void UpdateProductsLeftTimeText()
    {
        int currentTime = (int)Time.realtimeSinceStartup;
        int leftTime = productsEndTime - currentTime;
        productInfoText.text = "����� " + TimeUtils.GetTimeAsStringBySeconds(leftTime);
    }
}
