using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public List<Player> availablePlayers;
    private Player _currentPlayer;

    private InputActions _input;

    private void Awake()
    {
        _input = new InputActions();
    }

    private void Start()
    {
        _currentPlayer = availablePlayers[0];
        _currentPlayer.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentPlayer.enabled = false;
            int index = availablePlayers.IndexOf(_currentPlayer) + 1;
            if (index >= availablePlayers.Count) index = 0;
            _currentPlayer = availablePlayers[index];
            _currentPlayer.enabled = true;
        }
    }
}
