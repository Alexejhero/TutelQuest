using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public class CharacterSwitcher : MonoBehaviour
    {
        public List<Player> availablePlayers;
        private Player _currentPlayer;
        private int _currentIndex;
        public StudioEventEmitter music;

        private void Start()
        {
            SwitchTo(_currentIndex);
        }

        public void OnSwitchCharacter()
        {
            _currentPlayer.enabled = false;
            _currentIndex++;
            _currentIndex %= availablePlayers.Count;
            SwitchTo(_currentIndex);
        }

        private void SwitchTo(int index)
        {
            _currentPlayer = availablePlayers[index];
            _currentPlayer.enabled = true;
            if (music)
                music.SetParameter("Character", _currentPlayer.playerType.ValueIndex());
        }
    }
}
