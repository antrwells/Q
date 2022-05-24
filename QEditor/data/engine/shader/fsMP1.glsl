#version 330 core
#extension GL_NV_shadow_samplers_cube : enable


uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightCol;
uniform vec3 lightSpec;
uniform float lightDepth; 
uniform float lightRange;
uniform vec3 ambCE;
uniform vec3 matSpec;
uniform float matS;
uniform vec3 matDiff;
uniform vec3 envFactor;
uniform vec3 viewVec;
// Interpolated values from the vertex shaders

in vec3 fragPos;
in vec2 texCoords;
in vec3 TLP;
in vec3 TVP;
in vec3 TFP;
in vec3 vNorm;
in vec3 pass_normal;
in vec3 reflectVector;
in mat3 normMat;

// Ouput data
out vec3 colorout;

uniform bool texCol;
uniform bool texNorm;
uniform bool texSpec;
uniform bool texEnv;

// Values that stay constant for the whole mesh.
uniform sampler2D tC;
uniform sampler2D tN;
uniform samplerCube tS;
uniform sampler2D tSpec;
uniform samplerCube tEM;


    vec3 sampleOffsetDirections[20] = vec3[]
(
   vec3( 1,  1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1,  1,  1), 
   vec3( 1,  1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1,  1, -1),
   vec3( 1,  1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1,  1,  0),
   vec3( 1,  0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1,  0, -1),
   vec3( 0,  1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0,  1, -1)
);   

void main(){
 
    float dis = abs(length(fragPos-lightPos));

    float dv = dis/lightRange;

    if(dv>1.0) dv = 1.0;
    dv = 1.0-dv;





    vec3 normal = vec3(0.5,0.5,1);
    
    if(texNorm){
        normal = texture2D(tN,texCoords).rgb;
    }
    normal = normalize(normal * 2.0 - 1.0);

    vec3 ref_Norm = reflectVector;


    vec3 color = matDiff;
    
    if(texCol){
        color = texture2D(tC,texCoords).rgb * matDiff;
    }
 

    vec3 lightDir = normalize(TLP - TFP);

    float diff = max(dot(lightDir,normal),0.0);

    vec3 diffuse = (diff * lightCol);


    vec3 viewDir = normalize(TVP-TFP);
    vec3 reflectDir = reflect(-lightDir,normal);
    vec3 halfwayDir = normalize(lightDir+viewDir);

    float spec = pow(max(dot(normal,halfwayDir),0.0),32.0);

    spec = spec;

    vec3 specular = ((lightSpec) * spec); 

    //Shadows
    float cosTheta = abs( dot( normalize(viewVec), pass_normal) );
    float fresnel = pow(1.0 - cosTheta, 4.0);


    float shadow = 0.0;
    float bias = 0.15f;
    int samples = 18;
    float viewDistance = length(viewPos - fragPos);
    float diskRadius = 0.003f;
    vec3 fragToLight = fragPos - lightPos;
    float currentDepth = length(fragToLight);
    float ld2 = currentDepth/lightRange;
    if(ld2>1.0) ld2 = 1.0;
    ld2 = 1.0 - ld2;
    fragToLight = normalize(fragToLight);
    for(int i=0;i<samples;i++){

        float closestDepth = textureCube(tS,fragToLight + sampleOffsetDirections[i] * diskRadius).r;
        closestDepth *= lightDepth;
        if((currentDepth - bias) > closestDepth){
            shadow += 1.0;
        }

    }
    shadow /= float(samples);
    shadow = 1.0 - shadow;

    color = color * dv;
    diffuse = diffuse * dv;
    specular = specular * dv;

    vec3 specCol = vec3(0.5,0.5,0.5);

    if(texSpec){

        specCol = texture2D(tSpec,texCoords).rgb;

    }
vec3 eM = vec3(0,0,0);
if(texEnv){

    eM = texture(tEM, normalize(reflectVector)).rgb;
  
    eM = eM * envFactor;

    vec3 rE;

    rE.x = 1.0 - envFactor.x;
    rE.y = 1.0 - envFactor.y;
    rE.z = 1.0 - envFactor.z;

    rE = rE * 0.5;

    diffuse = diffuse * rE;

}

  //  specular = vec3(1,1,1);
//    specCol = vec3(0,0,0);



    //vec3 ic =  (color * diffuse);
    //ic = ic +(specular * specCol);
   // ic = ic * shadow;
//    ic = ic * vec3(1.0-envFactor.x,1.0-envFactor.y,1.0-envFactor.z);

    vec3 finalCol = ((color * diffuse * dv) * shadow)+(specular*shadow);


    colorout = finalCol;





    /*
    vec3 ld = (fragPos-lightPos);

    float closeD = texture(tS,ld).r;

    closeD *= lightDepth;

    float curD = length(ld);


    float shade = 0.0;

    curD = curD - 2.0f;

    if(curD<closeD){
        shade = 1.0;
    }
    else{
        shade =  0.0;
    }


colorout =((diffuse) * vec3(shade,shade,shade))+specular;
*/
return;
 

    //colorout = textureCube(tS,normal).rgb;  //(color*diff*lightCol)+specular;




 
}
