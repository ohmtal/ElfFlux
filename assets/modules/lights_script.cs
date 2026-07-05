//------------------------------------------------------------------------------
// Lights script implementation
//------------------------------------------------------------------------------
#define GLSL_VERSION 330
#define VEC3_ZERO "0 0 0"

#define MAX_LIGHTS 4
#define LIGHT_DIRECTIONAL 0
#define LIGHT_POINT 1

//----------------------------------------------------------------------
function createlights_script() {
    %obj = new ScriptObject() {
        class = "LightsDemo";
    };
    return %obj;
}
//----------------------------------------------------------------------
//----------------------------------------------------------------------
function UpdateLightValues(/*Shader*/ /*%shader,*/ /*Light*/ %light)
{
    // Send to shader light enabled state and type
    %shader = %light.shader;

    SetShaderValue(%shader, %light.enabledLoc, %light.enabled, SHADER_UNIFORM_INT);
    SetShaderValue(%shader, %light.typeLoc, %light.type, SHADER_UNIFORM_INT);

    // Send to shader light position values
    // float position[3] = { %light.position.x, %light.position.y, %light.position.z };
    SetShaderValue(%shader, %light.positionLoc, %light.position, SHADER_UNIFORM_VEC3);

    // Send to shader light target position values
    // float target[3] = { %light.target.x, %light.target.y, %light.target.z };
    SetShaderValue(%shader, %light.targetLoc, %light.target, SHADER_UNIFORM_VEC3);

    // Send to shader light color values
    // float color[4] = { (float)%light.color.r/(float)255, (float)%light.color.g/(float)255,
    //     (float)%light.color.b/(float)255, (float)%light.color.a/(float)255 };

    %color.r = %light.color.r / 255;
    %color.g = %light.color.g / 255;
    %color.b = %light.color.b / 255;
    %color.a = %light.color.a / 255;

    SetShaderValue(%shader, %light.colorLoc, %color, SHADER_UNIFORM_VEC4);
}
//----------------------------------------------------------------------
function CreateLight(/*int*/ %type,
        /*Vector3*/ %position,
        /*Vector3*/ %target,
        /*Color*/ %color,
        /*Shader*/ %shader
)
{
    if ($lightsCount >= MAX_LIGHTS) return 0;

    %light = new ScriptObject();

    %light.enabled = true;
    %light.type = %type;
    %light.position = %position;
    %light.target = %target;
    %light.color = %color;
    %light.shader = %shader;

    // NOTE: Lighting shader naming must be the provided ones
    %light.enabledLoc = GetShaderLocation(%shader, strFormat("lights[%i].enabled", $lightsCount));
    %light.typeLoc = GetShaderLocation(%shader, strFormat("lights[%i].type", $lightsCount));
    %light.positionLoc = GetShaderLocation(%shader, strFormat("lights[%i].position", $lightsCount));
    %light.targetLoc = GetShaderLocation(%shader, strFormat("lights[%i].target", $lightsCount));
    %light.colorLoc = GetShaderLocation(%shader, strFormat("lights[%i].color", $lightsCount));

    UpdateLightValues( %light );

    $lightsCount++;

    return %light;
}
//----------------------------------------------------------------------
//----------------------------------------------------------------------
function LightsDemo::onAdd(%this) {
    // ---- camera
    %this.camera = new Camera3DObject() {
        position = "2.0 4.0 6.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 45.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type
    };

    // ---- shader
    // Load basic lighting shader

    %this.shader = LoadShader(
        "assets/shaders/glsl" @ GLSL_VERSION @ "/lighting.vs",
        "assets/shaders/glsl" @ GLSL_VERSION @ "/lighting.fs"
    );
    echo("** Loader shaderId:" SPC %this.shader);

    %this.viewPosLoc = GetShaderLocation(%this.shader, "viewPos");
    SetShaderLocation(%this.shader, SHADER_LOC_VECTOR_VIEW, %this.viewPosLoc);

    // Ambient light level (some basic lighting)
    %this.ambientLoc = GetShaderLocation(%this.shader, "ambient");
    %ambient = "0.1 0.1 0.1 1.0";
    SetShaderValue(%this.shader, %this.ambientLoc, %ambient, SHADER_UNIFORM_VEC4);

    // ---- lights
    $lightsCount = 0;

    %this.l0 = CreateLight(LIGHT_POINT, "-2 1 -2", VEC3_ZERO, ORANGE, %this.shader);
    %this.l1 = CreateLight(LIGHT_POINT, "2 1 2"  , VEC3_ZERO, RED   , %this.shader);
    %this.l2 = CreateLight(LIGHT_POINT, "-2 1 2" , VEC3_ZERO, GREEN , %this.shader);
    %this.l3 = CreateLight(LIGHT_POINT, "2 1 -2" , VEC3_ZERO, BLUE  , %this.shader);

    %this.l0.caption = "O";
    %this.l1.caption = "R";
    %this.l2.caption = "G";
    %this.l3.caption = "B";

    // check lights loaded:
    for (%i = 0; %i < MAX_LIGHTS; %i++)
    {
        %light = %this.l[%i];
        echo("LIGHT #" @ %i SPC (isObject(%light) ? "OK" : "FAILED"));
    }

    //----- gui
    %this.gui = new Gui() {
        class ="LightGui";
    };


    return true;
}
//----------------------------------------------------------------------
function LightsDemo::OnRemove(%this) {

    %this.gui.delete();
    %this.camera.delete();
    UnloadShader(%this.shader);
    for (%i = 0; %i < MAX_LIGHTS; %i++)
    {
        %light = %this.l[%i];
        if (isObject(%light)) %light.delete();
    }
}
//----------------------------------------------------------------------
function LightGui::LightBox(%this, %light) {
    if (%this.CheckBox( %light.caption,  %light.enabled)) {
        %light.enabled = !  %light.enabled;
        UpdateLightValues( %light );
    }
}
//----------------------------------------------------------------------
function LightsDemo::Render(%this) {
    ClearBackground("10 10 40 255");
    %cam = %this.camera;
    %shader =  %this.shader;
    %camPos = %this.camera.position;
    %locIndex = %this.viewPosLoc;
    SetShaderValue(%shader, %locIndex, %camPos, SHADER_UNIFORM_VEC3);

    %cam.update(CAMERA_ORBITAL);
    BeginMode3D(%cam);
    BeginShaderMode( %shader );
    DrawPlane(VEC3_ZERO, "10.0 10.0", WHITE);
    // DrawCube(VEC3_ZERO, 2.0, 4.0, 2.0, WHITE);
    DrawSphere("0 1.0 0", 1.0, WHITE);
    EndShaderMode();
    // Draw spheres to show where the lights are
    for (%i = 0; %i < MAX_LIGHTS; %i++)
    {
        %light = %this.l[%i];
        if (!isObject(%light)) continue;
        // %light.update();
        if (%light.enabled) DrawSphereEx(%light.position, 0.2, 8, 8, %light.color);
        else DrawSphereWires( %light.position, 0.2, 8, 8, ColorAlpha( %light.color, 0.3));
    }
    DrawGrid(10, 1.0);
    EndMode3D();

    %gui = %this.gui;
    %gui.Begin(10,10);
    %gui.Write("Lights and Gui Demo GLSL" SPC GLSL_VERSION, 20, WHITE);
    %gui.Separator(80);
    %gui.Write("LIGHTS:", 12, WHITE);

    %gui.LightBox(%this.l0);
    %gui.SameLine();
    %gui.LightBox(%this.l1);

    %gui.LightBox(%this.l2);
    %gui.SameLine();
    %gui.LightBox(%this.l3);

    %gui.Separator(80);


}



