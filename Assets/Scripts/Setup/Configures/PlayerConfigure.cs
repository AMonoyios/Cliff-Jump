using UnityEngine;

[System.Serializable]
public class PlayerConfigure
{
	[Range(0.0f, 100.0f), Tooltip("This percentage will determine how left or right the player will spawn. 0: Full left, 100: Full right")]
	public float spawnXOffset = 50.0f;
}