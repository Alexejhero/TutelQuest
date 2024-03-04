using System.Collections;
using SchizoQuest.Audio;
using SchizoQuest.Characters.Neuro;
using SchizoQuest.Characters.Vedal;
using SchizoQuest.End;
using SchizoQuest.Game.Mechanisms;
using SchizoQuest.Menu;
using SchizoQuest.Menu.PauseMenu;
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
                TutelForm tutel = CharacterSwitcher.Instance.availablePlayers[0].GetComponentInChildren<TutelForm>();
                EvilForm evil = CharacterSwitcher.Instance.availablePlayers[1].GetComponentInChildren<EvilForm>();
                PauseMenuBehaviour.Instance.Close();
                PauseMenuBehaviour.Instance.canToggle = false;

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

                CharacterSwitcher.Instance.availablePlayers.ForEach(p =>
                {
                    p.rb.simulated = false;
                    p.rb.velocity = Vector2.zero;
                    p.controller.enabled = false;
                    p.dying = true;
                    p.ForceFacingFront = true;
                    if (p.inventory.item is { itemType: Game.Items.ItemType.Rum })
                    {
                        EndManager.GotRum = true;
                    }
                });

                yield return new WaitForSeconds(swapTime);

                EffectsManager.Instance.PlayEffect(EffectsManager.Effects.gameFinish, 2f);
                yield return new WaitForSeconds(1.8f);
                
                AudioSystem.PauseSfx(true);
                MainMenu.startStage = MainMenu.StartStage.Ending;
                SceneManager.LoadScene(0);
            }
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
