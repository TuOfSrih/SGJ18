Shader "Unlit/Combine"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightingTex("LightingTex", 2D) = "white" {}
		_Normals("Normals", 2D) = "white" {}
		_Ambient("Ambient", Float) = 0.95
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
			
			#include "UnityCG.cginc"

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

			sampler2D _MainTex;
			sampler2D _Normals;
			sampler2D _LightingTex;
			float4 _MainTex_ST;
			float _Ambient;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 lighting = tex2D(_LightingTex, i.uv);
				float intensity = lighting.z;
				return lighting * col;
			}


			ENDCG
		}
	}
}
