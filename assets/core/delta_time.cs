/*******************************************************************************************
 *
 *   raylib [core] example - delta time
 *
 *   Script:
 *         - types like color or Vector2 are handled as strings SPC means SPACE and separete
 *           values.
 *         - variables start with $ are global like $speed
 *         - variables start with % are local like  %mouseWheel
 *
 *   NOTE to bind:
 *          - [.] Keys: $KEY_R
 *          - [.] Vector2 + Vector3
 *          - [.] IsKeyPressed
 *          - [.] GetMouseWheelMove
 *          - [ ] DrawCircleV($deltaCircle, $circleRadius, $RED);
 *
 ********************************************************************************************/
// $RAYWHITE = "255 255 255 255";
// $RED = "255 0 0 255";
// $BLUE = "0 0 255 255";
// $DARKGRAY = "64 64 64 255";

function MainInit() {


    $screenWidth = 800;
    $screenHeight = 450;

    InitWindow($screenWidth, $screenHeight,  "raylib [core] example - delta time");

    $currentFps = 60;

    // Store the position for the both of the circles
    //NOTE: using Stuct helper class
    $deltaCircle = new SimObject() {
        Vector2 = 0 SPC $screenHeight / 3.0 ;
    };
    $frameCircle = new SimObject() {
        Vector2 = 0 SPC $screenHeight * 2.0 / 3.0 ;
    };

    // The speed applied to both circles
    $speed = 10.0;
    $circleRadius = 32.0;

    SetTargetFPS($currentFps);

    return true;
}

function MainShutDown() {
    CloseWindow();        // Close window and OpenGL context
}

function MainLoop() {

        // Update
        //----------------------------------------------------------------------------------
        // Adjust the FPS target based on the mouse wheel
        %mouseWheel = GetMouseWheelMove();
        if (%mouseWheel != 0)
        {
            echo("current fps:" SPC $currentFps SPC "wheel:" SPC %mouseWheel);
            $currentFps += %mouseWheel;
            if ($currentFps < 0) $currentFps = 0;
            echo("new fps:" SPC $currentFps SPC "wheel:" SPC %mouseWheel);
            SetTargetFPS($currentFps);
        }

        // GetFrameTime() returns the time it took to draw the last frame, in seconds (usually called delta time)
        // Uses the delta time to make the circle look like it's moving at a "consistent" speed regardless of FPS

        // Multiply by 6.0 (an arbitrary value) in order to make the speed
        // visually closer to the other circle (at 60 fps), for comparison



        $deltaCircle.Vector2.x += GetFrameTime() * 6.0 * $speed;
        $frameCircle.Vector2.x += 0.1 * $speed;

        if ($deltaCircle.Vector2.x > $screenWidth) $deltaCircle.Vector2.x = 0;
        if ($frameCircle.Vector2.x > $screenWidth) $frameCircle.Vector2.x = 0;

        if (IsKeyPressed(KEY_R))
        {
            $deltaCircle.Vector2.x = 0;
            $frameCircle.Vector2.x = 0;
        }


        // NOTE this is the good and the ugly of typeless variables:
        // but i added a class Struct to get around this :)
        // %dcX = getword( $deltaCircle, 0);
        // %fcX = getword( $frameCircle, 0);
        //
        //
        // %dcX += GetFrameTime() * 6.0 * $speed;
        // // This circle can move faster or slower visually depending on the FPS
        // %fcX += 0.1 * $speed;
        //
        // // If either circle is off the screen, reset it back to the start
        // if (%dcX > $screenWidth) %dcX= 0;
        // if (%fcX> $screenWidth) %fcX = 0;
        //
        // // Reset both circles positions
        // if (IsKeyPressed(KEY_R))
        // {
        //     %dcX = 0;
        //     %fcX = 0;
        // }
        //
        //  $deltaCircle = setword( $deltaCircle, 0, %dcX);
        //  $frameCircle = setWord( $frameCircle, 0, %fcX );
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();
        ClearBackground(RAYWHITE);

        // Draw both circles to the screen
        DrawCircleV($deltaCircle.Vector2, $circleRadius, RED);
        DrawCircleV($frameCircle.Vector2, $circleRadius, BLUE);

        // Draw the help text
        // Determine what help text to show depending on the current FPS target
        // NOTE strFormat only takes ONE parameter so I use SPC concat in second
        if ($currentFps <= 0) %fpsText = strFormat("FPS: unlimited (%i)", GetFPS());
        else %fpsText = "FPS: " SPC GetFPS() SPC "target: " SPC $currentFps;
        DrawText(%fpsText, 10, 10, 20, DARKGRAY);
        DrawText(strFormat("Frame time: %02.02f ms", GetFrameTime()), 10, 30, 20, DARKGRAY);
        DrawText("Use the scroll wheel to change the fps limit, r to reset", 10, 50, 20, DARKGRAY);

        // Draw the text above the circles
        DrawText("FUNC: x += GetFrameTime()*speed", 10, 90, 20, RED);
        DrawText("FUNC: x += speed", 10, 240, 20, BLUE);

        EndDrawing();

}
