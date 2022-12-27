using UnityEngine;

// Terrain settings
[System.Serializable]
public class TerrainConfigure
{
    [Range(0.5f, 5.0f)]
    public float scale = 2.0f;

    [Space(GameManager.guiSpace)]
    public bool variableSpeed = false;
    [ConditionalHide(nameof(variableSpeed), true, true), Range(0.0f, 10.0f)]
    public float speed = 1.0f;
    [ConditionalHide(nameof(variableSpeed), true), MinMaxSlider(0.0f, 20.0f, nameof(minMaxSpeed))]
    public Vector2 minMaxSpeed;
    [ConditionalHide(nameof(variableSpeed), true), Range(0.0f, 0.00025f)]
    public float acceleration;

    [Space(GameManager.guiSpace)]
    public bool variableHeight = false;
    [ConditionalHide(nameof(variableHeight), true), Range(0.0f, 100.0f)]
    public float chanceForHeightChange;
    [ConditionalHide(nameof(variableHeight), true), MinMaxSlider(-2.0f, 2.0f, nameof(minMaxHeight))]
    public Vector2 minMaxHeight;
}
