using System.Collections;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        protected bool isAlt;

        protected CharacterSwitcher switcher;
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;
        public GameObject smokePoof;

        public float swapDelay = 0.100f;
        private bool _swapping;

        protected virtual void Start()
        {
            switcher = MonoSingleton<CharacterSwitcher>.Instance;
        }
        
        public void OnSwapForm()
        {
            if (!player.enabled) return;
            if (!CanSwap(!isAlt)) return;
            if (switcher.GlobalTransformCooldown > 0) return;
            
            StartCoroutine(PerformSwap());
        }
        public IEnumerator PerformSwap()
        {
            if (_swapping) yield break;
            _swapping = true;
            switcher.GlobalTransformCooldown = 0.75f;

            Instantiate(smokePoof, transform);
            yield return new WaitForSeconds(swapDelay);

            SwapImmediate();
        }

        protected void SwapImmediate()
        {
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
