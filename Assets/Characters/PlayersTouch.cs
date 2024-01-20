using System.Collections;
using SchizoQuest.Characters.Neuro;
using SchizoQuest.Characters.Vedal;
using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Helpers;
using SchizoQuest.VFX.Transition;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SchizoQuest.Characters
{
    public class PlayersTouch : Trigger<Player>
    {
        protected override void OnEnter(Player target)
        {
            StartCoroutine(CoOnEnter());
            return;

            IEnumerator CoOnEnter()
            {
                TutelForm tutel = target.GetComponentInChildren<TutelForm>() ?? GetComponentInParent<Player>().GetComponentInChildren<TutelForm>();
                EvilForm evil = GetComponentInParent<Player>().GetComponentInChildren<EvilForm>() ?? target.GetComponentInChildren<EvilForm>();

                float swapTime = 0;
                if (tutel.IsAlt)
                {
                    tutel.StartCoroutine(tutel.PerformSwap());
                    swapTime = 0.5f;
                }
                if (evil.IsAlt)
                {
                    evil.StartCoroutine(evil.PerformSwap());
                    swapTime = 1;
                }

                MonoSingleton<CharacterSwitcher>.Instance.availablePlayers.ForEach(p =>
                {
                    p.rb.simulated = false;
                    p.rb.velocity = Vector2.zero;
                    p.controller.enabled = false;
                    p.dying = true;
                    p.winning = true;
                });

                yield return new WaitForSeconds(swapTime);

                EffectsManager.Instance.PlayEffect(EffectsManager.Effects.gameFinish, 2f);
                yield return new WaitForSeconds(1.8f);
                SceneManager.LoadScene("End");
            }
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
