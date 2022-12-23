using UnityEngine;

[System.Serializable]
public class TerrainConfigure
{
    public bool variableSpeed;

    [ConditionalHide(nameof(variableSpeed), true, true), Range(0.0f, 10.0f)]
    public float speed;

    [ConditionalHide(nameof(variableSpeed), true), MinMaxSlider(0.0f, 20.0f, nameof(minMaxSpeed))]
    public Vector2 minMaxSpeed;

    [ConditionalHide(nameof(variableSpeed), true), Range(0.0f, 0.00025f)]
    public float acceleration;

    [Space(10.0f)]
    public bool variableHeight;
    [ConditionalHide(nameof(variableHeight), true), Range(0.0f, 100.0f)]
    public float chanceForHeightChange;

    [ConditionalHide(nameof(variableHeight), true), MinMaxSlider(-5.0f, 5.0f, nameof(minMaxHeight))]
    public Vector2 minMaxHeight;
}
