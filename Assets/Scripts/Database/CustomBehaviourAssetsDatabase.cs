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
            IsRegistered(behaviour.GetGameObject);
        }
        else
        {
            Debug.LogWarning($"Asset {behaviour.GetGameObject} (HashCode: {behaviour.GetGameObject.GetHashCode()}) already registered in Database.");
        }

        return behaviour;
    }

    /// <summary>
    ///     Returns all behaviours of specified type already registered in the database.
    /// </summary>
    /// <typeparam name="T">The specified type to return.</typeparam>
    /// <remarks>Note: This will return self component as well!</remarks>
    public static T GetBehaviour<T>(GameObject gameObject) where T : class, IBehaviour
    {
        if (IsRegistered(gameObject) && behaviours[gameObject.GetHashCode()] is T behaviour)
            return behaviour;

        return null;
    }

    public static bool IsRegistered(GameObject gameObject)
    {
        if (!behaviours.ContainsKey(gameObject.GetHashCode()))
        {
            Debug.LogWarning($"{gameObject.name} (HashCode: {gameObject.GetHashCode()}) is not registered in Database.");
            return false;
        }

        return true;
    }
}
