#version 410

uniform mat4 model;
uniform sampler2D tEmissive;




in vec3 fVert;
in vec2 tuv;
// Ouput data
out vec4 color;

void main(){
 
    vec2 nuv = tuv;
    nuv.y = 1.0 - nuv.y;

    vec4 em = texture2D(tEmissive,nuv).rgba;

    

    color = em;
}