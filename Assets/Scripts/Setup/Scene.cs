using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene
{
    public Scene()
	{
		// init
	}

	public bool SetupTerrain(Pool pool, string tileID)
	{
		GameObject terrainTilePrefab = pool.PeekObjectEntryFromPool(tileID).prefab;
		if (!terrainTilePrefab)
			return false;

		Vector3 lowerRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, -Camera.main.transform.position.z));
		Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));

		Vector3 startPos = new(lowerRightCorner.x, lowerRightCorner.y + (terrainTilePrefab.transform.localScale.y / 2.0f), lowerRightCorner.z);

		//Instantiate(terrainTilePrefab, startPos, Quaternion.identity);
		pool.SpawnFromPool(tileID, startPos, Quaternion.identity);

		float nextPosOffset = terrainTilePrefab.transform.localScale.x;
		Vector3 nextPos = new(startPos.x - nextPosOffset, startPos.y, startPos.z);
		int tilesCount = 1;
		while (nextPos.x >= lowerLeftCorner.x)
		{
			//Instantiate(terrainTilePrefab, nextPos, Quaternion.identity);
			pool.SpawnFromPool(tileID, nextPos, Quaternion.identity);

			nextPos.x -= nextPosOffset;
			tilesCount++;
		}

		return true;
	}
}
