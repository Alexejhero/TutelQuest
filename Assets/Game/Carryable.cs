using System.Linq;
using UnityEngine;

namespace SchizoQuest.Game
{
    public sealed class Carryable : MonoBehaviour
    {
        public Transform plug;
        private SpriteRenderer[] _renderers;
        private int[] _savedLayers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _savedLayers = _renderers
                .Select(rend => rend.sortingLayerID)
                .ToArray();
        }

        public void OnPickedUp()
        {
            GetComponent<Collider2D>().enabled = false;
            for (int i = 0; i < _renderers.Length; i++)
            {
                SpriteRenderer rend = _renderers[i];
                rend.sortingLayerName = "InFrontOfPlayer";
            }
        }

        public void OnDropped()
        {
            GetComponent<Collider2D>().enabled = true;
            for (int i = 0; i < _renderers.Length; i++)
            {
                SpriteRenderer rend = _renderers[i];
                rend.sortingLayerID = _savedLayers[i];
            }
        }
    }
}
