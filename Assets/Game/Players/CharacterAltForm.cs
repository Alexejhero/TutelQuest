using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        private bool isAlt;
        
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;

        [Range(0f, 5f)]
        public float swapCooldown = 1;
        protected float nextSwapTime;
        
        public void OnSwapForm()
        {
            if (!player.enabled) return;
            if (Time.time < nextSwapTime) return;
            nextSwapTime = Time.time + swapCooldown;

            isAlt = !isAlt;
            altForm.SetActive(isAlt);
            regularForm.SetActive(!isAlt);
            OnSwap(isAlt);
        }
        protected virtual void OnSwap(bool isAlt)
        {
            // play smoke particles
        }
    }
}
