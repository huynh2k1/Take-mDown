Shader "Custom/CurvedWorld"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Curvature ("Curvature", Float) = 0.001
        _CurvatureSpeed ("Curvature Speed", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Curvature;
            float _CurvatureSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Lấy vị trí thế giới
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                // Tính toán độ cong
                float distance = worldPos.z;
                distance += _Time.y * _CurvatureSpeed; // Thêm chuyển động theo thời gian
                worldPos.y -= _Curvature * distance * distance;
                
                // Chuyển về clip space
                o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}