#version 300 es
precision mediump float;

in vec3 fragPosition;
in vec2 fragTexCoord;
in vec3 fragNormal;
in vec4 fragColor;

uniform sampler2D texture0;
uniform vec4 colDiffuse;

uniform vec3 sunDirection;
uniform vec4 sunColor;

out vec4 finalColor;

void main()
{
    vec4 texelColor = texture(texture0, fragTexCoord) * colDiffuse * fragColor;

    // fix for alpha transparency
    if (texelColor.a < 0.1)
    {
        discard;
    }

    vec3 lightDir = normalize(-sunDirection);
    vec3 normal = normalize(fragNormal);

    float diffFactor = max(dot(normal, lightDir), 0.0);

    vec3 ambientLight = vec3(0.2, 0.2, 0.25);
    vec3 diffuseLight = sunColor.rgb * diffFactor;

    vec3 finalLight = ambientLight + diffuseLight;

    finalColor = vec4(texelColor.rgb * finalLight, texelColor.a);
}


// #version 330
//
// in vec3 fragPosition;
// in vec2 fragTexCoord;
// in vec3 fragNormal;
// in vec4 fragColor;
//
// uniform sampler2D texture0;
// uniform vec4 colDiffuse;
//
// uniform vec3 sunDirection;
// uniform vec4 sunColor;
//
// out vec4 finalColor;
//
// void main()
// {
//     vec4 texelColor = texture(texture0, fragTexCoord) * colDiffuse * fragColor;
//
//     // fix for alpha transparency
//     if (texelColor.a < 0.1)
//     {
//         discard;
//     }
//
//     vec3 lightDir = normalize(-sunDirection);
//     vec3 normal = normalize(fragNormal);
//
//     float diffFactor = max(dot(normal, lightDir), 0.0);
//
//     vec3 ambientLight = vec3(0.2, 0.2, 0.25);
//     vec3 diffuseLight = sunColor.rgb * diffFactor;
//
//     vec3 finalLight = ambientLight + diffuseLight;
//
//     finalColor = vec4(texelColor.rgb * finalLight, texelColor.a);
// }
