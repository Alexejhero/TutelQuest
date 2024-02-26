using System;
using SchizoQuest.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace SchizoQuest.Game
{
    public class TrackEvilPhase : MonoBehaviour, IOnFormSwap
    {
        private enum NeuroPhaseTarget { GetFromLayer, Neuro, Evil }
        public UnityEvent<bool> calls;
        [SerializeField, Tooltip("Which form sends true to the called events")]
        private NeuroPhaseTarget target;

        private void Start()
        {
            if (target == NeuroPhaseTarget.GetFromLayer)
            {
                target = GetTargetFromLayer();
                if (target == default)
                    Debug.LogWarning("\"Get From Layer\" target with incorrect layer - needs a Neuro or Evil layer", this);
            }
            CallEvent(PlayerType.Neuro);
        }

        private void OnValidate()
        {
            if (target == NeuroPhaseTarget.GetFromLayer && GetTargetFromLayer() == default)
                Debug.LogError("With \"Get From Layer\" target, layer must be a Neuro/Evil layer");
        }

        private NeuroPhaseTarget GetTargetFromLayer()
        {
            string layerName = LayerMask.LayerToName(gameObject.layer);
            if (layerName.Contains("Evil")) return NeuroPhaseTarget.Evil;
            if (layerName.Contains("Neuro")) return NeuroPhaseTarget.Neuro;
            return default;
        }

        private void OnEnable() => OnFormSwapRegistry.Register(this);

        private void OnDisable() => OnFormSwapRegistry.Unregister(this);

        public void OnFormSwap(PlayerType playerType, bool isAlt)
        {
            CallEvent(playerType);
        }

        private void CallEvent(PlayerType plr)
        {
            switch (plr) { 
                case PlayerType.Vedal: return;
                case PlayerType.Neuro: calls.Invoke(target == NeuroPhaseTarget.Neuro); break;
                case PlayerType.Evil: calls.Invoke(target == NeuroPhaseTarget.Evil); break;
            }
        }
    }
}