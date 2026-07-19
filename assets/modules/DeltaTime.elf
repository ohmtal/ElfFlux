function CreateDeltaTime() {
    %module = new ScriptObject() {
        class = "DeltaTime";
        TypeS32 currentFps = 60;
        TypeF32 speed = 10;
        TypeF32 circleRadius = 32;
    };
    return %module;

}

function DeltaTime::onRemove(%this) {
    SetTargetFPS(60); //guess it was 60 :P
}

function DeltaTime::OnAdd(%this) {

    %this.screenHeight = GetScreenHeight();
    %this.screenWidth = GetScreenWidth();


    %this.addTypeField("deltaCircle", "TypeVector2", 0 SPC %this.screenHeight / 3.0 );
    %this.addTypeField("frameCircle", "TypeVector2", 0 SPC %this.screenHeight * 2.0 / 3.0 );

    %this.frameCircle = %this.deltaCircle;
    return true;
}

function DeltaTime::render(%this) {
    // update>>>>
    // Adjust the FPS target based on the mouse wheel
    %mouseWheel = GetMouseWheelMove();
    if (%mouseWheel != 0)
    {
        %this.currentFps += %mouseWheel;
        if (%this.currentFps < 0) %this.currentFps = 0;
        SetTargetFPS(%this.currentFps);
    }

    %this.deltaCircle.x += GetFrameTime() * 6.0 * %this.speed;
    %this.frameCircle.x += 0.1 * %this.speed;

    if (%this.deltaCircle.x > %this.screenWidth) %this.deltaCircle.x = 0;
    if (%this.frameCircle.x > %this.screenWidth) %this.frameCircle.x = 0;

    if (IsKeyPressed(KEY_R))
    {
        %this.deltaCircle.x = 0;
        %this.frameCircle.x = 0;
        %this.currentFps = 60;
        SetTargetFPS(%this.currentFps);
    }
    if (IsKeyPressed(KEY_U))
    {
        %this.currentFps = 0;
        SetTargetFPS(%this.currentFps);
    }

    //<<<update


    ClearBackground("120 120 120");

    // Draw both circles to the screen
    DrawCircleV(%this.deltaCircle, %this.circleRadius + 2, RED);
    DrawCircleV(%this.frameCircle, %this.circleRadius, BLUE);

    // Draw the help text
    // Determine what help text to show depending on the current FPS target
    // NOTE strFormat only takes ONE parameter so I use SPC concat in second
    if (%this.currentFps <= 0) %fpsText = strFormat("FPS: unlimited (%i)", GetFPS());
    else %fpsText = "FPS: " SPC GetFPS() SPC "target: " SPC %this.currentFps;
    DrawText(%fpsText, 10, 10, 20, DARKGRAY);
    DrawText(strFormat("Frame time: %02.02f ms", GetFrameTime()), 10, 30, 20, DARKGRAY);
    DrawText("Use the scroll wheel to change the fps limit, r to reset, u for unlimited", 10, 50, 20, DARKGRAY);

    // Draw the text above the circles
    DrawText("FUNC: x += GetFrameTime()*speed", 10, 90, 20, RED);
    DrawText("FUNC: x += speed", 10, 240, 20, BLUE);

}
