using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionDetection;
using Utils;

public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	public PlayerConfigure PlayerConfig { get; }
	public TerrainComponent ClosestTerrainComponent { get; private set; }
	private PhysicsConfigure PhysicsConfig { get; }

	private bool isColliding;
	private float feetLevel;
	private float stepLevel;
	private float groundLevel;
	private bool isGrounded;
	private bool gameOver;

	private float velocity;

	public PlayerComponent(GameObject playerGameObject, PlayerConfigure playerConfig, PhysicsConfigure physicsConfig)
	{
		GetGameObject = playerGameObject;

		PlayerConfig = playerConfig;
		PhysicsConfig = physicsConfig;

		isColliding = true;
		isGrounded = true;
		gameOver = false;
	}

	public void Update()
	{
		UpdateGravity();

		if (Input.touchCount > 0)
		{
			Touch touch  = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Stationary)
			{
				Jump();
			}
		}

		ApplyPhysics();
	}

	public void FixedUpdate()
	{
		ClosestTerrainComponent = GetGameObject.transform.GetClosestTerrainComponent();

		isColliding = CollisionCheck.BoxToSphere(ClosestTerrainComponent.GetGameObject.transform, GetGameObject.transform);
		feetLevel = GetGameObject.transform.position.y - (GetGameObject.transform.localScale.y / 2.0f);
		groundLevel = ClosestTerrainComponent.GetGameObject.transform.position.y + (ClosestTerrainComponent.GetGameObject.transform.localScale.y / 2.0f);

        isGrounded = isColliding && groundLevel >= feetLevel;

		stepLevel = feetLevel + PlayerConfig.stepThreshold;
		bool canStep = isGrounded && groundLevel >= stepLevel;
		gameOver = isColliding && !canStep;

		// FIXME: step functionality (glitchy)
		// stepLevel = groundLevel + PlayerConfig.stepThreshold;
		// bool canStep = feetLevel + PlayerConfig.stepThreshold + 0.1f <= stepLevel;
        // gameOver = isColliding && !canStep;

		// if (feetLevel + PlayerConfig.stepThreshold <= stepLevel && isGrounded)
		// 	velocity = 1.0f;
    }

	private void UpdateGravity()
	{
		velocity += PhysicsConfig.GetGravity * PhysicsConfig.gravityScale * Time.deltaTime;
		if (isGrounded && velocity < 0)
		{
			velocity = 0;
		}
	}

	private void Jump()
	{
		velocity = PlayerConfig.jumpForce;
	}

	private void ApplyPhysics()
	{
		GetGameObject.transform.Translate(new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
	}

	public void OnDrawGizmos()
	{
		if (isGrounded)
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
		else
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);

		#region FeetLevel line
		Gizmos.color = Color.black;
		Vector3 leftGroundPoint = new
		(
			x: GetGameObject.transform.position.x - (GetGameObject.transform.localScale.x / 2.0f),
			y: feetLevel,
			z: GetGameObject.transform.position.z
		);
		Vector3 rightGroundPoint = new
		(
			x: GetGameObject.transform.position.x + (GetGameObject.transform.localScale.x / 2.0f),
			y: feetLevel,
			z: GetGameObject.transform.position.z
		);
		Gizmos.DrawLine(leftGroundPoint, rightGroundPoint);
		#endregion

		#region StepLevel line
		Gizmos.color = Color.blue;
		Vector3 leftStepPoint = new
		(
			x: GetGameObject.transform.position.x - (GetGameObject.transform.localScale.x / 2.0f),
			y: stepLevel,
			z: GetGameObject.transform.position.z
		);
		Vector3 rightStepPoint = new
		(
			x: GetGameObject.transform.position.x + (GetGameObject.transform.localScale.x / 2.0f),
			y: stepLevel,
			z: GetGameObject.transform.position.z
		);
		Gizmos.DrawLine(leftStepPoint, rightStepPoint);
		#endregion

		if (gameOver)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
		}
	}
}
