using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField]
    private PoolSetupSettingsSO pools;

    private const string terrainID = "terrain";

    private Pool pool;
    private Scene scene;
    
    // Start is called before the first frame update
    void Start()
    {
        new Promise<bool>()
            .Add(InitPool)
            .Add(InitScene)
            .Add(SetupTerrain)
            .Condition((value) => value)
            .Execute()
            .OnComplete(() => Debug.Log("Finished game init."));
    }

    private bool InitPool()
	{
        pool = new(pools);
        return pool.Init();
	}
    private bool InitScene()
	{
        scene = new();
        return scene != null;
	}
    private bool SetupTerrain()
	{
        if (!scene.SetupTerrain(pool, terrainID))
            return false;

        Debug.Log("Finished scene setup.");
        return true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
