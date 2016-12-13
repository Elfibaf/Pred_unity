Shader "Custom/Interior" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
       
		Tags { "RenderType" = "Opaque" }

		Cull Off

		CGPROGRAM

		#pragma surface surf Lambert vertex:vert

		void vert(inout appdata_full v)
		{
		  	v.normal.xyz = v.normal * -1;
		}

		struct Input {
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		   	o.Albedo = 1;
		}

		ENDCG

	}

	FallBack "Diffuse"
}
