using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    private class Pool
	{
        [SerializeField]
        private string id;
        public string GetId { get { return id; } }

        [SerializeField]
        private GameObject prefab;
        public GameObject GetPrefab { get { return prefab; } }

        public int size;
    }
    [SerializeField]
    private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        InitPool(
            onSuccess: () =>
            {
                Debug.Log("Pool initialized.");
                InitScene(
                    onSuccess: () => Debug.Log("Scene setup succesfully."),
                    onError: () => Debug.LogError("Failed to setup Scene!")
                );
            },
            onError: () => Debug.LogError("Failed to initialize Pool!")
        );
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitPool(System.Action onSuccess, System.Action onError = null)
	{
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        onSuccess();
	}

    private void InitScene(System.Action onSuccess, System.Action onError = null)
	{
        GameObject terrainTilePrefab = GetPoolById("terrain").GetPrefab;
		if (terrainTilePrefab == null)
            onError?.Invoke();

        Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 10.0f));
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 10.0f));
        
        Vector3 startSpawnPos = new Vector3(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);
        Instantiate(terrainTilePrefab, startSpawnPos, Quaternion.identity);

        float nextSpawnPosOffset = terrainTilePrefab.transform.localScale.x;
        Vector3 nextSpawnPos = new Vector3(startSpawnPos.x - nextSpawnPosOffset, startSpawnPos.y, startSpawnPos.z);
        int tilesCount = 1;
        while (nextSpawnPos.x >= lowerLeftCorner.x)
        {
            Instantiate(terrainTilePrefab, nextSpawnPos, Quaternion.identity);
            nextSpawnPos.x -= nextSpawnPosOffset;
            tilesCount++;
        }
        GetPoolById("terrain").size = tilesCount;

        onSuccess();
    }

    private Pool GetPoolById(string id)
	{
        foreach (Pool pool in pools)
        {
            if (pool.GetId == id)
            {
                return pool;
            }
        }
        
        Debug.LogError($"{id} was not found in Pool!");
        return null;
    }
}
