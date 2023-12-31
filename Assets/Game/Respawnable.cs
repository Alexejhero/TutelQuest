using System;
using System.Collections;
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
            StartCoroutine(Coroutine());
            return;

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(1f);
                transform.position = checkpoint;
            }
        }
        public void SetCheckpoint(Vector3 pos)
            => checkpoint = pos;
    }
}
