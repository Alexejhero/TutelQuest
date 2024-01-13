using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Game
{
    public class Timescale : MonoSingleton<Timescale>
    {
        [Range(0.001f, 10)]
        public float timescale = 1f;
        public void Update()
        {
            Time.timeScale = timescale;
        }
    }
}