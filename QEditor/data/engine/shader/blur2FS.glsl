#version 440 core

in vec2 UV;
in vec4 col;

out vec4 color;

uniform float blur;
uniform vec2 res;

uniform sampler2D tR;



void main(){
    vec2 nv = UV;
    


    



    float bf = blur;

    


    int x,y;

    x=0;
    y=0;
    int cc = 0;

    vec3 fc = vec3(0,0,0);

    for(x=-12;x<12;x++)
    {
        for(y=-12;y<12;y++)
        {
            vec2 buv = nv;
            buv.x = buv.x + (x*bf);
            buv.y = buv.y + (y*bf);
          
            if(buv.x<0 || buv.x>1 || buv.y<0 || buv.y>1){
            }else{
            fc = fc + texture2D(tR,buv).rgb;
            cc++;
            }
        }
    }
    //cc = cc / 2;

//    cc = cc / 4;

    fc = fc / cc;


    


    color = vec4(fc,col.a);
}