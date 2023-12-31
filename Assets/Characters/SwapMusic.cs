using FMODUnity;
using SchizoQuest.Game;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class SwapMusic : MonoBehaviour
    {
        public Collider2D collider_;
        public CharacterSwitcher switcher;
        public StudioEventEmitter track1;
        public StudioEventEmitter track2;

        private bool _isFading;
        private float _fade1 = 100f;
        private float _fade2 = 0;

        public void FixedUpdate()
        {
            foreach (var player in switcher.availablePlayers)
            {
                if (!Physics2D.IsTouching(player.controller.collider_, collider_))
                    return;
            }

            track2.Play();
            _isFading = true;
        }

        public void Update()
        {
            if (!_isFading) return;
            _fade1 -= Time.deltaTime * 100;
            _fade2 += Time.deltaTime * 100;
            track1.SetParameter("Fade", _fade1);
            track2.SetParameter("Fade", _fade2);
            if (_fade2 >= 100)
                Destroy(this);
        }
    }
}