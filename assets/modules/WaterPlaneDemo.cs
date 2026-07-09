exec("assets/include/CameraFree.cs");
//----------------------------------------------------------------------
#define WAVE_SPEED 2.0
#define WATER_TYPE lake
// lake (unfinished) or sea
#define NOISE_SCALE 16

function createWaterPlaneDemo() {
    %obj = new ScriptObject() {
        class = "WaterPlaneDemo";
    };

    return %obj;
}
//----------------------------------------------------------------------
function WaterPlaneDemo::onAdd(%this) {
    // ---- camera
    %this.camera = new Camera3DObject() {
        class = "CameraFree";
        position = "20.0 30.0 20.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 60.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type

        moveSpeed = 100.0;
        panSpeed  = 30.0;
    };

    // ---- shader
    %this.waterShader = LoadShader(
        "assets/shaders/custom/water_" @ WATER_TYPE @ ".vert",
        "assets/shaders/custom/water_" @ WATER_TYPE @ ".frag"
    );
    if (%this.waterShader == 0) return false;


    // Load perlin noise texture
    %perlinNoiseImage = GenImagePerlinNoise(512, 512, 0, 0, NOISE_SCALE); // NOTE 12
    %this.perlinNoiseMap = LoadTextureFromImage(%perlinNoiseImage);
    UnloadImage(%perlinNoiseImage);

    // Set shader uniform location
    %perlinNoiseMapLoc = GetShaderLocation(%this.waterShader, "perlinNoiseMap");

    SetShaderValue(%this.waterShader, %perlinNoiseMapLoc, MATERIAL_MAP_BRDF, SHADER_UNIFORM_INT);


    %this.sunDirLoc = GetShaderLocation(%this.waterShader, "sunDirection");
    %this.sunColorLoc = GetShaderLocation(%this.waterShader, "sunColor");
    %this.viewPosLoc = GetShaderLocation(%this.waterShader, "viewPos");


    // Create a plane mesh and model
    %planeMesh =  GenMeshPlane(50, 50, 150, 150); //GenMeshPlane(50, 50, 50, 50);
    %this.planeModel = LoadModelFromMesh(%planeMesh);

    SetModelMapTexture(%this.planeModel, %this.perlinNoiseMap, 0, MATERIAL_MAP_BRDF);

    // Set plane model material
    SetModelShader(%this.planeModel, %this.waterShader, 0);

    %this.timeLoc =  GetShaderLocation(%this.waterShader, "time");

    %this.time = 0;


    return true;
}
//----------------------------------------------------------------------
function WaterPlaneDemo::OnRemove(%this) {
    UnloadShader(%this.waterShader);
    UnloadModel(%this.planeModel);
    UnloadTexture(%this.perlinNoiseMap);
    %this.camera.delete();

}
//----------------------------------------------------------------------
function WaterPlaneDemo::Render(%this) {
    ClearBackground("80 60 80");
    %cam = %this.camera;

    %dt = GetFrameTime();
    %cam.update(%dt);

    //TODO change this when in terrain!
    SetShaderValue(%this.waterShader, %this.sunDirLoc, "0.5 1.0 0.3", SHADER_UNIFORM_VEC3);
    SetShaderValue(%this.waterShader, %this.sunColorLoc, "0.5 0.5 0.75 1.0", SHADER_UNIFORM_VEC4);
    SetShaderValue(%this.waterShader, %this.viewPosLoc, %cam.position, SHADER_UNIFORM_VEC3);


    %this.time += %dt * WAVE_SPEED;
    SetShaderValue(%this.waterShader,%this.timeLoc, %this.time, SHADER_UNIFORM_FLOAT );
    %cam.Begin();
     // does nothing ?! BeginBlendMode(BLEND_ALPHA);
      DrawModel(%this.planeModel, "0 2 0", 1.0, RAYWHITE);
     // EndBlendMode();
     DrawGrid(100, 1.0);
    %cam.End();

    DrawText("Water type:" SPC WATER_TYPE, 10, 10, 20, WHITE, true);
}
