Shader "Custom/CRTFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.12
        _ScanlineSpeed ("Scanline Speed", Float) = 2.0
        _Curvature ("Curvature", Range(0, 0.2)) = 0.02
        _RGBOffset ("RGB Offset", Range(0, 0.01)) = 0.0015
        _BloomStrength ("Bloom Strength", Range(0, 1)) = 0.08
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float _ScanlineIntensity;
            float _ScanlineSpeed;
            float _Curvature;
            float _RGBOffset;
            float _BloomStrength;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                float2 centeredUV = uv * 2.0 - 1.0;

                centeredUV *= 1.0 + (_Curvature * centeredUV.xy * centeredUV.xy);

                uv = centeredUV * 0.5 + 0.5;

                float r = tex2D(_MainTex, uv + float2(_RGBOffset, 0)).r;

                float g = tex2D(_MainTex, uv).g;

                float b = tex2D(_MainTex, uv - float2(_RGBOffset, 0)).b;

                fixed4 col = fixed4(r, g, b, 1.0);

                fixed4 bloom = 0;

                bloom += tex2D(_MainTex, uv + float2(0.002, 0));
                bloom += tex2D(_MainTex, uv - float2(0.002, 0));

                bloom += tex2D(_MainTex, uv + float2(0, 0.002));
                bloom += tex2D(_MainTex, uv - float2(0, 0.002));

                bloom *= 0.25;

                col.rgb += bloom.rgb * _BloomStrength;

                float scanline =
                    sin((uv.y * 240.0) + (_Time.y * _ScanlineSpeed));

                scanline = scanline * 0.5 + 0.5;

                col.rgb -= scanline * _ScanlineIntensity;

                return col;
            }
            ENDCG
        }
    }
}