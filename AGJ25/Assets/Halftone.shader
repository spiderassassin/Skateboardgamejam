Shader "Custom/Halftone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DotSize ("Dot Size", Float) = 80.0
        _Angle ("Dot Angle (deg)", Range(0,180)) = 45
        _Contrast ("Contrast", Range(0,2)) = 1.0
        _PosterizeLevels ("Posterize Levels", Range(2,16)) = 5
        _EdgeThreshold ("Edge Threshold", Range(0,1)) = 0.2
        _EdgeThickness ("Edge Thickness", Range(0,4)) = 1.0
        _OutlineStrength ("Outline Strength", Range(0,1)) = 1.0
        _Softness ("Dot Softness", Range(0.0,0.5)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _DotSize;
            float _Angle;
            float _Contrast;
            float _PosterizeLevels;
            float _EdgeThreshold;
            float _EdgeThickness;
            float _OutlineStrength;
            float _Softness;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            // luminance helper
            float lum(float3 c) {
                return dot(c, float3(0.299, 0.587, 0.114));
            }

            // rotate a 2D point around center 0.5,0.5 by angle (radians)
            float2 rotateUV(float2 uv, float2 center, float a) {
                float2 p = uv - center;
                float ca = cos(a);
                float sa = sin(a);
                float2 rp = float2(p.x * ca - p.y * sa, p.x * sa + p.y * ca);
                return rp + center;
            }

            // basic sobel-ish using luminance sampling
            float edgeResponse(sampler2D tex, float2 uv) {
                float2 ts = _MainTex_TexelSize.xy;
                // sample 4 neighbors (simple approximation)
                float c = lum(tex2D(tex, uv).rgb);
                float cx = lum(tex2D(tex, uv + float2(ts.x,0)).rgb) - lum(tex2D(tex, uv - float2(ts.x,0)).rgb);
                float cy = lum(tex2D(tex, uv + float2(0,ts.y)).rgb) - lum(tex2D(tex, uv - float2(0,ts.y)).rgb);
                return sqrt(cx*cx + cy*cy);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 screen = float2(1.0/_MainTex_TexelSize.x, 1.0/_MainTex_TexelSize.y);

                // 1) sample original color
                float3 col = tex2D(_MainTex, uv).rgb;

                // 2) posterize (quantize colors) - operate on each channel (or you can posterize luminance only)
                float levels = max(1.0, round(_PosterizeLevels));
                col = floor(col * levels) / (levels - 1.0);
                // contrast tweak
                col = saturate( (col - 0.5) * _Contrast + 0.5 );

                // 3) compute luminance to drive dot size (we'll base on original luminance to avoid double posterize effect)
                float originalLum = lum(tex2D(_MainTex, uv).rgb);

                // 4) rotated halftone grid:
                float angleRad = radians(_Angle);
                // Rotation centered at 0.5,0.5 then scale by dot-size relative to screen
                float2 center = float2(0.5, 0.5);
                // scale the UV so that integer grid maps to cells. Use _DotSize (pixels per cell)
                float2 scaledUV = uv * screen; // absolute pixel coords
                // rotate around screen center in pixel space for consistent dot direction
                float2 screenCenter = screen * 0.5;
                float2 p = scaledUV - screenCenter;
                float ca = cos(angleRad), sa = sin(angleRad);
                float2 pr = float2(p.x * ca - p.y * sa, p.x * sa + p.y * ca);
                // scale to grid
                float cell = max(1.0, _DotSize);
                float2 grid = pr / cell;
                float2 cellFrac = frac(grid) - 0.5; // position relative to cell center in [-0.5,0.5]
                float2 cellCenterPos = cellFrac * cell; // back to pixel offsets from center

                // compute radius target: darker means larger dot — map originalLum [0..1] -> radius [max..min]
                float maxR = cell * 0.5;
                // invert luminance so dark -> 1
                float invL = 1.0 - saturate(originalLum);
                // radius scaled and softened
                float r = invL * maxR;

                float dist = length(cellCenterPos);
                // use smoothstep for anti-aliased dot edge; softness in normalized 0..1 relative to radius
                float softnessPx = max(0.001, _Softness * cell); // in pixels
                float dotMask = 1.0 - smoothstep(r - softnessPx, r + softnessPx, dist);

                // 5) mix dot into grayscale or black-on-white style: here we multiply posterized color by dotMask:
                // Option: multiply luminance by dotMask to create typical engraved halftone (dark areas more filled)
                float3 halftoneCol = col * dotMask;

                // 6) edge detection to draw black outlines
                float edge = edgeResponse(_MainTex, uv);
                float outline = smoothstep(_EdgeThreshold, _EdgeThreshold + 0.02 * _EdgeThickness, edge) * _OutlineStrength;

                // Outline is black overlay (mix in)
                float3 final = lerp(halftoneCol, float3(0,0,0), outline);

                return float4(final, 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}
