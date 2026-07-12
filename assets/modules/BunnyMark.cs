// ElfScipt is not very good on such loops ... going int CPU bottlenek
// cheated a bit border check is only applied every 8th tick
#define MAX_BUNNIES 80000
#define BUNNY_BATCH 1000

#define WINDOW_WIDTH 800
#define WINDOW_HEIGHT 450

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

    %obj = new SpriteObject() {
        position = GetMousePosition() SPC %z;
        color = %r SPC %g SPC %b SPC %a;
        TextureID = %this.bunny;
        // ForwardVector = getRandomf(-1,1) SPC getRandomf(-0.9,0.9);
        Speed = GetRandomValue(50,300);
    };
    %obj.setForwardVectorByAngle(GetRandomValue(0,359));
    %this.bunnies.add(%obj);
    $b = %obj;
}
//----------------------------------------------------------------------
function BunnyMark::onAdd(%this) {
    SetWindowSize(WINDOW_WIDTH,WINDOW_HEIGHT);
    %this.bunny = LoadTexture("assets/texture/raybunny.png");

    %this.bunnies = new SimSet();
    %this.width = GetScreenWidth() - 32;
    %this.height = GetScreenHeight() - 32;


    return true;
}
//----------------------------------------------------------------------
function BunnyMark::OnRemove(%this) {
    unloadTexture(%this.bunny);
    %this.bunnies.delete();
    ClientContainer2DDeleteAllObjects();
}
//----------------------------------------------------------------------
function BunnyMark::Render(%this) {
    ClearBackground(WHITE);

    %list = %this.bunnies;
    %cnt = %list.getCount();
    // %w = %this.width;
    // %h = %this.height;

    $ticks++; //cheat ;)
    if ($ticks % 8 == 0) {
        for (%i = 0; %i < %cnt; %i++) {
            %b = %list.getObject(%i);
            //%b.doBounce(0,0,800,450);
            %b.doBounce(0,0,WINDOW_WIDTH,WINDOW_HEIGHT);
        }
    }


    if (IsMouseButtonPressed(MOUSE_BUTTON_LEFT) && %cnt < MAX_BUNNIES) {
        ClientContainer2DSetSort(false);
        for (%i = 0; %i < BUNNY_BATCH; %i++) %this.spawnBunny();
        ClientContainer2DSetSort(true);
    }

    // -------
    ClientContainer2DDrawObjects();
    DrawText( "BunnyMark:" SPC %cnt,10, 10, 32, RAYWHITE, true);
}

