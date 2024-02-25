using System.Collections;
using UnityEngine;
using SchizoQuest.Characters;
using SchizoQuest.Game.Mechanisms;

namespace SchizoQuest
{
    [RequireComponent(typeof(Collider2D))]
    public class TutelDashHint : Trigger<Player>
    {
        public SpriteRenderer[] hintRenderers;
        public AnimationCurve curve;
        public float hintDuration = 0.8f;
        public float hintInterval = 0.4f;

        private IEnumerator PlayRoutine(SpriteRenderer ren, float startTime)
        {
            yield return new WaitUntil(() => Time.time >= startTime);

            for (float t = -hintDuration; t < hintDuration; t += Time.deltaTime)
            {
                ren.material.SetFloat("_GlowTexBlend", curve.Evaluate(1 - Mathf.Abs(t / hintDuration)));
                yield return null;
            }
            ren.material.SetFloat("_GlowTexBlend", 0f);
        }

        public void PlayHints()
        {
            for (int i = 0; i < hintRenderers.Length; i++)
            {
                StartCoroutine(PlayRoutine(hintRenderers[i], Time.time + hintInterval * i));
            }
        }

        protected override void OnEnter(Player target)
        {
            PlayHints();
        }

        protected override void OnExit(Player target)
        {
        }
    }
}
