// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ProceduralThing"
{
	Properties
	{
		_Tex1("Tex1", 2D) = "white" {}
		_Tex2("Tex2", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Tex1;
		uniform sampler2D _Tex2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord8 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 temp_cast_0 = (0.1).xx;
			float2 uv_TexCoord4 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner3 = ( ( uv_TexCoord4 * float2( 0.2,0.2 ) ) + 1.0 * _Time.y * temp_cast_0);
			o.Emission = ( tex2D( _Tex1, uv_TexCoord8 ) * pow( tex2D( _Tex2, panner3 ) , 3.0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
7;136;1321;604;1133.033;283.3708;1.3;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1272.622,-47.54727;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-982.4618,130.1581;Float;False;Constant;_Pan;Pan;2;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-969.1086,-22.14324;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.2,0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;3;-817.4031,86.674;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-854.507,-152.7829;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-616.2076,38.2106;Float;True;Property;_Tex2;Tex2;1;0;Assets/AmplifyShaderEditor/Examples/Assets/Textures/Misc/RainbowGradient.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-617.5,-163;Float;True;Property;_Tex1;Tex1;0;0;Assets/Materials/Texture/EmissionMap.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;14;-291.3588,49.33963;Float;False;2;0;COLOR;0.0;False;1;FLOAT;3.0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1025.078,242.2188;Float;False;Constant;_Float0;Float 0;2;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1150.65,422.1954;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;9;-830.1962,263.9757;Float;True;CoolWave;-1;;1;a4ec317493edf3b439fcd463a40eca0d;6;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT2;0,0;False;5;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1059.078,303.2188;Float;False;Constant;_Float3;Float 3;2;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-93.15074,-3.882821;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;170.3969,4.425893;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ProceduralThing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;4;0
WireConnection;3;0;7;0
WireConnection;3;2;5;0
WireConnection;2;1;3;0
WireConnection;1;1;8;0
WireConnection;14;0;2;0
WireConnection;9;0;10;0
WireConnection;9;1;12;0
WireConnection;9;3;12;0
WireConnection;9;4;13;0
WireConnection;9;5;12;0
WireConnection;6;0;1;0
WireConnection;6;1;14;0
WireConnection;0;2;6;0
ASEEND*/
//CHKSM=3EED3248C20DECD7971C4164749531C4A07FC132