#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoords;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aBiTangent;
layout(location = 4) in vec3 aTangent;

// Values that stay constant for the whole mesh.
uniform mat4 view;
uniform mat4 model;
uniform mat4 proj;



out vec3 fVert;
out vec3 fNorm;

void main(){

	
     mat4 tMat = view * model;

     vec3 fP = vec3(tMat * vec4(aPos,1.0));

    fVert = fP;


		gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

