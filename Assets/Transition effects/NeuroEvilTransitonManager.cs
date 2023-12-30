using System.Collections.Generic;
using UnityEngine;

namespace SchizoQuest
{
    public class NeuroEvilTransitionManager : MonoBehaviour
    {
        private static readonly int _phaseID = Shader.PropertyToID("_Phase");

        public Material neuroToEvilTransitonMaterial;
        public Renderer rr;

        [Range(0f, 1f)]
        public float phase;

        private float _phase;
        public AnimationCurve phaseCurve;

        [Range(0f, 1f)]
        public float middleStripSize = 0.5f;

        [Range(0f, 1f)]
        public float centerGradientStrength = 0f;

        public Color centerColorNeuro = Color.blue;
        public Color middleColorNeruo = Color.cyan;
        public Color outsideColorNeuro = Color.black;

        public Color centerColorEvil = Color.green;
        public Color middleColorEvil = Color.red;
        public Color outsideColorEvil = Color.black;

        private Material _evilToNeuroTransitionMaterial;
        private List<Material> _materials;

        public bool isEvil = false;

        private float _playEndTime;
        private float _playStartTime;
        private float _duration;

        private void Awake()
        {
            _materials = new List<Material>
            {
                neuroToEvilTransitonMaterial
            };
            neuroToEvilTransitonMaterial.SetFloat("_MiddleThickness", middleStripSize);
            neuroToEvilTransitonMaterial.SetFloat("_GradCenter", centerGradientStrength);
            neuroToEvilTransitonMaterial.SetColor("_CenterColor", centerColorNeuro);
            neuroToEvilTransitonMaterial.SetColor("_MiddleColor", middleColorNeruo);
            neuroToEvilTransitonMaterial.SetColor("_OutsideColor", outsideColorNeuro);

            _evilToNeuroTransitionMaterial = new Material(neuroToEvilTransitonMaterial);
            _evilToNeuroTransitionMaterial.SetFloat("_MiddleThickness", middleStripSize);
            _evilToNeuroTransitionMaterial.SetFloat("_GradCenter", centerGradientStrength);
            _evilToNeuroTransitionMaterial.SetColor("_CenterColor", centerColorEvil);
            _evilToNeuroTransitionMaterial.SetColor("_MiddleColor", middleColorEvil);
            _evilToNeuroTransitionMaterial.SetColor("_OutsideColor", outsideColorEvil);
        }

        public void Play(float duration = 1f)
        {
            _playStartTime = Time.time;
            _duration = Mathf.Max(0.05f, duration);
            _playEndTime = _playStartTime + _duration;
            rr.enabled = true;
        }

        private void Update()
        {
            if (Time.time > _playEndTime) { phase = 0; rr.enabled = false; }
            else
            {
                phase = (Time.time - _playStartTime) / _duration;
            }
            _materials[0] = isEvil ? _evilToNeuroTransitionMaterial : neuroToEvilTransitonMaterial;
            _phase = phaseCurve.Evaluate(phase);
            _materials[0].SetFloat(_phaseID, _phase);
            rr.SetMaterials(_materials);
        }
    }
}
