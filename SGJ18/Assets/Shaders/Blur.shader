Shader "Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Kernel("Kernel", Vector) = (1,1,1,1)
		_XDir("XDir", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float4 _Kernel;
			float _XDir;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 dir = float2(_XDir, 1 - _XDir);
				fixed4 col = tex2D(_MainTex, i.uv - 3 * _MainTex_TexelSize * dir) * _Kernel.x
					+ tex2D(_MainTex, i.uv - 2 * _MainTex_TexelSize * dir) * _Kernel.y
					+ tex2D(_MainTex, i.uv - 1 * _MainTex_TexelSize * dir) * _Kernel.z
					+ tex2D(_MainTex, i.uv - 0 * _MainTex_TexelSize * dir) * _Kernel.w
					+ tex2D(_MainTex, i.uv + 1 * _MainTex_TexelSize * dir) * _Kernel.z
					+ tex2D(_MainTex, i.uv + 2 * _MainTex_TexelSize * dir) * _Kernel.y
					+ tex2D(_MainTex, i.uv + 3 * _MainTex_TexelSize * dir) * _Kernel.x;
				

				return col;
			}
			ENDCG
		}
	}
}
