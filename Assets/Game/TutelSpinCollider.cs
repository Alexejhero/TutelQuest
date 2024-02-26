using SchizoQuest.Characters;
using SchizoQuest.Characters.Vedal;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class TutelSpinCollider : MonoBehaviour
    {
        [SerializeField]
        private Collider2D targetCollider;
        private TutelForm _tutelForm;

        private void Update()
        {
            if (!_tutelForm && CharacterSwitcher.Instance.CurrentPlayer.GetComponentInChildren<TutelForm>() is { } tutelForm)
            {
                _tutelForm = tutelForm;
            }

            targetCollider.enabled = _tutelForm.IsDashing;
        }
    }
}
