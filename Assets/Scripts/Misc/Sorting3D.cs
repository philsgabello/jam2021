using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting3D : MonoBehaviour
{

    public int sortingLayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mRenderer = GetComponent<MeshRenderer>();
        mRenderer.sortingLayerName = "Meshes";
        mRenderer.sortingOrder = sortingLayer;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
