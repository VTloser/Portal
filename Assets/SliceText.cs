using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceText : MonoBehaviour
{
    public GameObject graphic;
    Material[] materials;

    void Start()
    {
        materials = GetMaterials(graphic);
    }

    private Material[] GetMaterials(GameObject graphic)
    {
        var renderers = graphic.GetComponentsInChildren<MeshRenderer>();
        var matList = new List<Material>();
        foreach (var renderer in renderers) 
        {
            matList.Add(renderer.material);
        }
        return matList.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetVector("sliceCentre", this.transform.position);
            materials[i].SetVector("sliceNormal", this.transform.up); 
        }
    }
}
