using System.Collections;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        protected bool isAlt;

        public CharacterSwitcher characterSwitcher;
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;
        public GameObject smokePoof;

        public float swapDelay = 0.100f;
        private bool _swapping;
        
        public void OnSwapForm()
        {
            if (!player.enabled) return;
            if (!CanSwap(!isAlt)) return;
            if (characterSwitcher.GlobalTransformCooldown > 0) return;
            
            StartCoroutine(PerformSwap());
        }
        public IEnumerator PerformSwap()
        {
            if (_swapping) yield break;
            _swapping = true;
            characterSwitcher.GlobalTransformCooldown = 0.75f;

            Instantiate(smokePoof, transform);
            yield return new WaitForSeconds(swapDelay);

            isAlt = !isAlt;
            altForm.SetActive(isAlt);
            regularForm.SetActive(!isAlt);
            _swapping = false;
            OnSwap();
        }
        protected abstract bool CanSwap(bool toAlt);
        protected abstract void OnSwap();
    }
}
