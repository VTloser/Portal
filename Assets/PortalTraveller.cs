using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 previousOffserFromPortal { get; set; }

    public virtual void TelePort(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        this.transform.position = pos;
        this.transform.rotation = rot;

        Debug.Log(pos);
    }

    public virtual void EnterPortalThreshold()
    { 
    
    }
    public virtual void ExitPortalThreshold()
    {

    }

}
