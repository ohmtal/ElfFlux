#version 330

in vec3 vertexPosition;
in vec2 vertexTexCoord;
in vec3 vertexNormal;
in vec4 vertexColor;

uniform mat4 mvp;
uniform mat4 matModel;
uniform mat4 matNormal;
uniform float time;

out vec3 fragPosition;
out vec2 fragTexCoord;
out vec3 fragNormal;

// Testparams :
const float waveHeight = 0.5;
//

void main()
{
    float tinyWave = sin(vertexPosition.x * 0.2 + time) * cos(vertexPosition.z * 0.2 + time) * waveHeight;
    vec3 displacedPosition = vertexPosition + vec3(0.0, tinyWave, 0.0);

    fragPosition = vec3(matModel * vec4(displacedPosition, 1.0));
    fragTexCoord = vertexTexCoord;
    fragNormal = normalize(vec3(matNormal * vec4(vertexNormal, 1.0)));

    gl_Position = mvp * vec4(displacedPosition, 1.0);
}
