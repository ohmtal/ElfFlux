//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// FIXME forwardVector vs Velocty << use setter speed+ForwardVector => Velocty
// FIXME draw should not call linear movement
// TODO collisions - remove this stupid doBounce
//-----------------------------------------------------------------------------
#include "SceneObject2D.h"

#include "raylib.h"
#include "raymath.h"
#include "elfResource.h"
#include "ConsoleTypes.h"
#include "math/mMathFn.h"


namespace ElfObjects {


class SpriteObject : public SceneObject2D
{
    typedef SceneObject2D Parent;
public:
    DECLARE_CONOBJECT(SpriteObject);

    SpriteObject() {
         mCollisionType = CollisionType::Kinematic; // default Kinematic
    }

    S32 mTextureID = 0;
    Rectangle mTextureRect = {0.f,0.f,0.f,0.f};
    F32 mRotation = 0.f;
    Vector2 mRorationOrgin = {0.f, 0.f};
    // F32 mSpeed = 0.f;
    // Vector2 mForwardVector = {0.f, 0.f};
    bool mFlipX = false;
    bool mFlipY = false;

    // Velocty


    virtual void draw(const F32& dt) override;
    static void initPersistFields();

    // void doBounce(F32 x , F32 y, F32 width, F32 height);
    // static bool _setForwardVector( void *object, const char *index, const char *data );

    void setForwardVectorByRotation(F32 rotation, F32 speed = 1.f);
};
//-----------------------------------------------------------------------------
IMPLEMENT_CONOBJECT(SpriteObject);
//-----------------------------------------------------------------------------
void SpriteObject::draw(const F32& dt) {

    // if (mSpeed != 0.f) {
    //     F32 speed = mSpeed *  dt;
    //     mPosition.x += mForwardVector.x * speed;
    //     mPosition.y += mForwardVector.y * speed;
    //     refreshWorldBox();
    // }

    if (!mVisible) return;


    // RLAPI void DrawTexturePro(Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint);

    Texture* tex = ElfResource::TextureMap.get(mTextureID);
    if (!tex) return;

    Rectangle srcRect;
    if (mTextureRect.width <= 0.f || mTextureRect.height <= 0.f) {
        srcRect = {0.f, 0.f, (F32)tex->width, (F32)tex->height};
    } else {
        srcRect = mTextureRect;
    }


    if (mFlipX) srcRect.width = -srcRect.width;
    if (mFlipY) srcRect.height = -srcRect.height;

    ::DrawTexturePro(*tex, srcRect, mWorldBox, mRorationOrgin, mRotation, mColor);

    Parent::draw(dt);
}
//-----------------------------------------------------------------------------
void SpriteObject::setForwardVectorByRotation(F32 rotation, F32 speed) {
    F32 radians = rotation * M_DEG2RAD;
    // mForwardVector.x = ElfMath::mCos(radians);
    // mForwardVector.y = ElfMath::mSin(radians);

    mVelo.x = ElfMath::mCos(radians) * speed;
    mVelo.y = ElfMath::mSin(radians) * speed;
}
// //-----------------------------------------------------------------------------
// bool SpriteObject::_setForwardVector( void *object, const char *index, const char *data ){
//     SpriteObject* so = static_cast<SpriteObject*>( object );
//     if ( so )
//     {
//         Vector2 value( so->mForwardVector );
//         Con::setData( TypeVector2, &value, 0, 1, &data );
//         so->mForwardVector = Vector2Normalize( value );
//     }
//     return false;
// }
//-----------------------------------------------------------------------------
void SpriteObject::initPersistFields() {

    addField("TextureId", TypeS32, Offset(mTextureID, SpriteObject),"ID of the Texture");
    addField("TextureRect", TypeRectangle, Offset(mTextureRect, SpriteObject)
        , "source rect of the texture default 0.f,0.f,0.f,0.f using texture size.");
    addField("Rotation", TypeF32, Offset(mRotation, SpriteObject),"rotation in degree");
    addField("RotationOrgin", TypeF32, Offset(mRorationOrgin, SpriteObject),"point where the rotation orign is");
    // addField("Speed", TypeF32, Offset(mSpeed, SpriteObject),"motion speed");
    // addProtectedField("ForwardVector", TypeVector2, Offset(mForwardVector, SpriteObject)
    //     , &_setForwardVector,  &defaultProtectedGetFn
    //     ,"motion vector will be normaized, see also setForwardVectorByAngle and setRotationAndForwardVector");
    addField("FlipX", TypeBool, Offset(mFlipX, SpriteObject),"flip on xaxis");
    addField("FlipY", TypeBool, Offset(mFlipY, SpriteObject),"flip on yaxis");

    Parent::initPersistFields();
}
// -----------------------------------------------------------------------------
// void SpriteObject::doBounce(F32 x , F32 y, F32 width, F32 height) {
//     if (x < width) {
//         if (this->mPosition.x >= width) {
//             if (this->mForwardVector.x > 0.f)
//                 this->mForwardVector.x = -this->mForwardVector.x;
//         } else if (this->mPosition.x <= x) {
//             if (this->mForwardVector.x < 0.f)
//                 this->mForwardVector.x = -this->mForwardVector.x;
//         }
//     }
//     if (y < height) {
//         if (this->mPosition.y >= height) {
//             if (this->mForwardVector.y > 0.f)
//                 this->mForwardVector.y = -this->mForwardVector.y;
//         } else if (this->mPosition.y <= y) {
//             if (this->mForwardVector.y < 0.f)
//                 this->mForwardVector.y = -this->mForwardVector.y;
//         }
//     }
// }

// -----------------------------------------------------------------------------
DefineEngineMethod(SpriteObject, setForwardVector, void, (Vector2 vector, F32 speed)
,(1.f) ,"set the Velocty by a normalized forward vector ")
{
    vector = Vector2Normalize(vector) * speed;
    object->mVelo =vector;
}

// -----------------------------------------------------------------------------
DefineEngineMethod(SpriteObject, setForwardVectorByAngle, void, (F32 rot, F32 speed)
        ,(1.f) ,"set the forward vector by rot in degree")
{
    object->setForwardVectorByRotation(rot, speed);
}
// -----------------------------------------------------------------------------
DefineEngineMethod(SpriteObject, setRotationAndForwardVector, void, (F32 rot)
, ,"update the rotation (degree) and set the forward vector")
{
    object->mRotation = rot;
    object->setForwardVectorByRotation(rot);
}
// // -----------------------------------------------------------------------------
// DefineEngineMethod(SpriteObject, doBounce, void, (F32 x , F32 y, F32 width, F32 height)
//     , ,"usefull for bouncing on borders")
// {
//     object->doBounce(x,y,width,height);
// }
// // -----------------------------------------------------------------------------

} //namespace
