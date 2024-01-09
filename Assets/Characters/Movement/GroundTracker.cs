using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters.Movement
{
    public sealed class GroundTracker : MonoBehaviour
    {
        public float maxSurfaceAngle = 45f;
        private float _savedMaxSurfaceAngle;
        private float minSurfaceCos;
        public bool isOnGround;
        public Vector2 lastSurfacePoint;
        public Vector2 surfaceNormal;
        public Collider2D surfaceCollider;
        // cleared on FixedUpdate, (maybe) set in OnCollision****2D, then checked in Update
        // see https://docs.unity3d.com/Manual/ExecutionOrder.html
        // this does mean that changes are delayed until the next physics frame
        private bool _internalGroundCheck;

        private void FixedUpdate()
        {
            if (maxSurfaceAngle != _savedMaxSurfaceAngle)
                RecalculateSurfaceCos();
            _internalGroundCheck = false;
        }

        private void Update()
        {
            isOnGround = _internalGroundCheck;
        }

        private void RecalculateSurfaceCos()
        {
            _savedMaxSurfaceAngle = maxSurfaceAngle;
            minSurfaceCos = Mathf.Cos(Mathf.Deg2Rad * maxSurfaceAngle);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            GroundCheck(collision);
        }

        public void OnCollisionStay2D(Collision2D collision)
        {
            GroundCheck(collision);
        }

        private void GroundCheck(Collision2D collision)
        {
            if (!enabled) return;

            if (CheckContactNormals(collision))
            {
                _internalGroundCheck = true;
                surfaceCollider = collision.collider;
            }
        }

        private bool CheckContactNormals(Collision2D collision)
        {
            if (!collision.gameObject) return false;
            if (collision.contactCount == 0) return false;
            foreach (ContactPoint2D contact in collision.GetContacts())
            {
                Vector2 normal = contact.normal;
                if (Vector2.Dot(Vector2.up, normal) >= minSurfaceCos)
                {
                    surfaceNormal = normal;
                    lastSurfacePoint = contact.point;
                    return true;
                }
            }

            return false;
        }
#if DEBUG
        [SerializeField] private bool debug;
        private void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.color = isOnGround ? Color.green : Color.red;
            Gizmos.DrawRay(lastSurfacePoint, surfaceNormal * 2);
            Gizmos.DrawWireSphere(lastSurfacePoint, 0.2f);
        }
#endif
    }
}