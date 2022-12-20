using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionDetection
{
    public static class CollisionCheck
    {
        // AABB vs AABB collision check ( not Tested)
        // FIXME: don't seem to work correclty
        public static bool BoxToBox(Transform box1, Transform box2)
        {
            float distance = Vector3.Distance(box1.position, box2.position);

            return  (distance <= ((box1.localScale.x / 2.0f) + (box2.localScale.x / 2.0f))) ||
                    (distance <= ((box1.localScale.y / 2.0f) + (box2.localScale.y / 2.0f))) ||
                    (distance <= ((box1.localScale.z / 2.0f) + (box2.localScale.z / 2.0f)));
        }

        // AABB vs Sphere collision check (Untested)
        public static bool BoxToSphere(Transform box, Vector3 sphere, float radius)
        {
            Vector3 closestPoint = new
            (
                x: Mathf.Max(Mathf.Abs(box.position.x - (box.localScale.x / 2.0f)), Mathf.Min(sphere.x, Mathf.Abs(box.position.x + (box.localScale.x / 2.0f)))),
                y: Mathf.Max(Mathf.Abs(box.position.y - (box.localScale.y / 2.0f)), Mathf.Min(sphere.y, Mathf.Abs(box.position.y + (box.localScale.y / 2.0f)))),
                z: Mathf.Max(Mathf.Abs(box.position.z - (box.localScale.z / 2.0f)), Mathf.Min(sphere.z, Mathf.Abs(box.position.z + (box.localScale.z / 2.0f))))
            );

            float distance = Mathf.Sqrt(((closestPoint.x - sphere.x) * (closestPoint.x - sphere.x)) +
                                        ((closestPoint.y - sphere.y) * (closestPoint.y - sphere.y)) +
                                        ((closestPoint.z - sphere.z) * (closestPoint.z - sphere.z)));

            return distance < radius;
        }

        // AABB bs Point collision check (Untested)
        public static bool BoxToPoint(Transform box, Vector3 point)
        {
            return  (Mathf.Abs(box.position.x - (box.localScale.x / 2.0f)) <= point.x) ||
                    (Mathf.Abs(box.position.y - (box.localScale.y / 2.0f)) <= point.y) ||
                    (Mathf.Abs(box.position.z - (box.localScale.z / 2.0f)) <= point.z);
        }

        // Sphere to Sphere collision check (Untested)
        public static bool SphereToSphere(Vector3 sphere1, float radius1, Vector3 sphere2, float radius2)
        {
            float distance = Mathf.Sqrt(((sphere1.x - sphere2.x) * (sphere1.x - sphere2.x)) +
                                        ((sphere1.y - sphere2.y) * (sphere1.y - sphere2.y)) +
                                        ((sphere1.z - sphere2.z) * (sphere1.z - sphere2.z)));

            return distance < radius1 + radius2;
        }

        // Sphere to Point collision check (Untested)
        public static bool SphereToPoint(Vector3 sphere, float radius, Vector3 point)
        {
            float distance = Mathf.Sqrt(((point.x - sphere.x) * (point.x - sphere.x)) +
                                        ((point.y - sphere.y) * (point.y - sphere.y)) +
                                        ((point.z - sphere.z) * (point.z - sphere.z)));

            return distance < radius;
        }
    }
}