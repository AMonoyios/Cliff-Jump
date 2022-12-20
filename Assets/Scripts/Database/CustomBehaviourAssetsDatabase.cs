using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomBehaviourAssetsDatabase
{
    private static readonly Dictionary<int, IBehaviour> behaviours = new();
    public static Dictionary<int, IBehaviour>.ValueCollection Values => behaviours.Values;

    public static T Register<T>(T behaviour) where T : class, IBehaviour
    {
        if (!behaviours.ContainsKey(behaviour.GetGameObject.GetHashCode()))
        {
            behaviours.Add(behaviour.GetGameObject.GetHashCode(), behaviour);
        }
        else
        {
            Debug.LogWarning($"Asset {behaviour.GetGameObject} already exist in Database!");
        }

        return behaviour;
    }

    public static T GetBehaviour<T>(GameObject gameObject) where T : class, IBehaviour
    {
        if (behaviours.ContainsKey(gameObject.GetHashCode()) && behaviours[gameObject.GetHashCode()] is T t)
            return t;

        return null;
    }
}
