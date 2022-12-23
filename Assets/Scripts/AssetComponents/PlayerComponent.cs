using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	public PlayerComponent(GameObject playerGameObject)
	{
		GetGameObject = playerGameObject;
	}
}
