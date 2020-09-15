  Shader "Custom/Color emissive diffuse" {
    Properties {
      _MainTex ("Texture", 2D) = "diffuse" {}
      _Emission ("Fog Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" "Queue" = "Geometry-1000" }
      CGPROGRAM

      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      float4 _Emission;

      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          o.Emission = _Emission.rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }