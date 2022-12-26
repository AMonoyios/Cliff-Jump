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

    private bool outOfCameraBounds = false;

    public TerrainComponent(GameObject gameObject, TerrainConfigure terrainConfigure)
    {
        GetGameObject = gameObject;

        Speed = terrainConfigure.variableSpeed ? terrainConfigure.minMaxSpeed.x : terrainConfigure.speed;
        MaxSpeed = terrainConfigure.variableSpeed ? terrainConfigure.minMaxSpeed.y : terrainConfigure.speed;
        Acceleration = terrainConfigure.variableSpeed ? terrainConfigure.acceleration : 0.0f;

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

        if (outOfCameraBounds)
        {
            outOfCameraBounds = false;
            Respawn();
        }
    }

    public void FixedUpdate()
    {
        Transform cameraTransform = GameManager.cameraColliderComponent.GetGameObject.transform;

        outOfCameraBounds = !CollisionCheck.BoxToBox(GetGameObject.transform, cameraTransform) &&
            cameraTransform.gameObject.activeSelf &&
            GetGameObject.transform.position.x < cameraTransform.transform.position.x - (cameraTransform.transform.localScale.x / 2.0f);
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
        if (GameManager.playerComponent.ClosestTerrainComponent.GetGameObject.transform.GetInstanceID() == this.GetGameObject.transform.GetInstanceID())
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
        }

        // if (outOfCameraBounds)
        //     GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.green);
        // else
        //     GizmosExtra.DrawSphereAboveObject(GetGameObject.transform, Color.red);

        if (Speed >= MaxSpeed)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
        }
    }
}
