using SchizoQuest.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SchizoQuest.Game.Mechanisms
{
    public sealed class DevTools : MonoSingleton<DevTools>
    {
        public bool ctrlClickTogglesMechanisms;
        private void Start()
        {
            if (!Debug.isDebugBuild) return;
            Camera.main.EnsureComponent<Physics2DRaycaster>();
        }
    }
}