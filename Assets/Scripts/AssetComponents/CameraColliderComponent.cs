using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionDetection;

public class CameraColliderComponent : IBehaviour
{
    public GameObject GetGameObject { get; }

    public CameraColliderComponent(GameObject cameraColliderGameObject)
    {
        GetGameObject = cameraColliderGameObject;
    }

    public void FixedUpdate()
    {
        foreach (IBehaviour behaviours in CustomBehaviourAssetsDatabase.Values)
        {
            GameObject terrainGameObject = behaviours.GetGameObject;
            if (!CollisionCheck.BoxToBox(GetGameObject.transform, terrainGameObject.transform) &&
                terrainGameObject.activeSelf &&
                terrainGameObject.transform.position.x < Camera.main.transform.position.x)
            {
                TerrainComponent terrainComponent = CustomBehaviourAssetsDatabase.GetBehaviour<TerrainComponent>(terrainGameObject);
				terrainComponent.Respawn();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
    }
}
