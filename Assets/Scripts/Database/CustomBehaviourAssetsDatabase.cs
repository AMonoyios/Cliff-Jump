using System.Collections.Generic;
using UnityEngine;

// - This is the custom behaviour database.
// - This is where all custom behaviour components are registered so they can be accessed later from any where in the code.
public static class CustomBehaviourAssetsDatabase
{
    private static readonly Dictionary<int, IBehaviour> behaviours = new();
    public static Dictionary<int, IBehaviour>.ValueCollection Values => behaviours.Values;

    // Method that registers new custom behaviour components in the database
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

    // Method that checks if a specific GameObject is already registered in the database or not
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
