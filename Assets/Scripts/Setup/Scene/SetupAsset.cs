using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetupAsset
{
    public string id;
	public GameObject prefab;
	public int size;
	public TagRepo.TagEnums parentTagEnum;

	public SetupAsset(string id, GameObject prefab, int size, TagRepo.TagEnums parentTag = TagRepo.TagEnums.Untagged)
	{
		this.id = id;
		this.prefab = prefab;
		this.size = size;
		this.parentTagEnum = parentTag;
	}
}
