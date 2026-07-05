

function createshapes_starfield_effect_v1() {
    %obj = new ScriptObject() {
        class = "shapes_starfield_effect_v1";
        TypeF32 speed = 10.0/9.0;
        TypeColor bgColor = 0;

        TypeBool drawLines = true;
        TypeS32 startCount = 120; // 420;
    };
    return %obj;
}

function shapes_starfield_effect_v1::onRemove(%this) {
 // unload the variables
  deleteVariables("$starPoints*");
  deleteVariables("$starPointsScreenPos*");

   SetTargetFPS(60);
}

function shapes_starfield_effect_v1::OnAdd(%this) {
    %this.bgColor = ColorLerp( DARKBLUE,  BLACK, 0.89);

    %this.screenHeight = GetScreenHeight();
    %this.screenWidth = GetScreenWidth();

    for ( %i = 0; %i < %this.startCount; %i++)
    {
        %this.addTypeField("stars" @ %i, "TypeVector3", 0 SPC 0 SPC 0 ); // does not help
        $starPoints[%i] = "0 0 0";
        $starPointsScreenPos[%i] = "0 0";
        // failes on fields with fake array !!!!!!!!!!
        $starPoints[%i].x = GetRandomValue(-%this.screenWidth / 2 ,  %this.screenWidth / 2);
        $starPoints[%i].y = GetRandomValue(-%this.screenHeight / 2 ,   %this.screenHeight / 2);
        $starPoints[%i].z = 1.0;
        // $starPoints[%i] = GetRandomValue(-%this.screenWidth / 2 ,  %this.screenWidth / 2)
        //     SPC GetRandomValue(-%this.screenWidth / 2 ,  %this.screenWidth / 2)
        //     SPC 1.0;
        echo("STAR #" @ %i SPC $starPoints[%i] SPC "rand" SPC GetRandomValue(-%this.screenWidth / 2 ,  %this.screenWidth / 2));
    }

    SetTargetFPS(0);

    return true;
}

function shapes_starfield_effect_v1::UpdatePoint(%this, %i , %dt) {
    // Update star's timer
    $starPoints[%i].z -= %dt * %this.speed;

    // Calculate the screen position
    $starPointsScreenPos[%i] =
    %this.screenWidth * 0.5 + $starPoints[%i].x / $starPoints[%i].z SPC
    %this.screenHeight * 0.5 + $starPoints[%i].y/$starPoints[%i].z
    ;

    // If the star is too old, or offscreen, it dies and we make a new random one
    if (($starPoints[%i].z < 0.0) || ($starPointsScreenPos[%i].x < 0) || ($starPointsScreenPos[%i].y < 0.0) ||
        ($starPointsScreenPos[%i].x > %this.screenWidth) || ($starPointsScreenPos[%i].y > %this.screenHeight))
    {
        $starPoints[%i].x = GetRandomValue(-%this.screenWidth / 2, %this.screenWidth / 2);
        $starPoints[%i].y = GetRandomValue(-%this.screenHeight / 2, %this.screenHeight / 2);
        $starPoints[%i].z = 1.0;
    }
    // echo("STAR #" @ %i SPC $starPoints[%i]
    //     SPC "SCREENPOS:" SPC $starPointsScreenPos[%i]
    //     SPC "SCREEN:" SPC %this.screenWidth SPC %this.screenHeight);
}

function shapes_starfield_effect_v1::Render(%this) {

    if (IsKeyPressed(KEY_U)) {
        SetTargetFPS(0);
    }
    if (IsKeyPressed(KEY_R)) {
        SetTargetFPS(60);
    }

    // Toggle lines / points with space bar
    if (IsKeyPressed(KEY_SPACE)) {
        %this.drawLines = !%this.drawLines;
        echo("SPACE pressed new drawlines is: " SPC %this.drawLines );
    }



    %mouseMove = GetMouseWheelMove();

    if (%mouseMove != 0) %this.speed += 2.0 * %mouseMove / 9.0;
    if (%this.speed < 0.0) %this.speed = 0.1;
    else if (%this.speed > 2.0) %this.speed = 2.0;



     %dt = GetFrameTime();


    ClearBackground(%this.bgColor);
    for ( %i = 0; %i < %this.startCount; %i++)
    {
        %this.updatePoint(%i, %dt);
        if (%this.drawLines)
        {
            // Get the time a little while ago for this star, but clamp it
            %t = mClampF($starPoints[%i].z + 1.0 / 32.0 , 0.0 , 1.0 );
            // If it's different enough from the current time, we proceed
            if ((%t - $starPoints[%i].z) > 1e-3)
            {
                // Calculate the screen position of the old point
                %startPos =
                    %this.screenWidth  * 0.5 + $starPoints[%i].x  / %t SPC
                    %this.screenHeight * 0.5 + $starPoints[%i].y / %t;


                // Draw a line connecting the old point to the current point
                DrawLineV(%startPos, $starPointsScreenPos[%i], RAYWHITE);
            }
        }
        else
        {
            // Make the %this.radius grow as the star ages
            // Draw the circle
            %radius = mLerp($starPoints[%i].z, 1.0, 5.0);
            // DrawText("DrawCircles: Screenpos:" SPC $starPointsScreenPos[%i]
            //         SPC "rad:" SPC %radius
            //         SPC "starpos:" SPC $starPoints[%i]
            //         , 10, 100, 20, $RAYWHITE);
            DrawCircleV($starPointsScreenPos[%i], %radius, RAYWHITE);
        }
    }

    DrawText("(V1)[MOUSE WHEEL] Current Speed:" SPC  9.0 * %this.speed / 2.0 , 10, 40, 20, RAYWHITE);
    DrawText("[SPACE] Current draw mode:" SPC  ( %this.drawLines ? "Lines" : "Circles" ) , 10, 70, 20, RAYWHITE);

     DrawFPS(10, 10);
}
