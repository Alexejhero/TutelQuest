using System.Collections.Generic;
using SchizoQuest.Helpers;

namespace SchizoQuest.Menu
{
    public sealed class MainMenu : MonoSingleton<MainMenu>
    {
        public static bool skipNextIntro;
        public List<MenuStage> stages;
        internal MenuStage currentStage;

        private void Start()
        {
            SetStage(stages[skipNextIntro ? 2 : 0]);
            skipNextIntro = false;
        }

        public void SetStage(MenuStage stage)
        {
            if (currentStage) Deactivate(currentStage);
            Activate(stage);
        }

        public void NextStage()
        {
            int i = stages.IndexOf(currentStage);
            if (i + 1 >= stages.Count) return;
            SetStage(stages[i+1]);
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
    }
}
