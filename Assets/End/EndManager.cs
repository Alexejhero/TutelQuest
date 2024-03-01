using System.Collections;
using SchizoQuest.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace SchizoQuest.End
{
    public class EndManager : MonoBehaviour
    {
        public Graphic image;

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(1.75f);
            for (float t = 0; t < 4; t += Time.deltaTime)
            {
                image.color = new Color(1, 1, 1, t / 4);
                yield return null;
            }

            image.color = Color.white;

            yield return new WaitForSeconds(5.75F);

            AudioSystem.PauseSfx(false);
            Application.Quit();
        }
    }
}
