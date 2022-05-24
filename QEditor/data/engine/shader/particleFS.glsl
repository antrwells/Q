#version 410


in vec4 oCol;

uniform sampler2D tCol;

out vec4 color;

void main(){

  

    vec2 uv = vec2(0.5,0.5);


    vec4 fc = vec4(0,0,0,0);

    float alpha = texture(tCol,uv).a;


    fc.a = alpha;

    fc.rgb = texture2D(tCol,uv).rgb;

    fc = fc * oCol;

 


    color = fc;

}