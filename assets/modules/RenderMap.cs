/*
 * RenderTexture2D target = LoadRenderTexture(screenWidth, screenHeight);
 *  BeginTextureMode(target);
 *  EndTextureMode();
 *   DrawTextureRec(target.texture, (Rectangle){ 0, 0, (float)target.texture.width, (float)-target.texture.height }, (Vector2){ 0, 0 }, WHITE);
*/
#define _BATCH_COUNT_ 50000
#define _RECT_SIZE_ 20
#define _UPDATE_INTERVAL_ 3

function createRenderMap() {
    %obj = new ScriptObject() {
        class = "RenderMap";
    };

    return %obj;
}
//----------------------------------------------------------------------
function RenderMap::onAdd(%this) {
    // SetTargetFPS(0); //FPS TEST only calling module

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

    $target = new RenderTextureObject() { width = GetScreenWidth(); height = GetScreenHeight(); };

    echo(" ---------- init done");
    return true;
}
//----------------------------------------------------------------------
function RenderMap::OnRemove(%this) {
    $target.delete();
    $target = 0;
}
//----------------------------------------------------------------------
function RenderMap::Render(%this) {

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
        $target.BeginTextureMode();
         ClearBackground(WHITE);
         BatchDrawColoredRects();
        $target.EndTextureMode();
    }
    ClearBackground(WHITE);
    DrawTextureRec($target.texture
    , "0 0" SPC $target.width SPC -$target.height
    , "0 0"
    , WHITE );

    DrawText("Rects: _BATCH_COUNT_ Timer is:" SPC strFormat("%d",$updateTimer) SPC "/" SPC _UPDATE_INTERVAL_, 5, 5, 20, YELLOW, true, BLACK);

    if (IsWindowResized()) {
       $target.resize( GetScreenWidth(),GetScreenHeight());
    }

}

