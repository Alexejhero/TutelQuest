using System.Collections;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public abstract class CharacterAltForm : MonoBehaviour
    {
        private bool isAlt;

        public CharacterSwitcher characterSwitcher;
        public Player player;
        public GameObject regularForm;
        public GameObject altForm;
        public GameObject smokePoof;

        public float swapDelay = 0.100f;

        public IEnumerator OnSwapForm()
        {
            if (!player.enabled) yield break;
            if (characterSwitcher.GlobalTransformCooldown > 0) yield break;
            if (!CanSwap(!isAlt))
                yield break;
            characterSwitcher.GlobalTransformCooldown = 0.75f;

            Instantiate(smokePoof, transform);
            yield return new WaitForSeconds(swapDelay);

            isAlt = !isAlt;
            altForm.SetActive(isAlt);
            regularForm.SetActive(!isAlt);
            OnSwap(isAlt);
        }
        protected abstract bool CanSwap(bool toAlt);
        protected abstract void OnSwap(bool isAlt);
    }
}
