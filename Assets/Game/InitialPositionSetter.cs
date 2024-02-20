using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class InitialPositionSetter : MonoBehaviour
    {
        public Vector3 position;
        public bool onlyInBuild;

        private void Awake()
        {
            if (onlyInBuild && Application.isEditor) return;
            transform.position = position;
        }
    }
}
