#version 410

in vec2 UV;
in vec4 col;

out vec4 color;

uniform sampler2D tR;


void main(){

   
    vec4 tc = texture2D(tR,UV) * col;


  
    //co.rgb = vec3(1,1,1);
    //if(tc.a<0.1)
   // {
      //  discard;

    //}


    color = tc;

}