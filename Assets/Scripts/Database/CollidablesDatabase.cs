using System.Collections.Generic;
using UnityEngine;

public static class CollidablesDatabase
{
    private static readonly Dictionary<int, ICollidable> collidables = new();
    public static Dictionary<int, ICollidable>.ValueCollection Values => collidables.Values;

    public static T Register<T>(T behaviour) where T : class, ICollidable
    {
        collidables.Add(behaviour.gameObject.GetHashCode(), behaviour);

        return behaviour;
    }

    public static T GetBehaviour<T>(GameObject gameObject) where T : class, ICollidable
    {
        if (collidables.ContainsKey(gameObject.GetHashCode()) && collidables[gameObject.GetHashCode()] is T t)
            return t;

        return null;
    }
}
