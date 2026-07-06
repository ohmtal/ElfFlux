// #version 330
//
// in vec3 fragPosition;
// in vec2 fragTexCoord;
// in vec3 fragNormal;
//
// uniform sampler2D perlinNoiseMap;
// uniform float time;
//
// uniform vec3 sunDirection;
// uniform vec4 sunColor;
// uniform vec3 viewPos;
//
// out vec4 finalColor;
//
// void main()
// {
//     // Lake water color palette (Deep dark teal to calm blue)
//     vec4 lakeDark = vec4(0.01, 0.12, 0.22, 0.9);
//     vec4 lakeLight = vec4(0.05, 0.28, 0.42, 0.9);
//
//     // TRICK: Scroll the noise map in two completely different diagonal ways
//     // This creates independent ripples instead of a grid
//     vec2 speedA = vec2(time * 0.015, time * 0.005);
//     vec2 speedB = vec2(time * -0.008, time * 0.012);
//
//     // Scale the texture coordinate to make the ripples smaller and more frequent (lake style)
//     vec2 uvA = fragTexCoord * 8.0 + speedA;
//     vec2 uvB = fragTexCoord * 12.5 + speedB;
//
//     // Fetch noise values
//     float noiseA = texture(perlinNoiseMap, uvA).r;
//     float noiseB = texture(perlinNoiseMap, uvB).r;
//
//     // Create a dynamic normal perturbation based on the noise slopes
//     // This bends the surface normal dynamically without moving the vertex geometry
//     vec3 rippleNormal = vec3(
//         (noiseA - 0.5) * 0.2 + (noiseB - 0.5) * 0.15,
//         1.0, // Up direction remains dominant
//         (noiseA - 0.5) * 0.15 - (noiseB - 0.5) * 0.2
//     );
//
//     // Combine the base mesh normal with our new ripple normal
//     vec3 normal = normalize(fragNormal + rippleNormal);
//     vec3 lightDir = normalize(sunDirection);
//
//     // Fresnel effect: Water is more reflective at grazing angles, more transparent when looking straight down
//     vec3 viewDir = (viewPos != vec3(0.0)) ? normalize(viewPos - fragPosition) : vec3(0.0, 1.0, 0.2);
//     float fresnel = pow(1.0 - max(dot(normal, viewDir), 0.0), 4.0);
//
//     // Mix base water color based on fresnel (edges reflect more sky light)
//     vec4 waterColor = mix(lakeDark, lakeLight, fresnel);
//
//     // Specular Highlight (The golden/white sun glitter on the lake ripples)
//     vec3 reflectDir = reflect(-lightDir, normal);
//     // 128.0 exponent makes the sun highlights very tight, shiny and sharp like actual lake water
//     float specularFactor = pow(max(dot(viewDir, reflectDir), 0.0), 128.0);
//     vec3 specularLight = sunColor.rgb * specularFactor * 2.0;
//
//     // Ambient sky light contribution
//     vec3 ambientLight = vec3(0.05, 0.1, 0.15);
//     vec3 diffuseLight = sunColor.rgb * max(dot(normal, lightDir), 0.0) * 0.3;
//
//     // Combine everything
//     vec3 finalRGB = waterColor.rgb * (ambientLight + diffuseLight) + specularLight;
//
//     finalColor = vec4(finalRGB, waterColor.a);
// }


// NOTE: Stormy

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
    vec3 lightDir = normalize(sunDirection); // Richtung der Sonne

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
