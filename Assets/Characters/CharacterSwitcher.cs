using System;
using System.Collections.Generic;
using FMODUnity;
using SchizoQuest.Audio;
using SchizoQuest.Game;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class CharacterSwitcher : MonoBehaviour
    {
        public float GlobalTransformCooldown { get; set; } = 1;

        public List<Player> availablePlayers;
        private Player _currentPlayer;
        private int _currentIndex;
        [NonSerialized]
        public StudioEventEmitter _music;
        public bool enableSwitching = true;

        private bool _hintHidden;

        private void Awake()
        {
            _music = Camera.main!.GetComponent<StudioEventEmitter>();
        }

        private void Start()
        {
            SwitchTo(_currentIndex);
        }

        private void Update()
        {
            GlobalTransformCooldown -= Time.deltaTime;
            AudioSystem.UpdateSfxMuteWhileSwitchingCharacters(_currentPlayer);
        }

        public void OnSwitchCharacter()
        {
            if (GlobalTransformCooldown > 0) return;
            if (!enableSwitching) return;

            if (!_hintHidden)
            {
                _hintHidden = true;
                _currentPlayer.SendMessage("HideHint", HintType.C, SendMessageOptions.DontRequireReceiver);
            }

            _currentPlayer.enabled = false;
            _currentIndex++;
            _currentIndex %= availablePlayers.Count;
            SwitchTo(_currentIndex);
        }

        private void SwitchTo(int index)
        {
            _currentPlayer = availablePlayers[index];
            _currentPlayer.enabled = true;
            GlobalTransformCooldown = 0.75f;
            if (_music)
                _music.SetParameter("Character", _currentPlayer.playerType.ValueIndex());
        }
    }
}
