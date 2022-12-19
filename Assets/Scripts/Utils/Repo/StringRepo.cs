using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagEnums
{
    Untagged,
    Scene,
    Terrain,
    Obstacle,
}

public static class StringRepo
{
    public static class Assets
    {
        public static string Scene => "Scene";
        public static string Terrain => "Terrain";
    }

    public static class Tags
    {
        private static string Untagged => "Untagged";
        private static string Scene => "Scene";
        private static string Terrain => "Terrain";
        private static string Obstacle => "Obstacle";

        public static string ToString(TagEnums tagEnum)
        {
            return tagEnum switch
            {
                TagEnums.Untagged   => Untagged,
                TagEnums.Scene      => Scene,
                TagEnums.Terrain    => Terrain,
                TagEnums.Obstacle   => Obstacle,
                _                   => Untagged,
            };
        }
    }
}
