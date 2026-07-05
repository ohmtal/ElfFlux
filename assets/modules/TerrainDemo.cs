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
        position = "-36.0 41.0 -57.0";    // Camera position
        target = "0.0 0.5 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 45.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type

        moveSpeed = 100.0;
        panSpeed  = 30.0;
    };

    // ---- Terrain
    %terSize = 1024 * 4;
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

    // ---- Modeldefs
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

    // ---- Sun shader
    //
    %this.sunDirection = "-0.5 -1.0 -0.2";

    %this.sunShader = LoadShader(
        "assets/shaders/custom/sun.vert",
        "assets/shaders/custom/sun.frag"
    );
    echo("** Loader shaderId:" SPC %this.sunShader);
    %sunDirLoc   = GetShaderLocation(%this.sunShader, "sunDirection");
    %sunColorLoc = GetShaderLocation(%this.sunShader, "sunColor");

    SetShaderValue(%this.sunShader, %sunDirLoc, Vector3Normalize(%this.sunDirection), 2);
    SetShaderValue(%this.sunShader, %sunColorLoc, "1.0 0.95 0.85", 3); //normalized

    SetModelShader(%this.terrain.getModelId(), %this.sunShader, 0);

    // add/update shader on static models
    SetModelShader($TreeTrunkModel, %this.sunShader, 0);
    SetModelShader($TreeCrownModel, %this.sunShader, 0);

    // ---- Visual Sun
    // scale against terrain radius
    %scale = mAbs(%this.terrainRadiusNeg) / 1024 * 3;
    %this.sun = new ModelObject() {
        Position = "0 0 0";
        ModelId =$SunBillboardModel;
        Scale = %scale SPC %scale SPC %scale;
    };
    %this.levelObjects.add(%this.sun);


    // --- Apple Tree Test -----
    //FIXME transparency / shader
    %count = GetModelMatrialCount($AppleTreeModel, MATERIAL_MAP_DIFFUSE);
    for (%i = 0; %i < %count; %i++) SetModelShader($AppleTreeModel, %this.sunShader, %i);

    %this.appleTree = new ModelObject() {
        Position = %this.camera.position; //for testing )
        ModelId = $AppleTreeModel;
        Scale = "3 3 3";
    };
    %this.levelObjects.add(%this.appleTree);
    %this.DropToGround(%this.appleTree);



    // ---- Day night
    %this.sunTime = 0.0; // 0.0 .. 360.0


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

    UnloadShader(%this.sunShader);

}
//----------------------------------------------------------------------
function Camera3DObject::updateFly(%this, %dt)
{
    %speed = isKeyDown(KEY_LEFT_SHIFT) ? %this.moveSpeed * 2.0 : %this.moveSpeed;
    if (isKeyDown(KEY_W)) %this.moveForward(%dt * %speed, false);
    if (isKeyDown(KEY_S)) %this.moveForward(-%dt * %speed, false);
    if (isKeyDown(KEY_D)) %this.moveRight(%dt * %speed, false);
    if (isKeyDown(KEY_A)) %this.moveRight(-%dt * %speed, false);
    if (isKeyDown(KEY_E)) %this.moveUp(%dt * %speed);
    if (isKeyDown(KEY_Q)) %this.moveUp(-%dt * %speed);

    if (IsCursorHidden())
    {
        // === SMOOTH MOUSE ROTATION ===
        %mouseDelta = GetMouseDelta();
        %deltaX = %mouseDelta.x;
        %deltaY = %mouseDelta.y;

        %sensitivity = 0.0015;

        %this.targetYaw   = -%deltaX * %this.panSpeed * %sensitivity;
        %this.targetPitch = -%deltaY * %this.panSpeed * %sensitivity;

        %smoothFactor = 15.0 * %dt;
        if (%smoothFactor > 1.0) %smoothFactor = 1.0; // Absicherung

        if (%this.targetYaw != 0 || %this.targetPitch != 0) {

            %currentYaw = %this.targetYaw * %smoothFactor;
            %currentPitch = %this.targetPitch * %smoothFactor;

            %this.yaw(%currentYaw, false);
            %this.pitch(%currentPitch, true, false, false);

            %this.targetYaw   -= %currentYaw;
            %this.targetPitch -= %currentPitch;
        }
    }
}
//----------------------------------------------------------------------
function TerrainDemo::updateSun(%this, %dt)
{
    %sunSpeed = 5.0;
    %this.sunTime += %dt * %sunSpeed;
    if (%this.sunTime > 360.0) %this.sunTime -= 360.0;

    %rad = %this.sunTime * 0.0174532925; // DEG2RAD

    %sunX = mCos(%rad);
    %sunY = mSin(%rad);
    %sunZ = -0.2;

    %lightDirection = %sunX SPC (-%sunY) SPC %sunZ;
    %this.sunDirection = Vector3Normalize(%lightDirection);

    %sunDirLoc = GetShaderLocation(%this.sunShader, "sunDirection");
    SetShaderValue(%this.sunShader, %sunDirLoc, %this.sunDirection, 2);

    %camPos = %this.camera.position;
    %sunOffset = Vector3Scale(%this.sunDirection, %this.terrainRadiusNeg);
    %this.sun.position = Vector3Add(%camPos, %sunOffset);

    if (%sunY > 0.0)
    {
        %blend = %sunY;
        if (%blend > 1.0) %blend = 1.0;

        %r = mClamp((1.0 - %blend) * 200 + %blend * 60, 0, 255);
        %g = mClamp((1.0 - %blend) * 120 + %blend * 140, 0, 255);
        %b = mClamp((1.0 - %blend) * 80 + %blend * 230, 0, 255);

        %this.skyColor = %r SPC %g SPC %b;

        %sunColorLoc = GetShaderLocation(%this.sunShader, "sunColor");
        SetShaderValue(%this.sunShader, %sunColorLoc, "1.0 0.95 0.85", 3);
    }
    else
    {
        %blend = mAbs(%sunY) * 4.0;
        if (%blend > 1.0) %blend = 1.0;

        %r = mClamp((1.0 - %blend) * 60, 0, 255);
        %g = mClamp((1.0 - %blend) * 140 + %blend * 10, 0, 255);
        %b = mClamp((1.0 - %blend) * 230 + %blend * 25, 0, 255);

        %this.skyColor = %r SPC %g SPC %b;

        %sunColorLoc = GetShaderLocation(%this.sunShader, "sunColor");
        SetShaderValue(%this.sunShader, %sunColorLoc, "0.1 0.12 0.2", 3);
    }
}

//----------------------------------------------------------------------
function TerrainDemo::Render(%this) {

    if (IsMouseButtonPressed(MOUSE_BUTTON_RIGHT))  DisableCursor();
    else if (IsMouseButtonReleased(MOUSE_BUTTON_RIGHT)) EnableCursor();

    if (!IsCursorHidden() && IsMouseButtonPressed(MOUSE_BUTTON_LEFT)) %this.onMouseLeftClick();

    ClearBackground(%this.skyColor);
    %cam = %this.camera;
    %ter = %this.terrain;

    %dt = getFrameTime();
    %cam.updateFly(%dt);
    %this.updateSun(%dt);

    %cam.Begin();
      ClientContainerDrawObjects();
    %cam.End();


    %gui = %this.gui;
    %gui.Begin(10,10);
    %gui.Write("Terrain Demo", 20, WHITE);
    %gui.Separator(140);
    %gui.spacing ="10 3";
    %gui.Write("cam pos:" SPC %cam.position, 10, WHITE);
    %gui.Write("cam tar:" SPC %cam.target, 10, WHITE);
    %gui.Write("time   :" SPC %this.sunTime, 10, WHITE);

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
