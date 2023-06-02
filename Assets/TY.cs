using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TY : MonoBehaviour
{
    public Camera Camera;

    
    void Awake()
    {

        //获取到相机的投影矩阵
        Matrix4x4 m = Camera.nonJitteredProjectionMatrix;

        Vector4 Pos = new Vector4(this.transform.position.x,
                                  this.transform.position.y, 
                                  this.transform.position.z, 1);


        Pos = m * Pos;
        Pos /= Pos.w;
        this.transform.position = Pos;



        //Vector4 s = new Vector4(this.transform.localScale.x, 
        //                        this.transform.localScale.y, 
        //                        this.transform.localScale.z, 1);

        //s = m * s;
        //this.transform.localScale = s;



    }
}
