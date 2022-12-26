using UnityEngine;

// - Base interface for custom behaviour "components" in database.
// Note: All interfaces have a default constructor to prevent the necessity of implementing
// a constructor in each behaviour "component" that inherits this interface. 
public interface IBehaviour
{
    GameObject GetGameObject { get; }

    void Update() {}

    void FixedUpdate() {}

    void OnDrawGizmos() {}

    void OnDrawGizmosSelected() {}
}
