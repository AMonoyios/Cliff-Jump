using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private List<SetupAsset> setupAssets = new();

    [Header("Configure")]
    [SerializeField]
    private TerrainConfigure terrainConfig;

    private Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        new Promise<bool>()
            .Add(InitScene)
            .Add(InitTerrain)
            .Condition((value) => value)
            .Execute()
            .OnComplete(() => Debug.Log("Finished game init."));
    }

    private bool InitScene()
	{
        scene = new(setupAssets);
        if (scene == null)
        {
            Debug.LogError("Scene is null!");
            return false;
        }
        return true;
	}
    private bool InitTerrain()
	{
        if (!scene.SetupTerrain(setupAssets, terrainConfig))
        {
            Debug.LogError("Terrain failed to setup!");
            return false;
        }
        return true;
	}

    // Update is called once per frame
    private void Update()
    {
        foreach (IUpdatable asset in UpdatablesDatabase.Values)
        {
            if (asset.gameObject != null)
            {
                asset.Update();
            }
            else
            {
                Debug.LogError($"Asset {asset} is null in CustomBehaviourAssetDatabase!");
                return;
            }
        }
    }
}
