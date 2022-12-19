using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class CameraComponent
{
    public GameObject gameObject { get; }

    readonly Camera camera;
    Bounds cameraBounds;

    Vector3 boundBoxScale = Vector3.zero;

    GameObject cameraColliderGameObject;
    CameraColliderComponent cameraCollider;

    public CameraComponent(Camera camera)
    {
        gameObject = camera.gameObject;
        this.camera = camera;

        new Promise<bool>()
            .Add(CalculateCameraFrustumArea)
            .Add(CreateCameraCollisionArea)
            .Add(AddCameraColliderToDatabase)
            .Condition((result) => result)
            .Execute()
            .OnComplete(() => Debug.Log("Camera collider area registered."));
    }

    private bool CalculateCameraFrustumArea()
    {
        if (camera == null)
        {
            Debug.LogError("Camera is null or missing");
            return false;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
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

        cameraBounds = new(Vector3.zero, Vector3.zero);
        for (int i = 0; i < 6; ++i)
        {
            cameraBounds.Encapsulate(-planes[i].normal * planes[i].distance);
        }

        return true;
    }

    private bool CreateCameraCollisionArea()
    {
        cameraColliderGameObject = Create.NewGameObject("CameraCollisionArea", cameraBounds.center, Quaternion.identity, boundBoxScale, Camera.main.transform);
        cameraCollider = new(cameraColliderGameObject);

        if (cameraCollider == null)
        {
            Debug.LogError("Failed to create camera collider!");
            return false;
        }

        return true;
    }

    private bool AddCameraColliderToDatabase()
    {
        CollidablesDatabase.Register(cameraCollider);

        if (CollidablesDatabase.GetBehaviour<CameraColliderComponent>(camera.gameObject) == null)
        {
            Debug.LogError("Failed to find behaviour for camera collider gameobject in CollidablesDatabase.");
            return false;
        }

        return true;
    }
}
