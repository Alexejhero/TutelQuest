using SchizoQuest.Characters.Vedal;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class TutelDashAdditionalHint : MonoBehaviour
    {
        [SerializeField] private TutelForm tutelForm;
        [SerializeField] private GameObject vedalHint;
        [SerializeField] private GameObject tutelHint;

        private void OnChange(bool isTutel, bool isDashing)
        {
            vedalHint.SetActive(!isTutel);
            tutelHint.SetActive(isTutel && !isDashing);
        }

        private void Awake()
        {
            tutelForm.OnChange += OnChange;
            OnChange(tutelForm.IsAlt, tutelForm.IsDashing);
        }

        private void OnDestroy()
        {
            tutelForm.OnChange -= OnChange;
        }
    }
}
