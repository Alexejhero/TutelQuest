using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest
{
    public class MovingBlockParticles : MonoBehaviour
    {
        public ParticleSystem pSystem;
        public Rigidbody2D blockRb;
        public float velocityKillZone = 0.3f;
        public float particleRateMultiplier = 8f;
        public float camShakeMultiplier = 0.025f;

        [Min(1f)]
        public float camShakeMaxPlayerDist = 50f;

        private void Update()
        {
            float velmag = blockRb.velocity.magnitude;
            if (velmag > velocityKillZone)
            {
                Vector2 rnd = Random.insideUnitCircle;
                float playerDist = 1f - Mathf.InverseLerp(0, camShakeMaxPlayerDist, Vector2.Distance(Player.ActivePlayer.transform.position, transform.position));
                Camera.main.transform.position += new Vector3(rnd.x, rnd.y) * velmag * playerDist * camShakeMultiplier;
            }
            ParticleSystem.EmissionModule emm = pSystem.emission;
            emm.rateOverTimeMultiplier = velmag * particleRateMultiplier;
        }
    }
}
