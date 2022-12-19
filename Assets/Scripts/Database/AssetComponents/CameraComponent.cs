using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class CameraComponent : ICollidable
{
    public GameObject gameObject => Camera.main.gameObject;

    public void FixedUpdate()
    {
    }
}
