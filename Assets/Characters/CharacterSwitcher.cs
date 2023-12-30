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
        public StudioEventEmitter music;
        public float switchCooldown;
        public float switchDelay;
        private float _nextSwitchTime;

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
            if (music)
                music.SetParameter("Character", _currentPlayer.playerType.ValueIndex());
        }
    }
}
