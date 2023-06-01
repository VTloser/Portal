using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal LinkedPortal;

    public MeshRenderer screen;

    [SerializeField] Camera playerCam;

    [SerializeField] Camera portalCam;

    RenderTexture viewTexture;

    void Start()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();


        portalCam.enabled = false;
    }

    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

            portalCam.targetTexture = viewTexture;

            LinkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    // 仅仅在Player Camera 在渲染的时候调用
    public void Render()
    {
        //screen.enabled = false;

        CreateViewTexture();

        var m = transform.localToWorldMatrix * LinkedPortal.transform.localToWorldMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        //Render the Camrea
        portalCam.Render();

        portalCam.enabled = true;

    }
    
}
