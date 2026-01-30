Shader "Custom/GridReveal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize ("Grid Size", Vector) = (10,10,0,0)
        _TimeValue ("Time", Range(0,2)) = 0
        _OddDelay ("Odd Delay", Float) = 0.2
        _AppearSpeed ("Appear Speed", Float) = 2
        [Toggle] _ClosingDirection ("Close Direction", Float) = 1

        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BorderSize ("Border Size", Float) = 0.05

        _RotationAngle ("Grid Rotation (degrees)", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _GridSize;
            float _TimeValue;
            float _OddDelay;
            float _AppearSpeed;
            float _ClosingDirection;

            float4 _BorderColor;
            float _BorderSize;

            float _RotationAngle;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GetCellBorder(float halfSize, float dist, float borderSize, float innerOffset)
            {
                float innerTarget = halfSize - innerOffset;
                
                float innerBorder = smoothstep(
                    innerTarget - borderSize,
                    innerTarget,
                    dist
                ) - smoothstep(
                    innerTarget,
                    innerTarget + borderSize,
                    dist
                );

                return innerBorder;
            }

            float AdjustBorder(float border, float mask, float2 smoothingSizeLimits, float t)
            {
                float growthBorder = 1.0 - smoothstep(smoothingSizeLimits.x, smoothingSizeLimits.y, t);
                border *= mask;
                border *= growthBorder;
                return border;
            }

            float3 ColorBorder(float3 currentColor, float3 borderColor, float borderFactor)
            {
                return lerp(currentColor, borderColor, borderFactor);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // ================= ROTACIÓN =================
                float angle = radians(_RotationAngle);
                float cosA = cos(angle);
                float sinA = sin(angle);
                float2 centeredUV = uv - 0.5;
                float2 rotatedUV;
                rotatedUV.x = centeredUV.x * cosA - centeredUV.y * sinA;
                rotatedUV.y = centeredUV.x * sinA + centeredUV.y * cosA;
                rotatedUV += 0.5;

                // ================= GRID =================
                float2 gridUV = rotatedUV * _GridSize.xy;
                float2 cellId = floor(gridUV);
                float2 cellUV = frac(gridUV);
                //float2 centeredCellUV = frac((centeredUV + 0.5) * _GridSize.xy);

                // ================= PAR / IMPAR =================
                float parity = fmod(cellId.x + cellId.y, 2);

                // ================= DIRECCIÓN =================
                float openingDirection = 1 - _ClosingDirection;
                float columnDelay = cellId.x / _GridSize.x;
                columnDelay = columnDelay * _ClosingDirection
                            + (1 - columnDelay) * openingDirection;

                // ================= TIEMPO =================
                float appearTime = columnDelay + parity * _OddDelay;
                float t = saturate((_TimeValue - appearTime) * _AppearSpeed);

                // ================= ESCALA DESDE CENTRO =================
                float2 centered = abs(cellUV - 0.5);
                float dist = max(centered.x, centered.y);
                float mask = step(dist, t * 0.5);

                // ================= COLOR BASE =================
                uv = TRANSFORM_TEX(uv, _MainTex);
                fixed4 col = tex2D(_MainTex, uv);

                // ================= BORDE EXTERNO =================
                float halfSize = t * 0.5;
                float3 baseColor = col.rgb;
                
                float borderOuter = GetCellBorder(halfSize, dist, _BorderSize, 0);
                borderOuter = AdjustBorder(borderOuter, mask, float2(0.8, 1.0), t);

                float borderInner = GetCellBorder(halfSize, dist, _BorderSize * 0.25, 0.1);
                borderInner = AdjustBorder(borderInner, mask, float2(0.8, 1.0), t);
                
                float borderMask = saturate(borderOuter + borderInner);
                
                col.rgb = lerp(baseColor, _BorderColor.rgb, borderMask);

                // ================= ALPHA FINAL =================
                col.a *= mask;

                return col;
            }

            ENDCG
        }
    }
}
