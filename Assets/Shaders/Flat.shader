Shader "Custom/Flat"
{
	Properties
	{
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_BaseColor("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_BorderColor ("Border Color", Color) = (0,0,0,1)
		_Border ("Border Width", Range (.002, 0.03)) = .005
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct vertexInput 
{
	float4 vertex : POSITION;
};
 
struct vertexOutput
{
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Border;
uniform float4 _BorderColor;
 
vertexOutput vert(vertexInput v) 
{
	vertexOutput o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
  	float2 xz = normalize(v.vertex.xz);
    float3 norm = mul ((float3x3)UNITY_MATRIX_MV, float3(xz.x, 1, xz.y));
    float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Border;
	o.color = _BorderColor;
	return o;
}
ENDCG
 
	SubShader 
	{
CGPROGRAM
#pragma surface surf Lambert
 
fixed4 _Color;
uniform fixed4 _BaseColor;
 
struct Input
{
	float2 uv_MainTex;
};
 
void surf(Input IN, inout SurfaceOutput o)
{
	fixed4 c = _Color * _BaseColor;
	o.Albedo = c.rgb;
}
ENDCG
 
		// note that a vertex shader is specified here but its using the one above
		Pass
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			//Offset 50,50
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(vertexOutput i) :COLOR { return i.color; }
			ENDCG
		}
	}
	Fallback "Diffuse"
}
