using System.Collections.Generic;
using UnityEngine;

public static class SimpleCollision
{
    private static readonly Dictionary<int, IUpdatable> collidables = new();
    public static Dictionary<int, IUpdatable>.ValueCollection Values => collidables.Values;

    public static T Register<T>(T behaviour) where T : class, IUpdatable
    {
        collidables.Add(behaviour.gameObject.GetHashCode(), behaviour);
        return behaviour;
    }

    public static T GetBehaviour<T>(GameObject gameObject) where T : class, IUpdatable
    {
        if (collidables.ContainsKey(gameObject.GetHashCode()) && collidables[gameObject.GetHashCode()] is T t)
        {
            return t;
        }

        return null;
    }
}
