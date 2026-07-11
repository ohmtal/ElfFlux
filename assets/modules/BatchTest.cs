// - started using pointers to global vars .. this should speed up even more
// - but depenting on update intervall it's running in GPU limit
// - MAYBE: also custom draw can speed up even more

#define _BATCH_COUNT_ 5000
#define _RECT_SIZE_ 50
#define _UPDATE_INTERVAL_ 2

function createBatchTest() {
    %obj = new ScriptObject() {
        class = "BatchTest";
    };


    return %obj;
}
//----------------------------------------------------------------------
function BatchTest::onAdd(%this) {
    SetTargetFPS(0); //FPS TEST only calling module

    BatchCreate(_BATCH_COUNT_);

    $h = GetScreenHeight() - _RECT_SIZE_;
    $w = GetScreenWidth() - _RECT_SIZE_;
    $b0Z = $b0W = _RECT_SIZE_;

    // set colors
    for (%i = 0; %i < _BATCH_COUNT_; %i++) {
        $b3X = GetRandomValue(0, 255);  //r
        $b3Y = GetRandomValue(0, 255);  //g
        $b3Z = GetRandomValue(0, 255);  //b
        $b3W = GetRandomValue(64, 200); //a
        BatchPush(%i, 3);

        $b0Z = GetRandomValue( 3, _RECT_SIZE_); //w
        $b0W = GetRandomValue( 3, _RECT_SIZE_); //h
        BatchPushVec2(%i,1, "$b0Z", "$b0W");

    }
    // BatchDumpRects();

    $updateTimer = _UPDATE_INTERVAL_;

    echo(" ---------- init done");
    return true;
}
//----------------------------------------------------------------------
function BatchTest::OnRemove(%this) {

}
//----------------------------------------------------------------------
function BatchTest::Render(%this) {
    ClearBackground(WHITE);
    $updateTimer += GetFrameTime();

    if ( $updateTimer > _UPDATE_INTERVAL_ ) {
        $h = GetScreenHeight() - _RECT_SIZE_;
        $w = GetScreenWidth() - _RECT_SIZE_;
        $updateTimer -= _UPDATE_INTERVAL_;
        for (%i = 0; %i < _BATCH_COUNT_; %i++) {
            $b0X = GetRandomValue(0, $w);
            $b0Y = GetRandomValue(0, $h);
            BatchPushVec2(%i, 0, "$b0X", "$b0Y");
        }
    }
    BatchDrawColoredRects();
    DrawText("Rects: _BATCH_COUNT_ Timer is:" SPC strFormat("%d",$updateTimer) SPC "/" SPC _UPDATE_INTERVAL_, 5, 5, 20, YELLOW, true, BLACK);

}

