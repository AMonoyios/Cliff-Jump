using UnityEngine;

public interface ICollidable
{
    GameObject gameObject { get; }

    void FixedUpdate();
}
