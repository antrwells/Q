#version 330 core

in vec2 UV;
in vec4 col;


out vec4 color;

uniform sampler2D tDiffuse;
uniform float blurFactor;

void main(){


      vec4 tc = texture2D(tDiffuse,UV);

    vec2 buv = UV;
    vec3 cc = vec3(0,0,0);
    int samples = 0;

    for(int y=-8;y<8;y++){

        for(int x=-8;x<8;x++){

            vec2 nuv = buv;

            float xc = x;
            float yc = y;

            nuv.x = nuv.x + (xc*0.0028*blurFactor);
            nuv.y = nuv.y + (yc*0.0031*blurFactor);

            vec3 nc = texture2D(tDiffuse,nuv).rgb;

            cc = cc + nc;

            samples++;
        }

    }

    cc = cc / samples;

  //if(tc.a < 0.1) discard;


    color  = vec4(cc,1.0);



}