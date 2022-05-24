#version 330 core

uniform mat4 model;
uniform vec3 camP;
uniform float minZ;
uniform float maxZ;




in vec3 fVert;
in vec3 fNorm;

// Ouput data
out vec3 color;

void main(){
 
 

    color = fVert;
}