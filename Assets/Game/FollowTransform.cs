using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class FollowTransform : MonoBehaviour
    {
        public Transform target;

        [SerializeField]
        [Range(0.01f, 0.9f)]
        private float smoothTime = 0.35f;
        private Vector3 currVelocity = Vector3.zero;

        public void Update()
        {
            var desiredCamPos = target.position;
            var camPos = transform.position;
            desiredCamPos.z = camPos.z;
            camPos = Vector3.SmoothDamp(camPos, desiredCamPos, ref currVelocity, smoothTime);
            transform.position = camPos;
        }
    }
}