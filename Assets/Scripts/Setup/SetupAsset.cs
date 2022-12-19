using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetupAsset
{
    public string id;
	public GameObject prefab;
	public int size;
	public TagEnums parentTag;

	public string Name
	{
		get
		{
			return prefab == null ?
				"Unknown" :
				prefab.name;
		}
	}

	public SetupAsset(string id, GameObject prefab, int size, TagEnums parentTag = TagEnums.Untagged)
	{
		this.id = id;
		this.prefab = prefab;
		this.size = size;
		this.parentTag = parentTag;
	}
}
