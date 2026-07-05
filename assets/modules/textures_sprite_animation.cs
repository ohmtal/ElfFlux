/*******************************************************************************************
*
*   raylib [textures] example - sprite animation
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.3, last time updated with raylib 1.3
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2014-2025 Ramon Santamaria (@raysan5)
*
*   Ported to raylib ElfScript 2026 XXTH
*
********************************************************************************************/
function createtextures_sprite_animation() {
    %obj = new ScriptObject() {
        class = "textures_sprite_animation";
    };

    %this = %obj;
    // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
    %this.scarfy = LoadTexture("assets/texture/scarfy.png");        // Texture loading
    if (%this.scarfy == 0 ) return false;

    %this.position =  "350.0 280.0";
    %this.frameRec =  "0 0" SPC getTextureWidth(%this.scarfy) / 6.0 SPC getTextureHeight(%this.scarfy);
    %this.currentFrame = 0;

    %this.framesCounter = 0;
    %this.framesSpeed = 8;            // Number of spritesheet frames shown by second


    return %obj;
}
//----------------------------------------------------------------------
function textures_sprite_animation::onAdd(%this) {
    SetTargetFPS(60);
    return true;
}
//----------------------------------------------------------------------
function textures_sprite_animation::OnRemove(%this) {
    SetTargetFPS(60);
    UnloadTexture(%this.scarfy);
}
//----------------------------------------------------------------------
function textures_sprite_animation::Render(%this) {

    //------------------------------------------------------------------
    // Update
    //------------------------------------------------------------------
    %this.framesCounter++;

    if (%this.framesCounter >= ( 60 / %this.framesSpeed))
    {
        %this.framesCounter = 0;
        %this.currentFrame++;

        if (%this.currentFrame > 5) %this.currentFrame = 0;

        %this.frameRec.x =  %this.currentFrame * getTextureWidth(%this.scarfy) / 6.0;
        // echo(%this.frameRec);
    }

    // Control frames speed
    if (IsKeyPressed(KEY_RIGHT)) %this.framesSpeed++;
    else if (IsKeyPressed(KEY_LEFT)) %this.framesSpeed--;

    if (%this.framesSpeed > MAX_FRAME_SPEED) %this.framesSpeed = MAX_FRAME_SPEED;
    else if (%this.framesSpeed < MIN_FRAME_SPEED) %this.framesSpeed = MIN_FRAME_SPEED;
    //------------------------------------------------------------------
    // Draw
    //------------------------------------------------------------------
    ClearBackground(RAYWHITE);

    DrawTexture(%this.scarfy, 15, 40, WHITE);
    DrawRectangleLines(15, 40, getTextureWidth(%this.scarfy), getTextureHeight(%this.scarfy), LIME);
    DrawRectangleLines(15 + %this.frameRec.x, 40 + %this.frameRec.y, %this.frameRec.width, %this.frameRec.height, RED);


    // DrawRectangleLines(15 + frameRec.x SPC 40 + frameRec.y SPC frameRec.z SPC frameRec.w, RED);

    DrawText("FRAME SPEED: ", 165, 210, 10, DARKGRAY);
    DrawText(strFormat("%02i FPS", %this.framesSpeed), 575, 210, 10, DARKGRAY);
    DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, DARKGRAY);

    for (%i = 0; %i < MAX_FRAME_SPEED; %i++)
    {
        if (%i < %this.framesSpeed) DrawRectangle(250 + 21 * %i, 205, 20, 20, RED);
        DrawRectangleLines(250 + 21 * %i, 205, 20, 20, MAROON);
    }

    DrawTextureRec(%this.scarfy, %this.frameRec, %this.position, WHITE);  // Draw part of the texture

    DrawText("(c) Scarfy sprite by Eiden Marsal", GetScreenWidth() - 200, GetScreenHeight() - 20, 10, GRAY);
}

#define MAX_FRAME_SPEED     15
#define MIN_FRAME_SPEED      1
