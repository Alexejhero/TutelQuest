using UnityEngine;

namespace SchizoQuest.VFX.Particles.Smoke
{
    public class SmokePoof : MonoBehaviour
    {
        public void OnAnimDone()
        {
            Destroy(gameObject);
        }
    }
}