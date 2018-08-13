Shader "QIQIQI/ScreenMask" {
	Properties {
		_MainTex("Source RenderTexture", 2D) = "white"{}
		_MaskColor("Mask Color", color) = (0,0,0,0)
		_LerpValue("Lerp Value to Mask Color", float) = 0
	}
	SubShader {
		Cull Off
		ZTest Always
		ZWrite Off

		pass
		{
		CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert
#pragma fragment frag

			sampler2D _MainTex;
		fixed4 _MaskColor;
		float _LerpValue;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			return o;
		}

		fixed4 frag(v2f i) : SV_TARGET
		{
			fixed4 origin = tex2D(_MainTex, i.uv);
		fixed4 col = lerp(origin, _MaskColor, _LerpValue);
		return col;
		}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
