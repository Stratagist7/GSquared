/*
 * Guide written by Benjamin Swee on YouTube
 */
Shader "Unlit/Vignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float _Radius, _Feather;
            float4 _TintColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 newUV = i.uv * 2 - 1;
                float circle = length(newUV);
                float mask = 1 - smoothstep(_Radius, _Radius + _Feather, circle);
                float invertMask = 1 - mask;

                float3 displayColor = col.rgb * mask;
                float3 vignetteColor = invertMask * _TintColor;
                
                return fixed4(displayColor + vignetteColor, 1);
            }
            ENDCG
        }
    }
}
