using System.Collections.Generic;
using SchizoQuest.Characters;

namespace SchizoQuest.Game
{
    public interface IOnFormSwap
    {
        void OnFormSwap(PlayerType playerType, bool isAlt);
    }

    public static class OnFormSwapRegistry
    {
        private static readonly HashSet<IOnFormSwap> _registry = new();

        public static void Register(IOnFormSwap onFormSwap)
        {
            _registry.Add(onFormSwap);
        }

        public static void Unregister(IOnFormSwap onFormSwap)
        {
            _registry.Remove(onFormSwap);
        }

        public static void OnFormSwap(PlayerType playerType, bool isAlt)
        {
            foreach (IOnFormSwap onFormSwap in _registry)
            {
                onFormSwap.OnFormSwap(playerType, isAlt);
            }
        }
    }
}