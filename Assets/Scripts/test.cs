using CollisionDetection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class test : IBehaviour
{
	public GameObject GetGameObject { get; }

	private bool drawGizmo = false;

	public test(GameObject go)
    {
		GetGameObject = go;
        CustomBehaviourAssetsDatabase.Register<test>(this);
    }

    public void FixedUpdate()
    {
		foreach (IBehaviour behaviour in CustomBehaviourAssetsDatabase.Values)
		{
			CameraColliderComponent comp = CustomBehaviourAssetsDatabase.GetBehaviour<CameraColliderComponent>(behaviour.GetGameObject);

			Debug.Log(comp.GetGameObject.name);

			if (comp != null)
			{
				if (!CollisionCheck.BoxToBox(GetGameObject.transform, comp.GetGameObject.transform) && comp.GetGameObject.activeSelf)
				{
					drawGizmo = true;
				}
				else
				{
					drawGizmo = false;
				}
			}
		}
	}

	public void OnDrawGizmos()
	{
		if (drawGizmo)
		{
			GizmosExtra.DrawSphereAboveObject(GetGameObject.transform);
		}
	}
}
