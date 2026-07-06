#version 330

in vec3 fragPosition;
in vec2 fragTexCoord;
in vec3 fragNormal;
in float height;

uniform vec3 sunDirection;
uniform vec4 sunColor;


uniform vec3 viewPos;

out vec4 finalColor;

void main()
{
    vec4 deepWater = vec4(0.005, 0.15, 0.35, 0.85);
    vec4 shallowWater = vec4(0.05, 0.45, 0.65, 0.85);

    float mixFactor = clamp(height * 2.0, 0.0, 1.0);
    vec4 waterBaseColor = mix(deepWater, shallowWater, mixFactor);

    vec3 normal = normalize(fragNormal);
    vec3 lightDir = normalize(sunDirection);

    float diffuseFactor = max(dot(normal, lightDir), 0.0);
    vec3 diffuseLight = sunColor.rgb * diffuseFactor;

    vec3 viewDir = (viewPos != vec3(0.0)) ? normalize(viewPos - fragPosition) : vec3(0.0, 1.0, 0.2);
    vec3 reflectDir = reflect(-lightDir, normal);

    float specularFactor = pow(max(dot(viewDir, reflectDir), 0.0), 64.0);
    vec3 specularLight = sunColor.rgb * specularFactor * 1.5;

    vec3 ambientLight = vec3(0.1, 0.2, 0.3);

    vec3 finalRGB = waterBaseColor.rgb * (ambientLight + diffuseLight) + specularLight;

    finalColor = vec4(finalRGB, waterBaseColor.a);
}
