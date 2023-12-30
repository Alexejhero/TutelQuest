using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SchizoQuest.Characters
{
	public enum NeuroState { Neuro, Evil };

    public sealed class CameraController : MonoBehaviour
    {
        public Transform target;

		public NeuroState CurrentPlayerState { get; set; }

		public List<CameraLayerMask> masks;

		[SerializeField]
        [Range(0.01f, 0.9f)]
        private float smoothTime = 0.35f;

        private Vector3 currVelocity = Vector3.zero;
		private Camera cam;

		private void Awake()
		{
			cam = GetComponent<Camera>();
		}

		public void Update()
        {
            Vector3 desiredCamPos = target.position;
            Vector3 camPos = transform.position;
            desiredCamPos.z = camPos.z;
            camPos = Vector3.SmoothDamp(camPos, desiredCamPos, ref currVelocity, smoothTime);
            transform.position = camPos;
        }

		/// <summary>
		/// Call SetLayer externally to change camera layer set
		/// </summary>
		/// <param name="ls">Neuro's state <see cref="NeuroState"/></param>
		public void SetNeuroState(NeuroState ls)
		{
			CurrentPlayerState = ls;

			foreach (CameraLayerMask cm in masks)
			{
				if (cm.set == ls)
				{
					cam.cullingMask = cm.layers;
					break;
				}
			}
		}
	}

	[Serializable]
	public struct CameraLayerMask
	{
		public NeuroState set;
		public LayerMask layers;
	}

}
