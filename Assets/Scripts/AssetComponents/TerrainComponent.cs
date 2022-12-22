using System.Collections;
using System.Collections.Generic;
using CollisionDetection;
using UnityEngine;
using Utils;

[System.Serializable]
public class TerrainConfigure
{
    public float startSpeed = 0.5f;
    [Range(0.5f, 3.0f)]
    public float maxSpeed = 2.5f;
    [Range(0.0f, 0.0025f)]
    public float speedIncreasePerFrame = 0.00125f;
    [Range(0.0f, 1.0f)]
    public float chanceForHeightChange = 0.5f;
    public Vector2 heightOffset = new(-0.25f, 0.25f);
}

public sealed class TerrainComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    private float CurrentSpeed { get; set; }
    private float MaxSpeed { get; }
    private float SpeedIncrease { get; }
    private float ChanceForHeightChange { get; }
    private Vector2 HeightOffset { get; }

    private bool collided = false;

    public TerrainComponent(GameObject gameObject, TerrainConfigure terrainConfigure)
    {
        GetGameObject = gameObject;

        CurrentSpeed = terrainConfigure.startSpeed;
        MaxSpeed = terrainConfigure.maxSpeed;
        SpeedIncrease = terrainConfigure.speedIncreasePerFrame;
        ChanceForHeightChange = terrainConfigure.chanceForHeightChange;
        HeightOffset = terrainConfigure.heightOffset;
    }

    public void Update()
    {
        float speed = CurrentSpeed > MaxSpeed ? MaxSpeed : CurrentSpeed += SpeedIncrease;
        GetGameObject.transform.Translate(speed * Time.deltaTime * -Vector3.right);

        if (collided)
        {
            collided = false;
            Respawn();
        }
    }

    public void FixedUpdate()
    {
        Transform cameraTransform = GameManager.cameraColliderComponent.GetGameObject.transform;

        bool collided = CollisionCheck.BoxToBox(GetGameObject.transform, cameraTransform);
        bool activeGO = cameraTransform.gameObject.activeSelf;
        bool onLeftSide = GetGameObject.transform.position.x < cameraTransform.transform.position.x;

        this.collided = !collided && activeGO && onLeftSide;
    }

    private void Respawn()
    {
        Vector3 newPosition = GameManager.terrainSpawnPosition;
        if (Random.Range(0.01f, 1.0f) <= ChanceForHeightChange)
        {
            newPosition = new
            (
                x: newPosition.x,
                y: newPosition.y + Random.Range(HeightOffset.x, HeightOffset.y),
                z: newPosition.z
            );
        }

        GetGameObject.transform.position = newPosition;
    }

    public void OnDrawGizmos()
    {
        if (collided)
            GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
        else
            GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);
    }
}
