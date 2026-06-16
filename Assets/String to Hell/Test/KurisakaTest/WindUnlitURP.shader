Shader "Custom/UnlitTransparentFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _FadeSize ("Fade Size", Range(0,0.5)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;   // ← UVスクロールに必要
            float4 _Color;
            float _FadeSize;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // ★ UVスクロール対応（mainTextureOffset が反映される）
                float2 uv = i.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                // ★ Tint × Texture
                fixed4 col = tex2D(_MainTex, uv) * _Color;

                // ★ 端フェード（Quad の四角さを消す）
                float2 centered = i.uv - 0.5;
                float dist = length(centered);
                float fade = smoothstep(0.5 - _FadeSize, 0.5, dist);
                col.a *= (1.0 - fade);

                return col;
            }
            ENDCG
        }
    }
}
