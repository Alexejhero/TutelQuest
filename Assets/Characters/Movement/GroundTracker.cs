using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters.Movement
{
    public sealed class GroundTracker : MonoBehaviour
    {
        public float maxSurfaceAngle = 45f;
        private float _savedMaxSurfaceAngle;
        private float minSurfaceCos;
        public bool IsGrounded => isOnGround;
        /// <summary>Grounded or in coyote time.</summary>
        public bool IsRecentlyGrounded => isOnGround || coyoteTimer < coyoteTime;
        public float coyoteTime = 0.15f;
        [Header("Runtime Info")]
        [SerializeField] private bool isOnGround;
        public float coyoteTimer;
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
            if (isOnGround)
                coyoteTimer = 0;
            else
                coyoteTimer += Time.deltaTime;
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
                if (!contact.enabled) continue; // platform effectors
                
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
        [SerializeField] private bool debugGizmos;
        private void OnDrawGizmos()
        {
            if (!debugGizmos) return;
            Gizmos.color = isOnGround ? Color.green
                : coyoteTimer < coyoteTime ? Color.yellow
                : Color.red;
            Gizmos.DrawRay(lastSurfacePoint, surfaceNormal * 2);
            Gizmos.DrawWireSphere(lastSurfacePoint, 0.2f);
        }
#endif
    }
}