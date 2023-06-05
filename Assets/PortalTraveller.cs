using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 previousOffserFromPortal { get; set; }

    public GameObject graphicsObject;

    public GameObject graphicsClone { get; set; }


    public Material[] OriginalMaterials { get; set; }
    public Material[] CloneMaterials { get; set; }



    public virtual void TelePort(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        this.transform.position = pos;
        this.transform.rotation = rot;

        Debug.Log(pos);
    }

    public virtual void EnterPortalThreshold()
    {
        if (graphicsClone == null)
        {
            graphicsClone = Instantiate(graphicsObject);

            graphicsClone.transform.parent = graphicsObject.transform.parent;
            graphicsClone.transform.localScale = graphicsObject.transform.localScale;
            OriginalMaterials = GetMaterials(graphicsObject);
            CloneMaterials = GetMaterials(graphicsClone);
        }
        else
        {
            graphicsClone.SetActive(true);
        }
    }
    public virtual void ExitPortalThreshold()
    {
        graphicsClone.SetActive(false);
        // Disable slicing
        for (int i = 0; i < OriginalMaterials.Length; i++)
        {
            OriginalMaterials[i].SetVector("sliceNormal", Vector3.zero);
        }
    }

    Material[] GetMaterials(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<MeshRenderer>();
        var matList = new List<Material>();
        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.materials)
            {
                matList.Add(mat);
            }
        }
        return matList.ToArray();
    }




}
