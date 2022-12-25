using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class CameraBounds
{
    public Vector2 XCameraBounds { get; }
    public Vector2 YCameraBounds { get; }
    public Vector2 ZCameraBounds { get; }

    public CameraBounds(Vector2 xBounds, Vector2 yBounds, Vector2 zBounds)
    {
        XCameraBounds = xBounds;
        YCameraBounds = yBounds;
        ZCameraBounds = zBounds;
    }
}

public class CameraColliderComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    public CameraColliderComponent(GameObject cameraColliderGameObject)
    {
        GetGameObject = cameraColliderGameObject;
    }

    // public void OnDrawGizmos()
    // {
    //     GizmosExtra.DrawOutlinedCube(GetGameObject.transform, Color.green / 3.0f, Color.green);
    // }
}
