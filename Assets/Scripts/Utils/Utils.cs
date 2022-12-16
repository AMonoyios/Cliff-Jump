using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public static class Create
	{
		public static GameObject NewGameObject(string name, Transform parent = null, string tag = "Untagged")
		{
			 return NewGameObject(name, Vector3.zero, Quaternion.identity, Vector3.one, parent, tag);
		}
		public static GameObject NewGameObject(string name, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null, string tag = "Untagged")
		{
			GameObject gameObject = new()
			{
				name = name
			};
			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.transform.localScale = scale;
			gameObject.transform.parent = parent;
			gameObject.transform.tag = tag;

			return gameObject;
		}

		public static GameObject NewPrefab(GameObject prefab, Transform parent = null, string tag = "Untagged")
		{
			return NewPrefab(prefab, prefab.transform.position, prefab.transform.rotation, prefab.transform.localScale, parent, tag);
		}
		public static GameObject NewPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null, string tag = "Untagged")
		{
			GameObject newPrefab = GameManager.Instantiate(prefab, position, rotation, parent);
			newPrefab.transform.localScale = scale;
			newPrefab.transform.tag = tag;

			return newPrefab;
		}
	}
	
	
}
