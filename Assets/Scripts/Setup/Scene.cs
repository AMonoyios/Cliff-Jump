using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Scene
{
	private readonly Transform sceneTransform;

	private const string untagged = "Untagged";
	private List<SceneSetupSO.Asset> assets;

	public Scene(SceneSetupSO sceneSetupSO, Pool pool)
	{
		sceneTransform = Create.NewGameObject("Scene").transform;
		Create.NewGameObject("Terrain", parent: sceneTransform, tag: "Terrain");

		// populate pool with scene assets from Scriptable object
		assets = new(sceneSetupSO.Assets);
		foreach (SceneSetupSO.Asset soPoolEntry in assets)
		{
			Queue<GameObject> objectPool = new();

			for (int i = 0; i < soPoolEntry.size; i++)
			{
				GameObject newPrefab = Create.NewPrefab(soPoolEntry.prefab);

				if (soPoolEntry.parentTag != untagged)
					newPrefab.transform.parent = GameObject.FindGameObjectWithTag(soPoolEntry.parentTag).transform;

				newPrefab.SetActive(false);

				objectPool.Enqueue(newPrefab);
			}

			pool.AddToPoolDictionary(assets[0].id, objectPool);
		}
	}

	public bool SetupTerrain(Pool pool, string tileID)
	{
		GameObject terrainTilePrefab = pool.PeekObjectEntryFromPool(assets, tileID).prefab;
		if (!terrainTilePrefab || sceneTransform == null)
			return false;

		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

		Vector3 startPos = new(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);

		pool.SpawnFromPool(tileID, startPos, Quaternion.identity);

		float nextPosOffset = terrainTilePrefab.transform.localScale.x;
		Vector3 nextPos = new(startPos.x - nextPosOffset, startPos.y, startPos.z);
		int tilesCount = 1;
		while (nextPos.x >= lowerLeftCorner.x)
		{
			pool.SpawnFromPool(tileID, nextPos, Quaternion.identity);

			nextPos.x -= nextPosOffset;
			tilesCount++;
		}

		return true;
	}
}
