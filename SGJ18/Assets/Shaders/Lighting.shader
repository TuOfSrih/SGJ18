

Shader "Lighting"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightingPos("LightingPos", Vector) = (0,0,0,0)
		_MaxLength("MaxLength", Float) = 0
		_MinDistance("MinDistance", Float) = 0
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		

		Pass
		{
			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
			float _MinDistance;
			float4 _MainTex_TexelSize;
			float4 _Color;

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
			float interpolate(float x) {
				return 4* (sqrt(x) - x);
			}
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = //normalize(i.pos - _LightingPos) / 2 + .5;
				float distance = length(i.pos.xy - _LightingPos.xy);
				distance =  (distance - _MinDistance) / (_MaxLength - _MinDistance);
				distance = interpolate(distance);
				//return tex2D(_MainTex, i.uv);
				return  distance * _Color;
			}
			ENDCG
		}
	}
}
