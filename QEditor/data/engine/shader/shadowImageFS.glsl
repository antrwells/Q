#version 330 core


out vec4 color;

uniform sampler2D tDiffuse;

in vec2 UV;

void main(){


   vec4 tc = texture2D(tDiffuse,UV);

    if(tc.a<0.1){
        discard;
    }
    tc = vec4(1,1,1,1);
    color  = tc;



}