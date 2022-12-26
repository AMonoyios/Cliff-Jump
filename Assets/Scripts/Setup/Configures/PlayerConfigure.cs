using UnityEngine;

[System.Serializable]
public class PlayerConfigure
{
	[Range(0.01f, 0.5f)]
	public float scale = 0.1f;

	[Space(GameManager.guiSpace)]
	[Range(0.0f, 100.0f), Tooltip("This percentage will determine how left or right the player will spawn. 0: Full left, 100: Full right")]
	public float spawnXOffset = 50.0f;

	[Space(GameManager.guiSpace)]
	[Range(4.0f, 15.0f)]
	public float jumpForce = 8.0f;
    [SerializeField, Range(1, 5)]
    public int maxJumps = 2;
	[Range(0.125f, 0.5f)]
	public float stepThreshold = 0.25f;
}
