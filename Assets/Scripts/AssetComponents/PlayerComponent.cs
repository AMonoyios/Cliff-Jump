using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
public class PlayerConfigure
{
	[Range(0.0f, 100.0f)]
	public float spawnXOffset = 0.0f;
}

public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	// private Vector3 spawnPosition;

	public PlayerComponent(GameObject playerGameObject)//, Vector3 spawnPosition)
	{
		GetGameObject = playerGameObject;

		// this.spawnPosition = spawnPosition;
	}

	// public void OnDrawGizmos()
	// {
	// 	Gizmos.color = ColorExtra.Brown;
	// 	Gizmos.DrawSphere(spawnPosition, GetGameObject.transform.localScale.x / 2.0f);
	// }
}
