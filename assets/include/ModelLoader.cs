// Modelloader
// NOTE: if a shader is set on the Model the drawing fails in a other module
// this happen in TerrainDemo => ModelViewer
// solution added => ResetModelMaterial
function ModelCache::loadModel(%this, %filename) {
    if (%filename $= "") return 0;
    %hash = getStringHash("MODEL_" @ %filename);
    %result = %this.getFieldValue(%hash);
    if (%result > 0) {
        return %result;
    }
    %result = LoadModel(%filename);
    %this.setFieldValue(%hash, %result);
    return %result;
}

function ModelCache::loadTexture(%this, %filename) {
    if (%filename $= "") return 0;
    %hash = getStringHash("TEXTURE_" @ %filename);
    %result = %this.getFieldValue(%hash);
    if (%result > 0) return %result;

    %result = LoadTexture(%filename);
    %this.setFieldValue(%hash, %result);
    return %result;
}

function ModelCache::loadAnimation(%this, %filename) {
    if (%filename $= "") return 0;
    %hash = getStringHash("ANIMATION_" @ %filename);
    %result = %this.getFieldValue(%hash);
    if (%result > 0) return %result;

    %result = LoadModelAnimations(%filename);
    %this.setFieldValue(%hash, %result);
    return %result;
}
// =============================================================================
function LoadModelResources(
        %modelFileName
        , %animationFilename // default ""
        , %textureFilename // default ""
        , %startAnim // default 0
        , %spawnPoint // default "0 0 0"
        , %fps //default 30
        , %class //default  ""
    ) {
    if (!isObject(ModelCache)) {
        new ScriptObject(ModelCache);
    }

    %model = ModelCache.loadModel(%modelFileName);
    echo("\t* loaded Model:" SPC %modelFileName SPC "id" SPC %model);
    if (%model == 0) return false;

    %texture = ModelCache.loadTexture(%textureFilename);
    echo("\t* loaded Texture:" SPC %textureFilename SPC "id" SPC %texture);
    %animation = ModelCache.loadAnimation(%animationFilename);
    echo("\t* loaded Animation:" SPC %animationFilename SPC "id" SPC %animation);

    //FIXME GPU animation !!! CPU one is slow ...
    %result = new ModelObject() {
        class = %class;
        Position = %spawnPoint $= "" ? "0 0 0" : %spawnPoint ;
        ModelId = %model;
        Scale = "1 1 1";
        AnimationBlockId = %animation;
        AnimationIndex = %startAnim $="" ? 0 : %startAnim;
        AnimationFPS = %fps $= "" ? 30 : %fps;
    };

    // fixme texture should be added to ModelObject -or better not - not sure
    %result.MaterialCount = 0;
    if (%texture != 0) {
        %result.MaterialCount = GetModelMaterialCount(%model, MATERIAL_MAP_ALBEDO);
        // set all ... to be sure .. WARNING works only when model have only on texture!
        for (%i = 0; %i < %result.MaterialCount ; %i++) {
            ResetModelMaterial(%model,%i);
            SetModelMapTexture(%model,%texture, %i, MATERIAL_MAP_ALBEDO);
        }

    }

    return %result;
}
