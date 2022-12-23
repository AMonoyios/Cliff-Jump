using System.Collections;
using UnityEngine;

public class MinMaxSliderAttribute : PropertyAttribute
{
    // $ becomes the name of the max property
    // example: [MinMaxSlider] float duration; float durationMin;
    public string minName = "$Min";
    public float? max;
    public float min;

    public MinMaxSliderAttribute(string minName = null)
    {
        if (minName != null)
            this.minName = minName;
    }
    public MinMaxSliderAttribute(float max, string minName = null) : this(0, max, minName) { }
    public MinMaxSliderAttribute(float min, float max, string minName = null) : this(minName)
    {
        this.max = max;
        this.min = min;
    }
}
