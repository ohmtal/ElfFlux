// WaterPlane
//----------------------------------------------------------------------
#define WAVE_SPEED 2.0
#define NOISE_SCALE 16

// type lake or sea
// Vector3 size 50 50 150 150
function createWaterPlane(%type, %sunObject, %cameraObject, %size, %position) {

    %obj = new ScriptObject() {
        class = "WaterPlane";
        type = %type $= "" ? "lake" : %type;
        sun = %sunObject;
        cam = %cameraObject;
        size = %size $= "" ? "50 50 150 150" : %size;
        position = %position $= "" ? "0 0 0" : %position;
    };

    return %obj;
}
//----------------------------------------------------------------------
function WaterPlane::onAdd(%this) {
    // ---- shader
    // Load vertex_displacement shader

    %this.waterShader = LoadShader(
        "assets/shaders/custom/water_" @ %this.type @ ".vert",
        "assets/shaders/custom/water_" @ %this.type @ ".frag"
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
    %planeMesh =  GenMeshPlane(%this.size.x, %this.size.y, %this.size.width, %this.size.height); //GenMeshPlane(50, 50, 50, 50);
    %this.planeModel = LoadModelFromMesh(%planeMesh);

    SetModelMapTexture(%this.planeModel, %this.perlinNoiseMap, 0, MATERIAL_MAP_BRDF);

    // Set plane model material
    SetModelShader(%this.planeModel, %this.waterShader, 0);

    %this.timeLoc =  GetShaderLocation(%this.waterShader, "time");

    %this.time = 0;


    return true;
}
//----------------------------------------------------------------------
function WaterPlane::OnRemove(%this) {
    UnloadShader(%this.waterShader);
    UnloadModel(%this.planeModel);
    UnloadTexture(%this.perlinNoiseMap);

}
//----------------------------------------------------------------------
function WaterPlane::draw(%this, %dt) {

    BeginBlendMode(BLEND_ALPHA); // does nothing ?!
      DrawModel(%this.planeModel, %this.position, 1.0, RAYWHITE);
    EndBlendMode();
}
//----------------------------------------------------------------------
function WaterPlane::update(%this, %dt) {
    %floatColor = ColorToVector4(%this.sun.skyColor);
    SetShaderValue(%this.waterShader, %this.sunDirLoc, -%this.sun.sunDirection, SHADER_UNIFORM_VEC3);
    SetShaderValue(%this.waterShader, %this.sunColorLoc, %floatColor, SHADER_UNIFORM_VEC4);
    SetShaderValue(%this.waterShader, %this.viewPosLoc, %this.cam.position, SHADER_UNIFORM_VEC3);
    %this.time += %dt * WAVE_SPEED;
    SetShaderValue(%this.waterShader,%this.timeLoc, %this.time, SHADER_UNIFORM_FLOAT );
}
