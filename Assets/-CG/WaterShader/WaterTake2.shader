Shader "Unlit/WaterTake2"
{
    Properties
    {
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
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend One OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float dist2Cam : TEXCOORD2;
            };

            sampler2D _Noise1, _Noise2;
            float3 _Move1, _Move2;
            float _Scale1, _Scale2;
            float4 _DeepColor, _ShallowColor;
            float _DistThresh1, _DistThresh2, _DistMult1, _DistMult2;

            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.dist2Cam = distance(_WorldSpaceCameraPos, o.worldPos);
                o.uv = v.uv;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col;

                if(i.dist2Cam > _DistThresh2){
                    _Scale1 /= _DistMult2;
                    _Move1 /= _DistMult2;
                    _Scale2 /= _DistMult2;
                    _Move2 /= _DistMult2;
                }else if(i.dist2Cam > _DistThresh1){
                    _Scale1 /= _DistMult1;
                    _Move1 /= _DistMult1;
                    _Scale2 /= _DistMult1;
                    _Move2 /= _DistMult1;
                }


                float2 topDownPorj = i.worldPos.xz;
                topDownPorj = (topDownPorj - 0.5) * _Scale1 + 0.5;
                topDownPorj += _Time.x * _Move1;
                col = tex2D(_Noise1, topDownPorj)/2;

                topDownPorj = i.worldPos.xz;
                topDownPorj = (topDownPorj - 0.5) * _Scale2 + 0.5;
                
                // return i.dist2Cam;
                // distance(cameraworldspacedistance, vertexworldspaceposition);
                topDownPorj += _Time.x * _Move2;
                col += tex2D(_Noise2, topDownPorj)/2;

                col = lerp(_DeepColor, _ShallowColor, col);

                col.w = _Transparency;
                return col;
            }
            ENDHLSL
        }
    }
}
