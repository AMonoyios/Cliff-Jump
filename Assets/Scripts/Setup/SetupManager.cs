using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SetupManager
{
	private readonly Transform sceneTransform;

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
	}

	public bool SetupTerrain(List<SetupAsset> setupAssets, TerrainConfigure terrainConfigure)
	{
		GameObject terrainTilePrefab = setupAssets.FindById(StringRepo.Assets.Terrain).prefab;
		if (!terrainTilePrefab || sceneTransform == null)
			return false;

		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

		Vector3 spawnPos = new
		(
			lowerRightCorner.x + terrainTilePrefab.transform.localScale.x,
			lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f),
			lowerRightCorner.z
		);

		if (!Pool.ContainsKey(StringRepo.Assets.Terrain))
			return false;

		List<GameObject> pooledTerrain = new();
		for (int i = 0; spawnPos.x >= lowerLeftCorner.x; i++)
		{
			pooledTerrain.Add(Pool.Spawn(StringRepo.Assets.Terrain, spawnPos, Quaternion.identity));
			spawnPos = new(spawnPos.x - terrainTilePrefab.transform.localScale.x, spawnPos.y, spawnPos.z);
		}

        foreach (GameObject terrain in pooledTerrain)
		{
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
