exec("assets/include/CameraFree.cs");
exec("assets/include/Sun.cs");
exec("assets/include/WaterPlane.cs");


//----------------------------------------------------------------------
// ---- Global Modeldefs
// TreeBuilder as global reusable model:
if ( $TreeTrunkModel * 1 == 0)
{
    $TreeTrunkModel = GenModelCube(0.4, 2.0, 0.4);
    $TreeCrownModel = GenModelSphere(1.2, 16, 16);

    SetModelMapColor($TreeTrunkModel, "100 65 30 255", 0, 0);
    SetModelMapColor($TreeCrownModel, "40 120 40 255", 0, 0);


    $SunBillboardModel = GenModelSphere(10.0, 16, 16); // big one
    SetModelMapColor($SunBillboardModel, "255 255 200 255", 0, 0); // yellow


    $AppleTreeModel = LoadModel("assets/3d/appletree/appletree.obj");

}
//----------------------------------------------------------------------
function createTerrainDemo() {
    %obj = new ScriptObject() {
        class = "TerrainDemo";
    };

    return %obj;
}
//----------------------------------------------------------------------
function TerrainDemo::onAdd(%this) {
    // keep objects here to cleanup, we also have all
    // Terrain/Model objects in ClientContainer
    %this.levelObjects = new SimSet();

    // ---- camera
    %this.camera = new Camera3DObject(CAMERA) {
        class = "CameraFree";
        position = "-36.0 41.0 -57.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 45.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type

        moveSpeed = 100.0;
        panSpeed  = 30.0;
    };

    // ---- Terrain
    %terSize = 1024 * 2;
    %yVal = %terSize / 256 * 16;

    %this.terrain = new TerrainObject(TERRAIN) {
        HeightMapFilename = "assets/terrain/ohmtal_HM.png";
        // HeightMapFilename = "assets/terrain/ohmtal_HM_1024.png";
        // HeightMapFilename = "assets/terrain/Tenerife_HM_1024.png";

        Size =  %terSize SPC %yVal SPC %terSize;
    };

    warn("Terrain size is: " SPC %this.terrain.Size);
    %this.terrainRadiusNeg =  %this.terrain.Size / -2.0;
    %this.terrain.Position = %this.terrainRadiusNeg SPC 0 SPC %this.terrainRadiusNeg;
    %this.levelObjects.add(%this.terrain);




    // ---- Sun
    %this.Sun = createSun(%this.terrainRadiusNeg, $SunBillboardModel,"-0.5 -1.0 -0.2");
    if (!isObject(%this.Sun)) return false;
    %this.levelObjects.add(%this.Sun);


    SetModelShader(%this.terrain.getModelId(), %this.Sun.sunShader, 0);

    // add/update shader on static models
    SetModelShader($TreeTrunkModel, %this.Sun.sunShader, 0);
    SetModelShader($TreeCrownModel, %this.Sun.sunShader, 0);


    // water plane
    %this.waterPlane = createWaterPlane("lake"
        , %this.Sun
        , %this.camera
        , %terSize SPC %terSize SPC "150 150"
        , "0 15 0"
        // , %this.terrainRadiusNeg SPC 30 SPC %this.terrainRadiusNeg
    );
    %this.levelObjects.add(%this.waterPlane);


    // --- Apple Tree Test -----
    %count = GetModelMaterialCount($AppleTreeModel, MATERIAL_MAP_DIFFUSE);
    for (%i = 0; %i < %count; %i++) SetModelShader($AppleTreeModel, %this.Sun.sunShader, %i);

    %this.appleTree = new ModelObject() {
        Position = Vector3add(%this.camera.position, "120 0 120"); //for testing )
        ModelId = $AppleTreeModel;
        Scale = "3 3 3";
    };
    %this.levelObjects.add(%this.appleTree);
    %this.DropToGround(%this.appleTree);


    // player Test !!!

    // global! -  keep in memory until app ends ... (auto cleaned up by resource manager)
    if ($kenneyModel * 1 == 0) {
        %path = "assets/models/kenney_animated-characters-retro/";
        %file = "characterMedium.gltf";
        $kenneyModel = LoadModel(%path @ %file);
        $kennyAnimations = LoadModelAnimations(%path @ %file);
        $skinZombie = LoadTexture(%path @ "Skins/zombieMaleA.png");

        /*! set a texture for a model material map like model.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texture;SetModelMapTexture($model, $texture) << matIndex default 0,  matMap default MATERIAL_MAP_DIFFUSE  */
        // bool SetModelMapTexture( int modelId, int textureId, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}

        SetModelMapTexture($kenneyModel,$skinZombie/*, 0, MATERIAL_MAP_ALBEDO*/); // << model.materials[0].maps[MATERIAL_MAP_ALBEDO].texture = skinZombie;
        SetModelMapColor($kenneyModel, WHITE);
    }

    %spawnPoint = "-26 17 -102";
    %this.player = new ModelObject(Player) {
        Position = %spawnPoint;
        ModelId = $kenneyModel;
        Scale = "0.01 0.01 0.01";
        AnimationBlockId = $kennyAnimations;
        AnimationIndex = 2; //run
        AnimationFPS = 15;
    };

    // mount test FIXME doing strange things !! :: doubled and mount matrix not correct
    %mountPoint = GetModelBoneIndexByName($kenneyModel, "RightHandIndex3_end");

    %weapon = new ModelObject() {
        Position ="0 0 0";
        ModelId = $AppleTreeModel;
        Scale = "3 3 3";
        MountBoneIndex = %mountPoint;
    };
    // %weapon = %this.appleTree.clone();
    // %weapon.position = "0 0 0";
    // %weapon.scale ="10 10 10";
    // %weapon.MountBoneIndex = %mountPoint;
    // %weapon.refresh();
    %this.player.add(%weapon);



    %count = GetModelMaterialCount($kenneyModel, MATERIAL_MAP_DIFFUSE);
    echo("KENNEY MODEL MATERIAL COUNT IS:" SPC %count);
    for (%i = 0; %i < %count; %i++) {
        SetModelMapTexture($kenneyModel,$skinZombie, %i, MATERIAL_MAP_ALBEDO);
        SetModelMapColor($kenneyModel, WHITE);
        SetModelShader($kenneyModel, %this.Sun.sunShader, %i);
    }

    // %this.player.animation = $kennyAnimations;
    // //
    // %frame = 0; //
    // UpdateModelAnimation( %this.player.ModelId, %this.player.animation, 0, %frame );
    warn("MODELID:" SPC %this.player.ModelId SPC "animation id:"
         SPC %this.player.animation SPC "animation count:" SPC GetModelAnimationCount(%this.player.animation)
         SPC "texture id:" SPC $skinZombie
         SPC "mountpoint" SPC %mountPoint
         SPC "weapon" SPC %weapon.getId()
    );



    %this.levelObjects.add(%this.player);
    %this.levelObjects.add(%weapon);
    %this.DropToGround(%this.player);



    %this.gui = new Gui() {
        class ="LightGui";
    };

    return true;
}
//----------------------------------------------------------------------
function TerrainDemo::OnRemove(%this) {
    EnableCursor();

    %this.camera.delete();
    %this.levelObjects.deleteObjects();
    %this.levelObjects.delete();
    %this.gui.delete();


}

//----------------------------------------------------------------------
function TerrainDemo::Render(%this) {
    %sun = %this.sun;
    %waterPlane = %this.waterPlane;

    if (!isObject(%waterPlane)) return false;
    if (!IsCursorHidden() && IsMouseButtonPressed(MOUSE_BUTTON_LEFT)) %this.onMouseLeftClick();

    ClearBackground(%sun.skyColor);
    %cam = %this.camera;
    %ter = %this.terrain;


    %dt = getFrameTime();
    %cam.update(%dt);
    %sun.update(%dt);
    %waterPlane.update(%dt);

    %cam.Begin();
        %waterPlane.draw();
        ClientContainerDrawObjects();

    %cam.End();


    %gui = %this.gui;
    %gui.Begin(10,10);
    %gui.Write("Terrain Demo", 20, WHITE);
    %gui.Separator(140);
    %gui.spacing ="10 3";
    %gui.Write("cam pos:" SPC %cam.position, 10, WHITE);
    %gui.Write("cam tar:" SPC %cam.target, 10, WHITE);
    %gui.Write("time   :" SPC %sun.sunTime, 10, WHITE);

}

//----------------------------------------------------------------------
function TerrainDemo::spawnScriptTree(%this, %worldPos)
{
    %trunkHeight = getRandomF(1.5, 3.5);
    %trunkThickness = getRandomF(0.4, 1.2);

    %trunk = new ModelObject() {
        Position = %worldPos.x SPC (%worldPos.y + (%trunkHeight / 2.0)) SPC %worldPos.z;
        ModelId = $TreeTrunkModel;
        Scale = %trunkThickness SPC %trunkHeight SPC %trunkThickness;
    };

    %crownScaleX = getRandomF(1.0, 1.5);
    %crownScaleY = getRandomF(1.0, 1.5);
    %crownScaleZ = getRandomF(1.0, 1.5);

    %crownRadiusWorld = 1.2 * %crownScaleY;
    %relY = 0.5 + (%crownRadiusWorld / %trunkHeight);

    %finalCrownScaleX = %crownScaleX / %trunkThickness;
    %finalCrownScaleY = %crownScaleY / %trunkHeight;
    %finalCrownScaleZ = %crownScaleZ / %trunkThickness;

    %crown = new ModelObject() {
        Position = "0.0" SPC %relY SPC "0.0";
        ModelId = $TreeCrownModel;
        Scale = %finalCrownScaleX SPC %finalCrownScaleY SPC %finalCrownScaleZ;
    };

    %trunk.add(%crown);

    %this.levelObjects.add(%trunk);

    return %trunk;
}
//----------------------------------------------------------------------
function TerrainDemo::onMouseLeftClick(%this)
{
    %camera = %this.camera;
    %terrain = %this.terrain;
    %model = %this.selectedModel;

    %mousePos = GetMousePosition();
    %ray = %this.camera.getScreenToWorldRay(%mousePos); //<< GetMouseRay

    %collisionStr = %terrain.getRayCollision(%ray);

    if (%collisionStr $= "") {
        return;
    }
    // or simply as vector3 string:
    %hitPoint = getWords(%collisionStr, 0,2);
    %hitNormal = getWords(%collisionStr, 3,5);
    warn("HITPOINT" SPC %hitPoint SPC "HITNORMAL" SPC %hitNormal);
    %distance = getWord(%collisionStr, 6);

    // FIXME gui for selecting what to spawn
    // %tree = %this.spawnScriptTree(%hitPoint);

    // AppleTree
    %tree = %this.appleTree.clone();
    %tree.position = %hitPoint;
    %this.levelObjects.add(%tree);

    echo("new Tree:" SPC  %tree SPC "created. Position:" SPC %tree.position);
}

//----------------------------------------------------------------------
function TerrainDemo::DropToGround(%this, %obj){

    %groundHeight = %this.terrain.getHeight(%obj.position);
    %obj.position.y = %groundHeight;

}
