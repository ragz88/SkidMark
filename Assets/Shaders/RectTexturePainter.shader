Shader "TNTC/RectTexturePainter"{

    Properties{
        _PainterColor("Painter Color", Color) = (0, 0, 0, 0) 
        //_TopLeft("Top Left Corner", Vector) = (0, 0, 0, 0) 
        //_TopRight("Top Right Corner", Vector) = (0, 0, 0, 0)
        //_BottomLeft("Bottom Left Corner", Vector) = (0, 0, 0, 0)
        //_BottomRight("Bottom Right Corner", Vector) = (0, 0, 0, 0)
    }

        SubShader{
            Cull Off ZWrite Off ZTest Off

            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float3 _PainterPosition;
                float _Radius;
                float _Hardness;
                float _Strength;
                float4 _PainterColor;
                float _PrepareUV;

               // float2 _TopLeft;
               // float2 _TopRight;
               // float2 _BottomLeft;
               // float2 _BottomRight;

                float4 _Debug;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 worldPos : TEXCOORD1;
                };

                //float mask(float3 position, float3 center, float radius, float hardness) {
                //    float m = distance(center, position);
                //    return 1 - smoothstep(radius * hardness, radius, m);
                //}

                /*float mask(float3 position)
                {
                    float4 pos = UnityObjectToClipPos(position);

                    // CUrrently assumes this is just a rectangle

                    if (pos.x > _TopLeft.x && pos.x < _TopRight.x && pos.y < _TopLeft.y && pos.y > _BottomRight.y)
                    {
                        return (1, 1, 1, 1);
                    }
                    else
                    {
                        return (0, 0, 0, 1);
                    }
                }*/

                float mask(float3 position, float3 center, float radius, float hardness) {
                    
                    // Keeps track of distance between position of brush, and position of each fragment.
                    // Remember, because not every frag lies on a vert, some will be interpolated to get their colour.
                    // Only frags that fall within our brush circle will be affected.
                    // Frags further from the centre will be affected less.

                    // Circle Code Below
                    // ----------------------------------------------------------
                    //float m = distance(center, position);
                    //return 1 - smoothstep(radius * hardness, radius, m);
                    // ----------------------------------------------------------

                    // Square Code Below
                    // -------------------------------------------------------------
                    /*
                    if (abs(position.x - center.x) < radius && abs(position.z - center.z) < radius)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                    */
                    // -------------------------------------------------------------


                    // Ellipse Code Below
                    // -------------------------------------------------------------

                    // equation for an ellipse:  (x/a)^2 + (y/b)^2 = 1
                    // if we plug a given x and y in, and the value is smaller than 1, the point is within the ellipse.
                    // if greater, it is outside.
                    // a = width     b = height

                    float pointX = (position.x - center.x);
                    float pointY = (position.z - center.z);

                    float width = 6;
                    float height = 2;
                    float rotation = 12;

                    // No rotation
                    //float f = ((pointX * pointX) / width) + ((pointY * pointY) / height);

                    // with rotation
                    // These formulas are used to identify two new x and y positions, which have been rotated about the origin.
                    float rotatedX = pointX * cos(rotation) - pointY * sin(rotation);
                    float rotatedY = pointY * cos(rotation) + pointX * sin(rotation);

                    float f = ((rotatedX * rotatedX) / width) + ((rotatedY * rotatedY) / height);
                    

                    if (f <= 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                    // -------------------------------------------------------------
                }

                // Converts world space to UV space
                v2f vert(appdata v) {
                    v2f o;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    o.uv = v.uv;
                    float4 uv = float4(0, 0, 0, 1);
                    uv.xy = float2(1, _ProjectionParams.x) * (v.uv.xy * float2(2, 2) - float2(1, 1));
                    o.vertex = uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target{
                    if (_PrepareUV > 0) {
                        return float4(0, 0, 1, 1);
                    }

                    float4 col = tex2D(_MainTex, i.uv);
                    float f = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
                    float edge = f * _Strength;
                    return lerp(col, _PainterColor, edge);
                }
                ENDCG
            }
    }
}