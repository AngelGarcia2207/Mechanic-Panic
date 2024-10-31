using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentFinder
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
}