Shader "Custom/BgColor" {
	Properties 
	{
		_ColorA("ColorA", Color) = (1,1,1,1)
		_ColorB("ColorB", Color) = (1,1,1,1)
	}
	SubShader {
		Pass
		{
			Tags { "Queue"="Geometry-2100" }

			ZWrite Off

			LOD 200
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		
			float4 _ColorA;
			float4 _ColorB;

			void vert(float4 pos:POSITION, out float4 oPos:POSITION, out float4 oColor:COLOR)
			{
				oPos = float4(0,0,0,0);
				oColor = float4(1,1,1,1);
				if(pos.x < 0)
				{
					oPos.x = -1;
				}else if(pos.x > 0)
				{
				  oPos.x = 1;
				}

				if(pos.y > 0)
				{
				  oPos.y = 1;
				  oColor = _ColorA;
				}else if(pos.y < 0)
				{
				  oPos.y = -1;
				  oColor = _ColorB;
				}

				oPos.z = 0;
				oPos.w = 1;
			}

			void frag(float4 color:COLOR, out float4 oColor:COLOR)
			{
				oColor = color;
			}
			ENDCG
		}
		
	} 
	FallBack "Diffuse"
}
