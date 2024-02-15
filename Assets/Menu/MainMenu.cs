using System.Collections.Generic;
using FMODUnity;
using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public sealed class MainMenu : MonoSingleton<MainMenu>
    {
        public static bool skipNextIntro;
        [SerializeField]
        private List<MenuStage> stages;
        internal MenuStage currentStage;
        public StudioEventEmitter music;
        public MenuStage playMusicAfter;
        private int _playMusicAfter;

        private void Start()
        {
            _playMusicAfter = stages.IndexOf(playMusicAfter);
            SetStage(skipNextIntro ? 2 : 0);
            skipNextIntro = false;
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
