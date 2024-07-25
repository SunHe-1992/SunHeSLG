
Shader "yk/Effect/BlendMask"
{
	Properties
	{	
		_MainTex ("MainTexture", 2D) = "white" {}		
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_ColorLight("ColorLight", Range( 0 , 8)) = 1
		_MaskTex("MaskTexture", 2D) = "white" {}		
		_UVRoll("UVRoll", vector) = (0,0,0,0)
		[Toggle]_useCustom("UseCustom", Float) = 0
		[Space(10)][Toggle] _UseExposureFX("UseExposureFX",float) = 1
        [MaterialEnum(off,0,on,4.0)] _ZTest0("ZTest", float) = 4.0
        [MaterialEnum(RGB,14.0,RGBA,15.0)] _ColorMask("ColorMask", float) = 15.0
	}

	SubShader 
	{
		Tags 
		{
		 "Queue"="Transparent" 
		 "IgnoreProjector"="True"
		 "RenderType"="Transparent" 
		 "PreviewType"="Plane" 
		 }
		 	
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]	
		Cull Off
		Lighting Off 
		ZWrite Off
		ZTest[_ZTest0]

		Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
                #pragma target 3.0
				#pragma multi_compile_instancing //这里,第一步
				struct a2v 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float4 custom : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID //这里,第二步
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					float4 custom : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID //这里,第二步
				};
				
				sampler2D _MainTex;	half4 _MainTex_ST;
				sampler2D _MaskTex; half4 _MaskTex_ST;				
				half4 _Color;
				half _ColorLight;	
				fixed _useCustom;	
				float4 _UVRoll;
				half _ExposureFX;
				fixed _UseExposureFX;

				v2f vert ( a2v v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v); //这里第三步
					UNITY_TRANSFER_INSTANCE_ID(v, o); //第三步
					o.vertex = UnityObjectToClipPos(v.vertex);					
					o.color = v.color;
					//两张贴图的uv用同一个寄存器，避免浪费
					o.texcoord = v.texcoord;
					o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);	
					o.texcoord.zw = TRANSFORM_TEX(v.texcoord,_MaskTex);	
					o.custom = v.custom;
					return o;
				}

				half4 frag ( v2f i  ) : SV_Target
				{
				    UNITY_SETUP_INSTANCE_ID(i); //最后一步
					float2 uv1 = i.texcoord.xy;
					float2 uv2 = i.texcoord.zw;
					float2 speed = i.custom.xy;
					float2 MainUV = ( uv1 + _Time.y * _UVRoll.xy + speed*_useCustom);
					float2 MaskUV = ( uv2 + _Time.y * _UVRoll.zw );
					

					half4 col = ( _Color * ( i.color *  ( tex2D( _MainTex, MainUV ) * tex2D(_MaskTex, MaskUV).r )  ) );	
					col.rgb *=_ColorLight;	
					col.rgb = lerp(col.rgb, col.rgb*(_ExposureFX+1.0), _UseExposureFX);
					return col;
				}
				ENDCG 
			}
			
	}
	
}
