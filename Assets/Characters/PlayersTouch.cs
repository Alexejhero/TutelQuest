using SchizoQuest.Game.Mechanisms;
using UnityEngine;

namespace SchizoQuest.Characters
{
    public class PlayersTouch : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            Debug.LogWarning("touched");
        }

        protected override void OnExit(Player target)
        {
        }
    }
}