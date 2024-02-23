using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace SchizoQuest.Input
{
    public sealed class SwappingInputPrompt : MonoBehaviour
    {
        public GameObject keyboard;
        public GameObject xbox;
        public GameObject ps;
        public GameObject @switch;

        private void OnEnable()
        {
            OnInputDeviceChange(default, InputUserChange.ControlSchemeChanged, null);
            InputUser.onChange += OnInputDeviceChange;
        }

        private void OnDisable()
        {
            InputUser.onChange -= OnInputDeviceChange;
        }

        private void OnInputDeviceChange(InputUser _user, InputUserChange change, InputDevice _device)
        {
            if (change != InputUserChange.ControlSchemeChanged) return;

            keyboard.SetActive(false);
            xbox.SetActive(false);
            ps.SetActive(false);
            @switch.SetActive(false);

            PlayerInput player = PlayerInput.GetPlayerByIndex(0);
            if (string.IsNullOrWhiteSpace(player.currentControlScheme)) return;

            string controlScheme = player.currentControlScheme.ToLower();

            if (controlScheme.Contains("keyboard"))
            {
                keyboard.SetActive(true);
            }
            else if (controlScheme.Contains("xbox"))
            {
                xbox.SetActive(true);
            }
            else if (controlScheme.Contains("ps") || controlScheme.Contains("playstation") || controlScheme.Contains("play station"))
            {
                ps.SetActive(true);
            }
            else if (controlScheme.Contains("switch") || controlScheme.Contains("nintendo"))
            {
                @switch.SetActive(true);
            }
        }
    }
}
