// #version 330
//
// in vec3 vertexPosition;
// in vec2 vertexTexCoord;
// in vec3 vertexNormal;
// in vec4 vertexColor;
//
// uniform mat4 mvp;
// uniform mat4 matModel;
// uniform mat4 matNormal;
// uniform float time;
//
// out vec3 fragPosition;
// out vec2 fragTexCoord;
// out vec3 fragNormal;
//
// void main()
// {
//     float tinyWave = sin(vertexPosition.x * 0.2 + time) * cos(vertexPosition.z * 0.2 + time) * 0.1;
//     vec3 displacedPosition = vertexPosition + vec3(0.0, tinyWave, 0.0);
//
//     fragPosition = vec3(matModel * vec4(displacedPosition, 1.0));
//     fragTexCoord = vertexTexCoord;
//     fragNormal = normalize(vec3(matNormal * vec4(vertexNormal, 1.0)));
//
//     gl_Position = mvp * vec4(displacedPosition, 1.0);
// }




// NOTE: Stormy

#version 330

// Input vertex attributes
in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;

// Input uniform values
uniform mat4 mvp;
uniform mat4 matModel;
uniform mat4 matNormal;
uniform float time;
uniform sampler2D perlinNoiseMap;

// Output vertex attributes (to fragment shader)
out vec3 fragPosition;
out vec2 fragTexCoord;
out vec3 fragNormal;
out float height;

// Helper function to calculate wave height at a specific position
float getWaterHeight(vec2 uv, vec3 pos) {
    // Distinct speeds and directions to break any grid alignment
    vec2 coordA = uv * 1.0 + vec2(time * 0.015, time * 0.008);
    vec2 coordB = uv * 1.73 + vec2(time * -0.011, time * 0.023);

    // Micro-wave layer to add fine surface detail
    vec2 coordC = uv * 4.11 + vec2(time * 0.04, time * -0.035);

    float noiseA = texture(perlinNoiseMap, coordA).r;
    float noiseB = texture(perlinNoiseMap, coordB).r;
    float noiseC = texture(perlinNoiseMap, coordC).r;

    float baseWave = (noiseA + noiseB) * 0.5;
    float fineDetail = noiseC * 0.15;

    return (baseWave + fineDetail) * 0.5; // 3.5 = Overall wave height amplitude
}

void main()
{
    float displacement = getWaterHeight(vertexTexCoord, vertexPosition);
    vec3 displacedPosition = vertexPosition + vec3(0.0, displacement, 0.0);

    // Dynamic normal calculation
    float delta = 0.01;
    float heightX = getWaterHeight(vertexTexCoord + vec2(delta, 0.0), vertexPosition + vec3(delta, 0.0, 0.0));
    float heightZ = getWaterHeight(vertexTexCoord + vec2(0.0, delta), vertexPosition + vec3(0.0, 0.0, delta));

    vec3 tangentX = vec3(delta, heightX - displacement, 0.0);
    vec3 tangentZ = vec3(0.0, heightZ - displacement, delta);
    vec3 animatedNormal = normalize(cross(tangentZ, tangentX));

    // Fill output attributes
    fragPosition = vec3(matModel * vec4(displacedPosition, 1.0));
    fragTexCoord = vertexTexCoord;

    fragNormal = normalize(vec3(matNormal * vec4(animatedNormal, 0.0)));
    height = displacedPosition.y * 0.2;

    gl_Position = mvp * vec4(displacedPosition, 1.0);
}


