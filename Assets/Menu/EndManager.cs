using System.Collections;
using SchizoQuest.Menu;
using SchizoQuest.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SchizoQuest.End
{
    public class EndManager : MenuStage
    {
        public static bool GotRum { get; set; }
        public Graphic image;
        [Min(0)]
        public float fadeInTime = 2;
        [Min(0)]
        public float fadeOutTime = 2;

        private IEnumerator Start()
        {
            // the whole event duration is ~1:08
            // at 1:10 (70s) - past the loop - there's a transition marker to the ending part
            MainMenu.Instance.music.EventInstance.setTimelinePosition(70000);
            yield return new WaitForSeconds(0.5F);

            yield return CommonCoroutines.DoOverTime(fadeInTime,
                t => image.color = new Color(1, 1, 1, t / fadeInTime));

            image.color = Color.white;

            yield return new WaitForSeconds(8F - fadeOutTime);
            
            yield return CommonCoroutines.DoOverTime(fadeOutTime,
                t => image.color = new Color(1, 1, 1, 1 - t / fadeOutTime));
            
            Done();
        }

        // lets us play the ending from the main menu any time
        private void OnEnable() => StartCoroutine(Start());
        private void OnDisable() => StopAllCoroutines();
    }
}
