Shader "Custom/ScratchOffDesaturateEffect"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _MaskTex("Scratch Mask", 2D) = "black" {}
        _Saturation("Saturation", Range(0,1)) = 1.0 // 1 = fully saturated, 0 = fully desaturated
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            sampler2D _MaskTex;
            float _Saturation;

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Sample the main texture color.
                fixed4 col = tex2D(_MainTex, IN.uv);

                // Sample the scratch mask (using the red channel as our blend factor).
                float mask = tex2D(_MaskTex, IN.uv).r;

                // Compute the grayscale value of the pixel using weighted luminance.
                float gray = dot(col.rgb, float3(0.3, 0.59, 0.11));
                
                // Create a desaturated version of the color.
                // _Saturation of 1.0 retains full color, while 0.0 results in complete grayscale.
                fixed3 desatColor = lerp(float3(gray, gray, gray), col.rgb, _Saturation);
                
                // Blend between the fully saturated (original) and the desaturated version based on the mask.
                // When mask = 0, the pixel stays as the original color.
                // When mask = 1, the pixel becomes desaturated.
                col.rgb = lerp(col.rgb, desatColor, mask);

                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
