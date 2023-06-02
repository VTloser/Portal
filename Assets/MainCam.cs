using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{

    Portal[] portals;

    private void Awake()
    {
        portals = FindObjectsOfType<Portal>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in portals)
        {
            item.Render();
        }
    }
}
