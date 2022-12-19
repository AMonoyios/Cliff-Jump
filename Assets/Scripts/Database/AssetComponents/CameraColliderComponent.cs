using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionDetection;

public class CameraColliderComponent : ICollidable
{
    public GameObject gameObject { get; }

    public CameraColliderComponent(GameObject cameraColliderGameObject)
    {
        gameObject = cameraColliderGameObject;
    }

    public void FixedUpdate()
    {
        Debug.Log("Camera collider checks");
    }
}
