Shader "Custom/Bordered"
{
	Properties
	{
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct vertexInput 
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct vertexOutput
{
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _OutlineColor;
 
vertexOutput vert(vertexInput v) 
{
	vertexOutput o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
  	float2 xz = normalize(v.vertex.xz);
    float3 norm = mul ((float3x3)UNITY_MATRIX_MV, float3(xz.x, 1, xz.y));
    float2 offset = TransformViewToProjection(norm.xy);
 
	o.pos.xy += offset * o.pos.z * _Outline;
	o.color = _OutlineColor;
	return o;
}
ENDCG
 
	SubShader 
	{
CGPROGRAM
#pragma surface surf Lambert
 
sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
 
struct Input 
{
	float2 uv_MainTex;
	float2 uv_BumpMap;
};
 
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
	o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
}
ENDCG
 
		// note that a vertex shader is specified here but its using the one above
		Pass {
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
	} //
 
SubShader 
{
CGPROGRAM
#pragma surface surf Lambert
 
uniform sampler2D _MainTex;
uniform sampler2D _BumpMap;
uniform fixed4 _Color;
 
struct Input
{
	float2 uv_MainTex;
};
 
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
			#pragma vertex vert
			#pragma exclude_renderers gles xbox360 ps3
			ENDCG
			SetTexture [_MainTex] { combine primary }
		}
	}
	Fallback "Diffuse"
}