using SchizoQuest.Characters.Neuro;
using SchizoQuest.Game.Mechanisms;

namespace SchizoQuest.Characters
{
    public sealed class DisableEvilFormArea : Trigger<EvilForm>
    {
        protected override void OnEnter(EvilForm target)
        {
            target.enableSwitching = false;
        }

        protected override void OnExit(EvilForm target)
        {
            target.enableSwitching = true;
            Destroy(this);
        }
    }
}