using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace SchizoQuest.VFX.Particles
{
    [RequireComponent(typeof(SortingGroup))]
    public class ParticleSystemManager : MonoBehaviour
    {
        [SerializeField]
        private SortingGroup sortingGroup;
        private ParticleSystem[] _systems;
        public int SortingLayer { set { sortingGroup.sortingLayerID = value; } get {return sortingGroup.sortingLayerID; } }

        private void Awake()
        {
            _systems = GetComponentsInChildren<ParticleSystem>();
        }

        public bool IsPlaying()
        {
            return _systems.ToList().TrueForAll(x => x.isPlaying);
        }

        public void StopAll(ParticleSystemStopBehavior how)
        {
            foreach (ParticleSystem system in _systems)
            {
                system.Stop(true, how);
            }
        }

        public void SetExternalForces(bool active)
        {
            foreach (ParticleSystem system in _systems)
            {
                ParticleSystem.ExternalForcesModule efm = system.externalForces;
                efm.enabled = active;
            }
        }
    }
}
