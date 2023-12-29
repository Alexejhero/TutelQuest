using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        for (var i = 0; i < _renderers.Length; i++)
        {
            var renderer = _renderers[i];
            renderer.sortingLayerName = "InFrontOfPlayer";
        }
    }

    public void OnDropped()
    {
        GetComponent<Collider2D>().enabled = true;
        for (var i = 0; i < _renderers.Length; i++)
        {
            var renderer = _renderers[i];
            renderer.sortingLayerID = _savedLayers[i];
        }
    }
}