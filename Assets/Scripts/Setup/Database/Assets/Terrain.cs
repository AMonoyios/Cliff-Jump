using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Terrain : IUpdatable
{
    public GameObject gameObject => gameObject;

    public void Update()
    {
        Debug.Log($"Terrain tile {gameObject.name}");
    }
}
