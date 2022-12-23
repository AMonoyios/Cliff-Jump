using System.Collections;
using System.Collections.Generic;
using CollisionDetection;
using UnityEngine;
using Utils;

public sealed class TerrainComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    private float Speed { get; set; }
    private float MaxSpeed { get; }
    private float Acceleration { get; }
    private float ChanceForHeightChange { get; }
    private float MinHeight { get; }
    private float MaxHeight { get; }

    private bool collided = false;

    public TerrainComponent(GameObject gameObject, TerrainConfigure terrainConfigure)
    {
        GetGameObject = gameObject;

        Speed = terrainConfigure.variableSpeed ? terrainConfigure.minMaxSpeed.x : terrainConfigure.speed;
        MaxSpeed = terrainConfigure.variableSpeed ? terrainConfigure.minMaxSpeed.y : terrainConfigure.speed;
        Acceleration = terrainConfigure.variableSpeed ? terrainConfigure.acceleration : 0.0f;

        // FIXME: does not take affect height
        if (terrainConfigure.variableHeight)
        {
            ChanceForHeightChange = terrainConfigure.chanceForHeightChange;
            if (ChanceForHeightChange > 0.0f)
            {
                MinHeight = terrainConfigure.minMaxHeight.x;
                MaxHeight = terrainConfigure.minMaxHeight.y;
            }
        }
    }

    public void Update()
    {
        float speed = Speed > MaxSpeed ? MaxSpeed : Speed += Acceleration;

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

        // FIXME: if asset is under collider but at the right side it won't despawn
        bool onLeftSide = GetGameObject.transform.position.x < cameraTransform.transform.position.x;

        this.collided = !collided && activeGO && onLeftSide;
    }

    private void Respawn()
    {
        float heightOffset = Random.Range(0.1f, 1.0f).RoundToDecimals(1) < ChanceForHeightChange ? Random.Range(MinHeight, MaxHeight) : 0.0f;
        Vector3 spawnPos = GameManager.terrainSpawnPosition;
        GetGameObject.transform.position = new
        (
            x: spawnPos.x,
            y: spawnPos.y + heightOffset,
            z: spawnPos.z
        );
    }

    public void OnDrawGizmos()
    {
        if (collided)
            GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
        else
            GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);

        if (Speed >= MaxSpeed)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
        }
    }
}
