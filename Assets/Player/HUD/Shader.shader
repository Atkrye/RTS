Shader "Custom/Shader" {
    Properties {

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)

        _Outline ("Outline Width", Float) = 0.00005

    }

    Pass {

        Name "Outline"

        Tags { "LightMode" = "Always"}

        Lighting Off

        Cull Front

            CGPROGRAM

            #pragma vertex vert

            #pragma fragment frag

           

            #include "UnityCG.cginc"

               

            struct v2f

            {

                float4 pos : SV_POSITION;

            };

           

            float _Outline;

           

            v2f vert(appdata_base v) {

                v2f o;

                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

                float3 norm = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);

                norm.x *= UNITY_MATRIX_P[0][0];

                norm.y *= UNITY_MATRIX_P[1][1];

                o.pos.xy += norm.xy * o.pos.z * _Outline;

                return o;

            }

           

            float4 _OutlineColor;

           

            float4 frag(v2f i) :COLOR {

                return float4(_OutlineColor.rgb,0);

            }

        ENDCG
