Shader "Custom/WaterSurface"
{
    Properties{
        _Transparency ("Transparency", Range(0,1)) = 0
        [Space(10)]
        [Header(First Noise Map)]
        [Space]
        _Noise1 ("Noise 1", 2D) = "white" {}
        _Scale1 ("Scale 1", Range(0,1)) = 1
        _Move1 ("Movement 1", Vector) = (0,0,0)

        [Space(20)]
        [Header(Second Noise Map)]
        [Space]
        _Noise2 ("Noise 2", 2D) = "white" {}
        _Scale2 ("Scale 2", Range(0,1)) = 1
        _Move2 ("Movement 2", Vector) = (0,0,0)

        _DeepColor ("Deep", Color) = (0, 0, 0, 1)
        _ShallowColor ("Shallow", Color) = (0, 0, 0, 1)

        [Space(10)]
        [Header(Distance Thresholds)]
        _DistThresh1 ("Medium Threshold", float) = 0
        _DistMult1 ("Medium Multiplier", float) = 0
        _DistThresh2 ("Far Threshold", float) = 0
        _DistMult2 ("Medium Multiplier", float) = 0
    }

    SubShader{
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
        Blend One OneMinusSrcAlpha
        ZWrite Off
        LOD 200
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade vertex:vert glsl

        struct Input{
            float3 worldPos;
            float2 uv : TEXCOORD0;
        };





        float _Transparency;
        sampler2D _Noise1;
        float4 _DeepColor, _ShallowColor;
        float _Scale1;

        void vert(inout appdata_full v){
            float4 v0 = v.vertex;
            
            // float4 screenPos = ComputeScreenPos(UnityObjectToClipPos(v0.xyz));
            // float4 depth = tex2Dlod(_Noise1, (v0.xy, 0, 0));
            float2 worldPos = v.vertex.xz;
            worldPos += _Time;

            float4 tex = tex2Dlod (_Noise1, float4(v.texcoord.xy,0,0));
            v0.y += tex * 10;
            
            // v0.y += depth.b;
            v.vertex = v0;
        }

        void surf(Input IN, inout SurfaceOutput o){
            float2 worldPos = IN.worldPos.xz;
            worldPos = (worldPos - 0.5) * _Scale1 + 0.5;
            // o.worldPos.y = lerp(0, 1, tex2D(_Noise1, worldPos));

            // o.Albedo = lerp(_ShallowColor, _DeepColor, o.uv);
            o.Alpha = _DeepColor.w;
            // o.Albedo = _DeepColor;
            // o.Albedo = tex2D(_MainTex, IN.uv);
            o.Albedo = (lerp(_ShallowColor, _DeepColor, tex2D(_Noise1, worldPos)));
        }

        ENDCG
    }
    Fallback "Diffuse"
}
