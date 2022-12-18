using System.Collections.Generic;
using UnityEngine;

public static class CustomBehaviourAssetDatabase
{
    private static readonly Dictionary<int, IUpdatable> assets = new();
    public static Dictionary<int, IUpdatable>.ValueCollection Values => assets.Values;

    public static T Register<T>(T behaviour) where T : class, IUpdatable
    {
        assets.Add(behaviour.gameObject.GetHashCode(), behaviour);
        return behaviour;
    }

    public static T GetBehaviour<T>(GameObject gameObject) where T : class, IUpdatable
    {
        if (assets.ContainsKey(gameObject.GetHashCode()) && assets[gameObject.GetHashCode()] is T t)
        {
            return t;
        }

        return null;
    }
}
