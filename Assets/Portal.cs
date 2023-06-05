using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    //检查物体可见
    bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }


    // 仅仅在Player Camera 在渲染的时候调用
    public void Render()
    {
        //如果看不到LinkedPortal.screen ，取消后续渲染提升性能
        if (!VisibleFromCamera(LinkedPortal.screen, playerCam))
        {
            //测试用
            //var testTexture = new Texture2D(1, 1);
            //testTexture.SetPixel(0, 0, Color.red);
            //testTexture.Apply();
            //LinkedPortal.screen.material.SetTexture("_MainTex", testTexture);
            return;
        }
        //LinkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        screen.enabled = false;  //渲染之前隐藏屏幕
        CreateViewTexture();  //创建ViewTexture

        //设置位置变换
        var m = transform.localToWorldMatrix * LinkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        //相机进行渲染
        portalCam.Render();

        //开启屏幕
        screen.enabled = true;
    }

    private void Update()
    {
        ProtoectScreenFromClipping();
        SetNearClipPlane();
        foreach (var traveller in trackedTravellers)
        {
            UpdateSliceParams(traveller);
        }
    }



    private void LateUpdate()
    {
        HandleTraveellers();
    }


    public void HandleTraveellers()
    {

        for (int i = 0; i < trackedTravellers.Count; i++)
        {
            PortalTraveller traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;
            Matrix4x4 m = LinkedPortal.transform.localToWorldMatrix * this.transform.worldToLocalMatrix * traveller.transform.localToWorldMatrix;

            Vector3 offsetFromPortal = travellerT.position - this.transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, this.transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffserFromPortal, this.transform.forward));

            if (portalSide != portalSideOld)
            {
                var positionOld = travellerT.position;
                var rotOld = travellerT.rotation;
                traveller.TelePort(this.transform, LinkedPortal.transform, m.GetColumn(3), m.rotation);

                traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);

                //不能依赖与OntriggerEnxit去调用下一帧
                LinkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                i--;
            }
            else
            {
                traveller.graphicsClone.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);
                traveller.previousOffserFromPortal = offsetFromPortal;
            }               
        }
    }





    [SerializeField]
    List<PortalTraveller> trackedTravellers = new List<PortalTraveller>();

    void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if (!trackedTravellers.Contains(traveller))
        {
            traveller.EnterPortalThreshold();
            traveller.previousOffserFromPortal = traveller.transform.position - this.transform.position;

            trackedTravellers.Add(traveller);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1111);
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller)
            OnTravellerEnterPortal(traveller);

    }

    private void OnTriggerExit(Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller && trackedTravellers.Contains(traveller))
        {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }


    //设置传送门厚度 防止摄像机夹住
    void ProtoectScreenFromClipping()
    {
        //相机的近剪裁平面要尽可能的小
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;

        Transform ScreenT = screen.transform;
        bool CamFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - playerCam.transform.position) > 0;
        ScreenT.localScale = new Vector3(ScreenT.localScale.x, ScreenT.localScale.y, dstToNearClipPlaneCorner);
        ScreenT.localPosition = Vector3.forward * dstToNearClipPlaneCorner * ((CamFacingSameDirAsPortal) ? 0.5f : -0.5f);

    }

    //使用自定义投影矩阵使得近剪裁平面平行与Portal 
    void SetNearClipPlane()
    {
        Transform clipPlane = this.transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, this.transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;

        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x,camSpaceNormal.y,camSpaceNormal.z,camSpaceDst);

        //基于新平面更新投影
        //计算玩家摄像头的矩阵，这样玩家摄像头的设置fov等就会被使用。

        portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }


    void UpdateSliceParams(PortalTraveller traveller)
    {
        //计算切片法线
        int side = SideOfPortal(traveller.transform.position);
        Vector3 sliceNormal = this.transform.forward *- side;
        Vector3 cloneSliceNormal = LinkedPortal.transform.forward * side;

        //计算切片中心
        Vector3 slicePos = this.transform.position;
        Vector3 cloneSlicePos = LinkedPortal.transform.position;

        //应用参数
        for (int i = 0; i < traveller.OriginalMaterials.Length; i++)
        {
            traveller.OriginalMaterials[i].SetVector("sliceCentre", slicePos);
            traveller.OriginalMaterials[i].SetVector("sliceNormal", sliceNormal);

            traveller.CloneMaterials[i].SetVector("sliceCentre", cloneSlicePos);
            traveller.CloneMaterials[i].SetVector("sliceNormal", cloneSliceNormal);
        }

    }





    int SideOfPortal(Vector3 pos)
    {
        return System.Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));
    }

    bool SameSideOfPortal(Vector3 posA, Vector3 posB)
    {
        return SideOfPortal(posA) == SideOfPortal(posB);
    }

    Vector3 portalCamPos
    {
        get
        {
            return portalCam.transform.position;
        }
    }

}
