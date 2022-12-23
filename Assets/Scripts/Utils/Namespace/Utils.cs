using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public static class Create
	{
		public static GameObject NewGameObject(string name, Transform parent = null)
		{
			 return NewGameObject(name, Vector3.zero, Quaternion.identity, Vector3.one, parent);
		}
		public static GameObject NewGameObject(string name, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
		{
			GameObject gameObject = new()
			{
				name = name
			};
			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.transform.parent = parent;

			gameObject.transform.localScale = scale;

			return gameObject;
		}

		public static GameObject NewPrefab(GameObject prefab, Transform parent = null)
		{
			return NewPrefab(prefab, prefab.transform.position, prefab.transform.rotation, prefab.transform.localScale, parent);
		}
		public static GameObject NewPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
		{
			GameObject newPrefab = Object.Instantiate(prefab, position, rotation, parent);

			newPrefab.transform.localScale = scale;

			return newPrefab;
		}
	}

	public static class Extensions
	{
		public static SetupAsset FindById(this List<SetupAsset> setupAssets, string id)
		{
			foreach (SetupAsset asset in setupAssets)
			{
				if (asset.id == id)
				{
					return asset;
				}
			}

			return null;
		}

		public static float RoundToDecimals(this float value, float precision, float threshold = 0.1f)
		{
            return value > threshold ? Mathf.Floor(value / precision) * precision : value;
        }
	}

	public static class GizmosExtra
	{
		public static void DrawSphereAboveObject(Transform target)
		{
			DrawSphereAboveObject(target, Color.red);
		}
		public static void DrawSphereAboveObject(Transform target, Color color)
		{
			Gizmos.color = color;
        	Vector3 gizmoPos = new(	target.position.x,
                               		target.position.y + target.localScale.y,
                               		target.position.z);
        	Gizmos.DrawSphere(gizmoPos, 0.25f);
		}

		public static void DrawOutlinedCube(Transform target, Color facesColor, Color edgesColor)
		{
			Gizmos.color = facesColor;
        	Gizmos.DrawCube(target.position, target.localScale);
        	Gizmos.color = edgesColor;
        	Gizmos.DrawWireCube(target.position, target.localScale);
		}
	}

	public static class ColorExtra
	{
		public static Color Brown
		{
			get { return new Color(0.5f, 0.25f, 0.016f, 1.0f); }
		}

		public static Color Orange
		{
			get { return new Color(1.0f, 0.55f, 0.1f, 1.0f); }
		}
	}
}
