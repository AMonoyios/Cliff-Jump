using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Pool
{
    private static Dictionary<string, Queue<GameObject>> poolDictionary;

    public Pool()
	{
        poolDictionary = new();
    }

    public bool Init()
	{
        if (poolDictionary == null)
        {
            Debug.LogError("PoolDictionary is null!");
            return false;
		}

        return true;
    }

    public void AddToPoolDictionary(string id, Queue<GameObject> gameObjectsQueue)
    {
        poolDictionary.Add(id, gameObjectsQueue);
    }

    public GameObject SpawnFromPool (string id, Vector3 position, Quaternion rotation)
    {
        if (!PoolContainsKey(id))
            return null;

        GameObject pooledObject = poolDictionary[id].Dequeue();

        pooledObject.SetActive(true);
        pooledObject.transform.SetPositionAndRotation(position, rotation);

        // optional
        if (pooledObject.TryGetComponent(out IPooled pooled))
            pooled.OnObjectSpawn();

        poolDictionary[id].Enqueue(pooledObject);

        return pooledObject;
    }

    public SceneSetupSO.Asset PeekObjectEntryFromPool (List<SceneSetupSO.Asset> assets, string id)
	{
        if (!PoolContainsKey(id))
            return null;

        return new(assets[0].id, assets[0].prefab, assets[0].size, assets[0].parentTag);
    }

    private bool PoolContainsKey(string id)
	{
        if (!poolDictionary.ContainsKey(id))
        {
            Debug.LogError($"Key: {id} does not exist in pool dictionary.");
            return false;
        }

        return true;
    }
}
