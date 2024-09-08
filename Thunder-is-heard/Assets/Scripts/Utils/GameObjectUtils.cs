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
}
