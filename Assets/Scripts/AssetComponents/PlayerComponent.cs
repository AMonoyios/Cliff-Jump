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

	#region Variables and conditions
	private int jumpCount = 1;

	private bool isColliding;
	private float feetLevel;
	private float stepLevel;
	private float groundLevel;
	private bool isGrounded;

	private bool gameOver;

	private float velocity;
	#endregion

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
		if (ClosestTerrainComponent != GetGameObject.transform.GetClosestTerrainComponent())
		{
			GameManager.Score++;
			ClosestTerrainComponent = GetGameObject.transform.GetClosestTerrainComponent();
		}

		isColliding = CollisionCheck.BoxToSphere(ClosestTerrainComponent.GetGameObject.transform, GetGameObject.transform);

		feetLevel = GetGameObject.transform.position.y - (GetGameObject.transform.localScale.y / 2.0f);
		stepLevel = feetLevel + PlayerConfig.stepThreshold;
		groundLevel = ClosestTerrainComponent.GetGameObject.transform.position.y + (ClosestTerrainComponent.GetGameObject.transform.localScale.y / 2.0f);
        isGrounded = isColliding && groundLevel >= feetLevel;

		// FIXME: Step functionality is glitchy
		// bool canStep = groundLevel < stepLevel && Mathf.Approximately(velocity, 0.0f);
		// if (canStep)
		// 	velocity = 1.0f;

        gameOver = groundLevel > stepLevel;
		if (gameOver)
        	GameManager.GameOver = true;
	}

	private void UpdateGravity()
	{
		velocity += PhysicsConfig.GetGravity * PhysicsConfig.gravityScale * Time.deltaTime;
		if (isGrounded && velocity < 0)
		{
			velocity = 0;
		}
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
}
