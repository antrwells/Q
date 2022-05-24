#version 330 core

uniform mat4 model;
uniform vec3 camP;
uniform float minZ;
uniform float maxZ;

uniform sampler2D tExtra;




in vec3 fVert;
in vec3 fNorm;
in vec2 fUV;

// Ouput data
out vec3 color;

void main(){
 
   vec3 col = texture(tExtra,fUV).rgb;

    color = col;
}