using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	public PlayerConfigure PlayerConfig { get; }
	public TerrainComponent ClosestTerrainComponent { get; private set; }
	private PhysicsConfigure PhysicsConfig { get; }

	private float groundLevel;
	private float feetLevel;
	private bool isGrounded;

	private float velocity;

	public PlayerComponent(GameObject playerGameObject, PlayerConfigure playerConfig, PhysicsConfigure physicsConfig)
	{
		GetGameObject = playerGameObject;

		PlayerConfig = playerConfig;
		PhysicsConfig = physicsConfig;
		isGrounded = true;
	}

	public void Update()
	{
		velocity += PhysicsConfig.GetGravity * PhysicsConfig.gravityScale * Time.deltaTime;
		if (isGrounded && velocity < 0)
		{
			velocity = 0;
		}
		if (Input.touchCount > 0)
		{
			Touch touch  = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Stationary)
			{
				velocity = PlayerConfig.jumpForce;
			}
		}
		GetGameObject.transform.Translate(new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);

		//TODO: Implement "game over" collisions
	}

	public void FixedUpdate()
	{
		ClosestTerrainComponent = GetGameObject.transform.GetClosestTerrainComponent();
		groundLevel = ClosestTerrainComponent.GetGameObject.transform.position.y + (ClosestTerrainComponent.GetGameObject.transform.localScale.y / 2.0f);

		feetLevel = GetGameObject.transform.position.y - (GetGameObject.transform.localScale.y / 2.0f);
        isGrounded = groundLevel >= feetLevel;
    }

	public void OnDrawGizmos()
	{
		if (isGrounded)
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
		else
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);
	}
}
