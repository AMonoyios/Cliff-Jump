using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// The ONLY MonoBehaviour class that is responsible for all the game.
public sealed class GameManager : MonoBehaviour
{
    public const float guiSpace = 10.0f;

    #region Setup
    [Header("Setup")]
    [SerializeField]
    private List<SetupAsset> setupAssets = new();
    #endregion

    #region Configure
    [Header("Configure")]
    [SerializeField]
    private TerrainConfigure terrainConfig;
    [SerializeField]
    private PlayerConfigure playerConfig;
    [SerializeField]
    private PhysicsConfigure physicsConfig;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI scoreTMP;
    [SerializeField]
    private Transform gameOverPanel;
    [SerializeField]
    private Button restartGameBtn;
    #endregion

    #region Variables/States
    public static int Score = 0;
    public static bool GameOver = false;
    public static Vector3 terrainSpawnPosition;
    #endregion

    #region Class references
    private SetupManager setupManager;
    public static CameraColliderComponent cameraColliderComponent;
    public static PlayerComponent playerComponent;
    #endregion

    public static bool IsSetupDone = false;

    // Start is called before the first frame update
    private void Start()
    {
        // Promise for game init to ensure the correct order of initializations.
        new Promise<bool>()
            .Add(InitUI)
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
    private bool InitUI()
    {
        gameOverPanel.gameObject.SetActive(false);

        if (restartGameBtn.onClick == null)
        {
            Debug.LogError("Restart button method is not assigned!");
            return false;
        }

        return true;
    }
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
        if (!GameOver)
        {
            if (!IsSetupDone)
            return;

            foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
            {
                if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                    asset.Update();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameOver)
        {
            if (!IsSetupDone && !GameOver)
            return;

            foreach (IBehaviour asset in CustomBehaviourAssetsDatabase.Values)
            {
                if (asset.GetGameObject != null && asset.GetGameObject.activeSelf)
                    asset.FixedUpdate();
            }

            scoreTMP.text = $"Score: {Score}";
            if (GameOver) gameOverPanel.gameObject.SetActive(true);
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

    public void Restart()
    {
        StartCoroutine(RestartGameCoroutine());
    }
    private IEnumerator RestartGameCoroutine()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation progress = SceneManager.LoadSceneAsync(sceneIndex);

        while (!progress.isDone)
        {
            yield return null;
        }

        Debug.Log($"Restarting scene with index: {sceneIndex}");

        Score = 0;
        scoreTMP.text = $"Score: {Score}";
        GameOver = false;
    }
}
