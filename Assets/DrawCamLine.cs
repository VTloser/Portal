using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class DrawCamLine : MonoBehaviour
{
    Camera _camera;
    Transform _camTrans;
    float _farDistance;     //Զ�ӿھ���
    float _nearDistance;    //���ӿھ���


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
    /// ���ƽ�Զ���ӿ�
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
    /// ���ƽϽ����ӿ�
    /// </summary>
    void OnDrawNearView()
    {
        Vector3[] corners = GetCorners(_nearDistance);

        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.red);//����-����
        Debug.DrawLine(corners[1], corners[3], Color.red);//����-����
        Debug.DrawLine(corners[3], corners[2], Color.red);//����-����
        Debug.DrawLine(corners[2], corners[0], Color.red);//����-����
    }


    /// <summary>
    /// ���� camera ����׶ ����
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



    //��ȡ����ӿ��ĸ��ǵ�����
    //���� distance  �ӿھ���
    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];

        //fovΪ��ֱ��Ұ  ˮƽfovȡ�����ӿڵĿ�߱�  �Զ�Ϊ��λ


        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//һ��fov
        float aspect = _camera.aspect;//����ӿڿ�߱�

        float height = distance * Mathf.Tan(halfFOV);//distance����λ�ã�����ӿڸ߶ȵ�һ��
        float width = height * aspect;//����ӿڿ�ȵ�һ��

        //����
        corners[0] = _camTrans.position - (_camTrans.right * width);//������� - �ӿڿ��һ��
        corners[0] += _camTrans.up * height;//+�ӿڸߵ�һ��
        corners[0] += _camTrans.forward * distance;//+�ӿھ���

        // ����
        corners[1] = _camTrans.position + (_camTrans.right * width);//������� + �ӿڿ��һ��
        corners[1] += _camTrans.up * height;//+�ӿڸߵ�һ��
        corners[1] += _camTrans.forward * distance;//+�ӿھ���

        // ����
        corners[2] = _camTrans.position - (_camTrans.right * width);//������� - �ӿڿ��һ��
        corners[2] -= _camTrans.up * height;//-�ӿڸߵ�һ��
        corners[2] += _camTrans.forward * distance;//+�ӿھ���

        // ����
        corners[3] = _camTrans.position + (_camTrans.right * width);//������� + �ӿڿ��һ��
        corners[3] -= _camTrans.up * height;//-�ӿڸߵ�һ��
        corners[3] += _camTrans.forward * distance;//+�ӿھ���

        return corners;
    }





}
