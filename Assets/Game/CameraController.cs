using System;
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

        private Vector3 currVelocity = Vector3.zero;
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
