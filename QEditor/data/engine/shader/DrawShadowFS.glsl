#version 330 core

in vec2 UV;
in vec4 col;

out vec4 color;

uniform sampler2D tShadow;



uniform vec3 lPos;

uniform float lRange;

uniform float sWidth;
uniform float sHeight;

void main(){

   

    
    vec2 pos = gl_FragCoord.xy;
    


vec2 cp = pos;

    float lx = pos.x-lPos.x;
    float ly = pos.y-(sHeight-lPos.y);

    float ldis = sqrt(lx*lx+ly*ly);

    float cc = 150;

    lx = lx / cc;
    ly = ly / cc;

    

    float lc = 1.0;

    
    while(true){

      cp.x = cp.x - lx;
      cp.y = cp.y - ly;

      if(cp.x<2) break;
      if(cp.y<2) break;
      if(cp.x>sWidth-2) break;
      if(cp.y>sHeight-2) break;

      vec2 suv = vec2(cp.x/sWidth,cp.y/sHeight);
        
      float sV = texture2D(tShadow,suv).r;

      if(sV>0.1)
      {
        lc = 0.0;
        break;
      }
      cc--;
      if(cc<0) break;

    }


 color = vec4(lc,lc,lc,1.0);    



}