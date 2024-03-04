using System;
using UnityEngine;

namespace SchizoQuest.Menu
{
    public abstract class MenuStage : MonoBehaviour
    {
        public MenuStage nextStage;
        public Action OnDone;
        protected void Done() => OnDone?.Invoke();
    }
}
