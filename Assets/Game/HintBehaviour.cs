using UnityEngine;

namespace SchizoQuest.Game
{
    public enum HintType
    {
        WASD,
        Space,
        VedalF,
        E,
        C,
        NeuroF,
        NeuroF_2,
        TutelDash
    }

    public class HintBehaviour : MonoBehaviour
    {
        public HintType myType;
    }
}
