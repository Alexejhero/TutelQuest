﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SchizoQuest.Audio;
using SchizoQuest.Game;
using SchizoQuest.Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace SchizoQuest.Characters
{
    public class CharacterSwitcher : MonoSingleton<CharacterSwitcher>
    {
        public float GlobalTransformCooldown { get; set; } = 1;

        public List<Player> availablePlayers;
        public Player CurrentPlayer { get; private set; }
        private int _currentIndex;
        [NonSerialized]
        public BackgroundMusic music;
        public bool enableSwitching = true;

        private bool _hintHidden;

        private void Start()
        {
            music = BackgroundMusic.Instance;
            SwitchTo(_currentIndex);
        }

        private void Update()
        {
            GlobalTransformCooldown -= Time.deltaTime;
        }

        [UsedImplicitly]
        public void OnSwitchCharacter()
        {
            if (GlobalTransformCooldown > 0) return;
            if (!enableSwitching) return;
            if (PauseMenuBehaviour.IsOpen) return;
            if (Player.ActivePlayer.dying) return;

            if (!_hintHidden)
            {
                _hintHidden = true;
                CurrentPlayer.SendMessage("HideHint", HintType.C, SendMessageOptions.DontRequireReceiver);
            }

            CurrentPlayer.enabled = false;
            _currentIndex++;
            _currentIndex %= availablePlayers.Count;
            SwitchTo(_currentIndex);
        }

        private void SwitchTo(int index)
        {
            CurrentPlayer = availablePlayers[index];
            CurrentPlayer.enabled = true;
            GlobalTransformCooldown = 0.75f;
            music.SetCharacter(CurrentPlayer.playerType);
            BroadcastMessage("OnSwitch", SendMessageOptions.DontRequireReceiver);
        }
    }
}
