using UnityEngine;

namespace SchizoQuest.Characters.Vedal
{
    public sealed class TutelFormObjectMover : MonoBehaviour
    {
        [SerializeField]
        private TutelForm tutelForm;

        public float VedalLocalYPos;
        public float TutelLocalYPos;

        private void Awake()
        {
            tutelForm.OnChange += OnChange;
        }

        private void OnChange(bool isAlt, bool _isDashing)
        {
            Transform myTransform = transform;
            Vector3 localPos = myTransform.localPosition;
            myTransform.localPosition = new Vector3(localPos.x, !isAlt ? VedalLocalYPos : TutelLocalYPos, localPos.z);
        }
    }
}
