using SchizoQuest.Characters;
using SchizoQuest.Characters.Vedal;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class TutelSpinCollider : MonoBehaviour
    {
        private Collider2D _collider;
        private TutelForm _tutelForm;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (!_tutelForm && CharacterSwitcher.Instance.CurrentPlayer.GetComponentInChildren<TutelForm>() is { } tutelForm)
            {
                _tutelForm = tutelForm;
            }

            _collider.enabled = _tutelForm.IsDashing;
        }
    }
}
