/*******************************************************************************************
 *
 *   raylib [core] example - delta time
 *   NOTE using  TYPEIDENT on object
 *
 *   Script:
 *         - types like color or Vector2 are handled as strings SPC means SPACE and separete
 *           values.
 *         - variables start with $ are global like $speed
 *         - variables start with % are local like  %mouseWheel
 *
 ********************************************************************************************/
// now defined in c
// $RAYWHITE = "255 255 255 255";
// $RED = "255 0 0 255";
// $BLUE = "0 0 255 128";
// $DARKGRAY = "64 64 64 255";


function MainInit() {

    $Game = new ScriptObject() {
        TypeS32 screenWidth = 800;
        TypeS32 screenHeight = 450;
        TypeS32 currentFps = 60;
        TypeF32 speed = 10;
        TypeF32 circleRadius = 32;
    };
    // add fields here because i can't access a field on constructor - can i? : not tested
    // TYPEIDENT must be cast as string here :
    $Game.addTypeField("deltaCircle", "TypeVector2", 0 SPC $Game.screenHeight / 3.0 );
    $Game.addTypeField("frameCircle", "TypeVector2", 0 SPC $Game.screenHeight * 2.0 / 3.0 );

    // test layer
    $game.frameCircle = $game.deltaCircle;

    // $game.dump();
    // echo($Game.deltaCircle SPC $Game.frameCircle);
    // return false;

    %flags = FLAG_WINDOW_RESIZABLE; // | $FLAG_WINDOW_MAXIMIZED;

    SetConfigFlags(%flags );

    InitWindow($Game.screenWidth, $Game.screenHeight,  "raylib [core] example - delta time");
    SetTargetFPS($Game.currentFps);

    // Image/Icon Test
    // LoadImage return a Image identifier (imageId)
    $Game.iconImg = LoadImage("assets/texture/raylib_32x32.png");
    SetWindowIcon($Game.iconImg);
    // IconImg can be unloaded now, but i keep it for later testing texture.
    // It's unloaded when programm ends automaticly
    //  UnloadImage($Game.iconImg);

    // $Game.Eyes = createEyes();
    // $Game.Eyes.init();

    return true;
}

function MainShutDown() {
    $Game.delete();
    CloseWindow();        // Close window and OpenGL context
}

function MainLoop() {

    // Update
    //----------------------------------------------------------------------------------
    // $Game.Eyes.update($Game.frameCircle);
    // $Game.Eyes.render();
    //----------------------------------------------------------------------------------
    // Adjust the FPS target based on the mouse wheel
    %mouseWheel = GetMouseWheelMove();
    if (%mouseWheel != 0)
    {
        $Game.currentFps += %mouseWheel;
        if ($Game.currentFps < 0) $Game.currentFps = 0;
        SetTargetFPS($Game.currentFps);
    }

    // GetFrameTime() returns the time it took to draw the last frame, in seconds (usually called delta time)
    // Uses the delta time to make the circle look like it's moving at a "consistent" speed regardless of FPS

    // Multiply by 6.0 (an arbitrary value) in order to make the speed
    // visually closer to the other circle (at 60 fps), for comparison



    $Game.deltaCircle.x += GetFrameTime() * 6.0 * $Game.speed;
    $Game.frameCircle.x += 0.1 * $Game.speed;

    if ($Game.deltaCircle.x > $Game.screenWidth) $Game.deltaCircle.x = 0;
    if ($Game.frameCircle.x > $Game.screenWidth) $Game.frameCircle.x = 0;

    if (IsKeyPressed( KEY_R))
    {
        $Game.deltaCircle.x = 0;
        $Game.frameCircle.x = 0;
        $Game.currentFps = 60;
        SetTargetFPS($Game.currentFps);
    }
    if (IsKeyPressed(KEY_U))
    {
        $Game.currentFps = 0;
        SetTargetFPS($Game.currentFps);
    }

    // Draw
    //----------------------------------------------------------------------------------
    BeginDrawing();
    ClearBackground("120 120 120");

    // Draw both circles to the screen
    DrawCircleV($Game.deltaCircle, $Game.circleRadius + 2, RED);
    DrawCircleV($Game.frameCircle, $Game.circleRadius, BLUE);

    // Draw the help text
    // Determine what help text to show depending on the current FPS target
    // NOTE strFormat only takes ONE parameter so I use SPC concat in second
    if ($Game.currentFps <= 0) %fpsText = strFormat("FPS: unlimited (%i)", GetFPS());
    else %fpsText = "FPS: " SPC GetFPS() SPC "target: " SPC $Game.currentFps;
    DrawText(%fpsText, 10, 10, 20, DARKGRAY);
    DrawText(strFormat("Frame time: %02.02f ms", GetFrameTime()), 10, 30, 20, DARKGRAY);
    DrawText("Use the scroll wheel to change the fps limit, r to reset, u for unlimited", 10, 50, 20, DARKGRAY);

    // Draw the text above the circles
    DrawText("FUNC: x += GetFrameTime()*speed", 10, 90, 20, RED);
    DrawText("FUNC: x += speed", 10, 240, 20, BLUE);

    EndDrawing();

}
