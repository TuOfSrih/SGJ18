// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Lighting"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightingPos("LightingPos", Vector) = (0,0,0,0)
		_MaxLength("MaxLength", Float) = 0
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
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 pos: POSITION1;
			};

			

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _LightingPos;
			float _MaxLength;
			float4 _MainTex_TexelSize;

			v2f vert(appdata v) {
				v2f o;
				o.pos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			/*float4 box(sampler2D tex, float2 uv, float4 size) {

				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y))
					+ tex2D(tex, uv + float2(-size.x, 0)) + tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0))
					+ tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) + tex2D(tex, uv + float2(size.x, -size.y))
					return c / 9;
			}*/

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = normalize(i.pos - _LightingPos) / 2 + .5;
				float distance = length(i.pos.xy - _LightingPos.xy);
				col.b =  1 - distance / _MaxLength;
				//return _LightingPos;
				//return float4(1, 0, 0, 1);
				return col;
			}
			ENDCG
		}
	}
}
