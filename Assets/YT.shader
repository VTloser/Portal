// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShowProjectedPosition" {
    Properties {
        // 定义需要在Unity Inspector中显示的属性
    }
    
    SubShader {
        // 定义顶点着色器
        Pass {
            CGPROGRAM
            #pragma vertex MyVertexShader
            #pragma fragment MyFragmentShader

            // 定义需要使用的包含系统变量的文件
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };
            
            struct v2f {
                float4 position : SV_POSITION;
                float4 worldPos : TEXCOORD0; // 存储顶点在世界坐标系中的位置
                float4 projectedPos : TEXCOORD1; // 存储顶点在经过裁剪空间变换后的位置
            };
            
            // 顶点着色器
            v2f MyVertexShader(appdata v) {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); // 将顶点从局部坐标系转换到世界坐标系
                o.projectedPos = UnityObjectToClipPos(v.vertex); // 应用投影矩阵将顶点变换到裁剪空间
                o.position = o.projectedPos; // 将顶点位置设为经过裁剪空间变换后的位置，直接绘制投影结果
                return o;
            }
            
            // 片段着色器
            fixed4 MyFragmentShader(v2f i) : SV_Target {
                // 此处可以对片段进行操作，不需要处理纹理等额外操作，直接输出颜色
                return fixed4(i.projectedPos.xyz, 1); // 输出顶点在经过裁剪空间变换后的位置
            }
            ENDCG
        }
    }
}