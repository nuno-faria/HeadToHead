﻿//Used to replace the color of the players shirts
Shader "Hidden/ColorSwap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Tags{ "Queue" = "Transparent" }
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _Color1;
			float4 _Color2;
			sampler2D _SwapTex;
			


			fixed4 frag(v2f i) : SV_Target{
				if (!tex2D(_MainTex, i.uv).a)
					discard;

				return tex2D(_SwapTex, float2(tex2D(_MainTex, i.uv).r, 0));
			}

			ENDCG
		}
	}
}
