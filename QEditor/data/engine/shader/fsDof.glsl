#version 330 core

in vec2 UV;

out vec3 color;

uniform sampler2D colorTex;
uniform sampler2D depthTex;

uniform float blur;
uniform float focalZ;
uniform float focalRange;

void main(){

    
    float fz = texture(depthTex,UV).r;

    float fd = (focalZ - fz);

    fd = abs(fd);

    float bf = fd / focalRange;

    
vec3 fc = vec3(0,0,0);
int fi=0;
for(int y=-7;y<7;y++){
   for(int x=-7;x<7;x++){
	float bx = x * 0.002 * blur;
	float by = y * 0.002 * blur;
    bx = bx * bf;
    by = by * bf;

     vec2 nv = vec2(UV.x + bx,UV.y + by);

    float td = texture(depthTex,nv).r;



    if(abs(td-fz)>0.1){
        continue;
    }


     if(nv.x>=0.0 && nv.x<=1.0)
     {
        if(nv.y>=0.0 && nv.y<=1.0){
             fc=fc + texture(colorTex,nv).rgb;
            fi=fi+1;
        }
         }

     }
    }
    fc = fc / fi;


    
    
    color = fc;
}