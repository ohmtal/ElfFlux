// Why is it so slow ... much slower then BatchTEST
// CPU bottleneck:
// BatchRender loop the raw vector and Container have to call
// the objects draw function
// .... i guess this is the overhead which slowdown
// works also with values like 50000 but then delete/cleanup takes
// very long ^^ << disabled garbagecollection make it fast again!
#define _BATCH_COUNT_ 5000
#define _RECT_SIZE_ 50

function createPrimitives2D() {
    %obj = new ScriptObject() {
        class = "Primitives2D";
    };

    return %obj;
}
//----------------------------------------------------------------------
function Primitives2D::onAdd(%this) {
     SetTargetFPS(0); //FPS TEST only calling module
    ClientContainer2DReserve(_BATCH_COUNT_ + 20);


    %this.cleanup = new SimSet();
    %obj =  new PrimitiveObject2D(poText) {
        type = 4;
        caption = "Hello PrimitiveObject2D BatchCount is:" SPC _BATCH_COUNT_;
        position = "10 10 0";
        size = "20 20"; //height is used here
        color = BLACK;

    };
    %this.cleanup.add( %obj );

    %obj =  new PrimitiveObject2D(poRect) {
        type = 0; //RECTANGLE
        position = "10 40 0.10"; //adding 0.01 as layer
        size = "100 100"; //height is used here
        color = "50 50 100 100";

    };
    %this.cleanup.add( %obj );

    %obj =  new PrimitiveObject2D(poRectOutline) {
        type = 0;
        filled = false;
        thick = 4;
        position = "10 40 0.09"; //adding 0.09 as layer
        size = "100 100"; //height is used here
        color = "100 100 200 255";

    };
    %this.cleanup.add( %obj );

    %obj =  new PrimitiveObject2D(poCircle) {
        type = 1; //CIRCLE

        position = "120 40 0.10"; //adding 0.01 as layer
        size = "100 100"; //height is used here
        color = "150 50 20 100";

    };
    %this.cleanup.add( %obj );

    %obj =  new PrimitiveObject2D(poCircleOutline) {
        type = 1; //CIRCLE
        filled = false;
        thick = 4; //unused on circle outline
        position = "120 40 0.09";
        size = "100 100"; //height is used here
        color = "150 50 20 255";

    };
    %this.cleanup.add( %obj );


    %obj =  new PrimitiveObject2D(poELLIPSE) {
        type = 2; //ELLIPSE
        position = "230 40 0.09";
        size = "80 100"; //height is used here
        color = "150 150 20 100";

    };
    %this.cleanup.add( %obj );
    %obj =  new PrimitiveObject2D(poELLIPSEOutLine) {
        type = 2; //ELLIPSE
        filled = false;
        thick = 4; //unused on circle outline
        position = "230 40 0.09";
        size = "80 100"; //height is used here
        color = "150 150 20 255";

    };
    %this.cleanup.add( %obj );

    %this.cleanup.add( %obj );
    %obj =  new PrimitiveObject2D(poLine) {
        type = 3; //LINE
        thick = 5;
        position = "10 150 0.0";
        size = "500 0"; //height is used here
        color = "255 20 20 255";

    };
    %this.cleanup.add( %obj );


    // batch test:
    ClientContainer2DSetSort(false);

    %this.batchList = new SimSet();
    $batchH = GetScreenHeight() - _RECT_SIZE_;
    $batchW = GetScreenWidth() - _RECT_SIZE_;

    %this.fillBatch();

    return true;
}
//----------------------------------------------------------------------
function Primitives2D::fillBatch(%this) {
    if (!isObject(%this.batchList)) return;
    %left = _BATCH_COUNT_ - %this.batchList.getcount();
    %cnt = getMin(100, %left);

    for (%i = 0; %i < %cnt; %i++) {
        %r = GetRandomValue(0, 255);  //r
        %g = GetRandomValue(0, 255);  //g
        %b = GetRandomValue(0, 255);  //b
        %a = GetRandomValue(64, 200); //a

        %x = GetRandomValue(0, $batchW);
        %y = GetRandomValue(0, $batchH);
        %z = GetRandomValue(0, 90) / 100 + 0.1; //0.1 .. 1.0

        %w = GetRandomValue( 3, _RECT_SIZE_); //w
        %h = GetRandomValue( 3, _RECT_SIZE_); //h
        %thick = GetRandomValue(1,3);

        %type = GetRandomValue(0,3);

        %obj =  new PrimitiveObject2D() {
            type =  5; //%type;
            filled = GetRandomValue(0,1);
            position = %x SPC %y SPC %z;
            size = %w SPC %h;
            color = %r SPC %g SPC %b SPC %a;
            thick = %thick;
        };
        %this.batchList.add( %obj );
    }



    if ( %left > 0) {
        %this.schedule(1, "fillBatch");

        // %this.fillBatch(); //cause crash!
    } else {
        ClientContainer2DSetSort(true);
    }
}
//----------------------------------------------------------------------
function Primitives2D::OnRemove(%this) {

    // first remove sets
    %this.cleanup.delete();
    %this.batchList.delete();
    // then fast delete
    ClientContainer2DDeleteAllObjects();

}
//----------------------------------------------------------------------
function Primitives2D::Render(%this) {
    ClearBackground(WHITE);

    ClientContainer2DDrawObjects();
}

