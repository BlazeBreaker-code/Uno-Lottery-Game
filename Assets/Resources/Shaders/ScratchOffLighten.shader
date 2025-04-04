Shader "Custom/ScratchOffLighten"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Lighten("Lighten Amount", Range(0,1)) = 0.0
        _ScratchProgress("Scratch Progress", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Name "SRPDefaultUnlit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
            float _Lighten;
            float _ScratchProgress;

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.uv);

                // Use alpha of the sprite as the "mask"
                float alphaMask = col.a;

                // Diagonal wipe (top-left to bottom-right)
                float diagonal = IN.uv.x + IN.uv.y;
                float revealMask = step(diagonal, _ScratchProgress * 2.0);

                float finalMask = alphaMask * revealMask;

                // Lighten the color toward white
                fixed3 lightColor = lerp(col.rgb, float3(1, 1, 1), _Lighten);

                // Blend based on mask
                col.rgb = lerp(col.rgb, lightColor, finalMask);

                // Ensure the original alpha is preserved
                col.a = alphaMask;

                return col;
            }
            ENDCG
        }
    }
}
