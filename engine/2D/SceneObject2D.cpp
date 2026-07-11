//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#include "SceneObject2D.h"
#include "SceneContainer2D.h"

#include "raylib.h"
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
DefineEngineMethod(SceneObject2D, setSize,void, (F32 x, F32 y), , "Set the size and update the worldbox") {
    object->setSize({x,y});
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, refresh, void, (), , "refresh the worldbox, needs to be called when position / size is changed") {
    object->refreshWorldBox();
}
}//namespace
