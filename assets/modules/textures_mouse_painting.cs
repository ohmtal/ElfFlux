/*******************************************************************************************
*
*   raylib [textures] example - mouse painting
*
*   Example complexity rating: [★★★☆] 3/4
*
*   Example originally created with raylib 3.0, last time updated with raylib 3.0
*
*   Example contributed by Chris Dill (@MysteriousSpace) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2019-2025 Chris Dill (@MysteriousSpace) and Ramon Santamaria (@raysan5)
*
*   Ported to raylib ElfScript 2026 XXTH
*
********************************************************************************************/
function createtextures_mouse_painting() {
    %obj = new ScriptObject() {
        class = "textures_mouse_painting";
    };

    %this = %obj;
    // Testing elfScript "string" array:
    // Colors to choose from
    // ... gee .. my preprocessor kill this
    // %this.colors =
    //     "RAYWHITE YELLOW GOLD ORANGE PINK RED MAROON GREEN LIME DARKGREEN" SPC
    //     "SKYBLUE BLUE DARKBLUE PURPLE VIOLET DARKPURPLE BEIGE BROWN DARKBROWN" SPC
    //     "LIGHTGRAY GRAY DARKGRAY BLACK";

    // SO i use TAB separated :D
    %this.colorFields =
        RAYWHITE TAB YELLOW TAB GOLD TAB ORANGE TAB PINK TAB RED TAB MAROON TAB GREEN TAB LIME TAB DARKGREEN TAB
        SKYBLUE TAB BLUE TAB DARKBLUE TAB PURPLE TAB VIOLET TAB DARKPURPLE TAB BEIGE TAB BROWN TAB DARKBROWN TAB
        LIGHTGRAY TAB GRAY TAB DARKGRAY TAB BLACK;


    echo("Color count is:" SPC getFieldCount(%this.colorFields));

    %rf = "";
    for (%i = 0; %i < MAX_COLORS_COUNT; %i++)
    {
        %r = 10 + 30 * %i + 2 * %i SPC 10 SPC 30 SPC 30;
        %rf = %rf TAB %r;
    }

    %this.colorRectFields = trim(%rf);
    echo("Rect count is:" SPC getFieldCount(%this.colorRectFields) SPC "max:" SPC MAX_COLORS_COUNT);

    %this.colorSelected = %this.colorSelectedPrev = %this.colorMouseHover = 0;
    %this.brushSize = 20.0;
    %this.mouseWasPressed = false;

    %this.btnSaveRec =  750 SPC 10 SPC 40 SPC 30;
    %this.btnSaveMouseHover = false;
    %this.showSaveMessage = false;
    %this.saveMessageCounter = 0;


    %this.exportFile = "my_amazing_texture_painting.png";

    // Create a RenderTexture2D to use as a canvas
    %this.target = new RenderTextureObject() { width = GetScreenWidth(); height = GetScreenHeight(); };
    // Clear render texture before entering the game loop
    %this.clear();


    SetTargetFPS(120);              // Set our game to run at 120 frames-per-second



    return %obj;
}
//----------------------------------------------------------------------
function textures_mouse_painting::clear(%this) {
    // Clear render texture before entering the game loop
    %this.target.BeginTextureMode();
    ClearBackground(RAYWHITE);
    %this.target.EndTextureMode();
}
//----------------------------------------------------------------------
function textures_mouse_painting::onAdd(%this) {
    return true;
}
//----------------------------------------------------------------------
function textures_mouse_painting::OnRemove(%this) {
    %this.target.delete();
}
//----------------------------------------------------------------------
function textures_mouse_painting::Render(%this) {
    //------------------------------------------------------------------
    // Update
    //------------------------------------------------------------------
    %mousePos = GetMousePosition();

    // Move between colors with keys
    if (IsKeyPressed(KEY_RIGHT)) %this.colorSelected++;
    else if (IsKeyPressed(KEY_LEFT)) %this.colorSelected--;

    // different to C demo, i wrap around
    if (%this.colorSelected >= MAX_COLORS_COUNT) %this.colorSelected = 0;
    else if (%this.colorSelected < 0) %this.colorSelected = MAX_COLORS_COUNT - 1;

    // Choose color with mouse
    for (%i = 0; %i < MAX_COLORS_COUNT; %i++)
    {
        if (CheckCollisionPointRec(%mousePos, getField(%this.colorRectFields, %i)))
        {
            %this.colorMouseHover = %i;
            break;
        }
        else %this.colorMouseHover = -1;
    }

    if ((%this.colorMouseHover >= 0) && IsMouseButtonPressed(MOUSE_BUTTON_LEFT))
    {
        %this.colorSelected = %this.colorMouseHover;
        %this.colorSelectedPrev = %this.colorSelected;
    }

    // Change brush size
    %this.brushSize += GetMouseWheelMove()*5;
    if (%this.brushSize < 2) %this.brushSize = 2;
    if (%this.brushSize > 50) %this.brushSize = 50;

    if (IsKeyPressed(KEY_C))
    {
        // Clear render texture to clear color
        %this.clear();
    }

    if (IsMouseButtonDown(MOUSE_BUTTON_LEFT) || (GetGestureDetected() == GESTURE_DRAG))
    {
        // Paint circle into render texture
        // NOTE: To avoid discontinuous circles, we could store
        // previous-next mouse points and just draw a line using brush size
        %this.target.BeginTextureMode();
        if (%mousePos.y > 50) {
            DrawCircle(
                %mousePos.x,
                %mousePos.y,
                %this.brushSize,
                getField(%this.colorFields, %this.colorSelected)
            );
        }
        %this.target.EndTextureMode();
    }

    if (IsMouseButtonDown(MOUSE_BUTTON_RIGHT))
    {
        if (!%this.mouseWasPressed)
        {
            %this.colorSelectedPrev = %this.colorSelected;
            %this.colorSelected = 0;
        }

        %this.mouseWasPressed = true;

        // Erase circle from render texture
        %this.target.BeginTextureMode();
        if (%mousePos.y > 50) {
            DrawCircle(
                %mousePos.x,
                %mousePos.y,
                %this.brushSize,
                RAYWHITE
            );
        }
        %this.target.EndTextureMode();

    }
    else if (IsMouseButtonReleased(MOUSE_BUTTON_RIGHT) && %this.mouseWasPressed)
    {
        %this.colorSelected = %this.colorSelectedPrev;
        %this.mouseWasPressed = false;
    }

    // Check mouse hover save button
    if (CheckCollisionPointRec(%mousePos, %this.btnSaveRec)) %this.btnSaveMouseHover = true;
    else %this.btnSaveMouseHover = false;

    // Image saving logic
    // NOTE: Saving painted texture to a default named image
    if ((%this.btnSaveMouseHover && IsMouseButtonReleased(MOUSE_BUTTON_LEFT)) || IsKeyPressed(KEY_S))
    {
        %img = LoadImageFromTexture(%this.target.texture);
        ImageFlipVertical( %img );
        ExportImage( %img , %this.exportFile);
        UnloadImage(%img);
        %this.showSaveMessage = true;
    }


    if (%this.showSaveMessage)
    {
        // On saving, show a full screen message for 2 seconds
        %this.saveMessageCounter++;
        if (%this.saveMessageCounter > 240)
        {
            %this.showSaveMessage = false;
            %this.saveMessageCounter = 0;
        }
    }
    //------------------------------------------------------------------
    // Draw
    //------------------------------------------------------------------
    ClearBackground(GRAY);

    // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
    %rect = "0 0" SPC %this.target.width SPC - %this.target.height;
    DrawTextureRec(%this.target.texture
        , %rect
        , "0 0"
        , WHITE );
    DrawRectangleLinesEx(%rect, 2, BLACK);

    // Draw drawing circle for reference
    if (%mousePos.y > 50)
    {
        if (IsMouseButtonDown(MOUSE_BUTTON_RIGHT)) {
            DrawCircleLines( mousePos.x, mousePos.y, %this.brushSize, GRAY);
        } else {
            DrawCircle(GetMouseX(), GetMouseY(), %this.brushSize
            , getField( %this.colorFields, %this.colorSelected)
            );
        }
    }

    // Draw top panel
    DrawRectangle(0, 0, GetScreenWidth(), 50, RAYWHITE);
    DrawLine(0, 50, GetScreenWidth(), 50, LIGHTGRAY);

    // Draw color selection rectangles
    for (%i = 0; %i < MAX_COLORS_COUNT; %i++) {
        // DrawRectangleRec(colorsRecs[i], colors[i]);
        DrawRectangleRec(
            getField(%this.colorRectFields, %i)
            ,getField(%this.colorFields, %i)
        );
    }
    DrawRectangleLines(10, 10, 30, 30, LIGHTGRAY);

    if (%this.colorMouseHover >= 0) {
        // DrawRectangleRec(colorsRecs[colorMouseHover], Fade(WHITE, 0.6f));
        DrawRectangleRec( getField(%this.colorRectFields, %this.colorMouseHover) , Fade(WHITE, 0.6));
    }
    %rect = getField(%this.colorRectFields, %this.colorSelected);
    DrawRectangleLinesEx(%rect.x - 2 SPC %rect.y - 2 SPC %rect.width + 4 SPC %rect.height + 4, 2, BLACK);


    // Draw save image button
    DrawRectangleLinesEx(%this.btnSaveRec, 2, %this.btnSaveMouseHover ? RED : BLACK);
    DrawText("SAVE!", 755, 20, 10, %this.btnSaveMouseHover ? RED : BLACK);


    // Draw save image message
    if (%this.showSaveMessage)
    {
        DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), Fade(RAYWHITE, 0.8));
        DrawRectangle(0, 150, GetScreenWidth(), 80, BLACK);
        DrawText("IMAGE" SPC %this.exportFile SPC "SAVED!", 150, 180, 20, RAYWHITE);
    }

}


// Number of colors available
#define MAX_COLORS_COUNT 23

