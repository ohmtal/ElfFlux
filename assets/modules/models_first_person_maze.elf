function createmodels_first_person_maze() {
    %obj = new ScriptObject() {
        class = "models_first_person_maze";
    };
    %this = %obj; // map to %this

    // ---- Camera

    %this.camera = new Camera3DObject() {
        position = "0.2 0.4 0.2";    // Camera position
        target = "0.185 0.4 0.0";      // Camera looking at point
        up = "0.0 1.0 0.0";          // Camera up vector (rotation towards target)
        fovy = 45.0;                  // Camera field-of-view Y
        projection = CAMERA_PERSPECTIVE;  // Camera projection type
    };

    %imMap = LoadImage("assets/texture/cubicmap.png");
    %this.cubicMap = LoadTextureFromImage(%imMap);
    %this.mapWidth = getTextureWidth(%this.cubicMap);
    %this.mapHeight = getTextureHeight(%this.cubicMap);
    %this.mesh = GenMeshCubicmap( %imMap, "1.0 1.0 1.0");
    %this.model = LoadModelFromMesh(%this.mesh);

    %this.texture = LoadTexture("assets/texture/cubicmap_atlas.png");
    SetModelMapTexture(%this.model, %this.texture);


    // Get map image data to be used for collision detection
    %this.mapPixels = LoadImageColors(%imMap);
    UnloadImage(%imMap);             // Unload image from RAM


    %this.mapPosition = "-16.0 0.0 -8.0";



    return %obj;
}
//----------------------------------------------------------------------
function models_first_person_maze::onAdd(%this) {
    DisableCursor();
    return true;
}
//----------------------------------------------------------------------
function models_first_person_maze::OnRemove(%this) {
    EnableCursor();

    %this.camera.delete();
    UnloadImageColors(%this.mapPixels);   // Unload color array
    UnloadTexture(%this.cubicmap);        // Unload cubicmap texture
    UnloadTexture(%this.texture);         // Unload map texture
    UnloadModel(%this.model);             // Unload map model

}
//----------------------------------------------------------------------
function models_first_person_maze::Render(%this) {
    ClearBackground(WHITE);

    // ----- Update
    %cam = %this.camera; //map for faster access
    %oldCamPos = %cam.position;
    %cam.update(CAMERA_FIRST_PERSON);

    // Check player collision (we simplify to 2D collision detection)
    %playerPos = %cam.position.x SPC %cam.position.z;
    %playerRadius = 0.1;

    %mapPosition = %this.mapPosition;
    %playerCellX = mFloor( %playerPos.x - %mapPosition.x + 0.5 );
    %playerCellY = mFloor( %playerPos.y - %mapPosition.z + 0.5 );

    // echo("%playerCellX:" SPC %playerCellX SPC "%playerCellY" SPC %playerCellY);

    %mW = %this.mapWidth;
    %mH = %this.mapHeight;

    // Out-of-limits security check
    if (%playerCellX < 0) %playerCellX = 0;
    else if (%playerCellX >= %mW) %playerCellX = %mW - 1;

    if (%playerCellY < 0) %playerCellY = 0;
    else if (%playerCellY >= %mH) %playerCellY = %mH - 1;


    // Check map collisions using image data and player position against surrounding cells only
    // optimized:
    %minY = (%playerCellY - 1 < 0) ? 0 : %playerCellY - 1;
    %maxY = (%playerCellY + 1 >= %mH) ? %mH - 1 : %playerCellY + 1;

    %minX = (%playerCellX - 1 < 0) ? 0 : %playerCellX - 1;
    %maxX = (%playerCellX + 1 >= %mW) ? %mW - 1 : %playerCellX + 1;

    for (%y = %minY; %y <= %maxY; %y++)
    {
        for (%x = %minX; %x <= %maxX; %x++)
        {
            %color = GetImageColorsColor(%this.mapPixels, %y * %mW + %x);
            if (%color.r != 255) continue;

            %colRect = %mapPosition.x - 0.5 + %x * 1.0 SPC %mapPosition.z - 0.5 + %y * 1.0 SPC 1.0 SPC 1.0;
            if (!CheckCollisionCircleRec(%playerPos, %playerRadius, %colRect)) continue;

            %cam.position = %oldCamPos;
            break;// break out here
        }
    }

    // for (%y = %playerCellY - 1; %y <= %playerCellY + 1; %y++)
    // {
    //     // Avoid map accessing out of bounds
    //     if ((%y >= 0) && (%y < %mH))
    //     {
    //         for (%x = %playerCellX - 1; %x <= %playerCellX + 1; %x++)
    //         {
    //             // NOTE: Collision: Only checking R channel for white pixel
    //             if ( %x < 0 ) continue;
    //             if ( %x >= %mW ) continue;
    //             %color = GetImageColorsColor( %this.mapPixels, %y * %mW + %x);
    //             if (%color.r != 255) continue;
    //             %colRect =%mapPosition.x - 0.5 + %x * 1.0 SPC %mapPosition.z - 0.5 + %y * 1.0 SPC  1.0 SPC 1.0;
    //             if (!CheckCollisionCircleRec(%playerPos, %playerRadius,%colRect)) continue;
    //
    //             // Collision detected, reset camera position
    //             %cam.position = %oldCamPos;
    //         }
    //     }
    // }
    // ----- Render
    ClearBackground(RAYWHITE);

    BeginMode3D(%cam);
        DrawModel(%this.model, %mapPosition, 1.0, WHITE);                     // Draw maze map
    EndMode3D();

    %mw4 = %this.mapWidth * 4;
    DrawTextureEx( %this.cubicmap,  (GetScreenWidth() - %mw4 - 20 SPC  20.0 ), 0.0, 4.0, WHITE);
    DrawRectangleLines(GetScreenWidth() - %mw4 - 20, 20, %mw4, %this.mapHeight * 4, GREEN);

    // Draw player position radar
    DrawRectangle(GetScreenWidth() - %mw4 - 20 + %playerCellX * 4, 20 + %playerCellY * 4, 4, 4, RED);

    // DrawFPS(10, 10);
}



// /*******************************************************************************************
// *
// *   raylib [models] example - first person maze
// *
// *   Example complexity rating: [★★☆☆] 2/4
// *
// *   Example originally created with raylib 2.5, last time updated with raylib 3.5
// *
// *   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
// *   BSD-like license that allows static linking with closed source software
// *
// *   Copyright (c) 2019-2025 Ramon Santamaria (@raysan5)
// *
// ********************************************************************************************/
//
// #include "raylib.h"
//
// #include <stdlib.h>           // Required for: free()
//
// //------------------------------------------------------------------------------------
// // Program main entry point
// //------------------------------------------------------------------------------------
// int main(void)
// {
//     // Initialization
//     //--------------------------------------------------------------------------------------
//     const int screenWidth = 800;
//     const int screenHeight = 450;
//
//     InitWindow(screenWidth, screenHeight, "raylib [models] example - first person maze");
//
//     // Define the camera to look into our 3d world
//     Camera camera = { 0 };
//     camera.position = (Vector3){ 0.2f, 0.4f, 0.2f };    // Camera position
//     camera.target = (Vector3){ 0.185f, 0.4f, 0.0f };    // Camera looking at point
//     camera.up = (Vector3){ 0.0f, 1.0f, 0.0f };          // Camera up vector (rotation towards target)
//     camera.fovy = 45.0f;                                // Camera field-of-view Y
//     camera.projection = CAMERA_PERSPECTIVE;             // Camera projection type
//
//     Image imMap = LoadImage("resources/cubicmap.png");      // Load cubicmap image (RAM)
//     Texture2D cubicmap = LoadTextureFromImage(imMap);       // Convert image to texture to display (VRAM)
//     Mesh mesh = GenMeshCubicmap(imMap, (Vector3){ 1.0f, 1.0f, 1.0f });
//     Model model = LoadModelFromMesh(mesh);
//
//     // NOTE: By default each cube is mapped to one part of texture atlas
//     Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");    // Load map texture
//     model.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texture;    // Set map diffuse texture
//
//     // Get map image data to be used for collision detection
//     Color *mapPixels = LoadImageColors(imMap);
//     UnloadImage(imMap);             // Unload image from RAM
//
//     Vector3 mapPosition = { -16.0f, 0.0f, -8.0f };  // Set model position
//
//     DisableCursor();                // Limit cursor to relative movement inside the window
//
//     SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
//     //--------------------------------------------------------------------------------------
//
//     // Main game loop
//     while (!WindowShouldClose())    // Detect window close button or ESC key
//     {
//         // Update
//         //----------------------------------------------------------------------------------
//         Vector3 oldCamPos = camera.position;    // Store old camera position
//
//         UpdateCamera(&camera, CAMERA_FIRST_PERSON);
//
//         // Check player collision (we simplify to 2D collision detection)
//         Vector2 playerPos = { camera.position.x, camera.position.z };
//         float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)
//
//         int playerCellX = (int)(playerPos.x - mapPosition.x + 0.5f);
//         int playerCellY = (int)(playerPos.y - mapPosition.z + 0.5f);
//
//         // Out-of-limits security check
//         if (playerCellX < 0) playerCellX = 0;
//         else if (playerCellX >= cubicmap.width) playerCellX = cubicmap.width - 1;
//
//         if (playerCellY < 0) playerCellY = 0;
//         else if (playerCellY >= cubicmap.height) playerCellY = cubicmap.height - 1;
//
//         // Check map collisions using image data and player position against surrounding cells only
//         for (int y = playerCellY - 1; y <= playerCellY + 1; y++)
//         {
//             // Avoid map accessing out of bounds
//             if ((y >= 0) && (y < cubicmap.height))
//             {
//                 for (int x = playerCellX - 1; x <= playerCellX + 1; x++)
//                 {
//                     // NOTE: Collision: Only checking R channel for white pixel
//                     if (((x >= 0) && (x < cubicmap.width)) &&
//                         (mapPixels[y*cubicmap.width + x].r == 255) &&
//                         (CheckCollisionCircleRec(playerPos, playerRadius,
//                         (Rectangle){ mapPosition.x - 0.5f + x*1.0f, mapPosition.z - 0.5f + y*1.0f, 1.0f, 1.0f })))
//                     {
//                         // Collision detected, reset camera position
//                         camera.position = oldCamPos;
//                     }
//                 }
//             }
//         }
//         //----------------------------------------------------------------------------------
//
//         // Draw
//         //----------------------------------------------------------------------------------
//         BeginDrawing();
//
//             ClearBackground(RAYWHITE);
//
//             BeginMode3D(camera);
//                 DrawModel(model, mapPosition, 1.0f, WHITE);                     // Draw maze map
//             EndMode3D();
//
//             DrawTextureEx(cubicmap, (Vector2){ GetScreenWidth() - cubicmap.width*4.0f - 20, 20.0f }, 0.0f, 4.0f, WHITE);
//             DrawRectangleLines(GetScreenWidth() - cubicmap.width*4 - 20, 20, cubicmap.width*4, cubicmap.height*4, GREEN);
//
//             // Draw player position radar
//             DrawRectangle(GetScreenWidth() - cubicmap.width*4 - 20 + playerCellX*4, 20 + playerCellY*4, 4, 4, RED);
//
//             DrawFPS(10, 10);
//
//         EndDrawing();
//         //----------------------------------------------------------------------------------
//     }
//
//     // De-Initialization
//     //--------------------------------------------------------------------------------------
//     UnloadImageColors(mapPixels);   // Unload color array
//
//     UnloadTexture(cubicmap);        // Unload cubicmap texture
//     UnloadTexture(texture);         // Unload map texture
//     UnloadModel(model);             // Unload map model
//
//     CloseWindow();                  // Close window and OpenGL context
//     //--------------------------------------------------------------------------------------
//
//     return 0;
// }
