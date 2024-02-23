using System.Collections;
using JetBrains.Annotations;
using SchizoQuest.Menu.PauseMenu;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        public bool IsAlt { get; protected set; }

        protected CharacterSwitcher switcher;
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;
        public GameObject smokePoof;

        public float swapDelay = 0.100f;
        private bool _swapping;
        protected bool IsSwapping => _swapping;

        protected virtual void Start()
        {
            switcher = CharacterSwitcher.Instance;
        }

        [UsedImplicitly]
        public void OnSwapForm()
        {
            if (!player.enabled) return;
            if (!CanSwap(!IsAlt)) return;
            if (PauseMenuBehaviour.IsOpen) return;
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

        protected void SwapImmediate() => StartCoroutine(SwapInnerCoro());
        private IEnumerator SwapInnerCoro()
        {
            IsAlt = !IsAlt;
            // doing it like this fixes pressure plates w/ tutel swap bc tutel forms have their own colliders
            // otherwise the previous collider will exit triggers before the new one re-enters them
            // which makes pressure plates un-press for a physics frame (or sometimes several 🙃)
            // also did you know that "OnTriggerEnter2D" gets sent late by a physics frame
            // yay unity i love unity
            GameObject toEnable = IsAlt ? altForm : regularForm;
            GameObject toDisable = IsAlt ? regularForm : altForm;
            toEnable.SetActive(true);
            yield return new WaitForFixedUpdate();
            toDisable.SetActive(false);
            _swapping = false;
            OnSwap();
        }

        protected abstract bool CanSwap(bool toAlt);
        protected abstract void OnSwap();
    }
}
