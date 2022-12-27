using UnityEngine;

// Scene assets settings
[System.Serializable]
public class SetupAsset
{
    public string id;
	public GameObject prefab;
	public int size;
	[Tooltip("This is just for organization, if left empty object will spawn in root level. Tip: Generation will create a 'Scene' gameObject regardless.")]
	public string parentName;

	public string Name
	{
		get
		{
			return prefab == null ?
				"Unknown" :
				prefab.name;
		}
	}
}
