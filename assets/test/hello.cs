// Example without the default calls - so it's a pure script call only
// called with: ./raylib-elfscript --noloop --script assets/test/hello.cs
// WARNING since the c-source loop does not run. schedule does not work!

// Initialization
//--------------------------------------------------------------------------------------
$screenWidth = 800;
$screenHeight = 450;

InitWindow($screenWidth, $screenHeight, "raylib [core] example - basic window");

SetTargetFPS(60);               // Set our game to run at 60 frames-per-second

$text  = "Congrats! You created your first window!";
$fontSize = 30;
$textWidth = MeasureText($text,$fontSize);
$x = $screenWidth / 2 - $textWidth / 2;
$y = $screenHeight / 2 - $fontSize / 2;
echo("TextWidth:" SPC $textWidth SPC "x:" SPC $x);



while (!WindowShouldClose())
{
    // Update

    // Draw
    //----------------------------------------------------------------------------------
    BeginDrawing();

    ClearBackground("30 20 60");
    DrawFPS(10, 10);
    DrawText($text, $x, $y, $fontsize, LIGHTGRAY, true, BLACK);

    EndDrawing();
    //----------------------------------------------------------------------------------
}

// De-Initialization
//--------------------------------------------------------------------------------------
CloseWindow();        // Close window and OpenGL context
//--------------------------------------------------------------------------------------
