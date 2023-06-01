using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC : MonoBehaviour
{

    public Transform Player_Door;
    public Transform PlayCam;


    public Transform Portal_Door;
    public Transform Portal_Cam;


    public MeshRenderer Player_scene;

    public RenderTexture viewTexture;

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 m = Player_Door.worldToLocalMatrix * Portal_Door.localToWorldMatrix * PlayCam.localToWorldMatrix;
        Portal_Cam.SetPositionAndRotation(m.GetColumn(3), m.rotation);


        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
            //viewTexture = new RenderTexture(Screen.width, Screen.height, 0);


        }


        Portal_Cam.GetComponent<Camera>().targetTexture = viewTexture;

        Player_scene.material.SetTexture("_MainTex", viewTexture);


    }
}
