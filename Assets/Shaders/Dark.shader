﻿Shader "Custom/Moody"
{
	Properties
	{
		_Color("Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex("Diffuse, gloss(a)", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump" {}
		_EmitMap("Emission", 2D) = "black" {}
		_SpecColor("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Float) = 10
		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0.1, 10)) = 3.0
		_BumpDepth("Bump Depth", Range(0.0, 1.0)) = 1.0
		_EmitStrength("Emission Strength", Range(0.0, 2.0)) = 0.0
	}
	SubShader
	{
		Pass
		{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			uniform fixed4 _Color;
			uniform fixed4 _SpecColor;
			uniform fixed4 _RimColor;
			uniform half _Shininess;
			uniform half _RimPower;
			uniform fixed _BumpDepth;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _BumpMap;
			uniform half4 _BumpMap_ST;
			uniform sampler2D _EmitMap;
			uniform half4 _EmitMap_ST;
			uniform fixed _EmitStrength;
			
			uniform half4 _LightColor0;
			
			struct vertexInput
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 texcoord : TEXCOORD0;
				half4 tangent : TANGENT;
			};
			struct vertexOutput
			{
				half4 pos : SV_POSITION;
				half4 tex : TEXCOORD0;
				fixed4 lightDirection : TEXCOORD1;
				fixed3 viewDirection : TEXCOORD2;
				fixed3 normalWorld : TEXCOORD3;
				fixed3 tangentWorld : TEXCOORD4;
				fixed3 binormalWorld : TEXCOORD5;
				float4 color : COLOR;
				LIGHTING_COORDS(6, 7)
			};
			
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				
				o.normalWorld = normalize(mul(half4(v.normal, 0.0), _World2Object).xyz);
				o.tangentWorld = normalize(mul(_Object2World, v.tangent).xyz);
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);
				
				half4 posWorld = mul(_Object2World, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				o.viewDirection = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
				
				half3 fragmentToLight = _WorldSpaceLightPos0.xyz - posWorld.xyz;
					
				o.lightDirection = fixed4(
				normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLight, _WorldSpaceLightPos0.w)),
				lerp(1.0, 1.0/length(fragmentToLight), _WorldSpaceLightPos0.w));
				
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				
				return o;
			}
			
			fixed4 frag(vertexOutput i) : COLOR
			{	
				//Texture maps
				fixed4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				fixed4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				fixed4 texE = tex2D(_EmitMap, i.tex.xy * _EmitMap_ST.xy + _EmitMap_ST.zw);
				
				//unpackNormal func
				fixed3 localCoords = float3(2.0 * texN.ag - float2(1.0, 1.0), _BumpDepth);
				
				//normal transpose matrix
				fixed3x3 local2WorldTranspose = fixed3x3(
					i.tangentWorld,
					i.binormalWorld,
					i.normalWorld
				);
				
				//calc normal dir
				fixed3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				
				//lighting
				//dot product
				fixed nDotL = saturate(dot(normalDirection, i.lightDirection.xyz));
				
				fixed3 diffuseReflection = i.lightDirection.w * _LightColor0.xyz * nDotL;
				fixed3 specularReflection = diffuseReflection * _SpecColor.xyz * pow(saturate(dot(reflect(-i.lightDirection.xyz, normalDirection), i.viewDirection)), _Shininess);
				
				//Rimlighting
				fixed rim = 1 - nDotL;
				fixed3 rimLighting = nDotL * _RimColor.xyz * _LightColor0.xyz * pow(rim, _RimPower);
			
				fixed3 lightFinal = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection + (specularReflection * tex.a) + rimLighting + (texE.xyz * _EmitStrength);
				
				return fixed4(tex.xyz * lightFinal * _Color.xyz * (LIGHT_ATTENUATION(i) + 0.9f), 1.0);
			}
			ENDCG
		}
		Pass
		{
			Tags{"LightMode" = "ForwardAdd"}
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			uniform fixed4 _Color;
			uniform fixed4 _SpecColor;
			uniform fixed4 _RimColor;
			uniform half _Shininess;
			uniform half _RimPower;
			uniform fixed _BumpDepth;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _BumpMap;
			uniform half4 _BumpMap_ST;
			uniform sampler2D _EmitMap;
			uniform half4 _EmitMap_ST;
			uniform fixed _EmitStrength;
			
			uniform half4 _LightColor0;
			
			struct vertexInput
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 texcoord : TEXCOORD0;
				half4 tangent : TANGENT;
			};
			struct vertexOutput
			{
				half4 pos : SV_POSITION;
				half4 tex : TEXCOORD0;
				fixed4 lightDirection : TEXCOORD1;
				fixed3 viewDirection : TEXCOORD2;
				fixed3 normalWorld : TEXCOORD3;
				fixed3 tangentWorld : TEXCOORD4;
				fixed3 binormalWorld : TEXCOORD5;
			};
			
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				
				o.normalWorld = normalize(mul(half4(v.normal, 0.0), _World2Object).xyz);
				o.tangentWorld = normalize(mul(_Object2World, v.tangent).xyz);
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);
				
				half4 posWorld = mul(_Object2World, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				o.viewDirection = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
				
				half3 fragmentToLight = _WorldSpaceLightPos0.xyz - posWorld.xyz;
					
				o.lightDirection = fixed4(
				normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLight, _WorldSpaceLightPos0.w)),
				lerp(1.0, 1.0/length(fragmentToLight), _WorldSpaceLightPos0.w));
				
				return o;
			}
			
			fixed4 frag(vertexOutput i) : COLOR
			{	
				//Texture maps
				fixed4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				fixed4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				
				//unpackNormal func
				fixed3 localCoords = float3(2.0 * texN.ag - float2(1.0, 1.0), _BumpDepth);
				
				//normal transpose matrix
				fixed3x3 local2WorldTranspose = fixed3x3(
					i.tangentWorld,
					i.binormalWorld,
					i.normalWorld
				);
				
				//calc normal dir
				fixed3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				
				//lighting
				//dot product
				fixed nDotL = saturate(dot(normalDirection, i.lightDirection.xyz));
				
				fixed3 diffuseReflection = i.lightDirection.w * _LightColor0.xyz * nDotL;
				fixed3 specularReflection = diffuseReflection * _SpecColor.xyz * pow(saturate(dot(reflect(-i.lightDirection.xyz, normalDirection), i.viewDirection)), _Shininess);
				
				//Rimlighting
				fixed rim = 1 - nDotL;
				fixed3 rimLighting = nDotL * _RimColor.xyz * _LightColor0.xyz * pow(rim, _RimPower);
			
				fixed3 lightFinal = diffuseReflection + (specularReflection * tex.a) + rimLighting;
				
				return fixed4(lightFinal, 1.0);
			}
			ENDCG
		}
	}
	//backup shader
	Fallback "Diffuse"
}