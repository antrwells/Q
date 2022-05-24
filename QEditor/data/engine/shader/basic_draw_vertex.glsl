#version 450 core
layout(location=0) in vec2 coord;
layout(location=1) in vec2 texCoord;
uniform mat4 proj;
uniform vec2 texSize;

out vec2 texc;
void main(void) {

     gl_Position = proj * vec4(coord, 0.0, 1.0);
     
     float nx = texCoord.x * texSize.x;
     float ny = texCoord.y * texSize.y;

     texc =  vec2(nx,ny);


}