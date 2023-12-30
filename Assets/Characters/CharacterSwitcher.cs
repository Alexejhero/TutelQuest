using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class CharacterSwitcher : MonoBehaviour
    {
        public List<Player> availablePlayers;
        private Player _currentPlayer;
        private int _currentIndex;
        [NonSerialized]
        public StudioEventEmitter _music;
        public float switchCooldown;
        public float switchDelay;
        private float _nextSwitchTime;

        private void Awake()
        {
            _music = Camera.main!.GetComponent<StudioEventEmitter>();
        }

        private void Start()
        {
            SwitchTo(_currentIndex);
        }

        public void OnSwitchCharacter()
        {
            if (Time.time < _nextSwitchTime) return;

            _currentPlayer.enabled = false;
            _currentIndex++;
            _currentIndex %= availablePlayers.Count;
            SwitchTo(_currentIndex);
        }

        private void SwitchTo(int index)
        {
            _currentPlayer = availablePlayers[index];
            _currentPlayer.enabled = true;
            _nextSwitchTime = Time.time + switchCooldown;
            if (_music)
                _music.SetParameter("Character", _currentPlayer.playerType.ValueIndex());
        }
    }
}
