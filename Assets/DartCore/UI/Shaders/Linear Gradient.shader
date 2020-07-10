Shader "DartCore/UI/Linear Gradient"
{
	Properties
	{
		_Color1("Color1", Color) = (0.08826337, 0.04313726, 0.8313726, 1)
		_Color2("Color2", Color) = (0.5680933, 0, 1, 1)
		[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
		Vector1_FF58C94F("CenterX", Range(0, 1)) = 0
		Vector1_6F88EBED("Angle", Range(0, 360)) = 0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
		SubShader
		{
			Tags
			{
				"RenderPipeline" = "UniversalPipeline"
				"RenderType" = "Transparent"
				"Queue" = "Transparent+0"
			}

			Pass
			{
				Name "Pass"
				Tags
				{
			// LightMode: <None>
		}

		// Render State
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		Cull Back
		ZTest [unity_GUIZTestMode]
		ZWrite Off
			// ColorMask: <None>
			Stencil
			   {
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			   }
			   ColorMask[_ColorMask]

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Debug
			// <None>

			// --------------------------------------------------
			// Pass

			// Pragmas
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			// Keywords
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma shader_feature _ _SAMPLE_GI
			// GraphKeywords: <None>

			// Defines
			#define _SURFACE_TYPE_TRANSPARENT 1
			#define _AlphaClip 1
			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define ATTRIBUTES_NEED_TEXCOORD0
			#define VARYINGS_NEED_POSITION_WS 
			#define VARYINGS_NEED_TEXCOORD0
			#define SHADERPASS_UNLIT

			// Includes
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

			// --------------------------------------------------
			// Graph

			// Graph Properties
			CBUFFER_START(UnityPerMaterial)
			float4 _Color1;
			float4 _Color2;
			float Vector1_FF58C94F;
			float Vector1_6F88EBED;
			CBUFFER_END
			TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
			SAMPLER(_SampleTexture2D_81FB8CC8_Sampler_3_Linear_Repeat);

			// Graph Functions

			void Unity_Multiply_float(float A, float B, out float Out)
			{
				Out = A * B;
			}

			void Unity_Divide_float(float A, float B, out float Out)
			{
				Out = A / B;
			}

			void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
			{
				//rotation matrix
				UV -= Center;
				float s = sin(Rotation);
				float c = cos(Rotation);

				//center rotation matrix
				float2x2 rMatrix = float2x2(c, -s, s, c);
				rMatrix *= 0.5;
				rMatrix += 0.5;
				rMatrix = rMatrix * 2 - 1;

				//multiply the UVs by the rotation matrix
				UV.xy = mul(UV.xy, rMatrix);
				UV += Center;

				Out = UV;
			}

			void Unity_Subtract_float(float A, float B, out float Out)
			{
				Out = A - B;
			}

			void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
			{
				Out = lerp(A, B, T);
			}

			void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
			{
				Out = A * B;
			}

			// Graph Vertex
			// GraphVertex: <None>

			// Graph Pixel
			struct SurfaceDescriptionInputs
			{
				float3 WorldSpacePosition;
				float4 ScreenPosition;
				float4 uv0;
			};

			struct SurfaceDescription
			{
				float3 Color;
				float Alpha;
				float AlphaClipThreshold;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				float4 _Property_23BD9701_Out_0 = _Color1;
				float4 _Property_9C4D6908_Out_0 = _Color2;
				float4 _ScreenPosition_643A2419_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
				float _Property_A15DB7F_Out_0 = Vector1_6F88EBED;
				float _Multiply_42636FF2_Out_2;
				Unity_Multiply_float(_Property_A15DB7F_Out_0, 3.14, _Multiply_42636FF2_Out_2);
				float _Divide_9872FF42_Out_2;
				Unity_Divide_float(_Multiply_42636FF2_Out_2, 180, _Divide_9872FF42_Out_2);
				float2 _Rotate_7BD290E4_Out_3;
				Unity_Rotate_Radians_float((_ScreenPosition_643A2419_Out_0.xy), float2 (0.5, 0.5), _Divide_9872FF42_Out_2, _Rotate_7BD290E4_Out_3);
				float _Split_AC103203_R_1 = _Rotate_7BD290E4_Out_3[0];
				float _Split_AC103203_G_2 = _Rotate_7BD290E4_Out_3[1];
				float _Split_AC103203_B_3 = 0;
				float _Split_AC103203_A_4 = 0;
				float _Property_A2DDDF19_Out_0 = Vector1_FF58C94F;
				float _Subtract_832701D8_Out_2;
				Unity_Subtract_float(_Split_AC103203_G_2, _Property_A2DDDF19_Out_0, _Subtract_832701D8_Out_2);
				float4 _Lerp_1B53928F_Out_3;
				Unity_Lerp_float4(_Property_23BD9701_Out_0, _Property_9C4D6908_Out_0, (_Subtract_832701D8_Out_2.xxxx), _Lerp_1B53928F_Out_3);
				float4 _SampleTexture2D_81FB8CC8_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
				float _SampleTexture2D_81FB8CC8_R_4 = _SampleTexture2D_81FB8CC8_RGBA_0.r;
				float _SampleTexture2D_81FB8CC8_G_5 = _SampleTexture2D_81FB8CC8_RGBA_0.g;
				float _SampleTexture2D_81FB8CC8_B_6 = _SampleTexture2D_81FB8CC8_RGBA_0.b;
				float _SampleTexture2D_81FB8CC8_A_7 = _SampleTexture2D_81FB8CC8_RGBA_0.a;
				float4 _Multiply_EE2F6889_Out_2;
				Unity_Multiply_float(_Lerp_1B53928F_Out_3, _SampleTexture2D_81FB8CC8_RGBA_0, _Multiply_EE2F6889_Out_2);
				surface.Color = (_Multiply_EE2F6889_Out_2.xyz);
				surface.Alpha = 1;
				surface.AlphaClipThreshold = 0.5;
				return surface;
			}

			// --------------------------------------------------
			// Structs and Packing

			// Generated Type: Attributes
			struct Attributes
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			// Generated Type: Varyings
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float3 positionWS;
				float4 texCoord0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Generated Type: PackedVaryings
			struct PackedVaryings
			{
				float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
				float3 interp00 : TEXCOORD0;
				float4 interp01 : TEXCOORD1;
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			// Packed Type: Varyings
			PackedVaryings PackVaryings(Varyings input)
			{
				PackedVaryings output = (PackedVaryings)0;
				output.positionCS = input.positionCS;
				output.interp00.xyz = input.positionWS;
				output.interp01.xyzw = input.texCoord0;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// Unpacked Type: Varyings
			Varyings UnpackVaryings(PackedVaryings input)
			{
				Varyings output = (Varyings)0;
				output.positionCS = input.positionCS;
				output.positionWS = input.interp00.xyz;
				output.texCoord0 = input.interp01.xyzw;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
				output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
				#endif
				#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
				output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
				#endif
				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				output.cullFace = input.cullFace;
				#endif
				return output;
			}

			// --------------------------------------------------
			// Build Graph Inputs

			SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
			{
				SurfaceDescriptionInputs output;
				ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





				output.WorldSpacePosition = input.positionWS;
				output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
				output.uv0 = input.texCoord0;
			#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
			#else
			#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
			#endif
			#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

				return output;
			}


			// --------------------------------------------------
			// Main

			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

			ENDHLSL
		}

		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}

				// Render State
				Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
				Cull Back
				ZTest [unity_GUIZTestMode]
				ZWrite On
				// ColorMask: <None>
				Stencil
			   {
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			   }
			   ColorMask[_ColorMask]

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				// Debug
				// <None>

				// --------------------------------------------------
				// Pass

				// Pragmas
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile_instancing

				// Keywords
				#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
				// GraphKeywords: <None>

				// Defines
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _AlphaClip 1
				#define ATTRIBUTES_NEED_NORMAL
				#define ATTRIBUTES_NEED_TANGENT
				#define SHADERPASS_SHADOWCASTER

				// Includes
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
				#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

				// --------------------------------------------------
				// Graph

				// Graph Properties
				CBUFFER_START(UnityPerMaterial)
				float4 _Color1;
				float4 _Color2;
				float Vector1_FF58C94F;
				float Vector1_6F88EBED;
				CBUFFER_END
				TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;

				// Graph Functions
				// GraphFunctions: <None>

				// Graph Vertex
				// GraphVertex: <None>

				// Graph Pixel
				struct SurfaceDescriptionInputs
				{
				};

				struct SurfaceDescription
				{
					float Alpha;
					float AlphaClipThreshold;
				};

				SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
				{
					SurfaceDescription surface = (SurfaceDescription)0;
					surface.Alpha = 1;
					surface.AlphaClipThreshold = 0.5;
					return surface;
				}

				// --------------------------------------------------
				// Structs and Packing

				// Generated Type: Attributes
				struct Attributes
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					float4 tangentOS : TANGENT;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				// Generated Type: Varyings
				struct Varyings
				{
					float4 positionCS : SV_POSITION;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
					#endif
				};

				// Generated Type: PackedVaryings
				struct PackedVaryings
				{
					float4 positionCS : SV_POSITION;
					#if UNITY_ANY_INSTANCING_ENABLED
					uint instanceID : CUSTOM_INSTANCE_ID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
					#endif
				};

				// Packed Type: Varyings
				PackedVaryings PackVaryings(Varyings input)
				{
					PackedVaryings output = (PackedVaryings)0;
					output.positionCS = input.positionCS;
					#if UNITY_ANY_INSTANCING_ENABLED
					output.instanceID = input.instanceID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					output.cullFace = input.cullFace;
					#endif
					return output;
				}

				// Unpacked Type: Varyings
				Varyings UnpackVaryings(PackedVaryings input)
				{
					Varyings output = (Varyings)0;
					output.positionCS = input.positionCS;
					#if UNITY_ANY_INSTANCING_ENABLED
					output.instanceID = input.instanceID;
					#endif
					#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
					output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
					#endif
					#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
					output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
					#endif
					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					output.cullFace = input.cullFace;
					#endif
					return output;
				}

				// --------------------------------------------------
				// Build Graph Inputs

				SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
				{
					SurfaceDescriptionInputs output;
					ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





				#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
				#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
				#else
				#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
				#endif
				#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

					return output;
				}


				// --------------------------------------------------
				// Main

				#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

				ENDHLSL
			}

			Pass
			{
				Name "DepthOnly"
				Tags
				{
					"LightMode" = "DepthOnly"
				}

					// Render State
					Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
					Cull Back
					ZTest LEqual
					ZWrite On
					ColorMask 0


					HLSLPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					// Debug
					// <None>

					// --------------------------------------------------
					// Pass

					// Pragmas
					#pragma prefer_hlslcc gles
					#pragma exclude_renderers d3d11_9x
					#pragma target 2.0
					#pragma multi_compile_instancing

					// Keywords
					// PassKeywords: <None>
					// GraphKeywords: <None>

					// Defines
					#define _SURFACE_TYPE_TRANSPARENT 1
					#define _AlphaClip 1
					#define ATTRIBUTES_NEED_NORMAL
					#define ATTRIBUTES_NEED_TANGENT
					#define SHADERPASS_DEPTHONLY

					// Includes
					#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
					#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
					#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
					#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
					#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

					// --------------------------------------------------
					// Graph

					// Graph Properties
					CBUFFER_START(UnityPerMaterial)
					float4 _Color1;
					float4 _Color2;
					float Vector1_FF58C94F;
					float Vector1_6F88EBED;
					CBUFFER_END
					TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;

					// Graph Functions
					// GraphFunctions: <None>

					// Graph Vertex
					// GraphVertex: <None>

					// Graph Pixel
					struct SurfaceDescriptionInputs
					{
					};

					struct SurfaceDescription
					{
						float Alpha;
						float AlphaClipThreshold;
					};

					SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
					{
						SurfaceDescription surface = (SurfaceDescription)0;
						surface.Alpha = 1;
						surface.AlphaClipThreshold = 0.5;
						return surface;
					}

					// --------------------------------------------------
					// Structs and Packing

					// Generated Type: Attributes
					struct Attributes
					{
						float3 positionOS : POSITION;
						float3 normalOS : NORMAL;
						float4 tangentOS : TANGENT;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : INSTANCEID_SEMANTIC;
						#endif
					};

					// Generated Type: Varyings
					struct Varyings
					{
						float4 positionCS : SV_POSITION;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
						#endif
					};

					// Generated Type: PackedVaryings
					struct PackedVaryings
					{
						float4 positionCS : SV_POSITION;
						#if UNITY_ANY_INSTANCING_ENABLED
						uint instanceID : CUSTOM_INSTANCE_ID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
						#endif
					};

					// Packed Type: Varyings
					PackedVaryings PackVaryings(Varyings input)
					{
						PackedVaryings output = (PackedVaryings)0;
						output.positionCS = input.positionCS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif
						return output;
					}

					// Unpacked Type: Varyings
					Varyings UnpackVaryings(PackedVaryings input)
					{
						Varyings output = (Varyings)0;
						output.positionCS = input.positionCS;
						#if UNITY_ANY_INSTANCING_ENABLED
						output.instanceID = input.instanceID;
						#endif
						#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
						output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
						#endif
						#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
						output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
						#endif
						#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
						output.cullFace = input.cullFace;
						#endif
						return output;
					}

					// --------------------------------------------------
					// Build Graph Inputs

					SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
					{
						SurfaceDescriptionInputs output;
						ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





					#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
					#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
					#else
					#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
					#endif
					#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

						return output;
					}


					// --------------------------------------------------
					// Main

					#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
					#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

					ENDHLSL
				}

		}
			FallBack "Hidden/Shader Graph/FallbackError"
}
