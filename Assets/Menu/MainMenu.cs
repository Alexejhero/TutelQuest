using System.Collections.Generic;
using FMODUnity;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public sealed class MainMenu : MonoSingleton<MainMenu>
    {
        public enum StartStage
        {
            Beginning = 0,   // funny
            TitleScreen = 2, // funnier
            Ending = 3,      // funniest
        }
        public static StartStage startStage;
        [SerializeField]
        private List<MenuStage> stages;
        internal MenuStage currentStage;
        public StudioEventEmitter music;
        public MenuStage playMusicAfter;
        private int _playMusicAfter;

        private void Start()
        {
            _playMusicAfter = stages.IndexOf(playMusicAfter);
            SetStage((int)startStage);
            startStage = default;
        }

        public void SetStage(int i)
        {
            if (currentStage) Deactivate(currentStage);
            i %= stages.Count;
            if (i > _playMusicAfter && !music.IsPlaying()) music.Play();
            Activate(stages[i]);
        }

        public void NextStage()
        {
            if (currentStage)
            {
                int customNext = stages.IndexOf(currentStage.nextStage);
                if (customNext >= 0)
                {
                    SetStage(customNext);
                    return;
                }
            }
            int i = stages.IndexOf(currentStage);
            SetStage(i+1);
        }

        private void Deactivate(MenuStage stage)
        {
            stage.gameObject.SetActive(false);
            stage.OnDone -= NextStage;
        }

        private void Activate(MenuStage stage)
        {
            stage.gameObject.SetActive(true);
            currentStage = stage;
            stage.OnDone += NextStage;
        }

        public void OnCancel()
        {
            if (!currentStage) return;
            currentStage.SendMessage(nameof(OnCancel), SendMessageOptions.DontRequireReceiver);
        }
    }
}
