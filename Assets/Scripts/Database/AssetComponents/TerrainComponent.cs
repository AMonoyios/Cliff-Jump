using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class TerrainConfigure
{
    public float startSpeed = 0.5f;
    public float maxSpeed = 5.0f;
    public float speedIncreasePerFrame = 0.005f;
}

public sealed class TerrainComponent : IUpdatable, ICollidable
{
    public GameObject gameObject { get; }

    public float currentSpeed;
    public float maxSpeed;
    public float speedIncreasePerFrame;

    public TerrainComponent(GameObject gameObject, TerrainConfigure terrainConfigure)
    {
        this.gameObject = gameObject;

        currentSpeed = terrainConfigure.startSpeed;
        maxSpeed = terrainConfigure.maxSpeed;
        speedIncreasePerFrame = terrainConfigure.speedIncreasePerFrame;

        CollidablesDatabase.Register(this);
    }

    public void Update()
    {
        gameObject.transform.Translate((currentSpeed > maxSpeed ? maxSpeed : currentSpeed += speedIncreasePerFrame) * Time.deltaTime * -Vector3.right);

        Debug.Log(currentSpeed);
    }

    public void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }
}
