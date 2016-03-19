Shader "Unlit/SChangeColor" {
	Properties {
		_MainTex ("Sprite", 2D) = "white" {}
		_Color ("Filter color", color) = (0, 0, 0, 1)
	}

	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass {
			Cull Off 
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog {
				Mode Off
			}
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;

			struct vectInput {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0; 
			};

			struct fragInput {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			fragInput vert (vectInput v) {
				fragInput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.pos);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (fragInput i) : COLOR {
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= _Color.rgb;
				return col;
			}
			ENDCG
		}
	}
}