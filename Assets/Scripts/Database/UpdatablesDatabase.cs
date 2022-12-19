using System.Collections.Generic;
using UnityEngine;

public static class UpdatablesDatabase
{
    private static readonly Dictionary<int, IUpdatable> updatables = new();
    public static Dictionary<int, IUpdatable>.ValueCollection Values => updatables.Values;

    public static T Register<T>(T behaviour) where T : class, IUpdatable
    {
        updatables.Add(behaviour.gameObject.GetHashCode(), behaviour);

        return behaviour;
    }

    public static T GetBehaviour<T>(GameObject gameObject) where T : class, IUpdatable
    {
        if (updatables.ContainsKey(gameObject.GetHashCode()) && updatables[gameObject.GetHashCode()] is T t)
            return t;

        return null;
    }
}
