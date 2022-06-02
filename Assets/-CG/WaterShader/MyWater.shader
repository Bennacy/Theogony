Shader "Custom/MyWater"{
    Properties{
        _NoiseTexA ("Noise Textrure A", 2D) = "white"{}
        _NoiseTexB ("Noise Textrure B", 2D) = "white"{}
        _DepthGradient ("Depth Gradient", 2D) = "white"{}
        _DeepColor ("Deep Color", Color) = (1, 1, 1, 1)
        _ShallowColor ("Shallow Color", Color) = (1, 1, 1, 1)
        
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white"{}
        _NoiseTex("Noise Texture", 2D) = "white"{}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0.5

        _height("Wave Height" , range(0,20)) = 1

        _waveFrequency1("First Wave Frequency" , range(0,20)) = 1
        _amplitude1("First Wave Amplitude" , range(-20,20)) = 1

        _waveFrequency2("Second Wave Frequency" , range(0,20)) = 1
        _amplitude2("Second Wave Amplitude" , range(-20,20)) = 1
    }
        SubShader{
            Pass{
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct VertexData {
                    float4 position : POSITION;
                    float3 normals : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct VertexToFragment {
                    float4 position : SV_POSITION;
                    float3 normals : NORMAL;
                    float2 uv : TEXCOORD0;
                    float2 uvNoise : TEXCOORD1;
                    float3 color : COLOR;
                };

                float4 _Color, _ShallowColor, _DeepColor;
                sampler2D _MainTex, _NoiseTex, _DepthGradient, _NoiseTexA;
                float4 _MainTex_ST, _NoiseTex_ST, _NoiseTexA_ST;
                float _DissolveThreshold;

                //custom
                float _height;

                float _waveFrequency1;
                float _amplitude1;

                float _waveFrequency2;
                float _amplitude2;

                VertexToFragment vert(VertexData v) {
                    VertexToFragment v2f;
                    //place to change constants
                    v.position.y += sin((_Time.y * _waveFrequency1) + (v.position.x * _amplitude1)) * _height;
                    // v.position.y += sin((_Time.y * _waveFrequency2) + (v.position.z * _amplitude2)) * _height;

                    float noisePattern = tex2D(_NoiseTexA, v.uv).x;
                    // v.position.y = lerp(0, 1, noisePattern);


                    v2f.position = UnityObjectToClipPos(v.position);
                    v2f.uv = v.position;

                    return v2f;
                }

                float4 frag(VertexToFragment v2f) : SV_TARGET{
                    // float4 color = lerp(float4(0, 0, 0, 0), tex2D(_GradientMap), v2f.position.y);

                    // float4 color = tex2D(_NoiseTexA, v2f.uv);
                    // float3 color = float3(tex2D(_GradientMap, v2f.position).x, tex2D(_GradientMap, v2f.position).y, tex2D(_GradientMap, v2f.position).z);
                    // float3 color = float3(0, (_GradientMap, float2(v2f.color.x, 0)).x, 0);
                    // float3 color = float3(v2f.uv.x, 0, v2f.uv.y);

                    // float4 noise = tex2D(_NoiseTex, v2f.uvNoise);
                    // clip(noise.rgb - _DissolveThreshold);

                    // float3 color = lerp(_ShallowColor, tex2D(_DepthGradient, float2(0,0)), ((v2f.uv.y / _height) + 1) / 2);
                    float4 color = lerp(_DeepColor, _ShallowColor, ((v2f.uv.y / _height) + 1) / 2);
                    float noisePattern = tex2D(_NoiseTexA, v2f.uv).x;
                    // float3 color = tex2D(_DepthGradient, float2(0,0));
                    // return color * _Color;
                    // return noisePattern;
                    return color;
                    // return lerp(_ShallowColor, _DeepColor, v2f.uv.y);
                }

                ENDHLSL
            }
        }
}
