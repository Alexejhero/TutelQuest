using UnityEngine;

namespace Schizo.ParticleEffects.Smoke
{
    public class SmokePoof : MonoBehaviour
    {
        public void OnAnimDone()
        {
            Destroy(gameObject);
        }
    }
}