using UnityEngine;

namespace SchizoQuest.Game.Mechanisms
{
    public class Toggleable : MonoBehaviour
    {
        public bool isOn;

        public virtual void Toggle()
        {
            isOn = !isOn;
        }
    }
}