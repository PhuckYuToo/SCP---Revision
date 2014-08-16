Shader "Transparent/InvisibleShadowCaster" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		  UsePass "VertexLit/SHADOWCOLLECTOR"    
        UsePass "VertexLit/SHADOWCASTER"
	} 
	FallBack off
}
