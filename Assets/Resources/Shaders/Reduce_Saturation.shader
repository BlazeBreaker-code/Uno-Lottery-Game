Shader "Custom/Reduce_Saturation"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Saturation("Saturation", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float     _Saturation;

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Sample the sprite texture
                fixed4 col = tex2D(_MainTex, IN.uv);

                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                col.rgb = lerp(float3(gray, gray, gray), col.rgb, _Saturation);

                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
