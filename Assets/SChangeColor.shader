Shader "Unlit/SChangeColor" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Mask", Color) = (1, 1, 1, 1)
        _MinX ("MinX", Float) = 0
        _MaxX ("MinY", Float) = 0
	}
	SubShader {
		Tags {
			"RenderType" = "Transparent"
			//"Queue" = "Transparent+1"
		}

		Pass {
			Cull Off 
            Lighting Off
            ZWrite Off
            Fog { Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vertexInput {
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct fragInput {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _MinX;
            float _MinY;
			
			fragInput vert (vertexInput v) {
				fragInput o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				v.uv.x += _MinX;
				v.uv.y += _MinY;
				o.uv = v.uv;
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (fragInput i) : SV_Target {
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;
                return col;
			}
			ENDCG
		}
	}
}
