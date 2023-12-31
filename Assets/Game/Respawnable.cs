using System;
using System.Collections;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class Respawnable : MonoBehaviour
    {
        private Vector3 checkpoint;
        public Action<Respawnable> OnReset;

        private void Awake()
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
                Player pl = GetComponent<Player>();
                yield return new WaitForSeconds(pl && pl == Player.ActivePlayer ? 1 : 0);
                transform.position = checkpoint;
            }
        }
        public void SetCheckpoint(Vector3 pos)
            => checkpoint = pos;
    }
}
