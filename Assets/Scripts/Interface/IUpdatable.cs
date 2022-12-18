using UnityEngine;

public interface IUpdatable
{
    GameObject gameObject { get; }

    void Update();

    void OnDrawGizmos();
}
