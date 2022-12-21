Shader "Custom/ExplosionDisplacment"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplacmentMap ("Displacment map", 2D) = "white" {}
        _Power ("Power", Float) = 0.025
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma fragment frag
            #pragma vertex vert

            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _DisplacmentMap;
            float _Power;
            float4 _MainTex_ST;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 disVector = tex2D(_DisplacmentMap, i.uv);
                fixed2 disorted = i.uv + _Power * disVector.xy;

                return tex2D(_MainTex, disorted);
            }
            ENDCG
        }
    }
}
