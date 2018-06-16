Shader "Transition"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Displacement("Displacement", 2D) = "black" {}
		_Magnitude("Magnitude", Range(0,0.2)) = 1
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
			sampler2D _Displacement;
			float _Magnitude;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 disp = tex2D(_Displacement, i.uv);
				disp = (disp * 2 - 1 ) * _Magnitude;
				fixed4 col = tex2D(_MainTex, i.uv + disp);
				// just invert the colors
				col.rgb = 1 - col.rgb;
				return col;
			}
			ENDCG
		}
	}
}
