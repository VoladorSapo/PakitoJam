Shader "Custom/HexReveal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize ("Grid Size", Vector) = (10,10,0,0)
        _TimeValue ("Time", Range(0,2)) = 0
        _OpeningDelay ("Opening Delay", Float) = 0.25
        _AppearSpeed ("Appear Speed", Float) = 2
        [Toggle] _ClosingDirection ("Close Direction", Float) = 1
        
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BorderSize ("Border Size", Float) = 0.05

        _RotationAngle ("Grid Rotation (degrees)", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
            float _OpeningDelay;
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ============================================================
            // HEXAGONAL FLAT-TOP CONFIG
            // ============================================================

            static const float2 tsnTriangleRatio = float2(sqrt(3.0), 1.0);

            float2 GetHexUV(float2 uv, float2 gridSize, float rotationAngle)
            {
                float2 hexedUV = uv * gridSize;
                float angle = radians(rotationAngle);
                float cosA = cos(angle);
                float sinA = sin(angle);
                
                float2 centeredUV = hexedUV - 0.5;
                float2 rotatedUV;
                rotatedUV.x = centeredUV.x * cosA - centeredUV.y * sinA;
                rotatedUV.y = centeredUV.x * sinA + centeredUV.y * cosA;
                rotatedUV += 0.5;
                
                return rotatedUV;
            }
            float Hex(float2 p)
            {
                p = abs(p);
                return max(dot(p, tsnTriangleRatio * 0.5), p.y);
            }

            float4 HexLattice(float2 uv)
            {
                float4 hexCenter = round(float4(uv, uv - float2(1.0, 0.5)) / tsnTriangleRatio.xyxy);

                float4 offset = float4(
                    uv - hexCenter.xy * tsnTriangleRatio,
                    uv - (hexCenter.zw + 0.5) * tsnTriangleRatio
                );

                return dot(offset.xy, offset.xy) < dot(offset.zw, offset.zw)
                    ? float4(offset.xy, hexCenter.xy)
                    : float4(offset.zw, hexCenter.zw + 0.5);
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
            
            
            // ============================================================
            // FRAGMENT
            // ============================================================

            float4 frag(v2f i) : SV_Target
            {
                float2 fixedUV = i.uv;
                
                float2 hexedUV = GetHexUV(fixedUV, _GridSize.xy, _RotationAngle);
                
                float4 h = HexLattice(hexedUV);
                float2 local = h.xy;
                float2 cell  = h.zw;

                float dist = Hex(local);

                // Reveal basado en pantalla
                float screenX = cell.x / _GridSize.x;
                float screenY = cell.y / _GridSize.y;
                
                // Dirección: 0 = L→R, 1 = R→L
                float openingDirection = 1 - _ClosingDirection;
                float delay = lerp(screenX, 1 - screenX, 1 - _ClosingDirection);
                delay -= _OpeningDelay * openingDirection;

                /*float xparity = fmod(cell.x, 1) / _GridSize.x;
                float parity = fmod(cell.x - xparity + cell.y, 2.0);
                delay += parity * 0.1;*/
                
                // Reveal temporal
                float t = saturate((_TimeValue - delay) * _AppearSpeed);
                // Tamaño del hexágono
                float radius = t * 0.5;
                // Máscara hexagonal
                float mask = 1.0 - step(radius, dist);

                fixedUV = TRANSFORM_TEX(fixedUV, _MainTex);
                fixed4 col = tex2D(_MainTex, fixedUV);

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
