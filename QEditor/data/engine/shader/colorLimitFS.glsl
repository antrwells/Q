#version 440 core

in vec2 UV;
in vec4 col;

out vec4 color;

uniform float limit;
uniform sampler2D tR;

void main(){
    vec2 nv = UV;

    vec4 col;
    col.rgb = texture2D(tR,nv).rgb;
    col.a = 1.0;

    float cv = (col.r+col.g+col.b)/3;

    if(cv<limit){
        col.rgb = vec3(0,0,0);
    }

    

    color = col;
}