using UnityEngine;

public interface IBehaviour
{
    GameObject GetGameObject { get; }

    void Update() {}

    void FixedUpdate() {}

    void OnDrawGizmos() {}

    void OnDrawGizmosSelected() {}
}
