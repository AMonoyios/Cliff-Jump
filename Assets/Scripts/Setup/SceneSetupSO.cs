using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSetupSO", menuName = "ScriptableObjects/SceneSetup")]
public class SceneSetupSO : ScriptableObject
{
	[System.Serializable]
	public class Asset
	{
		public string id;
		public GameObject prefab;
		public int size;
		[TagSelector]
		public string parentTag;

		public Asset(string id, GameObject prefab, int size, string parentTag = "Untagged")
		{
			this.id = id;
			this.prefab = prefab;
			this.size = size;
			this.parentTag = parentTag;
		}
	}

	public List<Asset> Assets;
}
