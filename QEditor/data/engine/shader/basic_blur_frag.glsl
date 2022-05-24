#version 450 core
readonly restrict uniform layout(rgba8) image2D image;
layout(location=0) out vec4 color;
uniform vec4 drawCol;
in vec2 texc;
uniform vec2 texSize;
void main(void) {
    
    //color = vec4(gl_FragCoord.xy,0,1);

    int sx = int(texc.x-16);
    int sy = int(texc.y-16);


    vec4 col = vec4(0,0,0,1);

    int samples=0;

    for(int dy=sy;dy<(sy+16);dy++){
        for(int dx=sx;dx<(sx+16);dx++)
        {
            if(dx>0 && dy>0 && dx<texSize.x && dy<texSize.y)
            {
                vec4 ncol = imageLoad(image,ivec4(dx,dy,0,0).xy) * drawCol;
                col = col + ncol;
                samples++;
            }
        }
    }


    col /= samples;

//    vec4 col = imageLoad(image,ivec4(texc,0,0).xy) * drawCol;
    
    color = col;
    

}
