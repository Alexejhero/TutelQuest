using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SchizoQuest.Game.Mechanisms
{
    public class Toggleable : MonoBehaviour, IPointerClickHandler
    {
        public bool isOn;

        public virtual void Toggle()
        {
            isOn = !isOn;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            DevToggle();
        }

        private void DevToggle()
        {
            if (!Application.isEditor || !DevTools.Instance.ctrlClickTogglesMechanisms) return;
            // Ctrl + Click
            if (Keyboard.current?.ctrlKey.isPressed ?? false)
                Toggle();
        }
    }
}
