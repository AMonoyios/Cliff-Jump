using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private const string untagged = "Untagged";

    private List<PoolSetupSettingsSO.PoolEntry> entries;
    private static Dictionary<string, Queue<GameObject>> poolDictionary;

    public Pool(PoolSetupSettingsSO settings)
	{
        this.entries = new(settings.PoolEntries);
        poolDictionary = new();
    }

    public bool Init()
	{
        if (entries.Count == 0)
        {
            Debug.LogError("Pool is not populated!");
            return false;
		}
		else
		{
            foreach (PoolSetupSettingsSO.PoolEntry pool in entries)
            {
                Queue<GameObject> objectPool = new();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject newObject = GameManager.Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                    if (pool.parentTag != untagged)
                        newObject.transform.parent = GameObject.FindGameObjectWithTag(pool.parentTag).transform;

                    newObject.SetActive(false);

                    objectPool.Enqueue(newObject);
                }

                poolDictionary.Add(pool.id, objectPool);
            }
        }

        return true;
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

    public PoolSetupSettingsSO.PoolEntry PeekObjectEntryFromPool (string id)
	{
        if (!PoolContainsKey(id))
            return null;

        return new(entries[0].id, entries[0].prefab, entries[0].size, entries[0].parentTag);
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
