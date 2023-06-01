using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoControl : MonoBehaviour
{
    public GameObject A;
    public GameObject A_Cam;

    public GameObject B;
    public GameObject B_Cam;


    private void Update()
    {
        Matrix4x4 matrix = B.transform.localToWorldMatrix * A.transform.worldToLocalMatrix * A_Cam.transform.localToWorldMatrix;
        B_Cam.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);




    }
}
