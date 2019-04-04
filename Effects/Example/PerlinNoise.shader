Shader "Unlit/PerlinNoise"
{
	Properties
	{
		_Step("Step",Vector) = (1,1,1,1)
		_Speed("Speed", float) = 1
		_Coeff("Coeff", float) = 1
	}

		CGINCLUDE

#include "UnityCustomRenderTexture.cginc"
#include "Assets/UtilPack/External/Noise/HLSL/ClassicNoise3D.hlsl"
		float2 _Step;
		float _Speed;
		float _Coeff;
		float4 frag(v2f_customrendertexture i) : SV_Target
		{
			float2 uv = i.globalTexcoord.xy;
			float b = cnoise(float3(uv.x*_Step.x, uv.y*_Step.y,_Time.y*_Speed));
			b *= _Coeff;
			b = min(b,1);
			return float4(b,b,b,1);
		}

			ENDCG
			
			SubShader
		{
			Cull Off ZWrite Off ZTest Always
				Pass
			{
				Name "Update"
				CGPROGRAM
				#pragma vertex CustomRenderTextureVertexShader
				#pragma fragment frag
				ENDCG
			}
		}
}
