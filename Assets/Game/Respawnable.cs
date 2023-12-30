using System;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class Respawnable : MonoBehaviour
    {
        private Vector3 checkpoint;
        public Action<Respawnable> OnReset;

        public void Awake()
        {
            SetCheckpoint(transform.position);
        }
        public void Respawn()
        {
            OnReset?.Invoke(this);
            transform.position = checkpoint;
        }
        public void SetCheckpoint(Vector3 pos)
            => checkpoint = pos;
    }
}