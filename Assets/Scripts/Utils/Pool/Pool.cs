using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class Pool
{
    private static readonly Dictionary<string, Queue<GameObject>> poolDictionary = new();

    public static void AddToPoolDictionary(string id, Queue<GameObject> gameObjectsQueue)
    {
        poolDictionary.Add(id, gameObjectsQueue);
    }

    public static GameObject SpawnFromPool (string id, Vector3 position, Quaternion rotation)
    {
        if (!PoolContainsKey(id))
            return null;

        GameObject pooledObject = poolDictionary[id].Dequeue();

        pooledObject.SetActive(true);
        pooledObject.transform.SetPositionAndRotation(position, rotation);

        //FIXME: optional replace with IUpdatable
        if (pooledObject.TryGetComponent(out IPooled pooled))
            pooled.OnObjectSpawn();

        poolDictionary[id].Enqueue(pooledObject);

        return pooledObject;
    }

    private static bool PoolContainsKey(string id)
	{
        if (!poolDictionary.ContainsKey(id))
        {
            Debug.LogError($"Key: {id} does not exist in pool dictionary.");
            return false;
        }

        return true;
    }
}
