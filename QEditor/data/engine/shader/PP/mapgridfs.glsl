#version 330 core

in vec2 UV;
in vec4 col;


out vec4 color;

uniform sampler2D tDiffuse;
uniform sampler2D tGrid;
uniform float xP;

void main(){


      vec4 tc = texture2D(tGrid,UV);
      
      tc = tc * texture2D(tDiffuse,UV);

      if((tc.r+tc.g+tc.b)<0.2){

          discard;

      }

    float xd = xP - UV.x;
    xd = abs(xd);
    xd = xd * 5;

    xd = 0.5 -xd;

  
    

    tc.r = tc.r * xd;
    tc.g = tc.g * xd;
    tc.b = tc.b * xd;

    if(tc.r<0) tc.r = 0;
    if(tc.g<0) tc.g = 0;
    if(tc.b<0) tc.b = 0;

  if(tc.a < 0.1) discard;
  tc.g = tc.r+tc.g+tc.b;
  tc.r= 0;
  tc.b= 0;
  tc.g = tc.g *0.2;

  tc.g = xd;


//tc.a = 0.4;
   // tc.a = xd;
   tc.a = xd;

    color  = tc;



}