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

            TerrainComponent terrainComponent = CustomBehaviourAssetsDatabase.GetBehaviour<TerrainComponent>(terrainGameObject);

            if (terrainComponent == null)
            {
                Debug.LogError("TerrainComponent is NULL");
                return;
            }

            bool collided = CollisionCheck.BoxToBox(GetGameObject.transform, terrainGameObject.transform);
            bool activeGO = terrainGameObject.activeSelf;
            bool passedLeftBound = terrainGameObject.transform.position.x < Camera.main.transform.position.x;

            terrainComponent.collided = !collided && activeGO && passedLeftBound;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetGameObject.transform.position, GetGameObject.transform.localScale);
    }
}
