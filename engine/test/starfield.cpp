/*******************************************************************************************
 *
 *   raylib [shapes] example - starfield effect
 *
 *   Example complexity rating: [★★☆☆] 2/4
 *
 *   Example originally created with raylib 5.5, last time updated with raylib 6.0
 *
 *   Example contributed by JP Mortiboys (@themushroompirates) and reviewed by Ramon Santamaria (@raysan5)
 *
 *   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
 *   BSD-like license that allows static linking with closed source software
 *
 *   Copyright (c) 2025 JP Mortiboys (@themushroompirates)
 *
 *   NOTE: Ported to raylib-elfscript by XXTH on 2026-06-25
 *
 ********************************************************************************************/

#include "console/engineAPI.h"
#include "raylib.h"
#include "raymath.h"
#include "ConsoleTypes.h"

namespace ElfObjects {

    class StarField: public SimObject
    {
        typedef SimObject Parent;

        S32 mStarCount;
        S32 mMode;
        F32 mSpeed;
        F32 mScreenWidth;
        F32 mScreenHeight;
        Color mBgColor;
        Color mColor;
        Vector<Vector3> mStars;
        Vector<Vector2> mStarsScreenPos;

        void setCount(S32 count) {
            if (count <=0) return;
            mStarCount = count;
            mStars.setSize(mStarCount);
            mStarsScreenPos.setSize(mStarCount);
            initStars();
        }


    public:
        DECLARE_CONOBJECT(StarField);


        StarField() {
            mColor = { 255, 255, 255, 255};
            setCount( 420 ) ;
            mSpeed = 1.0f;
            mBgColor = ColorLerp(DARKBLUE, BLACK, 0.69f);
        }
        // ---------------------------------------------------------------------
        static bool _setStarsCount(void* obj,const char* , const char* data) {
            StarField* object = static_cast<StarField*>(obj);
            if (!object || !data) return false;
            S32 size = dAtoi(data);
            if ( size >= 0 ) {
                object->setCount(size);
            }
            return false;
        }
        // ---------------------------------------------------------------------
        static void initPersistFields()
        {
            addProtectedField("count", TypeS32, Offset(mStarCount, StarField), &_setStarsCount,&defaultProtectedGetFn, 1,"Set the mStars count.");
            addField("mode", TypeS32,     Offset(mMode, StarField), "mode 0=lines, 1=rectangles, 2=circles");
            addField("speed", TypeF32 , Offset(mSpeed, StarField), "set the mSpeed of the mStars");
            addField("color", TypeColor,     Offset(mColor, StarField), "color of the mStars");
            addField("BackgroundColor", TypeColor,     Offset(mBgColor, StarField), "color of the background");
             Parent::initPersistFields();
        }
        // ---------------------------------------------------------------------
        void initStars() {
            if (mStarCount != mStars.size()) return;
            mScreenWidth = GetScreenWidth();
            mScreenHeight = GetScreenHeight();

            for (int i = 0; i < mStarCount; i++)
            {
                mStars[i].x = (float)GetRandomValue(-mScreenWidth / 2, (int)mScreenWidth / 2);
                mStars[i].y = (float)GetRandomValue(-mScreenHeight / 2, (int)mScreenHeight / 2);
                mStars[i].z = 1.0f;

                mStarsScreenPos[i] = { 0.f , 0.f };
            }
        }
        // ---------------------------------------------------------------------
        void Loop() {
            if (IsWindowResized()) initStars();
            float dt = GetFrameTime();
            ClearBackground(mBgColor);

            for (int i = 0; i < mStarCount; i++)
            {
                // Update star's timer
                mStars[i].z -= dt * mSpeed;

                // Calculate the screen position
                mStarsScreenPos[i] = (Vector2){
                    mScreenWidth*0.5f + mStars[i].x/mStars[i].z,
                    mScreenHeight*0.5f + mStars[i].y/mStars[i].z,
                };

                // If the star is too old, or offscreen, it dies and we make a new random one
                if ((mStars[i].z < 0.0f) || (mStarsScreenPos[i].x < 0) || (mStarsScreenPos[i].y < 0.0f) ||
                    (mStarsScreenPos[i].x > mScreenWidth) || (mStarsScreenPos[i].y > mScreenHeight))
                {
                    mStars[i].x = (float)GetRandomValue(-mScreenWidth / 2, mScreenWidth / 2);
                    mStars[i].y = (float)GetRandomValue(-mScreenHeight / 2, mScreenHeight / 2);
                    mStars[i].z = 1.0f;
                }
                // -----

                // ----------------------- DRAW ------------------------------
                // done in script! BeginDrawing();
                if (mMode == 2)
                {
                    // Make the radius grow as the star ages
                    float radius = Lerp(mStars[i].z, 1.0f, 5.0f);
                    // Draw the circle
                    DrawCircleV(mStarsScreenPos[i], radius, mColor);
                } else if (mMode == 1 ) {
                    float radius = Lerp(mStars[i].z, 1.0f, 5.0f);
                    // Draw the Rect
                    DrawRectangle(mStarsScreenPos[i].x, mStarsScreenPos[i].y, radius, radius, mColor);
                } else {

                    // Get the time a little while ago for this star, but clamp it
                    float t = Clamp(mStars[i].z + 1.0f/32.0f, 0.0f, 1.0f);

                    // If it's different enough from the current time, we proceed
                    if ((t - mStars[i].z) > 1e-3)
                    {
                        // Calculate the screen position of the old point
                        Vector2 startPos = (Vector2){
                            mScreenWidth*0.5f + mStars[i].x/t,
                            mScreenHeight*0.5f + mStars[i].y/t,
                        };

                        // Draw a line connecting the old point to the current point
                        DrawLineV(startPos, mStarsScreenPos[i], mColor);
                    }
                }
            }

            //  DrawText(TextFormat("[MOUSE WHEEL] Current Speed: %.0f", 9.0f*speed/2.0f), 10, 40, 20, RAYWHITE);
            //  DrawText(TextFormat("[SPACE] Current draw mode: %s", drawLines ? "Lines" : "Circles"), 10, 70, 20, RAYWHITE);

            // DrawFPS(10, 10);

            // done in script! EndDrawing();
            //----------------------------------------------------------------------------------
        }
        // ---------------------------------------------------------------------

    };

    IMPLEMENT_CONOBJECT(StarField);

    DefineEngineMethod(StarField, loop, void, (), ,"update and render startfield") {
        object->Loop();
    }
}
