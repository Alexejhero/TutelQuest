using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Game.Players;
using UnityEngine;

namespace SchizoQuest
{
    public class Checkpoint : Trigger<Player>
    {
        public GameObject available;
        public GameObject taken;

        protected override void OnEnter(Player target)
        {
            Activate(target);
        }

        protected override void OnExit(Player target)
        {
        }

        public void Activate(Player player)
        {
            player.checkpoint = transform.position;
            available.SetActive(false);
            taken.SetActive(true);
        }
    }
}
