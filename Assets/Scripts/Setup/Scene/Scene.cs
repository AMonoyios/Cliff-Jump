using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Scene
{
	private readonly Transform sceneTransform;
	private Vector2 xAxisBounds;
	private float yAxisTerrainSpawn;
	private float zAxisTerrainSpawn;

	public Scene(List<SetupAsset> setupAssets)
	{
    	new CameraComponent(Camera.main);

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
	}

	// FIXME: look at this probably will need rework
	public bool SetupTerrain(List<SetupAsset> setupAssets, TerrainConfigure terrainConfigure)
	{
		GameObject terrainTilePrefab = setupAssets.FindById(StringRepo.Assets.Terrain).prefab;
		if (!terrainTilePrefab || sceneTransform == null)
			return false;

		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));
		xAxisBounds = new(lowerLeftCorner.x, lowerRightCorner.x);

		yAxisTerrainSpawn = lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f);
		zAxisTerrainSpawn = lowerRightCorner.z;
		Vector3 startPos = new(lowerRightCorner.x, yAxisTerrainSpawn, zAxisTerrainSpawn);

		List<GameObject> pooledTerrain = new()
        {
            Pool.Spawn(StringRepo.Assets.Terrain, startPos, Quaternion.identity)
        };

        float xPositionOffset = terrainTilePrefab.transform.localScale.x;

		Vector3 prevPos = new Vector3(startPos.x + xPositionOffset, startPos.y, startPos.z).RoundToDecimals(1);
		GameManager.terrainSpawnPosition = prevPos;
		pooledTerrain.Add(Pool.Spawn(StringRepo.Assets.Terrain, prevPos, Quaternion.identity));

		Vector3 nextPos = new(startPos.x - xPositionOffset, startPos.y, startPos.z);
        for (int tilesCount = 1; nextPos.x >= lowerLeftCorner.x; tilesCount++)
        {
            pooledTerrain.Add(Pool.Spawn(StringRepo.Assets.Terrain, nextPos, Quaternion.identity));
            nextPos.x -= xPositionOffset;
        }

        foreach (GameObject terrain in pooledTerrain)
		{
			// Create.NewGameObject("StartPos", prevPos, Quaternion.identity, Vector3.one);
			CustomBehaviourAssetsDatabase.Register(new TerrainComponent(terrain, terrainConfigure));
		}

		return true;
	}

	public bool SetupPlayer(float x)
	{
		//float xSpawnPos = (Mathf.Abs(xAxisBounds.x) - Mathf.Abs(xAxisBounds.y)) / x;

		//Pool.Spawn("Pou", new(xSpawnPos, yAxisTerrainSpawn + 3.0f, zAxisTerrainSpawn), Quaternion.identity);

		return true;
	}
}
