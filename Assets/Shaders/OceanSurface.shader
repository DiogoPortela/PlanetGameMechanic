Shader "Custom/OceanSurface"
{
    Properties
    {
        _Color("SurfaceColor", Color) = (1,1,1,1)
        _Color_Bottom("BottomColor", Color) = (1,1,1,1)
        _Max_Deph("DepthValue", Float) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        //Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}

        // No culling or depth
        //Cull Off 
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        //ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            fixed4 _Color_Bottom;
            sampler2D _CameraDepthTexture;

            struct vert_in_struct
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct frag_in_struct
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VPOS_TYPE  screenPos : TEXCOORD1;
            };

            frag_in_struct vert (vert_in_struct v)
            {
                frag_in_struct o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }


            fixed4 frag(frag_in_struct i) : SV_Target
            {
                fixed4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
                float bottomDepth = LinearEyeDepth(depthSample).r;
                float surfaceDepth = i.vertex.w;

                float oceanDepth = (bottomDepth - surfaceDepth) * 100.0f;
                oceanDepth = clamp(oceanDepth, 0.0f, 1.0f);
                fixed4 col = lerp(_Color, _Color_Bottom, oceanDepth);

                return col;
            }
            ENDCG
        }
    }
}
