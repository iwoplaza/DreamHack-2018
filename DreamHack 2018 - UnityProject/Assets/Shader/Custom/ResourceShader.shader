// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ResourceShader"
{
	Properties
	{
		_BaseTexture("BaseTexture", 2D) = "white" {}
		_ResourceMap("ResourceMap", 2D) = "white" {}
		_ResolutionX("ResolutionX", Float) = 512
		_ResolutionY("ResolutionY", Float) = 512
		_Divider("Divider", Float) = 0.5
		_ResourceFull("ResourceFull", Color) = (0,0,0,0)
		_ResourceEmpty("ResourceEmpty", Color) = (0,0,0,0)
		_ValuePower("ValuePower", Float) = 1
		_CliffHeight("CliffHeight", Float) = 0
		[Toggle]_CliffGroundColor("CliffGroundColor", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _CliffHeight;
		uniform float _Divider;
		uniform float _CliffGroundColor;
		uniform float4 _ResourceFull;
		uniform float4 _ResourceEmpty;
		uniform sampler2D _ResourceMap;
		uniform float _ResolutionX;
		uniform float _ResolutionY;
		uniform float _ValuePower;
		uniform sampler2D _BaseTexture;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult10 = (float4(( ase_worldPos.x / _ResolutionX ) , ( ase_worldPos.z / _ResolutionY ) , 0.0 , 0.0));
			float4 temp_cast_1 = (_ValuePower).xxxx;
			float4 lerpResult24 = lerp( _ResourceFull , _ResourceEmpty , ( 1.0 - pow( ( 1.0 - tex2D( _ResourceMap, appendResult10.xy ) ) , temp_cast_1 ) ).r);
			float4 tex2DNode12 = tex2D( _BaseTexture, i.uv_texcoord );
			float4 ifLocalVar13 = 0;
			if( i.uv_texcoord.x <= _Divider )
				ifLocalVar13 = tex2DNode12;
			else
				ifLocalVar13 = lerpResult24;
			o.Albedo = (( ase_vertex3Pos.y > _CliffHeight ) ? (( i.uv_texcoord.x >= _Divider ) ? lerp(_ResourceFull,_ResourceEmpty,_CliffGroundColor) :  ifLocalVar13 ) :  ifLocalVar13 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
7;117;1321;604;1306.849;855.8245;2.44818;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-1390.679,-147.8344;Float;False;Property;_ResolutionX;ResolutionX;2;0;512;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1394.742,-73.50389;Float;False;Property;_ResolutionY;ResolutionY;3;0;512;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-1398.173,-289.486;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-1141.755,-150.7438;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-1147.922,-48.60996;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-997.9362,-103.3088;Float;False;FLOAT4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;2;-825.51,-187.5769;Float;True;Property;_ResourceMap;ResourceMap;1;0;Assets/Materials/Texture/Palette.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;30;-456.2113,-186.8838;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-454.9526,-67.15912;Float;False;Property;_ValuePower;ValuePower;7;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;32;-261.6564,-176.776;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;17;-567.0509,-592.6772;Float;False;Property;_ResourceFull;ResourceFull;5;0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-574.8885,-409.8756;Float;False;Property;_ResourceEmpty;ResourceEmpty;6;0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;33;-99.7782,-187.9829;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;15;-475.1187,84.25903;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-20.31332,24.98981;Float;True;Property;_BaseTexture;BaseTexture;0;0;Assets/Materials/Texture/Palette.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;24;90.436,-276.7272;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;22.99242,-105.3306;Float;False;Property;_Divider;Divider;4;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;51;182.7675,-372.5855;Float;False;Property;_CliffGroundColor;CliffGroundColor;9;0;0;2;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;13;358.3979,-111.1346;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;43;125.082,-520.0912;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;40;356.8654,-739.506;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareGreaterEqual;48;599.1577,-333.0416;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0.0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;367.9428,-572.5838;Float;False;Property;_CliffHeight;CliffHeight;8;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareGreater;49;814.6481,-310.2194;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1117.227,-320.5177;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ResourceShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;1;1
WireConnection;6;1;3;0
WireConnection;7;0;1;3
WireConnection;7;1;4;0
WireConnection;10;0;6;0
WireConnection;10;1;7;0
WireConnection;2;1;10;0
WireConnection;30;0;2;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;33;0;32;0
WireConnection;12;1;15;0
WireConnection;24;0;17;0
WireConnection;24;1;18;0
WireConnection;24;2;33;0
WireConnection;51;0;17;0
WireConnection;51;1;18;0
WireConnection;13;0;15;1
WireConnection;13;1;16;0
WireConnection;13;2;24;0
WireConnection;13;3;12;0
WireConnection;13;4;12;0
WireConnection;48;0;43;1
WireConnection;48;1;16;0
WireConnection;48;2;51;0
WireConnection;48;3;13;0
WireConnection;49;0;40;2
WireConnection;49;1;50;0
WireConnection;49;2;48;0
WireConnection;49;3;13;0
WireConnection;0;0;49;0
ASEEND*/
//CHKSM=40EEB03898D55AFC1F6F21913ED97D85210BB9C6