Shader "Custom/CustomeCreateNormalShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float4 getGrayScale(float3 color)
            {
                return color.r * 0.2126 + color.g * 0.7152 + color.b * 0.0722;
            }

            float3 getNormalByGray(float2 uv)
            {
                float deltaScale = 0.5;
                float hightScale = 0.01;
                float2 deltaU = float2(_MainTex_TexelSize.x * deltaScale, 0);
                float uh1 = getGrayScale(tex2D(_MainTex, uv + deltaU));
                float uh2 = getGrayScale(tex2D(_MainTex, uv - deltaU));
                float3 tangentU = float3(deltaU.x, 0, hightScale * (uh1 - uh2));

                float2 deltaV = float2(0, _MainTex_TexelSize.y * deltaScale);
                float vh1 = getGrayScale(tex2D(_MainTex, uv + deltaV));
                float vh2 = getGrayScale(tex2D(_MainTex, uv - deltaV));
                float3 tangentV = float3(0, deltaV.y, hightScale * (vh1 - vh2));

                return normalize(cross(tangentU, tangentV));
            }

            v2f vert(a2v t)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(t.vertex);
                o.uv = TRANSFORM_TEX(t.texcoord, _MainTex);

                return o;
            }

            fixed4 frag(v2f v) : SV_Target
            {
                float3 normal = getNormalByGray(v.uv);
                return fixed4(normal * 0.5 + 0.5, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
