#version 330 core

in vec2 UV;
in vec4 col;


out vec4 color;

uniform sampler2D tDiffuse;

void main(){


      vec4 tc = texture2D(tDiffuse,UV);

      if(tc.a>0)
      {
          tc.rgb = vec3(1,1,1);
      }

  if(tc.a < 0.1) discard;

    color  = tc;



}