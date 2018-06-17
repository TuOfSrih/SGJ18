Shader "Hidden/FXBloom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_blurSize ("BlurSize", Range(1,30)) = 3
		_XDir ("Xdir", Range(0,1)) = 0
		_intensity ("Intensity", Range(0,111)) = 2
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
			#pragma target 3.5

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
			float _blurSize = 3;
			float _XDir;
			float _intensity;

			fixed4 frag (v2f i) : SV_Target
			{

				float4 summ = 0;
				float s = 0;
				float2 dir = float2(_XDir, 1 - _XDir) * _MainTex_TexelSize.xy;

				float4 maxi = 0;

				//_blurSize = min(_blurSize,10);
				//[unroll(20)]
				for (int x = -_blurSize; x <= _blurSize; x++) {
					summ += tex2Dlod(_MainTex, float4(i.uv + dir * x,0,0)) * (1 - (abs(x) / _blurSize));
					maxi = max(maxi, tex2D(_MainTex, i.uv + dir * x) * (1 - (abs(x) / _blurSize)));
					s += 1 - (abs(x) / _blurSize);
				}

				//_blurSize = min(_blurSize, 10);
				//int x = 0;
				//[unroll(20)]
				//for (int j = 0; j<= _blurSize * 2; j++) {
				//	x = j - _blurSize;
				//	x*=4;
				//	summ += tex2Dlod(_MainTex, float4(i.uv + dir * x, 0, 0)) * (1 - (abs(x) / _blurSize / 4));
				//	maxi = max(maxi, tex2D(_MainTex, i.uv + dir * x) * (1 - (abs(x) / _blurSize) / 4));
				//	s += 1 - (abs(x) / _blurSize / 4);
				//
				//	x ++;
				//	summ += tex2Dlod(_MainTex, float4(i.uv + dir * x, 0, 0)) * (1 - (abs(x) / _blurSize / 4));
				//	maxi = max(maxi, tex2D(_MainTex, i.uv + dir * x) * (1 - (abs(x) / _blurSize / 4)));
				//	s += 1 - (abs(x) / _blurSize / 4);
				//
				//	x++;
				//	summ += tex2Dlod(_MainTex, float4(i.uv + dir * x, 0, 0)) * (1 - (abs(x) / _blurSize / 4));
				//	maxi = max(maxi, tex2D(_MainTex, i.uv + dir * x) * (1 - (abs(x) / _blurSize / 4)));
				//	s += 1 - (abs(x) / _blurSize / 4);
				//
				//	x++;
				//	summ += tex2Dlod(_MainTex, float4(i.uv + dir * x, 0, 0)) * (1 - (abs(x) / _blurSize / 4));
				//	maxi = max(maxi, tex2D(_MainTex, i.uv + dir * x) * (1 - (abs(x) / _blurSize / 4)));
				//	s += 1 - (abs(x) / _blurSize / 4);
				//}

				fixed4 col = maxi / 2 + summ / (s / _intensity) / 2;
				//fixed4 col = summ / (s / _intensity);//tex2D(_MainTex, i.uv);
//				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
