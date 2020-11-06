using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintLayer : MonoBehaviour
{
    public BoxCollider[] controlLines { get; private set; }
    public SphereCollider[] controlPoints { get; private set; }

    private void Awake()
    {
        controlLines = GetComponentsInChildren<BoxCollider>();
        controlPoints = GetComponentsInChildren<SphereCollider>();
    }
}
