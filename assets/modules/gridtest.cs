//----------------------------------------------------------------------
// GridTest ported from OhmFlux/ElfTest
// (c) 2026 XXTH
//----------------------------------------------------------------------
function creategridtest() {
    %obj = new SimSet() {
        class = "Gridtest";
    };

    return %obj;
}
//----------------------------------------------------------------------
function GridTest::onAdd(%this) {
    SetTargetFPS(60); //FPS TEST only calling module
    %this.Init();
    return true;
}
//----------------------------------------------------------------------
function GridTest::OnRemove(%this) {
    SetTargetFPS(60);
    %this.deleteObjects();
}
//----------------------------------------------------------------------
function GridTest::Render(%this) {
    ClearBackground("10 10 10");

    if (%this.crazy) %this.newTerrain();

    // ---- gui
    %gui = %this.gui;
    %gui.Begin(GetScreenWidth() - 120,10);
    %gui.Write("Grid Demo", 20, WHITE);
    %gui.Separator(80);
    if ( %gui.Button(80, "New Terrain") ) %this.newTerrain();
    %id = %this.getId();
    %gui.CheckBoxVar("Hide Terrain", %id @ ".hideTerrain");
    %gui.CheckBoxVar("Crazy!", %id @ ".crazy");


    if (!%this.hideTerrain) {
        %vert = %this.countX;
        %hor  = %this.countY;

        %idx = 0;
        %weight = 0;
        for (%i = 0; %i < %vert; %i++) {
            // %line = "";
            for (%j=0; %j < %hor; %j++) {
                %weight = %this.grid.getWeightByNodeId( %idx );
                // %this.writeText(65 + 50 * %j,%i*30 + 50,byteToHex(%weight), $align::left ,  %weight SPC "0.5 0.2");

                %x = 65 + 50 * %j;
                %y = %i*30 + 40;
                %rect = %x SPC %y SPC 35 SPC 28;
                %color = %weight  SPC "128 0";
                DrawRectangleRec(%rect, %color);
                DrawText(%weight, %x + 2 , %y + 2, 10, BLACK, false);
                %idx++;
            }
        }
    }


    // path
    %lastPoint = "";
    %path = %this.path;
    if ( isObject(%path) ) {
        %cnt = %path.getDynamicFieldCount();
        for (%i = 0; %i < %cnt; %i++) {
            %pos = %path.getFieldValue("node" @ %i);
            %realPos = %pos.x * 50  + 85 SPC  %pos.y * 30 + 55;
            if ( %lastPoint ) DrawLineV(%lastPoint, %realPos  );
            %lastPoint = %realPos;
        }
    }


}

// -----------------------------------------------------------------------------
function GridTest::Init(%this) {
    // Setup Grid
    %this.grid = new Grid();
    %x = 0; %y = 0;
    %w = 16; %h = 16;
    %this.grid.start = %x SPC %y;
    %this.grid.end  = %x + %w -1 SPC %y + %w - 1;
    %area  = %this.grid.start SPC %w SPC %h;
    %this.grid.squareSize = 1.0;
    echo("INIT:" SPC  %this.grid.init(%area, %this.grid.squareSize));
    %this.grid.getinfo();
    %this.add(%this.grid);


    %this.gui = new Gui() {
    };
    %this.add(%this.gui);


    %this.crazy  = false;
    %this.hideTerrain = false;
    %this.countX = %this.grid.getNodeCountY();
    %this.countY = %this.grid.getNodeCountX();
    %this.newTerrain();
}
// -----------------------------------------------------------------------------
function GridTest::newTerrain(%this) {
    %this.grid.addRandomMud();
    if ( isObject(%this.path) ) %this.path.delete();
    %this.path = %this.grid.findPath( %this.grid.start, %this.grid.end, false);
    if ( isObject(%this.path) ) %this.add( %this.path );
    // echo("PATH object is:" SPC %this.path);
}
// -----------------------------------------------------------------------------
function Grid::addRandomMud(%this) {
    %cnt = %this.getNodeCount();
    //reset grid
    for( %i = 0; %i < %cnt; %i++ ) {
        %this.setWeightByNodeId(%i, 0);
    }  
    
    for( %i = 0; %i < %cnt * 0.75; %i++ ) {
        %n = getRandom(%cnt - 1);
        // %w = getRandom(50,250);
        // let think about 7 ground textures:
        // max is 224, 256 would be invalid btw
        // 255 would block completly 
        %w = getRandom(6) * 32 + 32;
        %this.setWeightByNodeId(%n, %w);
    }
   
}




