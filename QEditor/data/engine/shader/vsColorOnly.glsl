#version 410
#extension GL_NV_bindless_texture : require 
#extension GL_NV_gpu_shader5 : require


// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aUv;
layout(location = 2) in vec3 aNorm;
layout(location = 3) in vec3 aBi;
layout(location = 4) in vec3 aTan;

uniform mat4 proj;
uniform mat4 model;
uniform mat4 view;
uniform vec3 viewPos;
uniform vec3 lPos;

out float tt;
out vec3 tuv;
out vec4 oCol;
out vec3 oFragPos;
out vec3 TLP;
out vec3 TVP;
out vec3 TFP;
out vec3 rPos;
out vec3 oNorm;
out vec3 reflectVector;
out vec3 pass_normal;
out mat3 normMat;
out mat3 TBN;
out vec3 Normal;
out vec3 Position;

void main(){

    tuv = aUv;
    
    oFragPos = vec3(model * vec4(aPos,1.0));

    mat3 normalMatrix = transpose(inverse(mat3(model)));

    normMat = normalMatrix;

    vec3 T = normalize(normalMatrix * aTan);
	vec3 N = normalize(normalMatrix * aNorm);

	vec4 worldPos = model * vec4(aPos,1.0);

	pass_normal = N;

    //
    
    vec3 unitNormal = normalize(N);

	vec3 viewVector = normalize(worldPos.xyz - viewPos);

	reflectVector = reflect(viewVector, unitNormal);

	oNorm = aPos;
	
	T = normalize(T-dot(T,N) *N);
	
	vec3 B = cross(N,T);

	TBN = transpose(mat3(T,B,N));


	TLP = TBN * lPos;
	TVP = TBN * viewPos;
	TFP = TBN * oFragPos;



	Normal = mat3(transpose(inverse(model))) * aNorm;
    Position = vec3(model * vec4(aPos, 1.0));

    gl_Position = proj * view *model* vec4(aPos,1.0);

}