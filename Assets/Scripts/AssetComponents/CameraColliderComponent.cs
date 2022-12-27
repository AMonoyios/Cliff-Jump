using UnityEngine;
using Utils;

// Class that holds bounds for camera collider
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

// Behaviour component for camera collider
public class CameraColliderComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    // Constructor for camera collider behaviour
    public CameraColliderComponent(GameObject cameraColliderGameObject)
    {
        GetGameObject = cameraColliderGameObject;
    }

    public void OnDrawGizmos()
    {
        GizmosExtra.DrawOutlinedCube(GetGameObject.transform, Color.green / 3.0f, Color.green / 1.5f);
    }
}
