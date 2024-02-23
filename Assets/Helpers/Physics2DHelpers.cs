using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class Physics2DHelpers
    {
        public static IEnumerable<ContactPoint2D> GetContacts(this Collision2D collision)
        {
            int count = collision.contactCount;
            for (int i = 0; i < count; i++)
            {
                yield return collision.GetContact(i);
            }
        }

        public static bool WillPassThroughPlatform(PlatformEffector2D platform, Vector2 normal, Rigidbody2D rb)
        {
            if (!platform) return false;

            Rigidbody2D platformRb = platform.GetComponent<Rigidbody2D>();
            Vector2 platformVelocity = platformRb ? platformRb.velocity : Vector2.zero;
            return WillPassThroughPlatform(platform, normal, rb.velocity - platformVelocity);
        }
        public static bool WillPassThroughPlatform(PlatformEffector2D platform, Vector2 normal, Vector2 relativeVelocity)
        {
            if (!platform) return false;
            if (!platform.useOneWay) return false;

            Vector2 platformUp = Rotate(platform.transform.up, platform.rotationalOffset);
            float sideAngle = platform.surfaceArc * 0.5f;
            return !normal.IsWithinArc(platformUp, sideAngle)
                || relativeVelocity.IsWithinArc(platformUp, sideAngle);
        }

        private static bool IsWithinArc(this Vector2 vector, Vector2 arcCenter, float arcSideAngle)
        {
            float angle = Vector2.Angle(arcCenter, vector);
            return angle < arcSideAngle;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees, bool clockwise = true)
        {
            if (!clockwise) degrees = -degrees;

            float radian = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radian);
            float cos = Mathf.Cos(radian);

            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }

        /// <summary>
        /// Aggregates the given colliders into a <see cref="Bounds"/> object that encloses them around the given <paramref name="center"/>.
        /// </summary>
        /// <param name="colliders">Colliders to encapsulate.</param>
        /// <param name="center">Center from which to make the bounds. Defaults to the center of the first collider (or <see cref="Vector3.zero"/> if none were provided).</param>
        /// <returns>A <see cref="Bounds"/> that encloses the given <paramref name="colliders"/> around the given <paramref name="center"/>.</returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Bounds Encapsulate(this IEnumerable<Collider2D> colliders, Vector3? center = null)
        {
            if (center is null)
            {
                Collider2D first = colliders.FirstOrDefault();
                center = first ? first.bounds.center : Vector3.zero;
            }
            Bounds bounds = new(center.Value, Vector3.zero);
            foreach (Collider2D coll in colliders)
            {
                if (!coll.isActiveAndEnabled) continue;
                bounds.Encapsulate(coll.bounds);
            }
            return bounds;
        }
    }
}
