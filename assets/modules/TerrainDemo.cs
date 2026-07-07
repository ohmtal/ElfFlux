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
function TerrainDemo::loadGreenManModel(%this,%spawnPoint) {

    if ($greenManModel * 1 == 0) {
        %path = "assets/models/gltf/";
        %file = "greenman.glb";
        $greenManModel = LoadModel(%path @ %file);
        $greenManAnimations = LoadModelAnimations(%path @ %file);

        %file = "greenman_hat.glb";
        $greenManHat = LoadModel(%path @ %file);

        %file = "greenman_sword.glb";
        $greenManSword = LoadModel(%path @ %file);

        %file = "greenman_shield.glb";
        $greenManShield = LoadModel(%path @ %file);

    }

    // add/update shader on static models
    // FIXME does not work, not sure why not .... << it's on 1 !
    SetModelShader($greenManModel, %this.Sun.sunShader, 1);
    // mhhh strange
    SetModelShader($greenManHat, %this.Sun.sunShader, 1);
    SetModelShader($greenManSword, %this.Sun.sunShader, 1);
    SetModelShader($greenManShield, %this.Sun.sunShader, 1);



    if (%spawnPoint $= "" ) %spawnPoint = "-14.0 35.0 -108.0";
    %obj = new ModelObject() {
        Position = %spawnPoint;
        ModelId = $greenManModel;
        Scale = "1.3 1.3 1.3";
        AnimationBlockId = $greenManAnimations;
        AnimationIndex = 1;
        AnimationFPS = 60;
    };

    // socket_hat
    // socket_hand_L
    // socket_hand_R

    %socket_hat = GetModelBoneIndexByName($greenManModel, "socket_hat");
    warn("socket_hat bone index = " SPC %socket_hat );

    %hat = new ModelObject() {
        Position ="0 0.05 0";
        ModelId = $greenManHat;
        Scale = "1 1 1";
        MountBoneIndex = %socket_hat;
    };
    %this.levelObjects.add(%hat);
    %obj.add(%hat); //mount

    %socket_hand_L = GetModelBoneIndexByName($greenManModel, "socket_hand_L");
    warn("socket_hand_L bone index = " SPC %socket_hand_L );

    %shield = new ModelObject() {
        Position ="0 0.0 0";
        ModelId = $greenManShield;
        Scale = "1 1 1";
        MountBoneIndex = %socket_hand_L;
    };
    %this.levelObjects.add(%shield);
    %obj.add(%shield); //mount

    %socket_hand_R = GetModelBoneIndexByName($greenManModel, "socket_hand_R");
    warn("socket_hand_R bone index = " SPC %socket_hand_R );



    %weapon = new ModelObject() {
        Position ="0 0 0";
        ModelId = $greenManSword;
        Scale = "1 1 1";
        MountBoneIndex = %socket_hand_R;
    };
    %this.levelObjects.add(%weapon);
    %obj.add(%weapon); //mount



// FIXME attach shader ....

    %this.levelObjects.add(%obj);
    %this.DropToGround(%obj);

    return %obj;
}
//----------------------------------------------------------------------
function TerrainDemo::loadKenneyModel(%this, %spawnPoint) {

    // global! -  keep in memory until app ends ... (auto cleaned up by resource manager)
    if ($kenneyModel * 1 == 0) {
        %path = "assets/models/kenney_animated-characters-retro/";
        %file = "characterMedium.gltf";
        $kenneyModel = LoadModel(%path @ %file);
        $kennyAnimations = LoadModelAnimations(%path @ %file);
        $skinZombie = LoadTexture(%path @ "Skins/zombieMaleA.png");
        SetModelMapTexture($kenneyModel,$skinZombie/*, 0, MATERIAL_MAP_ALBEDO*/); // << model.materials[0].maps[MATERIAL_MAP_ALBEDO].texture = skinZombie;
        SetModelMapColor($kenneyModel, WHITE);
    }

    if (%spawnPoint $= "" ) %spawnPoint = "-26 17 -102";
    %obj = new ModelObject() {
        Position = %spawnPoint;
        ModelId = $kenneyModel;
        Scale = "0.01 0.01 0.01";
        AnimationBlockId = $kennyAnimations;
        AnimationIndex = 2; //run
        AnimationFPS = 25;
    };


    %count = GetModelMaterialCount($kenneyModel, MATERIAL_MAP_ALBEDO);
    echo("KENNEY MODEL MATERIAL COUNT IS:" SPC %count);
    for (%i = 0; %i < %count; %i++) {
        SetModelMapTexture($kenneyModel,$skinZombie, %i, MATERIAL_MAP_ALBEDO);
        SetModelMapColor($kenneyModel, WHITE);
        SetModelShader($kenneyModel, %this.Sun.sunShader, %i);
    }


    // mount test FIXME doing strange things !! :: doubled and mount matrix not correct
    %mountPoint = GetModelBoneIndexByName($kenneyModel, "RightHandIndex3_end");

    %weapon = new ModelObject() {
        Position ="0 0 0";
        ModelId = $AppleTreeModel;
        Scale = "20 20 20";  // need to be scaled to parent size
        Rotation = "90 0 0"; //rotate
        MountBoneIndex = %mountPoint;
    };
    $MOUNTTREE = %weapon;
    %obj.add(%weapon);




    // %obj.animation = $kennyAnimations;
    // //
    // %frame = 0; //
    // UpdateModelAnimation( %obj.ModelId, %obj.animation, 0, %frame );
    warn("MODELID:" SPC %obj.ModelId SPC "animation id:"
    SPC %obj.animation SPC "animation count:" SPC GetModelAnimationCount(%obj.animation)
    SPC "texture id:" SPC $skinZombie
    SPC "mountpoint" SPC %mountPoint
    SPC "weapon" SPC %weapon.getId()
    );

    %this.levelObjects.add(%weapon);
    %this.levelObjects.add(%obj);
    %this.DropToGround(%obj);
    // <<<<<<<<<<<< kenny model


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
    %this.player = %this.loadKenneyModel();
    $PLAYER = %this.player; //DEBUG global access


    %this.greenMan = %this.loadGreenManModel();
    $GREENMAN = %this.greenMan;//DEBUG global access



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
