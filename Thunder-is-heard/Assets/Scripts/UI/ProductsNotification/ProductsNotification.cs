using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.IO;

public class ProductsNotifcation : UIElement
{
    public ResourcesProcessor resourcesProcessor;

    public string _sourceObjectId;
    public string _type;
    public TMP_Text _countText;
    public ResourcesData _gives;
    public Image _icon;
    public Image _background;
    public bool _isAvailableToCollect = false;

    public Sprite backgroundWhenCollectionAllowed, backgroundWhenCollectionForbidden, backgroundWhenIdle;


    public void Init(string sourceObjectId, string type, Sprite icon = null, int count = 1, ResourcesData gives = null)
    {
        _sourceObjectId = sourceObjectId;
        _type = type;
        _gives = gives;

        InitDependencies();
        InitSprites();
        InitEvents();

        _countText.text = count.ToString();
        if (count < 2)
        {
            _countText.gameObject.SetActive(false);
        }

        _icon.sprite = icon;

        if (type == ProductsNotificationTypes.waitingResourceCollection)
        {
            UpdateAvailability(resourcesProcessor.resources);
        }

        else if (type == ProductsNotificationTypes.idle)
        {
            _background.sprite = backgroundWhenIdle;
            _icon.gameObject.SetActive(false);
        }
        
        else
        {
            SetBackgroundAsCollectionAllowed();
        }
    }

    public void InitSprites()
    {
        backgroundWhenIdle = Resources.Load<Sprite>(Config.resources["productsCollectionBackgroundIconIdle"]);
        backgroundWhenCollectionAllowed = Resources.Load<Sprite>(Config.resources["productsCollectionBackgroundIconAllow"]);
        backgroundWhenCollectionForbidden = Resources.Load<Sprite>(Config.resources["productsCollectionBackgroundIconForbidden"]);
    }

    public void InitEvents()
    {
        if (_type == ProductsNotificationTypes.waitingResourceCollection)
        {
            EventMaster.current.ResourcesChanged += UpdateAvailability;
        }
        
        EventMaster.current.ProductsNotificationDeleted += SomeProductsNotificationDeleted;
    }

    public void InitDependencies()
    {
        resourcesProcessor = GameObject.FindGameObjectWithTag(Tags.resourcesProcessor).GetComponent<ResourcesProcessor>();
    }

    public void UpdateAvailability(ResourcesData newResources)
    {
        ResourcesData resourcesAfterAdd = newResources.Clone().Add(_gives);

        _isAvailableToCollect = !resourcesAfterAdd.IsOverflow();
        if (_isAvailableToCollect)
        {
            SetBackgroundAsCollectionAllowed();
        }

       else
        {
            SetBackgroundAsCollectionForbidden();
        }
    }

    public void SomeProductsNotificationDeleted(ProductsNotificationCacheItem productsNotification)
    {
        if (productsNotification.GetSourceObjectId() == _sourceObjectId) 
        {
            Destroy(this.gameObject);
        }
    }

    public void SetBackgroundAsCollectionAllowed()
    {
        _background.sprite = backgroundWhenCollectionAllowed;
    }

    public void SetBackgroundAsCollectionForbidden()
    {
        _background.sprite = backgroundWhenCollectionForbidden;
    }
}