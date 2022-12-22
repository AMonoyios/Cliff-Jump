using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class CameraColliderComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    public CameraColliderComponent(GameObject cameraColliderGameObject)
    {
        GetGameObject = cameraColliderGameObject;
    }

    public void OnDrawGizmos()
    {
        GizmosExtra.DrawOutlinedCube(GetGameObject.transform, Color.green / 3.0f, Color.green);
    }
}
