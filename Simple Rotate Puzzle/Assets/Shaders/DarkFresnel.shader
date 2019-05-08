Shader "Unlit/DarkFresnel"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_FresnelPower("Fresnel Power", Float) = 2
		_DarkenAmount("Darken Amount", Float) = 0.4
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

				float fresnel : FRESNEL;
            };

			float4 _Color;
			float _FresnelPower;
			float _DarkenAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float fresnelAmount = 1 - saturate(dot(v.normal, viewDir));
				fresnelAmount = pow(fresnelAmount, _FresnelPower);
				o.fresnel = fresnelAmount;

				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float brightness = 1 - i.fresnel * _DarkenAmount;
                fixed4 col = fixed4(_Color.rgb * brightness, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
