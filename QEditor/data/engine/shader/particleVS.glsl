#version 410


// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec4 aCol;

uniform mat4 proj;
uniform mat4 model;
uniform mat4 view;

out float tt;
out vec3 tuv;
out vec4 oCol;


void main(){

  

    oCol = aCol;    

    gl_Position = proj * view *model* vec4(aPos,1.0);

}