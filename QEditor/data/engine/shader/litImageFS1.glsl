#version 330 core

in vec2 UV;
in vec4 col;
in float iZ;

out vec4 color;

uniform sampler2D tDiffuse;
uniform sampler2D tNormal;
uniform sampler2D tShadow;

uniform int sShadow;

uniform float lZ;
uniform vec3 lPos;
uniform vec3 lDif;
uniform vec3 lSpec;
uniform float lShiny;
uniform float lRange;

uniform float sWidth;
uniform float sHeight;

void main(){


  vec4 tc = texture2D(tDiffuse,UV);

  vec3 nv = texture2D(tNormal,UV).rgb;

  nv = nv *2.0 - 1.0;




    vec2 pos = gl_FragCoord.xy;
    
  
  pos.y = sHeight-pos.y;
    vec2 lightDir = lPos.xy - pos;

   float d = length(lightDir);

   vec3 N = nv;
   vec3 L = vec3(normalize(lightDir),2.5);

   L.y = -L.y;
   L.x = -L.x;

   vec3 diff = (lDif) * max(dot(N,L),0.0);


  
    vec2 ss = vec2(pos.x/sWidth,pos.y/sHeight);


  

  
    vec2 lp = vec2(lPos.x,lPos.y);

    float xd = lp.x-pos.x;
    float yd = lp.y-pos.y;

    float dis = sqrt(xd*xd+yd*yd);

    dis = dis / lRange;


    if(dis>1.0)
    {

        dis = 1.0;

    }
    
    dis = 1.0-dis;

    float sv = texture2D(tShadow,ss).r;

     tc.xyz = tc.xyz * lDif * dis;

    float iz = iZ;
    float lz = lZ;


    if(iz<lz){

     tc.xyz = tc.xyz * sv;

    }



   tc.rgb = tc.rgb * diff;   



  tc.a = 1.0;

    color  = tc;



}