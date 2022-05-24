#version 450 core
readonly restrict uniform layout(rgba8) image2D image;
layout(location=0) out vec4 color;
uniform vec4 drawCol;
in vec2 texc;
void main(void) {
    
    //color = vec4(gl_FragCoord.xy,0,1);



    color = imageLoad(image, ivec4(texc,0,0).xy) * drawCol;
    

}
