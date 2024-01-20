using JetBrains.Annotations;
using UnityEngine;

namespace SchizoQuest.VFX.Particles.Smoke
{
    public class SmokePoof : MonoBehaviour
    {
        [UsedImplicitly]
        public void OnAnimDone()
        {
            Destroy(gameObject);
        }
    }
}
