/*******************************************************************************************
*
*   raylib [textures] example - background scrolling
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 2.0, last time updated with raylib 2.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2019-2025 Ramon Santamaria (@raysan5)
*
*   Ported to raylib ElfScript 2026 XXTH
*
********************************************************************************************/
function createtextures_background_scrolling() {
    %obj = new ScriptObject() {
        class = "textures_background_scrolling";
    };
    %this = %obj;

    %this.background = LoadTexture("assets/texture/cyberpunk_street_background.png");
    %this.backgroundWidth2 = getTextureWidth(%this.background) * 2.0;
    %this.midground = LoadTexture("assets/texture/cyberpunk_street_midground.png");
    %this.midgroundWidth2 = getTextureWidth(%this.midground) * 2.0;
    %this.foreground = LoadTexture("assets/texture/cyberpunk_street_foreground.png");
    %this.foregroundWidth2 = getTextureWidth(%this.foreground) * 2.0;

    %this.scrollingBack = 0.0;
    %this.scrollingMid = 0.0;
    %this.scrollingFore = 0.0;

    %this.bgColor = GetColor(0x052c46ff);

    return %obj;
}
//----------------------------------------------------------------------
function textures_background_scrolling::onAdd(%this) {
    SetTargetFPS(60); //FPS TEST only calling module
    return true;
}
//----------------------------------------------------------------------
function textures_background_scrolling::OnRemove(%this) {
    SetTargetFPS(60);

    UnloadTexture(%this.background);  // Unload background texture
    UnloadTexture(%this.midground);   // Unload midground texture
    UnloadTexture(%this.foreground);  // Unload foreground texture

}
//----------------------------------------------------------------------
function textures_background_scrolling::Render(%this) {
    //------------------------------------------------------------------
    // Update
    //------------------------------------------------------------------
    %ft = GetFrameTime();
    %this.scrollingBack -= 10 * %ft;
    %this.scrollingMid  -= 50 * %ft;
    %this.scrollingFore -= 100 * %ft;

    // NOTE: Texture is scaled twice its size, so it sould be considered on scrolling
    if (%this.scrollingBack <= -%this.backgroundWidth2 ) %this.scrollingBack = 0;
    if (%this.scrollingMid  <= -%this.midgroundWidth2 ) %this.scrollingMid = 0;
    if (%this.scrollingFore <= -%this.foregroundWidth2 ) %this.scrollingFore = 0;
    //------------------------------------------------------------------
    // Draw
    //------------------------------------------------------------------

    ClearBackground(%this.bgColor);
    // Draw background image twice
    // NOTE: Texture is scaled twice its size
    DrawTextureEx(%this.background, %this.scrollingBack SPC 20 , 0.0, 2.0, WHITE);
    DrawTextureEx(%this.background, (%this.backgroundWidth2 + %this.scrollingBack) SPC 20 , 0.0, 2.0, WHITE);

    // Draw midground image twice
    DrawTextureEx(%this.midground, %this.scrollingMid SPC 20 , 0.0, 2.0, WHITE);
    DrawTextureEx(%this.midground, %this.midgroundWidth2 + %this.scrollingMid SPC 20 , 0.0, 2.0, WHITE);

    // Draw foreground image twice
    DrawTextureEx(%this.foreground, %this.scrollingFore SPC 70 , 0.0, 2.0, WHITE);
    DrawTextureEx(%this.foreground, %this.foregroundWidth2 + %this.scrollingFore SPC 70, 0.0, 2.0, WHITE);

    DrawText("BACKGROUND SCROLLING & PARALLAX", 10, 10, 20, RED);
    DrawText("(c) Cyberpunk Street Environment by Luis Zuno (@ansimuz)", GetScreenWidth() - 330, GetScreenHeight() - 20, 10, RAYWHITE);


}
