using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class Pool
{
    private static readonly Dictionary<string, Queue<GameObject>> poolDictionary = new();

    public static void Add(string id, Queue<GameObject> gameObjectsQueue)
    {
        poolDictionary.Add(id, gameObjectsQueue);
    }

    public static GameObject Spawn(string id, Vector3 position, Quaternion rotation)
    {
        if (!ContainsKey(id))
            return null;

        return SetAssetState(id, true, position, rotation);
    }

    public static GameObject Despawn(string id)
    {
        if (!ContainsKey(id))
            return null;

        return SetAssetState(id, false, Vector3.zero, Quaternion.identity);
    }

    private static GameObject SetAssetState(string id, bool state, Vector3 position, Quaternion rotation)
    {
        GameObject pooledObject = poolDictionary[id].Dequeue();

        pooledObject.SetActive(state);
        pooledObject.transform.SetPositionAndRotation(position, rotation);

        poolDictionary[id].Enqueue(pooledObject);

        return pooledObject;
    }

    public static bool ContainsKey(string id)
	{
        if (!poolDictionary.ContainsKey(id))
        {
            Debug.LogError($"Key: {id} does not exist in pool dictionary.");
            return false;
        }

        return true;
    }
}
