#version 330 core

//in vec2 UV;

out vec3 color;

uniform mat4 proj,view;




uniform sampler2D tFrame;
uniform sampler2D tNorm;
uniform sampler2D tPos;
uniform sampler2D tExtra;

in vec2 UV;


uniform vec3 ssr_skyColor = vec3(0.0);
uniform int ssr_binarySearchCount = 30;
uniform int ssr_rayMarchCount = 100; // 60
uniform float ssr_step = 0.015; // 0.025
uniform float ssr_LLimiter = 0.17;
uniform float ssr_minRayStep = 0.02;

uniform vec3 bloom_vecThreshold = vec3(0.2126, 0.7152, 0.0722);
uniform float bloom_brightnessThreshold = 0.3;



#define getPosition(UV) texture(tPos, UV).xyz

vec2 binarySearch(inout vec3 dir, inout vec3 hitCoord, inout float dDepth) {
    float depth;

    vec4 projectedCoord;
 
    for(int i = 0; i < ssr_binarySearchCount; i++) {
        projectedCoord = proj * vec4(hitCoord, 1.0);
        projectedCoord.xy /= projectedCoord.w;
        projectedCoord.xy = projectedCoord.xy * 0.5 + 0.5;
 
        depth = getPosition(projectedCoord.xy).z;
 
        dDepth = hitCoord.z - depth;

        dir *= 0.5;
        if(dDepth > 0.0)
            hitCoord += dir;
        else
            hitCoord -= dir;    
    }

    projectedCoord = proj * vec4(hitCoord, 1.0);
    projectedCoord.xy /= projectedCoord.w;
    projectedCoord.xy = projectedCoord.xy * 0.5 + 0.5;
 
    return vec2(projectedCoord.xy);
}

vec2 rayCast(vec3 dir, inout vec3 hitCoord, out float dDepth) {
    dir *= ssr_step;
    
    for (int i = 0; i < ssr_rayMarchCount; i++) {
        hitCoord += dir;

        vec4 projectedCoord = proj* vec4(hitCoord, 1.0);
        projectedCoord.xy /= projectedCoord.w;
        projectedCoord.xy = projectedCoord.xy * 0.5 + 0.5; 

        float depth = getPosition(projectedCoord.xy).z;
        dDepth = hitCoord.z - depth;

        if((dir.z - dDepth) < 1.2 && dDepth <= 0.0) return binarySearch(dir, hitCoord, dDepth);
    }

    return vec2(-1.0);
}

#define scale vec3(.8, .8, .8)
#define k 19.19

vec3 hash(vec3 a) {
    a = fract(a * scale);
    a += dot(a, a.yxz + k);
    return fract((a.xxy + a.yxx)*a.zyx);
}

// source: https://www.standardabweichung.de/code/javascript/webgl-glsl-fresnel-schlick-approximation
#define fresnelExp 5.0

float fresnel(vec3 direction, vec3 normal) {
    vec3 halfDirection = normalize(normal + direction);
    
    float cosine = dot(halfDirection, direction);
    float product = max(cosine, 0.0);
    float factor = 1.0 - pow(product, fresnelExp);
    
    return factor;
}




void main(){

    float reflectionStrength = 0.7f;

    vec3 normal = vec3(texture(tNorm, UV));
    vec3 viewPos = getPosition(UV);

    vec3 worldPos = vec3(vec4(viewPos, 1.0) * inverse(view));
//    vec3 jitt = hash(worldPos) * texture(ssrValuesMap, texCoord).g;


    // Reflection vector
    vec3 reflected = normalize(reflect(normalize(viewPos), normalize(normal)));

    float spec = 0.7;

   vec3 hitPos = viewPos;
    float dDepth; 
    vec2 coords = rayCast(reflected * max(-viewPos.z, ssr_minRayStep), hitPos, dDepth);

    float L = length(getPosition(coords) - viewPos);
    L = clamp(L * ssr_LLimiter, 0, 1);
    float error = 1 - L;

    float fresnel = fresnel(reflected, normal);
    
    vec3 c2 = texture(tFrame, coords.xy).rgb * error * fresnel;

    // fragColor = vec3(fresnel);
    // return;
    vec3 fragCol = vec3(0.0);

    if (coords.xy != vec2(-1.0)) {
        fragCol = c2;

        vec3 rcol = texture(tFrame,UV).rgb;

        vec3 extra = texture(tExtra,UV).rgb;

        vec3 ncol = mix(rcol,fragCol,extra.r);  

        color = ncol;
        return;
    }

    vec3 extra = texture(tExtra,UV).rgb;

    fragCol = mix(texture(tFrame, UV), vec4(ssr_skyColor, 1.0), reflectionStrength).rgb;




    color = texture(tFrame,UV).rgb;



}
