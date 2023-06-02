Shader "Custom/Slice"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off
        CGPROGRAM
        //#pragma surface surf Standard fullforwardshadows
        #pragma surface surf Standard addshadow  //修改渲染方式为 addshadow 剔除丢弃像素的阴影

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float3 sliceCentre;
        float3 sliceNormal;


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
         //用于判断位于门的那一侧
            float sliceSide = dot(sliceNormal,IN.worldPos - sliceCentre);
            //将另一侧也就是负数侧的像素丢弃
            clip(-sliceSide);

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
