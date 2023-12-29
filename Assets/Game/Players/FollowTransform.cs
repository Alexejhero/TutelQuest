using UnityEngine;

namespace SchizoQuest.Game.Players
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
            Vector3 desiredCamPos = target.position;
            Vector3 camPos = transform.position;
            desiredCamPos.z = camPos.z;
            camPos = Vector3.SmoothDamp(camPos, desiredCamPos, ref currVelocity, smoothTime);
            transform.position = camPos;
        }
    }
}
