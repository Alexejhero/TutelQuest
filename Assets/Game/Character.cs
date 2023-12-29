using System;

namespace SchizoQuest.Game
{
    [Flags]
    public enum Character
    {
        Vedal = 0x01,
        Neuro = 0x02,
        Evil = 0x04,
    }
}