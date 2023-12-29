using System.Collections.Generic;
using UnityEngine;

namespace SchizoQuest.Game.Players
{
    public class CharacterSwitcher : MonoBehaviour
    {
        public List<Player> availablePlayers;
        private Player _currentPlayer;

        private void Start()
        {
            _currentPlayer = availablePlayers[0];
            _currentPlayer.enabled = true;
        }

        public void OnSwitchCharacter()
        {
            _currentPlayer.enabled = false;
            int index = availablePlayers.IndexOf(_currentPlayer) + 1;
            if (index >= availablePlayers.Count) index = 0;
            _currentPlayer = availablePlayers[index];
            _currentPlayer.enabled = true;
        }
    }
}
