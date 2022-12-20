using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerConfigure
{
	[Range(1.0f, 10.0f)]
	public float x = 3.0f;
}

public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	public PlayerComponent(GameObject playerGameObject)
	{
		GetGameObject = playerGameObject;
	}
}
