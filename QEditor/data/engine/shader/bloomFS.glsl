#version 330 core

in vec2 UV;

out vec3 color;

uniform sampler2D tR1;
uniform sampler2D tR2;



void main(){

    vec3 cv = texture2D(tR1,UV).rgb;
    vec3 bv = texture2D(tR2,UV).rgb;
    cv = cv * 1.5;

    vec3 fc = cv+bv;
   

    color = fc;
}