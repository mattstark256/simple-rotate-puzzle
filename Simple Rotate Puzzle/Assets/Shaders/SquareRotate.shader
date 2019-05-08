Shader "Unlit/SquareRotate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_RotateAngle("Rotate Angle", Float) = 60
		_Color1("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Color3("Color 3", Color) = (1,1,1,1)
		_Color4("Color 4", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _RotateAngle;
			fixed4 _Color1;
			fixed4 _Color2;
			fixed4 _Color3;
			fixed4 _Color4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

				float2 vectorFromCentre = o.uv - float2(0.5, 0.5);

				float rotateAngleRadians = radians(_RotateAngle);
				float cosAngle = cos(rotateAngleRadians);
				float sinAngle = sin(rotateAngleRadians);
				float2x2 rotateMatix = float2x2(cosAngle, sinAngle, -sinAngle, cosAngle);
				vectorFromCentre = mul(vectorFromCentre, rotateMatix);

				o.uv2 = vectorFromCentre + float2(0.5, 0.5);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

				fixed4 col2 = (i.uv2.x > 0.5) ? (i.uv2.y > 0.5) ? _Color1 : _Color2 : (i.uv2.y > 0.5) ? _Color4 : _Color3;

                return fixed4(col2.rgb, col.a);
            }
            ENDCG
        }
    }
}
