using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    GameObject gameObject { get; }

    void Update();
}
