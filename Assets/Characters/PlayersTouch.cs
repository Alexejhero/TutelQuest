using System.Collections;
using SchizoQuest.Game.Mechanisms;
using SchizoQuest.VFX.Transition;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SchizoQuest.Characters
{
    public class PlayersTouch : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            EffectsManager.Instance.PlayEffect(EffectsManager.Effects.gameFinish, 2f);
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(1.8f);
                SceneManager.LoadScene("End");
            }
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
