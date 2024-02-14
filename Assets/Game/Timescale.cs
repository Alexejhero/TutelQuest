using SchizoQuest.Helpers;
using UnityEngine;

namespace SchizoQuest.Game
{
    /// <summary>
    /// Singleton to control timescale from within the editor.<br/>
    /// For all other purposes, please use <see cref="Time.timeScale"/>.
    /// </summary>
    public class Timescale : MonoSingleton<Timescale>
    {
        [Range(0.001f, 10), SerializeField]
        internal float timescale = 1f;
        private float _last;

        private void Start()
        {
            _last = Time.timeScale;
        }
        public void Update()
        {
            // only changes real timescale if our timescale field was changed
            // if Unity's timescale property was changed, we adjust to that
            if (Time.timeScale != _last)
                timescale = _last;
            
            if (_last != timescale)
                Time.timeScale = timescale;
            _last = timescale;
        }
    }
}