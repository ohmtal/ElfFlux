// -----------------------------------------------------------------------------
// attempt to optimize speed with global variables
// the loop over starcount is not really the best idea when using interpreted
// script.
// -----------------------------------------------------------------------------
// Benchmark (weak: only looking on fps)
// with 420 stars on a i5-7200U (Thinkpad T570 running arch):
// nativ c gpu limit lines ~ 2500fps Circles ~ 1000fps
// optimized torquescript ~75fps cpu limit
// not optimizes torquescript ~60fps cpu limit
// -----------------------------------------------------------------------------
// more optimations local variables are much faster than global
// batched is a bit better and release build make a big difference
// -----------------------------------------------------------------------------

// orig 420
#define STAR_COUNT 420

function createshapes_starfield_effect() {
    %obj = new ScriptObject() {
        class = "shapes_starfield_effect";
        // TypeF32 speed = 10.0/9.0;
        TypeColor bgColor = 0;

        TypeBool drawLines = true;
    };
    return %obj;
}
// -----------------------------------------------------------------------------
function shapes_starfield_effect::onRemove(%this) {
 // unload the variables
  deleteVariables("$starPoints*");
   // deleteVariables("$starPointsScreenPos*");
  SetTargetFPS(60);
}
// -----------------------------------------------------------------------------
function shapes_starfield_effect::UpdateSize(%this) {
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
function shapes_starfield_effect::OnAdd(%this) {
    %this.bgColor = ColorLerp( DARKBLUE, BLACK, 0.69);
    %this.updateSize();
    for ( %i = 0; %i < STAR_COUNT; %i++)
    {
        $starPoints_X[%i] = GetRandomValue($minH , $maxH);
        $starPoints_Y[%i] = GetRandomValue($minW , $maxW);
        $starPoints_Z[%i] = 1.0;

        // echo("STAR #" @ %i SPC $starPoints_X[%i] SPC $starPoints_Y[%i] SPC $starPoints_Z[%i]
        // SPC "rand" SPC GetRandomValue($minH ,  $maxH));
    }

    $speed = 10.0/9.0;


    SetTargetFPS(0);

    return true;
}

// -----------------------------------------------------------------------------
function shapes_starfield_effect::Render(%this) {

    %mouseMove = GetMouseWheelMove();

    if (%mouseMove != 0) $speed += %mouseMove / 10.0;
    $speed = mClampF($speed, 0.05, 5.0);


    // Toggle lines / points with space bar
    if (IsKeyPressed(KEY_SPACE)) {
        %this.drawLines = !%this.drawLines;
        echo("SPACE pressed new drawlines is: " SPC %this.drawLines );
    }
    if (IsKeyPressed(KEY_U)) {
        SetTargetFPS(0);
    }
    if (IsKeyPressed(KEY_R)) {
        SetTargetFPS(60);
    }

    if (IsWindowResized()) {
        %this.updateSize();
    }

     %dt = GetFrameTime();


    ClearBackground( %this.bgColor);

    %startDiv = 1.0 / 32.0;
    %drawLines = %this.drawLines;

    for ( %i = 0; %i < STAR_COUNT; %i++)
    {
        // put into local variables ... did not change so much as the %screenPos_Y variables
        %x = $starPoints_X[%i];
        %y = $starPoints_Y[%i];
        %z = $starPoints_Z[%i];

        // >>>>>>>>>>>>>>>> update point
        %z -= %dt * $speed;

        // Calculate the screen position
        %invZ = 1.0 / %z;
        %screenPos_X = $hW + (%x * %invZ);
        %screenPos_Y = $hH + (%y * %invZ);

        if ( %z < 0.0 || %screenPos_X < 0.0 || %screenPos_Y < 0.0 || %screenPos_X > $screenWidth || %screenPos_Y > $screenHeight)
        {
            %x = GetRandomValue($minH, $maxH);
            %y = GetRandomValue($minW, $maxW);
            %z = 1.0;

            // store new values
            $starPoints_X[%i] = %x;
            $starPoints_Y[%i] = %y;

            %screenPos_X = $hW + %x;
            %screenPos_Y = $hH + %y;
        }
        // <<<<<<<<<<<<<<<<<<<<<
        if (%drawLines)
        {
            %t = mClampF(%z + %startDiv , 0.0 , 1.0 );

            if ( (%t - %z) > 1e-3)
            {
                %invT = 1.0 / %t;
                %startX = $hW + (%x * %invT);
                %startY = $hH + (%y * %invT);

                DrawLine(%startX, %startY, %screenPos_X, %screenPos_Y, RAYWHITE);
            }
        }
        else
        {
            %radius = mLerp(3.0, 1.0, %z);
            DrawCircle(%screenPos_X, %screenPos_Y, %radius, RAYWHITE);
        }

        // store back z it change every time
        $starPoints_Z[%i] = %z;
    }


    DrawText("[MOUSE WHEEL] Current Speed:" SPC  strFormat("%.2f",$speed) , 10, 40, 20, RAYWHITE);
    DrawText("[SPACE] Current draw mode:" SPC  ( %this.drawLines ? "Lines" : "Circles" ) , 10, 70, 20, RAYWHITE);

    DrawFPS(10, 10);
}
