#version 450
#extension GL_ARB_separate_shader_objects : enable

layout(binding = 0) uniform sampler2D TEXTURE_0;

layout(location = 0) out vec4 outColor;

layout(location = 0) in vec3 vertex;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 coords;

#define ALPHA_TEST 1

void main() {
    vec3 LightDirection = vec3(0.25f, -1.0f, 0.5f);
    vec3 lightDir = normalize(-LightDirection);

    float diff = max(dot(normal, lightDir), 0.0);

    vec3 diffuse = vec3(1, 1, 1) * diff;
    vec3 ambient = vec3(0.5f, 0.5f, 0.5f);

    vec3 result = diffuse + ambient;

    vec4 color = texture(TEXTURE_0, coords);

#ifdef ALPHA_TEST
	if(color.a < 0.5)
		discard;
#endif

    outColor = color * vec4(result, 1.0);
}