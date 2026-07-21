#pragma once

#include "RayTal/Main.h"

namespace RayTal::Demo {
class Eyes: public Core {
    Vector2 mScleraLeftPosition;
    Vector2 mScleraRightPosition;
    F32 mScleraRadius = 60;

    Vector2 mIrisLeftPosition;
    Vector2 mIrisRightPosition;
    F32 mIrisRadius = 24;

    F32 mIrisAngle = 0.0f;
    F32 dx = 0.0f, dy = 0.0f, dxx = 0.0f, dyy = 0.0f;
public:
    //----------------------------------------------------------------------
    bool Init() override {
        mScleraLeftPosition = { GetScreenWidth() - 200.0f, GetScreenHeight() - 100.f };
        mScleraRightPosition = { GetScreenWidth() - 60.0f, GetScreenHeight() - 100.f };

        mIrisLeftPosition = mScleraLeftPosition;
        mIrisRightPosition = mScleraRightPosition;

        return true;
    }
    //----------------------------------------------------------------------
    void Update(F32 dt) override {

        if (IsWindowResized()) {
            mScleraLeftPosition = { GetScreenWidth() - 200.0f, GetScreenHeight() - 100.f };
            mScleraRightPosition = { GetScreenWidth() - 60.0f, GetScreenHeight() - 100.f };
        }

        mIrisLeftPosition = GetMousePosition();
        mIrisRightPosition = GetMousePosition();

        // Check not inside the left eye sclera
        if (!CheckCollisionPointCircle(mIrisLeftPosition, mScleraLeftPosition, mScleraRadius - mIrisRadius))
        {
            dx = mIrisLeftPosition.x - mScleraLeftPosition.x;
            dy = mIrisLeftPosition.y - mScleraLeftPosition.y;

            mIrisAngle = atan2f(dy, dx);

            dxx = (mScleraRadius - mIrisRadius)*cosf(mIrisAngle);
            dyy = (mScleraRadius - mIrisRadius)*sinf(mIrisAngle);

            mIrisLeftPosition.x = mScleraLeftPosition.x + dxx;
            mIrisLeftPosition.y = mScleraLeftPosition.y + dyy;
        }

        // Check not inside the right eye sclera
        if (!CheckCollisionPointCircle(mIrisRightPosition, mScleraRightPosition, mScleraRadius - mIrisRadius))
        {
            dx = mIrisRightPosition.x - mScleraRightPosition.x;
            dy = mIrisRightPosition.y - mScleraRightPosition.y;

            mIrisAngle = atan2f(dy, dx);

            dxx = (mScleraRadius - mIrisRadius)*cosf(mIrisAngle);
            dyy = (mScleraRadius - mIrisRadius)*sinf(mIrisAngle);

            mIrisRightPosition.x = mScleraRightPosition.x + dxx;
            mIrisRightPosition.y = mScleraRightPosition.y + dyy;
        }

    }
    //----------------------------------------------------------------------
    void Render() override {
        // eyes
        DrawCircleV(mScleraLeftPosition, mScleraRadius, LIGHTGRAY);
        DrawCircleV(mIrisLeftPosition, mIrisRadius, BROWN);
        DrawCircleV(mIrisLeftPosition, 10, BLACK);

        DrawCircleV(mScleraRightPosition, mScleraRadius, LIGHTGRAY);
        DrawCircleV(mIrisRightPosition, mIrisRadius, DARKGREEN);
        DrawCircleV(mIrisRightPosition, 10, BLACK);

    }

};
}
