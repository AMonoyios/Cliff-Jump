using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

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
    [SerializeField]
    private PhysicsConfigure physicsConfig;

    private SetupManager setupManager;
    public static CameraColliderComponent cameraColliderComponent;
    public static PlayerComponent playerComponent;

    public static Vector3 terrainSpawnPosition;

    public static bool IsSetupDone = false;

    // Start is called before the first frame update
    private void Start()
    {
        new Promise<bool>()
            .Add(InitScene)
            .Add(InitPhysics)
            .Add(SetupCamera)
            .Add(SetupTerrain)
            .Add(SetupPlayer)
            .Condition((value) => value)
            .Execute()
            .OnComplete(() =>
                {
                    IsSetupDone = true;
                    Debug.Log("Finished game init.");
                });
    }

#region InitGame
    private bool InitScene()
	{
        setupManager = new(setupAssets);
        return setupManager != null;
	}
    private bool InitPhysics()
    {
        physicsConfig = new();
        return physicsConfig != null;
    }
    private bool SetupCamera()
    {
        return setupManager.SetupCamera(Camera.main);
    }
    private bool SetupTerrain()
	{
        return setupManager.SetupTerrain(setupAssets, terrainConfig);
	}
    private bool SetupPlayer()
    {
        return setupManager.SetupPlayer(StringRepo.Assets.Player, playerConfig, physicsConfig);
	}
#endregion

#region GameEngine life cycles
    private void Update()
    {
        if (!IsSetupDone)
            return;

        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.Update();
        }
    }

    private void FixedUpdate()
    {
        if (!IsSetupDone)
            return;

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
        if (!IsSetupDone)
            return;

        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.OnDrawGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!IsSetupDone)
            return;

        foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
        {
            if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                asset.OnDrawGizmosSelected();
        }
    }
    #endif
#endregion
}
