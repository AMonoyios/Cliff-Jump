using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string id = "newObject";
    public GameObject prefab = null;
    public int size = 10;

    private static Dictionary<string, Queue<GameObject>> poolDictionary = new();

    public static bool TryAdd(string id, Queue<GameObject> objectPool)
    {
        if (poolDictionary.TryAdd(id, objectPool))
        {
            Debug.Log($"Added to {id} pool");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to add to {id}!");
            return false;
        }
    }

    public static void TrySpawnFromPool (string id, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(id))
        {
            
        }

        GameObject spawnObject = poolDictionary[id].Dequeue();

        spawnObject.SetActive(true);
        spawnObject.transform.SetPositionAndRotation(position, rotation);

        poolDictionary[id].Enqueue(spawnObject);
    }
}
