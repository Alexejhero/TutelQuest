using SchizoQuest.Characters;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class CameraController : MonoSingleton<CameraController>
    {
        public Transform target;

        [SerializeField]
        [Range(0.01f, 0.9f)]
        private float smoothTime = 0.35f;

        [Min(0)]
        public float maxDistance = 200f;

        internal static Vector3 currVelocity = Vector3.zero;
        internal static float DistanceToActivePlayer => Vector3.Distance(Player.ActivePlayer.transform.position, Camera.main.transform.position);

        public void Update()
        {
            Vector3 desiredCamPos = target.position;
            Vector3 camPos = transform.position;
            desiredCamPos.z = camPos.z;
            var direction = desiredCamPos - camPos;
            if (direction.magnitude > maxDistance)
                camPos = desiredCamPos - direction.normalized * maxDistance;
            camPos = Vector3.SmoothDamp(camPos, desiredCamPos, ref currVelocity, smoothTime);
            transform.position = camPos;
        }
    }
}
