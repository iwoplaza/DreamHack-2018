// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ResourceShader"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_ResolutionY("ResolutionY", Float) = 512
		_ResolutionX("ResolutionX", Float) = 512
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float _ResolutionX;
		uniform float _ResolutionY;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult10 = (float4(( ase_worldPos.x / _ResolutionX ) , ( _ResolutionY / ase_worldPos.z ) , 0.0 , 0.0));
			o.Albedo = tex2D( _TextureSample0, appendResult10.xy ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
7;107;1321;654;888.741;273.5916;1.021944;False;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-962.1666,2.753953;Float;False;Property;_ResolutionX;ResolutionX;1;0;512;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-969.66,-138.8974;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;4;-966.2306,77.08446;Float;False;Property;_ResolutionY;ResolutionY;1;0;512;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-719.4097,101.9784;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-713.2435,-0.1554775;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-569.4252,47.27957;Float;False;FLOAT4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;2;-408.1332,0.881443;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Assets/Materials/Texture/Palette.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ResourceShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;4;0
WireConnection;7;1;1;3
WireConnection;6;0;1;1
WireConnection;6;1;3;0
WireConnection;10;0;6;0
WireConnection;10;1;7;0
WireConnection;2;1;10;0
WireConnection;0;0;2;0
ASEEND*/
//CHKSM=E7D352BFBEE7FC436728FF55C6BEB3DDC28AA485