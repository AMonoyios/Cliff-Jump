using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagRepo
{
    public enum TagEnums
    {
        Untagged,
        Scene,
        Terrain,
        Obstacle,
        Pou
    }

    public static string Tags(TagEnums tag)
    {
        switch (tag)
        {
            case TagEnums.Untagged:
            {
                return "Untagged";
            }
            case TagEnums.Scene:
            {
                return "Scene";
            }
            case TagEnums.Terrain:
            {
                return "Terrain";
            }
            case TagEnums.Obstacle:
            {
                return "Obstacle";
            }
            case TagEnums.Pou:
            {
                return "Pou";
            }
            default:
                return "Untagged";
        }
    }
}
