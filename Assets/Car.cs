using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PortalTraveller
{
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(-Vector3.forward * Time.deltaTime * 3);
    }
}
