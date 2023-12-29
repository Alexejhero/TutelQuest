using System.Collections;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        private bool isAlt;
        
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;
        public GameObject smokePoof;

        [Range(0f, 5f)]
        public float swapCooldown = 1;
        protected float nextSwapTime;
        public float swapDelay = 0.050f;
        
        public IEnumerator OnSwapForm()
        {
            if (!player.enabled) yield break;
            if (Time.time < nextSwapTime) yield break;
            nextSwapTime = Time.time + swapCooldown;

            Instantiate(smokePoof, transform);
            yield return new WaitForSeconds(swapDelay);

            isAlt = !isAlt;
            altForm.SetActive(isAlt);
            regularForm.SetActive(!isAlt);
            OnSwap(isAlt);
        }
        protected abstract void OnSwap(bool isAlt);
    }
}
