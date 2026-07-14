//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#include "SceneObject2D.h"
#include "SceneContainer2D.h"

#include "raylib.h"
#include "raymath.h"
#include "math/mMathFn.h"


//-----------------------------------------------------------------------------
namespace ElfObjects {

IMPLEMENT_CONOBJECT(SceneObject2D);

//-----------------------------------------------------------------------------
SceneObject2D::SceneObject2D() {
}

//-----------------------------------------------------------------------------
bool SceneObject2D::_setFieldPosition( void *object, const char *index, const char *data )
{
    SceneObject2D* so = static_cast<SceneObject2D*>( object );
    if ( so )
    {
        Vector3 newPos( so->mPosition );
        Con::setData( TypeVector3, &newPos, 0, 1, &data );
        so->setPosition( newPos );
    }
    return false;
}
//-----------------------------------------------------------------------------
bool SceneObject2D::_setFieldSize( void *object, const char *index, const char *data )
{
    SceneObject2D* so = static_cast<SceneObject2D*>( object );
    if ( so )
    {
        Vector2 newSize( so->mSize );
        Con::setData( TypeVector2, &newSize, 0, 1, &data );
        so->setSize( newSize );
    }
    return false;
}

//-----------------------------------------------------------------------------
void SceneObject2D::initPersistFields() {

    addProtectedField("Position", TypeVector3, Offset(mPosition, SceneObject2D),
             &_setFieldPosition, &defaultProtectedGetFn,
             "2D position + layer. ");
    addProtectedField("Size", TypeVector2, Offset(mSize, SceneObject2D),
             &_setFieldSize, &defaultProtectedGetFn,
             "Render Size.");

    addField("color", TypeColor, Offset(mColor, SceneObject2D), "The color or tint of the object");
    addField("Visible", TypeBool, Offset(mVisible, SceneObject2D), "Is the object drawn");

    // position Fields require refresh after settled
    addField("x", TypeF32, Offset(mPosition.x,SceneObject2D));
    addField("y", TypeF32, Offset(mPosition.y,SceneObject2D));
    addField("z", TypeF32, Offset(mPosition.z,SceneObject2D));


    addField("velocity", TypeVector2, Offset(mVelo,SceneObject2D));
    addField("veloX", TypeF32, Offset(mVelo.x,SceneObject2D));
    addField("veloY", TypeF32, Offset(mVelo.y,SceneObject2D));

    Parent::initPersistFields();
}

//-----------------------------------------------------------------------------

bool SceneObject2D::onAdd() {
    if (!Parent::onAdd()) return false;

    gClientSceneContainer2D.registerObject(this);

    refreshWorldBox();
    return true;
}
//-----------------------------------------------------------------------------

void SceneObject2D::onRemove() {

    gClientSceneContainer2D.unregisterObject(this);
    Parent::onRemove();
}
//-----------------------------------------------------------------------------
void SceneObject2D::setPosition(const Vector3& pos) {
    F32 savLayer = this->mPosition.z;
    this->mPosition = pos;
    this->refreshWorldBox();

    if (mPosition.z != savLayer) gClientSceneContainer2D.sortObjects();
}
//-----------------------------------------------------------------------------
void SceneObject2D::setSize(const Vector2& size) {
    this->mSize = size;
    this->refreshWorldBox();
}
//-----------------------------------------------------------------------------
// TestScript for layer clamp/MAX_2D_LAYERS:
//  $foo = new SceneObject2D(){ Position = "10 10 0.123456";}; echo($foo.position);
//  $foo.position.z = 5; $foo.refresh(); echo($foo.position);
void SceneObject2D::refreshWorldBox() {

    mWorldBox.x = mPosition.x;
    mWorldBox.y = mPosition.y;
    mWorldBox.width = mSize.x;
    mWorldBox.height = mSize.y;

    // added for raw rectangle drawing ...
    mWorldPoints.topLeft = { mPosition.x, mPosition.y};
    mWorldPoints.topRight = { mPosition.x + mSize.x, mPosition.y};
    mWorldPoints.bottomLeft = { mPosition.x, mPosition.y + mSize.y};
    mWorldPoints.bottomRight = { mPosition.x + mSize.x, mPosition.y + mSize.y};



    // layer should be in 0.f .. 1.f
    F32 z = ElfMath::mClampF(mPosition.z, 0.f, 1.f);

    // MAX_2D_LAYERS
    z = ElfMath::mFloor( z * (F32)MAX_2D_LAYERS ) / MAX_2D_LAYERS;

    mPosition.z = z;

}


//-----------------------------------------------------------------------------
void SceneObject2D::moveLinear(F32 dt) {
    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;
    // this->mPosition.z += this->mVelo.z * dt;
}
//NOTE gravity, softening, maxSpeed: depends on common object speed
void SceneObject2D::moveGravity(F32 dt, Vector2 gravity) {
    this->mVelo.x += gravity.x * dt;
    this->mVelo.y += gravity.y * dt;
    // this->mVelo.z += gravity.z * dt;

    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;
    // this->mPosition.z += this->mVelo.z * dt;

}
void SceneObject2D::moveOrbital(F32 dt, Vector2 centerPoint, F32 gravity, F32 softening, F32 maxSpeed ) {

    Vector2 direction = centerPoint;
    direction.x -= this->mPosition.x; ;
    direction.y -= this->mPosition.y; ;


    F32 distance = Vector2Length(direction);

    // F32 G = 9.81f;

    if (distance > 0.0001f) {
        direction = Vector2Normalize(direction);
        F32 gravityPull = (gravity * 1000.f) / (distance * distance + softening);

        this->mVelo.x += direction.x * gravityPull * dt;
        this->mVelo.y += direction.y * gravityPull * dt;
    }

    // F32 drag = 0.995f;
    // this->mVelo.x *= drag;
    // this->mVelo.y *= drag;

    F32 currentSpeed = sqrt(this->mVelo.x * this->mVelo.x + this->mVelo.y * this->mVelo.y);
    if (currentSpeed > maxSpeed && currentSpeed > 0.0f) {
        this->mVelo.x = (this->mVelo.x / currentSpeed) * maxSpeed;
        this->mVelo.y = (this->mVelo.y / currentSpeed) * maxSpeed;
    }

    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;
}
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
bool SceneObject2D::castRay(Vector2 pos, F32 minLayer, F32 maxLayer) {
    if (!mVisible || mPosition.z > maxLayer || mPosition.z < minLayer) return false;
    return ::CheckCollisionPointRec(pos, mWorldBox);
}
//-----------------------------------------------------------------------------
bool SceneObject2D::rectCollide(Rectangle rect, F32 minLayer, F32 maxLayer) {
    if (!mVisible || mPosition.z > maxLayer || mPosition.z < minLayer) return false;
    return ::CheckCollisionRecs(rect, mWorldBox);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, setPosition,void, (F32 x, F32 y, F32 z), , "Set the position and refresh the worldbox") {
    object->setPosition({ x,y,z });
;
}
DefineEngineMethod(SceneObject2D, setSize,void, (F32 x, F32 y), ,
                   "Set the size and update the worldbox") {
    object->setSize({x,y});
}
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveLinear, void, (F32 dt), ,"Move Linear position/velocity"
    "\nRequre manual refresh!") {
    object->moveLinear(dt);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveGravity, void, (F32 dt, F32 gravityX, F32 gravityY/*, F32 gravityZ*/),
        (0.f, 9.81f/*, 0.f*/) ,
        "Move with gravity acceleration default: 0, 9.81"
        "\nRequre manual refresh!") {
    object->moveGravity(dt, {gravityX, gravityY});
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveOrbital2D, void, (F32 dt,
        Vector2 centerPoint, F32 gravity, F32 softening, F32 maxSpeed),
        (10.f, 150.f, 350.f) ,
        "2D Safe Orbital Movement"
        "\nRequre manual refresh!" ) {
    object->moveOrbital(dt, centerPoint, gravity, softening, maxSpeed);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, refresh, void, (), , "refresh the worldbox, needs to be called when position / size is changed") {
    object->refreshWorldBox();
}
}//namespace
