using UnityEngine;

namespace SchizoQuest.Particle_Effects.Smoke
{
    public class SmokePoof : MonoBehaviour
    {
        public void OnAnimDone()
        {
            Destroy(gameObject);
        }
    }
}