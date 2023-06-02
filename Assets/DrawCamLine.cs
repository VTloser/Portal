using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class DrawCamLine : MonoBehaviour
{
    Camera _camera;
    Transform _camTrans;
    float _farDistance;     //远视口距离
    float _nearDistance;    //近视口距离


    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _camTrans = _camera.transform;
        _farDistance = _camera.farClipPlane;
        _nearDistance = _camera.nearClipPlane;


        OnDrawFarView();
        OnDrawNearView();
        OnDrawConeOfCameraVision();

        OnDrawTouYingView();
    }

    //
    void OnDrawTouYingView()
    {
        Vector3[] Pos = new Vector3[] { _camTrans.transform.position + new Vector3(0.5f, 0.5f, 0.5f),
                                        _camTrans.transform.position + new Vector3(0.5f, 0.5f, -0.5f),
                                        _camTrans.transform.position + new Vector3(0.5f, -0.5f,- 0.5f),
                                        _camTrans.transform.position + new Vector3(0.5f, -0.5f, 0.5f),

                                        _camTrans.transform.position + new Vector3(-0.5f, 0.5f, 0.5f),
                                        _camTrans.transform.position + new Vector3(-0.5f, 0.5f, -0.5f),
                                        _camTrans.transform.position + new Vector3(-0.5f, -0.5f,- 0.5f),
                                        _camTrans.transform.position + new Vector3(-0.5f, -0.5f, 0.5f),};


        for (int i = 0; i < Pos.Length; i++)
        {
            for (int j = 0; j < Pos.Length; j++)
            {

                if (Vector3.Distance(Pos[i], Pos[j]) <= 1.01f)
                {
                    Debug.DrawLine(Pos[i], Pos[j], Color.black); // UpperLeft -> UpperRight
                }
            }
        }
    }




    /// <summary>
    /// 绘制较远的视口
    /// </summary>
    void OnDrawFarView()
    {
        Vector3[] corners = GetCorners(_farDistance);

        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.yellow); // UpperLeft -> UpperRight
        Debug.DrawLine(corners[1], corners[3], Color.yellow); // UpperRight -> LowerRight
        Debug.DrawLine(corners[3], corners[2], Color.yellow); // LowerRight -> LowerLeft
        Debug.DrawLine(corners[2], corners[0], Color.yellow); // LowerLeft -> UpperLeft

    }

    /// <summary>
    /// 绘制较近的视口
    /// </summary>
    void OnDrawNearView()
    {
        Vector3[] corners = GetCorners(_nearDistance);

        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.red);//左上-右上
        Debug.DrawLine(corners[1], corners[3], Color.red);//右上-右下
        Debug.DrawLine(corners[3], corners[2], Color.red);//右下-左下
        Debug.DrawLine(corners[2], corners[0], Color.red);//左下-左上
    }


    /// <summary>
    /// 绘制 camera 的视锥 边沿
    /// </summary>
    void OnDrawConeOfCameraVision()
    {
        Vector3[] _farcorners = GetCorners(_farDistance);
        Vector3[] _neracorners = GetCorners(_nearDistance);

        // for debugging
        Debug.DrawLine(_neracorners[1], _farcorners[1], Color.green); // UpperLeft -> UpperRight
        Debug.DrawLine(_neracorners[3], _farcorners[3], Color.green); // UpperRight -> LowerRight
        Debug.DrawLine(_neracorners[2], _farcorners[2], Color.green); // LowerRight -> LowerLeft
        Debug.DrawLine(_neracorners[0], _farcorners[0], Color.green); // LowerLeft -> UpperLeft
    }



    //获取相机视口四个角的坐标
    //参数 distance  视口距离
    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];

        //fov为垂直视野  水平fov取决于视口的宽高比  以度为单位


        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//一半fov
        float aspect = _camera.aspect;//相机视口宽高比

        float height = distance * Mathf.Tan(halfFOV);//distance距离位置，相机视口高度的一半
        float width = height * aspect;//相机视口宽度的一半

        //左上
        corners[0] = _camTrans.position - (_camTrans.right * width);//相机坐标 - 视口宽的一半
        corners[0] += _camTrans.up * height;//+视口高的一半
        corners[0] += _camTrans.forward * distance;//+视口距离

        // 右上
        corners[1] = _camTrans.position + (_camTrans.right * width);//相机坐标 + 视口宽的一半
        corners[1] += _camTrans.up * height;//+视口高的一半
        corners[1] += _camTrans.forward * distance;//+视口距离

        // 左下
        corners[2] = _camTrans.position - (_camTrans.right * width);//相机坐标 - 视口宽的一半
        corners[2] -= _camTrans.up * height;//-视口高的一半
        corners[2] += _camTrans.forward * distance;//+视口距离

        // 右下
        corners[3] = _camTrans.position + (_camTrans.right * width);//相机坐标 + 视口宽的一半
        corners[3] -= _camTrans.up * height;//-视口高的一半
        corners[3] += _camTrans.forward * distance;//+视口距离

        return corners;
    }





}
