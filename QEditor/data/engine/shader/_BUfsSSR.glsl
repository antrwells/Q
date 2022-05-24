#version 330 core

in vec2 UV;

out vec3 color;

uniform mat4 pMatrix;
uniform mat4 InvPMatrix;

uniform sampler2D tDepth;
uniform sampler2D tNorm;
uniform sampler2D tFrame;





vec3 CalcViewPosition(in vec2 TexCoord)
{
    // Combine UV & depth into XY & Z (NDC)
    vec3 rawPosition                = vec3(TexCoord, texture(tDepth, TexCoord).r);

    // Convert from (0, 1) range to (-1, 1)
    vec4 ScreenSpacePosition        = vec4( rawPosition * 2 - 1, 1);

    // Undo Perspective transformation to bring into view space
    vec4 ViewPosition               = InvPMatrix * ScreenSpacePosition;

    // Perform perspective divide and return
    return                          ViewPosition.xyz / ViewPosition.w;
}

vec2 RayCast(vec3 dir, inout vec3 hitCoord, out float dDepth)
{
    dir *= 0.25f;  

    for(int i = 0; i < 20; ++i) {
        hitCoord               += dir; 

        vec4 projectedCoord     = pMatrix * vec4(hitCoord, 1.0);
        projectedCoord.xy      /= projectedCoord.w;
        projectedCoord.xy       = projectedCoord.xy * 0.5 + 0.5; 

        float depth             = CalcViewPosition(projectedCoord.xy).z;
        dDepth                  = hitCoord.z - depth; 

        if(dDepth < 0.0)
            return projectedCoord.xy;
    }

    return vec2(0.0f);
}


void main(){

    vec3 viewNorm = texture(tNorm,UV).rgb;
    vec3 viewPos = CalcViewPosition(UV);
  
     // Reflection vector
    vec3 reflected = normalize(reflect(normalize(viewPos.xyz), normalize(viewNorm))); 

   vec3 hitPos                 = viewPos.xyz;
    float dDepth; 
    float minRayStep            = 0.1f;
    vec2 coords                 = RayCast(reflected * max(minRayStep, -viewPos.z), hitPos, dDepth); 

    if(coords.x==0.0 && coords.y == 0.0){
        color = texture(tFrame,UV).rgb;
    }else{
    color = textureLod(tFrame,coords,0).rgb;
    }
}