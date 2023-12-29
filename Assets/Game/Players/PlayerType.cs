using System;

namespace SchizoQuest.Game.Players
{
    [Flags]
    public enum PlayerType
    {
        Vedal = 0x01,
        Neuro = 0x02,
        Evil = 0x04,
    }

    public static class PlayerTypeExtensions
    {
        //public static int ValueIndex(this PlayerType type) => Array.IndexOf(Enum.GetValues(typeof(PlayerType)), type);
        public static int ValueIndex(this PlayerType type)
            => type switch
            {
                PlayerType.Vedal => 0,
                PlayerType.Neuro => 1,
                PlayerType.Evil => 2,
                _ => -1
            };
    }
}
