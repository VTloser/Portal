// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShowProjectedPosition" {
    Properties {
        // ������Ҫ��Unity Inspector����ʾ������
    }
    
    SubShader {
        // ���嶥����ɫ��
        Pass {
            CGPROGRAM
            #pragma vertex MyVertexShader
            #pragma fragment MyFragmentShader

            // ������Ҫʹ�õİ���ϵͳ�������ļ�
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };
            
            struct v2f {
                float4 position : SV_POSITION;
                float4 worldPos : TEXCOORD0; // �洢��������������ϵ�е�λ��
                float4 projectedPos : TEXCOORD1; // �洢�����ھ����ü��ռ�任���λ��
            };
            
            // ������ɫ��
            v2f MyVertexShader(appdata v) {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); // ������Ӿֲ�����ϵת������������ϵ
                o.projectedPos = UnityObjectToClipPos(v.vertex); // Ӧ��ͶӰ���󽫶���任���ü��ռ�
                o.position = o.projectedPos; // ������λ����Ϊ�����ü��ռ�任���λ�ã�ֱ�ӻ���ͶӰ���
                return o;
            }
            
            // Ƭ����ɫ��
            fixed4 MyFragmentShader(v2f i) : SV_Target {
                // �˴����Զ�Ƭ�ν��в���������Ҫ��������ȶ��������ֱ�������ɫ
                return fixed4(i.projectedPos.xyz, 1); // ��������ھ����ü��ռ�任���λ��
            }
            ENDCG
        }
    }
}