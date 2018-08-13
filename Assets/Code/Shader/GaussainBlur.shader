/*
5x5 gaussain blur kernel from Wiki:

0.00079		0.00656		0.01330		0.00656		0.00079
0.00656		0.05472		0.11098		0.05472		0.00656
0.01330		0.11098		0.22508		0.11098		0.01330
0.00656		0.05472		0.11098		0.05472		0.00656
0.00079		0.00656		0.01330		0.00656		0.00079
------------------------------------------------------------line of sum
0.02800		0.23354		0.47364		

*/


Shader "QIQIQI/GaussainBlur" 
{
	Properties
	{
		_MainTex("Source RenderTexture", 2D) = "white"{}
		_BlurSpread("Blur Spread Distance", float) = 1
	}
		SubShader
		{
		//block #include
		CGINCLUDE
			#include "UnityCG.cginc"

			sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		float _BlurSpread;

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv[5] : TEXCOORD0;
		};

		v2f vert_horizontal(appdata_img v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv[0] = v.texcoord + _MainTex_TexelSize.xy * float2(-2, 0) * _BlurSpread;
			o.uv[1] = v.texcoord + _MainTex_TexelSize.xy * float2(-1, 0) * _BlurSpread;
			o.uv[2] = v.texcoord + _MainTex_TexelSize.xy * float2(0, 0) * _BlurSpread;
			o.uv[3] = v.texcoord + _MainTex_TexelSize.xy * float2(1, 0) * _BlurSpread;
			o.uv[4] = v.texcoord + _MainTex_TexelSize.xy * float2(2, 0) * _BlurSpread;
			return o;
		}

		v2f vert_vertical(appdata_img v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv[0] = v.texcoord + _MainTex_TexelSize.xy * float2(0, 2) * _BlurSpread;
			o.uv[1] = v.texcoord + _MainTex_TexelSize.xy * float2(0, 1) * _BlurSpread;
			o.uv[2] = v.texcoord + _MainTex_TexelSize.xy * float2(0, 0) * _BlurSpread;
			o.uv[3] = v.texcoord + _MainTex_TexelSize.xy * float2(0, -1) * _BlurSpread;
			o.uv[4] = v.texcoord + _MainTex_TexelSize.xy * float2(0, -2) * _BlurSpread;
			return o;
		}

		fixed4 frag(v2f i) : SV_TARGET
		{
			float kernelsum[3] = {0.02800, 0.23354, 0.47364};
		fixed4 colorsum = fixed4(0, 0, 0, 0);
		
		colorsum += tex2D(_MainTex, i.uv[2]) * kernelsum[2]; //row 2
		for (int r = 0; r < 2; r++)	//row 04 and row 13
		{
			colorsum += tex2D(_MainTex, i.uv[r]) * kernelsum[r];
			colorsum += tex2D(_MainTex, i.uv[4 - r]) * kernelsum[r];
		}

			return colorsum;
		}
		ENDCG
			//end of CG INCLUDE block

			Cull Off
			ZTest Always
			ZWrite Off

			pass
		{
			NAME "GAUSSIAN_BLUR_HORIZONTAL"

				CGPROGRAM
#pragma vertex vert_horizontal
#pragma fragment frag
				ENDCG
		}

		pass 
		{
			NAME "GAUSSIAN_BLUR_VERTICAL"
				
				CGPROGRAM
#pragma vertex vert_vertical
#pragma fragment frag
				ENDCG
		}

		}

	Fallback OFF
}