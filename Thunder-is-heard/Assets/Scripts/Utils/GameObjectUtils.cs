using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    public static Transform FindChildObjectByTag(GameObject root, string tag)
    {
        Transform[] childTransforms =  root.GetComponentsInChildren<Transform>(true);
        foreach (Transform childTransform in childTransforms)
        {
            if (childTransform.CompareTag(tag))
            {
                return childTransform;
            }
        }

        return null;
    }

    /// <summary>
    /// Находит GameObject по тегу, включая неактивные объекты
    /// </summary>
    /// <param name="tag">Тег для поиска</param>
    /// <returns>GameObject с указанным тегом или null</returns>
    public static GameObject FindGameObjectByTagIncludingInactive(string tag)
    {
        // Сначала пробуем найти активный объект
        GameObject activeObject = GameObject.FindGameObjectWithTag(tag);
        if (activeObject != null)
        {
            return activeObject;
        }

        // Если не найден, ищем среди всех объектов в сцене, включая неактивные
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }

        return null;
    }

    /// <summary>
    /// Находит компонент по тегу, включая неактивные объекты
    /// </summary>
    /// <typeparam name="T">Тип компонента</typeparam>
    /// <param name="tag">Тег для поиска</param>
    /// <returns>Компонент типа T или null</returns>
    public static T FindComponentByTagIncludingInactive<T>(string tag) where T : Component
    {
        GameObject obj = FindGameObjectByTagIncludingInactive(tag);
        if (obj != null)
        {
            return obj.GetComponent<T>();
        }
        return null;
    }
}
