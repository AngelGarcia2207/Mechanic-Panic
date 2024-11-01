using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Finder
{
    public static T FindComponentInParents<T>(Transform child) where T : Component
    {
        Transform currentTransform = child;

        while (currentTransform != null)
        {
            T component = currentTransform.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }

    public static Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform foundChild = FindChildRecursive(child, childName);
            if (foundChild != null)
            {
                return foundChild;
            }
        }
        return null;
    }
}