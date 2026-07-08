//----------------------------------------------------------------------
exec("assets/include/CameraFree.cs");
//----------------------------------------------------------------------
#define VEC3_ZERO "0 0 0"
//----------------------------------------------------------------------
function createModelViewer() {
    %obj = new ScriptObject() {
        class = "ModelViewer";
    };
    return %obj;
}
//----------------------------------------------------------------------
function ModelViewer::CleanUp(%this) {

    if (isObject(%this.curModel)) %this.curModelObject.delete();
    UnLoadModel(%this.model);
    %this.model = 0;
    UnloadTexture(%this.texture);
    %this.texture = 0;
    UnloadModelAnimations(%this.animation);
    %this.animation = 0;
}
//----------------------------------------------------------------------
function ModelViewer::LoadModelResources(%this, %modelFileName, %animationFilename, %textureFilename, %startAnim) {
    %this.CleanUp();

    %this.model = LoadModel(%modelFileName);
    if (%this.model == 0) return false;
    echo("\t* loaded Model:" SPC %modelFileName SPC "id" SPC %this.model);

    if (%textureFilename !$= "") {
        %this.texture = LoadTexture(%textureFilename);
        echo("\t* loaded Texture:" SPC %textureFilename SPC "id" SPC %this.texture);
    }
    if (%animationFilename !$= "") {
        %this.animation = LoadModelAnimations(%animationFilename);
        echo("\t* loaded Animation:" SPC %animationFilename SPC "id" SPC %this.animation);
    }


    %spawnPoint = "0 0 0";

    %this.curModelObject = new ModelObject() {
        Position = %spawnPoint;
        ModelId = %this.model;
        Scale = "1 1 1";
        AnimationBlockId = %this.animation;
        AnimationIndex = %startAnim $="" ? 0 : %startAnim;
        AnimationFPS = 30;
    };

    // fixme texture should be added to ModelObject
    %this.curModelObject.MaterialCount = 0;
    if (%this.texture != 0) {
        %cnt = GetModelMaterialCount(%this.model, MATERIAL_MAP_ALBEDO);
        %this.curModelObject.MaterialCount  = %cnt;
        // set all ... to be sure .. WARNING works only when model have only on texture!
        for (%i = 0; %i < %cnt ; %i++) {
            SetModelMapTexture(%this.model,%this.texture, %i, MATERIAL_MAP_ALBEDO);
        }

    }

}
//----------------------------------------------------------------------
function ModelViewer::load(%this,%id) {
    %this.mode *= 1;
    switch (%this.mode) {
        case 1: %this.loadBlockyChar(%id);
        default: %this.loadCubePet(%id);
    }
}
//----------------------------------------------------------------------
function ModelViewer::loadBlockyChar(%this, %id) {
    %this.path = "assets/models/kenney_blocky_characters/glb/";
    %chars = "a b c d e f g h i j k l m n o p q r";

    if (%id < 0 ) %id = getWordCount(%chars) -1;
    else if ( %id >= getWordCount(%chars) ) %id = 0;
    %this.modelName = "character-" @ getWord(%chars,%id) @ ".glb";
    %texture = "Textures/texture-" @ getWord(%chars,%id)  @ ".png";

    %this.LoadModelResources(%this.path @ %this.modelName,%this.path @ %this.modelName, %this.path @ %texture, 1 );
    %this.curId = %id;
}
//----------------------------------------------------------------------
function ModelViewer::loadCubePet(%this, %id) {

    %this.path = "assets/models/kenney_cube_pets/glb/";
    %this.models="animal-beaver.glb animal-bee.glb animal-bunny.glb animal-caterpillar.glb animal-cat.glb animal-chick.glb"
        SPC "animal-cow.glb animal-crab.glb animal-deer.glb animal-dog.glb animal-elephant.glb animal-fish.glb animal-fox.glb"
        SPC "animal-giraffe.glb animal-hog.glb animal-koala.glb animal-lion.glb animal-monkey.glb animal-panda.glb"
        SPC "animal-parrot.glb animal-penguin.glb animal-pig.glb animal-polar.glb animal-tiger.glb";

    if (%id < 0 ) %id = getWordCount(%this.models) -1;
    else if ( %id >= getWordCount(%this.models) ) %id = 0;

    %this.modelName = getWord(%this.models,%id);
    %this.LoadModelResources(%this.path @ %this.modelName,%this.path @ %this.modelName, %this.path @ "Textures/colormap.png", 1 );
    %this.curId = %id;


}
//----------------------------------------------------------------------
function ModelViewer::onAdd(%this) {
    %this.camera = new Camera3DObject() {
        class = "CameraFree";
        position = "2.0 1.0 6.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 60.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type

        moveSpeed = 10.0;
        panSpeed  = 30.0;
    };


    %this.gui = new Gui();

    // kenney pets:
    %this.mode = 1;
    %this.load(0);




    return true;
}
//----------------------------------------------------------------------
function ModelViewer::OnRemove(%this) {
    %this.CleanUp();
    %this.gui.delete();
    %this.camera.delete();
}
//----------------------------------------------------------------------
function ModelViewer::Render(%this) {
    ClearBackground(SKYBLUE);
    %cam = %this.camera;
    %obj = %this.curModelObject;
    %objIsObject = isObject(%obj);


    if (!$main.ConsoleGui.isOpen()) %cam.update(GetFrameTime());
    %cam.Begin();
    if (%objIsObject) %obj.draw();
    DrawGrid(10, 1.0);
    DrawPlane(VEC3_ZERO, "10.0 10.0", DARKGREEN);
    %cam.End();

    %gui = %this.gui;
    %gui.Begin(10,10);

    %gui.Write("Model:" SPC %this.modelName, 20, BLACK);
    %gui.Separator(80);
    %gui.Write(strFormat("~ %02d ~",%this.curId), 20, BLACK);
    if (%gui.Button(30, "<")) %this.load(%this.curId -1);
    %gui.SameLine();
    if (%gui.Button(30, ">")) %this.load(%this.curId +1);
    %gui.Separator(80);
    if (%objIsObject) {
        %count = %obj.getAnimationCount();
        %gui.Write("Animation:" SPC %obj.AnimationIndex SPC "/" SPC %count, 12, BLACK);
        %gui.Write(%obj.getAnimationName(), 20, BLACK);
        if (%gui.Button(30, "<")) {
            %obj.AnimationIndex =  (%obj.AnimationIndex-1) < 0 ?  %count - 1: %obj.AnimationIndex-1;
        }
        %gui.SameLine();
        if (%gui.Button(30, ">")) {
            %obj.AnimationIndex =  (%obj.AnimationIndex+1) >= %count ? 0 : %obj.AnimationIndex+1;

        }
        %gui.Separator(80);
        %gui.Write("Textures:" SPC %obj.MaterialCount,  12, BLACK);
    }
    %gui.Separator(80);
    if (%gui.Button(80, "Cube Pets")) { %this.mode = 0; %this.load(0); }
    if (%gui.Button(80, "Blocky Chars")) { %this.mode = 1;  %this.load(0); }

}
