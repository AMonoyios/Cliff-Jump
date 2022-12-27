using UnityEngine;

// - Namespace that holds all collision cases
// Disclaimer: Currently only 2 cases were needed for this project.
namespace CollisionDetection
{
    public static class CollisionCheck
    {
        // AABB vs AABB collision check
        public static bool BoxToBox(Transform box1, Transform box2)
        {
            Vector3 axisPositionDifference = new
            (
                x: Mathf.Abs(box1.position.x - box2.position.x),
                y: Mathf.Abs(box1.position.y - box2.position.y),
                z: Mathf.Abs(box1.position.z - box2.position.z)
            );

            Vector3 scaleDistance = new
            (
                x: (box1.localScale.x + box2.localScale.x) / 2.0f,
                y: (box1.localScale.y + box2.localScale.y) / 2.0f,
                z: (box1.localScale.z + box2.localScale.z) / 2.0f
            );

            return  axisPositionDifference.x <= scaleDistance.x &&
                    axisPositionDifference.y <= scaleDistance.y &&
                    axisPositionDifference.z <= scaleDistance.z;
        }

        // AABB vs Sphere collision check
        public static bool BoxToSphere(Transform box, Transform sphere)
        {
            float sphereRadius = sphere.gameObject.transform.lossyScale.x / 2.0f;

            Vector3 closestPoint = new
            (
                x: Mathf.Max(box.transform.position.x - (box.transform.localScale.x / 2.0f), Mathf.Min(sphere.transform.position.x, box.transform.position.x + (box.transform.localScale.x / 2.0f))),
                y: Mathf.Max(box.transform.position.y - (box.transform.localScale.y / 2.0f), Mathf.Min(sphere.transform.position.y, box.transform.position.y + (box.transform.localScale.y / 2.0f))),
                z: Mathf.Max(box.transform.position.z - (box.transform.localScale.z / 2.0f), Mathf.Min(sphere.transform.position.z, box.transform.position.z + (box.transform.localScale.z / 2.0f)))
            );

            float distance = Mathf.Sqrt(((closestPoint.x - sphere.transform.position.x) * (closestPoint.x - sphere.transform.position.x)) +
                                        ((closestPoint.y - sphere.transform.position.y) * (closestPoint.y - sphere.transform.position.y)) +
                                        ((closestPoint.z - sphere.transform.position.z) * (closestPoint.z - sphere.transform.position.z)));

            return distance < sphereRadius;
        }
    }
}
