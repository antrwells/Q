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
uniform mat4 view2;



out vec3 fVert;
out vec3 fNorm;
out vec2 fUV;

void main(){

	
	fVert = vec3(model * vec4(aPos,1.0));

	fUV = aTexCoords;

	mat4 normalMatrix = view * model;
    	vec3 N = vec3(normalize(normalMatrix * vec4(aNormal,0.0)));
    fNorm = N;


		gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

