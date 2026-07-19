// -----------------------------------------------------------------------------
// batch render version
// so much time to implement it and get it work and only 5-10 (130-140) fps more.
// rects are much faster than circle and i do not see a big difference: 200fps
// -----------------------------------------------------------------------------

// orig 420
#define STAR_COUNT 120

function createshapes_starfield_batch() {
    %obj = new BatchRender() { //NOTE BatchRender!!
        class = "shapes_starfield_batch";
        count = STAR_COUNT;
        TypeBool drawLines = false;
        TypeS32 drawMode = 0;
        TypeString modeName = "Lines";
        TypeF32 speed = 1;
    };

    return %obj;
}
// -----------------------------------------------------------------------------
function shapes_starfield_batch::onRemove(%this) {
    SetTargetFPS(60);
}
// -----------------------------------------------------------------------------
function shapes_starfield_batch::UpdateScreenSize(%this) {
    $screenHeight = GetScreenHeight();
    $screenWidth = GetScreenWidth();

    // save as min/max
    $minH = -$screenWidth / 2;
    $maxH = $screenWidth / 2;
    $minW = -$screenHeight / 2;
    $maxW = $screenHeight / 2;

    $hW = $screenWidth  * 0.5;
    $hH = $screenHeight  * 0.5;
}
// -----------------------------------------------------------------------------
function shapes_starfield_batch::OnAdd(%this) {
    %this.bgColor = ColorLerp(DARKBLUE, BLACK, 0.69);
    %this.updateScreenSize();


    for ( %i = 0; %i < STAR_COUNT; %i++)
    {
        %x = GetRandomValue($minH, $maxH);
        %y = GetRandomValue($minW, $maxW);
        %z = 1.0; // Using 'w' vector space as Z-depth
        %this.setVector4_1(%i, %x, %y, %z, 0);
    }


    // SetTargetFPS(0);
    return true;
}
// -----------------------------------------------------------------------------
function shapes_starfield_batch::Render(%this) {

    %mouseMove = GetMouseWheelMove();

    if (%mouseMove != 0) {
        %this.speed += %mouseMove / 10.0;
        %this.speed= mClampF(%this.speed, 0.05, 5.0);

    }
    // Toggle lines / points with space bar
    if (IsKeyPressed(KEY_SPACE)) {
        %this.drawMode = (%this.drawMode + 1) % 3;
        switch (%this.drawMode) {
            case 0: %this.modeName = "Lines";
            case 1: %this.modeName = "Rectangles";
            case 2: %this.modeName = "Circle";
        }
        echo("SPACE pressed new drawMode is: " SPC %this.modeName );
    }
    if (IsKeyPressed(KEY_U)) {
        SetTargetFPS(0);
    }
    if (IsKeyPressed(KEY_R)) {
        SetTargetFPS(60);
    }

    if (IsWindowResized()) {
        %this.updateScreenSize();
    }

    ClearBackground( %this.bgColor);
    DrawFPS(10, 10);

    // Fast local variable caching

    %startDiv = 1.0 / 32.0;
    %drawMode = %this.drawMode;

    // %dt = GetFrameTime();
    %speed = %this.speed;

    %moveVal = GetFrameTime() * %speed;

    %hW = $hW;
    %hH = $hH;
    %minH = $minH; %maxH = $maxH; %minW = $minW; %maxW = $maxW;
    %scrWidth = $screenWidth;
    %scrHeight = $screenHeight;

    for (%i = 0; %i < STAR_COUNT; %i++)
    {

        // NOTE: method 3 via setting global vars : fastest!
        %this.getVector4_1GlobalV(%i, "$x", "$y", "$z", "$w");

        $z -= %moveVal; // Update depth when setting local var **optimize**

        // 3. Project to screen
        %invZ = 1.0 / $z;
        %screenPos_X = %hW + ($x * %invZ);
        %screenPos_Y = %hH + ($y * %invZ);

        // 4. Boundary and respawn check
        if ( $z < 0.0 ||
            %screenPos_X < 0.0 || %screenPos_Y < 0.0 ||
            %screenPos_X > %scrWidth || %screenPos_Y > %scrHeight
        )
        {
            $x = GetRandomValue(%minH, %maxH);
            $y = GetRandomValue(%minW, %maxW);
            $z = 1.0;
            %screenPos_X = %hW + $x;
            %screenPos_Y = %hH + $y;
        }

        // 5. set renderer and position elements
        if (%drawMode == 0) {
            %t = mClampF($z + %startDiv , 0.0 , 1.0 );

            if ( (%t - $z) > 1e-3)
            {
                %invT = 1.0 / %t;
                %startX = %hW + ($x * %invT);
                %startY = %hH + ($y * %invT);

                %this.setVector4(%i, %startX, %startY, %screenPos_X, %screenPos_Y);
            }
        } else {
            // Rectangles and Circle use the same data
            %radius = mLerp(5.0, 0.5, $z); // eat some fps
            %this.setVector4(%i, %screenPos_X, %screenPos_Y, %radius, %radius);
        }

        %this.setVector4_1GlobalV(%i, "$x", "$y", "$z", "$w");
        // %this.setVector4_1(%i, $x,$y,$z,0); // store the position on the secound vector4
    }

    // 6. Bulk render everything in C++ with a single engine call!

    if (%drawMode == 0) %this.drawLines(STAR_COUNT, "200 200 255 255");
    else if (%drawMode == 1) %this.drawRects(STAR_COUNT, "155 255 155");
    else %this.drawCircles(STAR_COUNT, "200 80 80");


    DrawText("[MOUSE WHEEL] Current Speed:" SPC  strFormat("%.2f",%speed) , 10, 40, 20, RAYWHITE);
    DrawText("[SPACE] Current draw mode:"  SPC  %this.modeName , 10, 70, 20, RAYWHITE);

}
