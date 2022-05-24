#version 330 core

in vec2 UV;


out vec4 color;

uniform sampler2D tShadow;

void main(){

    float sv = 0.0;

    float sams = 0;

   

    for(float y=-5;y<5;y++){

        for(float x=-5;x<5;x++)
        {

            vec2 nuv = UV;

            nuv.x += (x)*0.004;
            nuv.y += (y)*0.004;

            sv = sv += texture2D(tShadow,nuv).r;
            sams=sams+1;

        }

    }

    sv = sv / sams;

    
     color = vec4(sv,sv,sv,1.0);    



}