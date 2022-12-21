using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class CameraComponent
{
    private Camera GetCamera { get; }

    private Vector3 boundBoxScale = Vector3.zero;

    public CameraComponent()
    {
        GetCamera = Camera.main;
    }

    public bool SetupCamera()
    {
        if (GetCamera == null)
        {
            Debug.LogError("Camera is null or missing");
            return false;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetCamera);
        if (planes.Length < 6)
        {
            Debug.LogError("Planes did not add up to 6 faces, unable to create bound box.");
            return false;
        }

        boundBoxScale = new
        (
            x: Vector3.Distance(-planes[0].normal * planes[0].distance, -planes[1].normal * planes[1].distance),
            y: Vector3.Distance(-planes[2].normal * planes[2].distance, -planes[3].normal * planes[3].distance),
            z: Vector3.Distance(-planes[4].normal * planes[4].distance, -planes[5].normal * planes[5].distance)
        );

        Bounds cameraBounds = new(Vector3.zero, Vector3.zero);
        for (int i = 0; i < 6; ++i)
        {
            cameraBounds.Encapsulate(-planes[i].normal * planes[i].distance);
        }

        GameObject cameraColliderGameObject = Create.NewGameObject("CameraCollisionArea", cameraBounds.center, Quaternion.identity, boundBoxScale, GetCamera.gameObject.transform);
        CameraColliderComponent cameraColliderComponent = new(cameraColliderGameObject);

        if (cameraColliderComponent == null)
        {
            Debug.LogError("Failed to create camera collider!");
            return false;
        }

        CustomBehaviourAssetsDatabase.Register(cameraColliderComponent);

        if (CustomBehaviourAssetsDatabase.GetBehaviour<CameraColliderComponent>(cameraColliderGameObject) == null)
        {
            Debug.LogError("Failed to find behaviour for camera collider gameobject in CollidablesDatabase.");
            return false;
        }

        return true;
    }
}
