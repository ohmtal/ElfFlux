function createEyes() {
    %eyes = new ScriptObject() {
        class = "Eyes";
        TypeVector2 mScleraLeftPosition = "0 0";
        TypeVector2 mScleraRightPosition = "0 0";
        TypeF32 mScleraRadius = 60;

        TypeVector2 mIrisLeftPosition = "0 0";
        TypeVector2 mIrisRightPosition = "0 0";
        TypeF32 mIrisRadius = 24;

        TypeF32 mIrisAngle = 0.0;
        TypeF32 dx = 0.0;
        TypeF32 dy = 0.0;
        TypeF32 dxx = 0.0;
        TypeF32 dyy = 0.0;
    };

    return %eyes;
}


function Eyes::Init(%this) {
    %this.mScleraLeftPosition  =  GetScreenWidth() - 200.0 SPC GetScreenHeight() - 100.0;
    %this.mScleraRightPosition =  GetScreenWidth() - 60.0  SPC GetScreenHeight() - 100.0;

    %this.mIrisLeftPosition = %this.mScleraLeftPosition;
    %this.mIrisRightPosition = %this.mScleraRightPosition;

    return true;
}
//----------------------------------------------------------------------
function Eyes::Update(%this, %customPos)  {

    if (IsWindowResized()) {
        %this.mScleraLeftPosition  =  GetScreenWidth() - 200.0 SPC GetScreenHeight() - 100.0;
        %this.mScleraRightPosition =  GetScreenWidth() - 60.0  SPC GetScreenHeight() - 100.0;
    }
    if (%customPos !$= "") %this.mIrisLeftPosition = %this.mIrisRightPosition = %customPos;
    else %this.mIrisLeftPosition = %this.mIrisRightPosition = GetMousePosition();

    // Check not inside the left eye sclera
    if (!CheckCollisionPointCircle(%this.mIrisLeftPosition,%this.mScleraLeftPosition,
        %this.mScleraRadius - %this.mIrisRadius))
    {
        %this.dx = %this.mIrisLeftPosition.x - %this.mScleraLeftPosition.x;
        %this.dy = %this.mIrisLeftPosition.y - %this.mScleraLeftPosition.y;

        //FIXME ?
        %this.mIrisAngle = mAtan(%this.dy, %this.dx);

        %this.dxx = (%this.mScleraRadius - %this.mIrisRadius) * mCos(%this.mIrisAngle);
        %this.dyy = (%this.mScleraRadius - %this.mIrisRadius) * mSin(%this.mIrisAngle);

        %this.mIrisLeftPosition.x =  %this.mScleraLeftPosition.x + %this.dxx;
        %this.mIrisLeftPosition.y =  %this.mScleraLeftPosition.y + %this.dyy;
    }

    // Check not inside the right eye sclera
    if (!CheckCollisionPointCircle(
        %this.mIrisRightPosition, %this.mScleraRightPosition,
        %this.mScleraRadius -  %this.mIrisRadius))
    {
        %this.dx =  %this.mIrisRightPosition.x -  %this.mScleraRightPosition.x;
        %this.dy =  %this.mIrisRightPosition.y -  %this.mScleraRightPosition.y;

        %this.mIrisAngle = mAtan(%this.dy, %this.dx);

        %this.dxx = (%this.mScleraRadius - %this.mIrisRadius) * mCos(%this.mIrisAngle);
        %this.dyy = (%this.mScleraRadius - %this.mIrisRadius) * mSin(%this.mIrisAngle);

        %this.mIrisRightPosition.x = %this.mScleraRightPosition.x + %this.dxx;
        %this.mIrisRightPosition.y = %this.mScleraRightPosition.y + %this.dyy;
    }

}
//----------------------------------------------------------------------
function Eyes::Render(%this) {
    // eyes
    DrawCircleV( %this.mScleraLeftPosition, %this.mScleraRadius, $LIGHTGRAY);
    DrawCircleV( %this.mIrisLeftPosition,  %this.mIrisRadius, $BROWN);
    DrawCircleV( %this.mIrisLeftPosition, 10, $BLACK);

    DrawCircleV( %this.mScleraRightPosition,  %this.mScleraRadius, $LIGHTGRAY);
    DrawCircleV( %this.mIrisRightPosition,  %this.mIrisRadius, $DARKGREEN);
    DrawCircleV( %this.mIrisRightPosition, 10, $BLACK);

}

