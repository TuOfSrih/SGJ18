Shader "Normals"
{
	Properties
	{
		_Normals("Normals", 2D) = "bump" {}
		_PlayerPos("PlayerPos", Vector) = (0,0,0,0)
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

			sampler2D _Normals;
			float4 _Normals_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Normals);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed3 col = UnpackNormal(tex2D(_Normals, i.uv)) / 2 + 0.5;
				//col = (col - 0.5) * 2;
				//col.z = atan2(col.y, col.x);
				
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(col, 1);
			}
			ENDCG
		}
	}
}
