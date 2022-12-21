using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Utils;

public sealed class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private List<SetupAsset> setupAssets = new();

    [Header("Configure")]
    [SerializeField]
    private TerrainConfigure terrainConfig;
    [SerializeField]
    private PlayerConfigure playerConfig;

    private SetupManager scene;
    private CameraComponent camera;

    public static Vector3 terrainSpawnPosition;

    // Start is called before the first frame update
    private void Start()
    {
        new Promise<bool>()
            .Add(InitScene)
            .Add(SetupCamera)
            .Add(SetupTerrain)
            .Add(SetupPlayer)
            .Condition((value) => value)
            .Execute()
            .OnComplete(() => Debug.Log("Finished game init."));
    }

#region InitGame
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
    private bool SetupCamera()
    {
        camera = new();

        return camera.SetupCamera();
    }
    private bool SetupTerrain()
	{
        if (!scene.SetupTerrain(setupAssets, terrainConfig))
        {
            Debug.LogError("Terrain failed to setup!");
            return false;
        }
        return true;
	}
    private bool SetupPlayer()
    {
        if (!scene.SetupPlayer(playerConfig.x))
        {
            Debug.LogError("Player failed to setup!");
            return false;
        }
        return true;
	}
#endregion

#region GameEngine life cycles
    private void Update()
    {
        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.Update();
        }
    }

    private void FixedUpdate()
    {
        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.FixedUpdate();
        }
    }
#endregion

#region Debug life cycles
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.OnDrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.OnDrawGizmosSelected();
        }
    }
    #endif
#endregion
}
