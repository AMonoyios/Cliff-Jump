using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private const string terrainID = "terrain";

    [SerializeField]
    private List<Pool> pools;

    // Start is called before the first frame update
    void Start()
    {
        Promise<bool> initGame = new Promise<bool>()
            .Add(InitPool)
            .Add(InitScene)
            .Condition((value) => value)
            .Execute()
            .OnComplete(() => Debug.Log("Finished game init."));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool InitPool()
	{
        if (pools.Count == 0)
        {
            Debug.LogError("Pool is not populated!");
            return false;
        }
        else
        {
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject newObject = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                    newObject.SetActive(false);

                    objectPool.Enqueue(newObject);
                }

                if (!Pool.TryAdd(terrainID, objectPool))
                    return false;
            }
        }

        return true;
	}

    private bool InitScene()
	{
        GameObject terrainTilePrefab = poolDictionary.GetValueOrDefault(terrainID).Enqueue()
        Queue<GameObject> firstTerrainTile = null;
        if (poolDictionary.TryGetValue(terrainID, out firstTerrainTile))
        {
            
        }
        else
        {
            Debug.LogError("Key terrain was not found in pool dictionary!");
            return false;
        }

        Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

        Vector3 startPos = new(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);
        //Instantiate(terrainTilePrefab, startPos, Quaternion.identity);

        float nextPosOffset = terrainTilePrefab.transform.localScale.x;
        Vector3 nextPos = new(startPos.x - nextPosOffset, startPos.y, startPos.z);
        int tilesCount = 1;
        while (nextPos.x >= lowerLeftCorner.x)
        {
            //Instantiate(terrainTilePrefab, nextPos, Quaternion.identity);
            nextPos.x -= nextPosOffset;
            tilesCount++;
        }

        return true;
    }

    
}
