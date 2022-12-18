using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Scene
{
	private readonly Transform sceneTransform;

	public Scene(List<SetupAsset> setupAssets)
	{
		sceneTransform = Create.NewGameObject(StringRepo.Assets.Scene).transform;

		foreach (SetupAsset asset in setupAssets)
		{
			Queue<GameObject> objectPool = new();

			for (int i = 0; i < asset.size; i++)
			{
				GameObject newPrefab = Create.NewPrefab(asset.prefab);

				if (asset.parentTagEnum != TagRepo.TagEnums.Untagged)
				{
					GameObject parent = GameObject.FindGameObjectWithTag(TagRepo.Tags(asset.parentTagEnum));
                    newPrefab.transform.parent = parent != null ? parent.transform : Create.NewGameObject(TagRepo.Tags(asset.parentTagEnum), parent: sceneTransform, TagRepo.Tags(asset.parentTagEnum)).transform;
                }

				newPrefab.SetActive(false);

				objectPool.Enqueue(newPrefab);
			}

			Pool.AddToPoolDictionary(asset.id, objectPool);
		}
	}

	public bool SetupTerrain(List<SetupAsset> setupAssets)
	{
		GameObject terrainTilePrefab = setupAssets.FindById(StringRepo.Assets.Terrain).prefab;
		if (!terrainTilePrefab || sceneTransform == null)
			return false;

		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

		Vector3 startPos = new(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);

		Pool.SpawnFromPool(StringRepo.Assets.Terrain, startPos, Quaternion.identity);

		float nextPosOffset = terrainTilePrefab.transform.localScale.x;
		Vector3 nextPos = new(startPos.x - nextPosOffset, startPos.y, startPos.z);
		int tilesCount = 1;
		while (nextPos.x >= lowerLeftCorner.x)
		{
			Pool.SpawnFromPool(StringRepo.Assets.Terrain, nextPos, Quaternion.identity);

			nextPos.x -= nextPosOffset;
			tilesCount++;
		}

		return true;
	}
}
