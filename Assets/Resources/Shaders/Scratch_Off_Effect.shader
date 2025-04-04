Shader "Custom/ScratchOffEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _MaskTex ("Scratch Mask", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        
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
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            sampler2D _MaskTex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the sprite's texture color
                fixed4 col = tex2D(_MainTex, i.uv);
                // Sample the scratch mask (assumes black = unscratch, white = scratched)
                float mask = tex2D(_MaskTex, i.uv).r;
                // Multiply the sprite's alpha by (1 - mask) so that white (1) makes it transparent.
                col.a *= (1 - mask);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}