Shader "Custom/Fog"
{
    Properties
    {
        [Header(Textures and color)]
        [Space]
        _MainTex ("Fog texture", 2D) = "white" {}
        _Color ("Color", color) = (1., 1., 1., 1.)
        [Space(10)]
        _SecTex ("Secondary texture", 2D) = "white" {}
        _SecColor ("Color", color) = (1., 1., 1., 1.)
        [Space(10)]
        _SecOffsetX ("Secondary offset x", float) = 1
        _SecOffsetY ("Secondary offset y", float) = 1
        [Space(20)]
 
        [Header(Behaviour)]
        [Space]
        _ScrollDirX ("Scroll along X", Range(-1., 1.)) = 1.
        _ScrollDirY ("Scroll along Y", Range(-1., 1.)) = 1.
        _Speed ("Speed", float) = 1.
        _SecSpeed ("Secondary Speed", float) = 1.
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        // Cull Off
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 vertCol : COLOR0;
                float2 uv : TEXCOORD0;
                float2 secUv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };
 
            sampler2D _MainTex;
            sampler2D _SecTex;
            float4 _MainTex_ST;
            float4 _SecTex_ST;
 
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.secUv = TRANSFORM_TEX(v.texcoord, _SecTex);
                o.uv2 = v.texcoord;
                o.vertCol = v.color;
                return o;
            }
 
            float _Speed;
            float _SecSpeed;
            fixed _SecOffsetX;
            fixed _SecOffsetY;
            fixed4 _Color;
            fixed4 _SecColor;
            fixed _ScrollDirX;
            fixed _ScrollDirY;
 
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv + fixed2(_ScrollDirX, _ScrollDirY) * _Speed * _Time.x;
                fixed4 col = tex2D(_MainTex, uv) * _Color * i.vertCol;
                i.secUv.x *= _SecOffsetX;
                i.secUv.y *= _SecOffsetY;
                uv = i.secUv + fixed2(_ScrollDirX, _ScrollDirY) * _SecSpeed * _Time.x;
                col += tex2D(_SecTex, uv) * _SecColor * i.vertCol;

                return col;
            }
            ENDCG
        }
    }
}