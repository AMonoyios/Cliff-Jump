using UnityEngine;
using CollisionDetection;
using Utils;

// Behaviour component for Player
public class PlayerComponent : IBehaviour
{
	public GameObject GetGameObject { get; }

	// References to config settings
	public PlayerConfigure PlayerConfig { get; }
	public TerrainComponent ClosestTerrainComponent { get; private set; }
	private PhysicsConfigure PhysicsConfig { get; }

	private int jumpCount = 1;

	private bool isColliding;
	private float feetLevel;
	private float stepLevel;
	private float groundLevel;
	private bool isGrounded;

	private bool gameOver;

	private float velocity;

	// Constructor for Player behaviour
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

		// Player controls
		if (Input.touchCount > 0)
		{
			Touch touch  = Input.GetTouch(0);

			// Jump(s) condition
			if (touch.phase == TouchPhase.Began && jumpCount < PlayerConfig.maxJumps)
			{
				jumpCount++;
				velocity = PlayerConfig.jumpForce;
			}
		}

		// jump(s) count reset
		if (isGrounded && jumpCount != 0)
			jumpCount = 1;

		// Move player depending on new velocity calculated
		GetGameObject.transform.Translate(new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
	}

	public void FixedUpdate()
	{
		// Keeps track of the current terrain closest to player and if there is a change of instance then increase the score
		if (ClosestTerrainComponent != GetGameObject.transform.GetClosestTerrainComponent())
		{
			GameManager.Score++;
			ClosestTerrainComponent = GetGameObject.transform.GetClosestTerrainComponent();
		}

		// checks if there is a collision between the player and the closest terrain tile
		isColliding = CollisionCheck.BoxToSphere(ClosestTerrainComponent.GetGameObject.transform, GetGameObject.transform);

		// Calculate the desired level to let the player walk as well as the tiles ground level
		feetLevel = GetGameObject.transform.position.y - (GetGameObject.transform.localScale.y / 2.0f);
		stepLevel = feetLevel + PlayerConfig.stepThreshold;
		groundLevel = ClosestTerrainComponent.GetGameObject.transform.position.y + (ClosestTerrainComponent.GetGameObject.transform.localScale.y / 2.0f);

		// Compare the above states to figure out if player is on the floor or not
		isGrounded = isColliding && groundLevel >= feetLevel;

		// FIXME: Step functionality is glitchy
		// bool canStep = groundLevel < stepLevel && Mathf.Approximately(velocity, 0.0f);
		// if (canStep)
		// 	velocity = 1.0f;

		// Using the ground level and max step level of player we can figure out if the player collided with an un-climbable terrain tile
        gameOver = groundLevel > stepLevel;
		if (gameOver)
        	GameManager.GameOver = true;
	}

	public void OnDrawGizmos()
	{
		if (isGrounded)
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
		else
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);

		GizmosExtra.DrawYLevelLine(GetGameObject.transform, feetLevel, Color.black);
		GizmosExtra.DrawYLevelLine(GetGameObject.transform, stepLevel, Color.blue);
		GizmosExtra.DrawYLevelLine(GetGameObject.transform, groundLevel, Color.grey);

		if (gameOver)
			GizmosExtra.DrawOutlinedCube(GetGameObject.transform, Color.red / 3.0f, Color.red);
	}

	private void UpdateGravity()
	{
		// Simple velocity calculations to "simulate" gravity forces
		velocity += PhysicsConfig.GetGravity * PhysicsConfig.gravityScale * Time.deltaTime;
		if (isGrounded && velocity < 0)
		{
			velocity = 0;
		}
	}
}
