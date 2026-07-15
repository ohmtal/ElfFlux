// ElfScipt is not very good on such loops ... going int CPU bottlenek
// cheated a bit border check is only applied every 8th tick

// NOTE it's not longer the bunnyMark its my physic/object playground

#define MAX_BUNNIES 80000
#define BUNNY_BATCH 1

#define BUNNY_SIZE 32

$BUNNY_CANVAS="64 64 1024 520";
$WALL_THICK=8;

function createBunnyMark() {
    %obj = new ScriptObject() {
        class = "BunnyMark";
    };

    return %obj;
}
//----------------------------------------------------------------------
function BunnyMark::spawnBunny(%this) {
    %r = GetRandomValue(50, 255);  //r
    %g = GetRandomValue(50, 255);  //g
    %b = GetRandomValue(50, 255);  //b
    %a = 255; //GetRandomValue(64, 200); //a

    %x = GetRandomValue(0, %this.width);
    %y = GetRandomValue(0, %this.height);
    %z = GetRandomValue(0, 90) / 100 + 0.1; //0.1 .. 1.0

    //
    %obj = new SpriteObject() {
        position = GetMousePosition() SPC %z;
        color = %r SPC %g SPC %b SPC %a;
        TextureID = %this.bunny;
        // ForwardVector = getRandomf(-1,1) SPC getRandomf(-0.9,0.9);

        // Collision
        TypeMask = 1 << 4; //bunny ;) BIT(4)
        CollisionMask = 1 << 1 | 1 << 4; //wall ;) BIT(1) | ...
        //Physics
        CollisionType = CollisionType_Bounce;
        Restitution = 1.0;
        Friction = 0.5;
        Damping = 0.1; // we are not a trampolin


        VeloX = GetRandomValue(-100,100);
        VeloY = GetRandomValue(-100,100);
    };
    // %speed = GetRandomValue(50,300);
    // %obj.setForwardVectorByAngle(GetRandomValue(0,359), %speed);

    %this.bunnies.add(%obj);
    $b = %obj;
}
//----------------------------------------------------------------------
function BunnyMark::onAdd(%this) {
    // SetWindowSize(WINDOW_WIDTH,WINDOW_HEIGHT);
    %this.bunny = LoadTexture("assets/texture/raybunny.png");

    %this.bunnies = new SimSet();
    %this.width = GetScreenWidth() - 32;
    %this.height = GetScreenHeight() - 32;

    $Gravity = "0 100";
    $damping = 0.5;
    $friction = 0.1;
    $ImpulseStrength = 100;

    // create Walls:
    %this.wallTop = new SceneObject2D() {
        position = $BUNNY_CANVAS.x - $WALL_THICK SPC $BUNNY_CANVAS.y - $WALL_THICK;
        size = $BUNNY_CANVAS.width SPC $WALL_THICK;
        TypeMask = 1 << 1; //wall ;) BIT(4)
        CollisionType = CollisionType_Static;
    };
    %this.wallBottom = %this.wallTop.clone();
    %this.wallBottom.position.y = $BUNNY_CANVAS.y + $BUNNY_CANVAS.height;

    %this.wallLeft = %this.wallTop.clone();
    %this.wallLeft.position.y += $WALL_THICK;
    %this.wallLeft.size = $WALL_THICK SPC $BUNNY_CANVAS.height;

    %this.wallRight = %this.wallLeft.clone();
    %this.wallRight.position.x = $BUNNY_CANVAS.x + $BUNNY_CANVAS.width - $WALL_THICK * 2;

    return true;
}
//----------------------------------------------------------------------
function BunnyMark::OnRemove(%this) {
    unloadTexture(%this.bunny);
    %this.bunnies.delete();
    %this.wallTop.delete();
    %this.wallBottom.delete();
    %this.wallLeft.delete();
    %this.wallRight.delete();
    ClientContainer2DDeleteAllObjects();
}
//----------------------------------------------------------------------
function BunnyMark::ApplyRadialImpulse(%this, %pos, %strength) {
    warn("ApplyRadialImpulse:" SPC %pos SPC %strength);
    %list = %this.bunnies;
    %cnt = %list.getCount();
    %dist = 150;
    for (%i = 0; %i < %cnt; %i++) {
        %b = %list.getObject(%i);
        %b.ApplyRadialImpulse(%pos.x, %pos.y, %strength, %dist);
    }
}
//----------------------------------------------------------------------
function BunnyMark::scatter(%this) {
    %list = %this.bunnies;
    %cnt = %list.getCount();
    %dist = 150;
    for (%i = 0; %i < %cnt; %i++) {
        %b = %list.getObject(%i);
        %b.VeloX = GetRandomValue(-200,200);
        %b.VeloY = GetRandomValue(-200,200);
    }
}

//----------------------------------------------------------------------
function BunnyMark::Render(%this) {
    ClearBackground(SEABLUE);

    DrawRectangleRec( %this.wallTop.GetWorldBox(), ORANGE);
    DrawRectangleRec( %this.wallBottom.GetWorldBox(), ORANGE);
    DrawRectangleRec( %this.wallLeft.GetWorldBox(), ORANGE);
    DrawRectangleRec( %this.wallRight.GetWorldBox(), ORANGE);

    %list = %this.bunnies;
    %cnt = %list.getCount();
    // %w = %this.width;
    // %h = %this.height;

    %dt = GetFrameTime();

    // #define BORDER 64
    //
    // %bx = GetScreenWidth() - BORDER * 2 ;
    // %by = GetScreenHeight() - BORDER * 2 ;
    //
    // DrawRectangleLines(BORDER,BORDER, %bx - BUNNY_SIZE, %by - BUNNY_SIZE, ORANGE);
    //
    // %damping = $damping;
    // %friction = $friction;
    %buttonDown = IsMouseButtonDown(MOUSE_BUTTON_MIDDLE);
    %mousePos = GetMousePosition();

    // $ticks++; //cheat ;)
    for (%i = 0; %i < %cnt; %i++) {
        %b = %list.getObject(%i);

        %b.beginMove();

        // // if ($ticks % 2 == 0) {
        //     //%b.doBounce(0,0,800,450);
        //     // %b.doBounce(0,0,WINDOW_WIDTH,WINDOW_HEIGHT);
        //     if (%b.x <= BORDER ) { %b.VeloX *= -%damping; %b.x = BORDER;}
        //     else if ( %b.x >= %bx) { %b.VeloX *= -%damping; %b.x = %bx - 0.01;}
        //     if (%b.y <= BORDER )  { %b.VeloY *= -%damping; %b.y = BORDER + 0.01;}
        //     else if ( %b.y >= %by) {
        //         %b.VeloY *= -%damping;
        //         %b.y = %by - 0.01;
        //         %b.VeloX *= %friction;
        //     }
        // // }

        %b.moveLinear(%dt);
        %b.moveGravity(%dt, $Gravity.x,$Gravity.y);

        if ( %buttonDown) {
            %b.moveOrbital2D(%dt, %mousePos, 1000, 5, 100);
        }

        %b.endMove();

    }



    if (IsMouseButtonPressed(MOUSE_BUTTON_RIGHT) && %cnt < MAX_BUNNIES) {
        ClientContainer2DSetSort(false);
        for (%i = 0; %i < BUNNY_BATCH; %i++) %this.spawnBunny();
        ClientContainer2DSetSort(true);
    }
    if (IsMouseButtonPressed(MOUSE_BUTTON_LEFT)) {
        %this.ApplyRadialImpulse(GetMousePosition(), $ImpulseStrength);
    }
    if (IsKeyPressed(KEY_SPACE)) {
        %this.scatter();
    }


    // -------
    ClientContainer2DDrawObjects();
    DrawText( "Bunnys:" SPC %cnt,10, 10, 32, RAYWHITE, true);
    // DrawText( "Damping:" SPC %damping SPC "Friction" SPC %friction,10, 50, 20, WHITE, false);
    // DrawText( "$Gravity:" SPC $Gravity SPC "$ImpulseStrength" SPC $ImpulseStrength,10, 70, 20, WHITE, false);
}

