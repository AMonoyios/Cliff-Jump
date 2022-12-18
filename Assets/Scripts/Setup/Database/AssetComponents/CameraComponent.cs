using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class CameraComponent : IUpdatable
{
    public GameObject gameObject => Camera.main.gameObject;

    public void Update()
    {
    }

    #if UNITY_EDITOR
    public void OnDrawGizmos()
    {
    }
    #endif
}
