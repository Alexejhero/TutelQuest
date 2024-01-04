using System;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class CameraController : MonoBehaviour
    {
        public Transform target;

        public LayerMask neuroMask;
        public LayerMask evilMask;

        [SerializeField]
        [Range(0.01f, 0.9f)]
        private float smoothTime = 0.35f;
        [Min(0)]
        public float maxDistance = 200f;

        internal static Vector3 currVelocity = Vector3.zero;
        internal static float DistanceToActivePlayer => Vector3.Distance(Player.ActivePlayer.transform.position, Camera.main.transform.position);
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            //SetNeuroState(false);
        }

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

        public void SetNeuroState(bool isEvil)
        {
            cam.cullingMask = isEvil ? evilMask : neuroMask;
        }
    }

    [Serializable]
    public struct CameraLayerMask
    {
        public LayerMask layers;
    }
}
