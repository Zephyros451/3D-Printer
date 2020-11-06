using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] layers;
    [SerializeField] private BoxCollider layerBase;

    private bool lastLayer = false;
    private GameObject currentLayer;

    private int currentLayerIndex = 0;
    public int CurrentLayerIndex => currentLayerIndex;

    private void Awake()
    {
        currentLayer = layers[currentLayerIndex];
    }


    public void NextLayer()
    {
        if (lastLayer) return;

        transform.Translate(new Vector3(0f, 0.1f, 0f));

        currentLayer.SetActive(false);
        currentLayer = layers[++currentLayerIndex];
        currentLayer.SetActive(true);

        if (currentLayerIndex == layers.Length-1)
        {
            lastLayer = true;
        }
    }
}
