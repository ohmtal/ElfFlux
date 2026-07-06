
#define WAVE_SPEED 2.0
#define WATER_TYPE sea
// lake (unfinished) or sea
#define NOISE_SCALE 16

function createWaterPlane() {
    %obj = new ScriptObject() {
        class = "WaterPlane";
    };

    return %obj;
}
//----------------------------------------------------------------------
function WaterPlane::onAdd(%this) {
    // ---- camera
    %this.camera = new Camera3DObject() {
        position = "20.0 5.0 20.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 60.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type
    };

    // ---- shader
    // Load vertex_displacement shader

    %this.shader = LoadShader(
        "assets/shaders/custom/water_" @ WATER_TYPE @ ".vert",
        "assets/shaders/custom/water_" @ WATER_TYPE @ ".frag"
    );
    if (%this.shader == 0) return false;


    // Load perlin noise texture
    %perlinNoiseImage = GenImagePerlinNoise(512, 512, 0, 0, NOISE_SCALE); // NOTE 12
    %this.perlinNoiseMap = LoadTextureFromImage(%perlinNoiseImage);
    UnloadImage(%perlinNoiseImage);

    // Set shader uniform location
    %perlinNoiseMapLoc = GetShaderLocation(%this.shader, "perlinNoiseMap");

    SetShaderValue(%this.shader, %perlinNoiseMapLoc, MATERIAL_MAP_BRDF, SHADER_UNIFORM_INT);


    %this.sunDirLoc = GetShaderLocation(%this.shader, "sunDirection");
    %this.sunColorLoc = GetShaderLocation(%this.shader, "sunColor");
    %this.viewPosLoc = GetShaderLocation(%this.shader, "viewPos");


    // Create a plane mesh and model
    %planeMesh =  GenMeshPlane(50, 50, 150, 150); //GenMeshPlane(50, 50, 50, 50);
    %this.planeModel = LoadModelFromMesh(%planeMesh);

    SetModelMapTexture(%this.planeModel, %this.perlinNoiseMap, 0, MATERIAL_MAP_BRDF);

    // Set plane model material
    SetModelShader(%this.planeModel, %this.shader, 0);

    %this.timeLoc =  GetShaderLocation(%this.shader, "time");

    %this.time = 0;


    return true;
}
//----------------------------------------------------------------------
function WaterPlane::OnRemove(%this) {
    UnloadShader(%this.shader);
    UnloadModel(%this.planeModel);
    UnloadTexture(%this.perlinNoiseMap);

}
//----------------------------------------------------------------------
function WaterPlane::Render(%this) {
    ClearBackground("80 60 20");
    %cam = %this.camera;
    %cam.update(CAMERA_FREE);

    //TODO change this when in terrain!
    SetShaderValue(%this.shader, %this.sunDirLoc, "0.5 1.0 0.3", SHADER_UNIFORM_VEC3);
    SetShaderValue(%this.shader, %this.sunColorLoc, "1.0 0.9 0.8 1.0", SHADER_UNIFORM_VEC4);
    SetShaderValue(%this.shader, %this.viewPosLoc, %cam.position, SHADER_UNIFORM_VEC3);


    %this.time += GetFrameTime() * WAVE_SPEED;
    SetShaderValue(%this.shader,%this.timeLoc, %this.time, SHADER_UNIFORM_FLOAT );
    %cam.Begin();
     // does nothing ?! BeginBlendMode(BLEND_ALPHA);
      DrawModel(%this.planeModel, "0 0 0", 1.0, RAYWHITE);
     // EndBlendMode();
     DrawGrid(100, 1.0);
    %cam.End();

    DrawText("Vertex displacement", 10, 10, 20, WHITE, true);
    DrawFPS(10, 40);
}
