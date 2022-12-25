using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SetupManager
{
	private readonly Transform sceneTransform;

	private CameraBounds cameraBounds;
	private float terrainScaleY;

	public SetupManager(List<SetupAsset> setupAssets)
	{
        sceneTransform = Create.NewGameObject(StringRepo.Assets.Scene).transform;

		foreach (SetupAsset asset in setupAssets)
		{
			Queue<GameObject> objectPool = new();

			for (int i = 0; i < asset.size; i++)
			{
				GameObject newPrefab = Create.NewPrefab(asset.prefab);

				if (asset.parentName != "")
				{
					// FIXME: Refactor the way parent is fetched (Not efficient).
					GameObject parent = GameObject.Find(asset.parentName);
                    newPrefab.transform.parent = parent != null ?
						parent.transform :
						Create.NewGameObject(asset.Name, sceneTransform).transform;
                }

				newPrefab.SetActive(false);

				objectPool.Enqueue(newPrefab);
			}

			Pool.Add(asset.id, objectPool);
		}

		Debug.Log("SetupManager initialized.");
	}

	public bool SetupCamera(Camera camera)
    {
        if (camera == null)
        {
            Debug.LogError("Camera is null or missing");
            return false;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (planes.Length < 6)
        {
            Debug.LogError("Planes did not add up to 6 faces, unable to create bound box.");
            return false;
        }

        Vector3 boundBoxScale = new
        (
            x: Vector3.Distance(-planes[0].normal * planes[0].distance, -planes[1].normal * planes[1].distance),
            y: Vector3.Distance(-planes[2].normal * planes[2].distance, -planes[3].normal * planes[3].distance),
            z: Vector3.Distance(-planes[4].normal * planes[4].distance, -planes[5].normal * planes[5].distance)
        );

        Bounds cameraColliderBounds = new(Vector3.zero, Vector3.zero);
        for (int i = 0; i < 6; ++i)
        {
            cameraColliderBounds.Encapsulate(-planes[i].normal * planes[i].distance);
        }

        GameObject cameraColliderGameObject = Create.NewGameObject("CameraCollisionArea", cameraColliderBounds.center, Quaternion.identity, boundBoxScale, camera.gameObject.transform);

		cameraBounds = new CameraBounds
		(
			xBounds: new(cameraColliderGameObject.transform.position.x - (cameraColliderGameObject.transform.localScale.x / 2.0f), cameraColliderGameObject.transform.position.x + (cameraColliderGameObject.transform.localScale.x / 2.0f)),
			yBounds: new(cameraColliderGameObject.transform.position.y - (cameraColliderGameObject.transform.localScale.y / 2.0f), cameraColliderGameObject.transform.position.y + (cameraColliderGameObject.transform.localScale.y / 2.0f)),
			zBounds: new(cameraColliderGameObject.transform.position.z - (cameraColliderGameObject.transform.localScale.z / 2.0f), cameraColliderGameObject.transform.position.z + (cameraColliderGameObject.transform.localScale.z / 2.0f))
		);

        GameManager.cameraColliderComponent = CustomBehaviourAssetsDatabase.Register(new CameraColliderComponent(cameraColliderGameObject));
        if (CustomBehaviourAssetsDatabase.IsRegistered(cameraColliderGameObject))
            Debug.Log("Camera collider registered.");

        Debug.Log("successfully setup camera collider gameobject.");

        return true;
    }

	public bool SetupTerrain(List<SetupAsset> setupAssets, TerrainConfigure terrainConfig)
	{
		GameObject terrainTilePrefab = setupAssets.FindById(StringRepo.Assets.Terrain).prefab;
		if (!terrainTilePrefab || sceneTransform == null)
		{
			Debug.LogError("Terrain prefab is NULL!");
			return false;
		}

		terrainScaleY = terrainTilePrefab.transform.localScale.y;

		// TODO: use cameraBounds rather than ViewportToWorldPoint
		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

		terrainTilePrefab.transform.localScale = new(terrainConfig.scale, terrainConfig.scale, terrainConfig.scale);
		Vector3 spawnPos = new
		(
			x: lowerRightCorner.x + terrainTilePrefab.transform.localScale.x,
			y: lowerRightCorner.y,
			z: lowerRightCorner.z
		);
		GameManager.terrainSpawnPosition = spawnPos;

		if (!Pool.ContainsKey(StringRepo.Assets.Terrain))
		{
			Debug.LogError("Pool does not contain Terrain asset.");
			return false;
		}

		for (int i = 0; spawnPos.x >= lowerLeftCorner.x; i++)
		{
			GameObject newTerrainGameObject = Pool.Spawn(StringRepo.Assets.Terrain, spawnPos, Quaternion.identity);
			spawnPos = new(spawnPos.x - terrainTilePrefab.transform.localScale.x, spawnPos.y, spawnPos.z);

			CustomBehaviourAssetsDatabase.Register(new TerrainComponent(newTerrainGameObject, terrainConfig));
		}

		Debug.Log("Terrain setup successfully.");

		return true;
	}

	public bool SetupPlayer(string playerID, PlayerConfigure playerConfig, PhysicsConfigure physicsConfig)
	{
		if (!Pool.ContainsKey(playerID))
		{
			Debug.LogError("Player ID does not exist in pool.");
			return false;
		}

		GameObject playerGameObject = Pool.Spawn(playerID, Vector3.zero, Quaternion.identity);

		float playerRelativeScreenScale = Mathf.Lerp(1, Mathf.Abs(cameraBounds.XCameraBounds.x - cameraBounds.XCameraBounds.y), playerConfig.scale);
		playerGameObject.transform.localScale = new(playerRelativeScreenScale, playerRelativeScreenScale, playerRelativeScreenScale);
		playerGameObject.transform.position = (new
		(
			x: Mathf.Lerp(cameraBounds.XCameraBounds.x, cameraBounds.XCameraBounds.y, playerConfig.spawnXOffset / 100.0f),
			y: GameManager.terrainSpawnPosition.y + (terrainScaleY / 2.0f) + (playerGameObject.transform.localScale.y / 2.0f),
			z: GameManager.terrainSpawnPosition.z
		));

		GameManager.playerComponent = new PlayerComponent(playerGameObject, playerConfig, physicsConfig);
		CustomBehaviourAssetsDatabase.Register(GameManager.playerComponent);
		if (!CustomBehaviourAssetsDatabase.IsRegistered(playerGameObject))
		{
			Debug.LogError("Failed to register PlayerComponent to Database.");
			return false;
		}

		Debug.Log("Player setup successfully.");

		return true;
	}
}
