using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Scene
{
	private readonly Transform sceneTransform;

	public Scene(List<SetupAsset> setupAssets)
	{
        _ = new CameraComponent();

        sceneTransform = Create.NewGameObject(StringRepo.Assets.Scene).transform;

		foreach (SetupAsset asset in setupAssets)
		{
			Queue<GameObject> objectPool = new();

			for (int i = 0; i < asset.size; i++)
			{
				GameObject newPrefab = Create.NewPrefab(asset.prefab);

				if (asset.parentTag != TagEnums.Untagged)
				{
					GameObject parent = GameObject.FindGameObjectWithTag(StringRepo.Tags.ToString(asset.parentTag));
                    newPrefab.transform.parent = parent != null ?
						parent.transform :
						Create.NewGameObject(asset.Name, sceneTransform, StringRepo.Tags.ToString(asset.parentTag)).transform;
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

		Vector3 startPos = new(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);

        List<GameObject> pooledTerrain = new()
        {
            Pool.Spawn(StringRepo.Assets.Terrain, startPos, Quaternion.identity)
        };

        float nextPosOffset = terrainTilePrefab.transform.localScale.x;
		Vector3 nextPos = new(startPos.x - nextPosOffset, startPos.y, startPos.z);
        for (int tilesCount = 1; nextPos.x >= lowerLeftCorner.x; tilesCount++)
        {
            pooledTerrain.Add(Pool.Spawn(StringRepo.Assets.Terrain, nextPos, Quaternion.identity));
            nextPos.x -= nextPosOffset;
        }

        foreach (GameObject terrain in pooledTerrain)
		{
			UpdatablesDatabase.Register(new TerrainComponent(terrain, terrainConfigure));
		}

		return true;
	}
}
