function MainInit() {
    // Initialization
    //--------------------------------------------------------------------------------------
    $screenWidth = 800;
    $screenHeight = 450;

    InitWindow($screenWidth, $screenHeight, "raylib [core] example - basic window");

    SetTargetFPS(60);               // Set our game to run at 60 frames-per-second

    return true;
}

function MainShutDown() {
    // De-Initialization
    //--------------------------------------------------------------------------------------
    CloseWindow();        // Close window and OpenGL context
    //--------------------------------------------------------------------------------------
}


function MainLoop()
{
        // Update
        //----------------------------------------------------------------------------------
        // TODO: Update your variables here
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();

        ClearBackground("20 60 20");

        DrawText("Congrats! You created your first window!", 190, 200, 20, "200 200 200 255");

        EndDrawing();
        //----------------------------------------------------------------------------------

    return 0;
}
