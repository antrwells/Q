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
uniform mat4 modelTurn;

uniform vec3 lightPos;
uniform vec3 viewPos;


// Output data ; will be interpolated for each fragment.
out vec3 fragPos;

out vec2 texCoords;
out vec3 TLP;
out vec3 TVP;
out vec3 TFP;
out vec3 rPos;
out vec3 vNorm;
out vec3 reflectVector;
out vec3 pass_normal;
out mat3 normMat;
out mat3 TBN;
void main(){

	fragPos = vec3(model * vec4(aPos,1.0));
	texCoords = aTexCoords;

	mat3 normalMatrix = transpose(inverse(mat3(model)));

	normMat = normalMatrix;

    vec3 T = normalize(normalMatrix * aTangent);
	vec3 N = normalize(normalMatrix * aNormal);

	vec4 worldPos = model * vec4(aPos,1.0);

	pass_normal = N;

vec3 unitNormal = normalize(N);

	vec3 viewVector = normalize(worldPos.xyz - viewPos);

	reflectVector = reflect(viewVector, unitNormal);

	vNorm = aPos;
	
	T = normalize(T-dot(T,N) *N);
	
	vec3 B = cross(N,T);

	TBN = transpose(mat3(T,B,N));

	TLP = TBN * lightPos;
	TVP = TBN * viewPos;
	TFP = TBN * fragPos;
	


	gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

