using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
public class TerrainConfigure
{
    public float startSpeed = 0.5f;
    [Range(0.5f, 3.0f)]
    public float maxSpeed = 2.5f;
    [Range(0.00001f, 0.00151f)]
    public float speedIncreasePerFrame = 0.00075f;
    [Range(0.0f, 1.0f)]
    public float chanceForHeightChange = 0.5f;
    public Vector2 heightOffset = new(-0.25f, 0.25f);
}

public sealed class TerrainComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    public float CurrentSpeed { get; private set; }
    public float MaxSpeed { get; }
    public float SpeedIncrease { get; }
    public float ChanceForHeightChange { get; }
    public Vector2 HeightOffset { get; }

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
        GetGameObject.transform.Translate((CurrentSpeed > MaxSpeed ? MaxSpeed : CurrentSpeed += SpeedIncrease) * Time.deltaTime * -Vector3.right);
    }

    // Note: this runs in fixed update of the camera
    public void Respawn()
    {
        string log = $"Respawning {GetGameObject.transform.name}";

        Vector3 newPosition = GameManager.terrainSpawnPosition;
        if (Random.Range(0.01f, 1.0f) <= ChanceForHeightChange)
        {
            newPosition = new
            (
                x: newPosition.x,
                y: newPosition.y + Random.Range(HeightOffset.x, HeightOffset.y),
                z: newPosition.z
            );

            log += $" with new position: {newPosition}";
        }

        GetGameObject.transform.position = newPosition;

        Debug.Log(log);
    }
}
