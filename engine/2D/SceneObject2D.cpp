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

#define MIN_MASS 0.0001f
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

    addField("collisionMask", TypeS32, Offset(mCollisionMask,SceneObject2D), "Collsion filter mask");
    addField("typeMask", TypeS32, Offset(mTypeMask,SceneObject2D), "Collision Type mask of this object");
    addField("collisionType", TypeS32, Offset(mCollisionType,SceneObject2D), "See also enum CollisionType_Kinematic, CollisionType_Bounce and other constants with  CollisionType_");

    addField("color", TypeColor, Offset(mColor, SceneObject2D), "The color or tint of the object");
    addField("visible", TypeBool, Offset(mVisible, SceneObject2D), "Is the object drawn");

    // position Fields require refresh after settled
    addField("x", TypeF32, Offset(mPosition.x,SceneObject2D));
    addField("y", TypeF32, Offset(mPosition.y,SceneObject2D));
    // use setZ or .position  addField("z", TypeF32, Offset(mPosition.z,SceneObject2D));


    addField("velocity", TypeVector2, Offset(mVelo,SceneObject2D));
    addField("veloX", TypeF32, Offset(mVelo.x,SceneObject2D));
    addField("veloY", TypeF32, Offset(mVelo.y,SceneObject2D));

    addField("mass", TypeF32, Offset(mMass,SceneObject2D), "Mass of the object");
    addField("restitution", TypeF32, Offset(mRestitution,SceneObject2D), "bouncyness: 0.0 = like a stone ; 1.0 = like a flummy ball");
    addField("friction", TypeF32, Offset(mFriction,SceneObject2D), "0.0 = like one ice; 1.0 = with handbreak on");
    addField("damping", TypeF32, Offset(mCollisionDamping,SceneObject2D), "restitution < 1= lower bounce > 1 = heigher bounce");

    Parent::initPersistFields();
}

//-----------------------------------------------------------------------------

bool SceneObject2D::onAdd() {
    if (!Parent::onAdd()) return false;

    gClientSceneContainer2D.registerObject(this);

    updateWorldBox();
    return true;
}
//-----------------------------------------------------------------------------

void SceneObject2D::onRemove() {

    gClientSceneContainer2D.unregisterObject(this);
    Parent::onRemove();
}
//-----------------------------------------------------------------------------
void SceneObject2D::setLayer(const F32 z) {
    if (z == this->mPosition.z) return;

    // layer should be in 0.f .. 1.f
    F32 finalZ = ElfMath::mClampF(z, 0.f, 1.f);

    // MAX_2D_LAYERS
    finalZ = ElfMath::mFloor( finalZ * (F32)MAX_2D_LAYERS ) / MAX_2D_LAYERS;

    mPosition.z = finalZ;
}
//-----------------------------------------------------------------------------
void SceneObject2D::setPosition(const Vector3& pos) {
    if (pos == this->mPosition) return;
    this->mPosition.x = pos.x;
    this->mPosition.y = pos.y;
    setLayer(pos.z);
    this->onPositionChanged();
}
//-----------------------------------------------------------------------------
void SceneObject2D::setSize(const Vector2& size) {
    this->mSize = size;
    this->updateWorldBox();
}
//-----------------------------------------------------------------------------
void SceneObject2D::solveCollision(const CollisionInfo2D& info) {
    if ( mCollisionType == CollisionType::None || mCollisionType == CollisionType::Static) {
        return; //should be never come here it's checked before
    }
    if (mCollisionType == CollisionType::Trigger) {
        //FIXME onTriggerEnter ... / also leave must be handled than
        return;
    }

    // --- move out of other object
    if (info.mOther->mCollisionType != CollisionType::Trigger && info.mOther->mCollisionType != CollisionType::None) {
        this->mPosition.x += info.mNormal.x * info.mOverlap;
        this->mPosition.y += info.mNormal.y * info.mOverlap;
        this->updateWorldBox();
    }

    // --- change speed
    if (mCollisionType == CollisionType::Kinematic) {
        // stop
        if (info.mNormal.x != 0.0f) this->mVelo.x = 0.0f;
        if (info.mNormal.y != 0.0f) this->mVelo.y = 0.0f;
    }
    //  with mass and oponent bounce
    else if (mCollisionType == CollisionType::Bounce) {
        F32 relVelX = this->mVelo.x - info.mOther->mVelo.x;
        F32 relVelY = this->mVelo.y - info.mOther->mVelo.y;

        F32 normalVel = relVelX * info.mNormal.x + relVelY * info.mNormal.y;

        if (normalVel < 0.0f) {

            F32 combinedRestitution = this->mRestitution * info.mOther->mRestitution;
            combinedRestitution *= info.mOther->mCollisionDamping;
            if (combinedRestitution < 0.0f) combinedRestitution = 0.0f;

            F32 massA = this->mMass;
            F32 massB = (info.mOther->mCollisionType == CollisionType::Static) ? 9999999.0f : info.mOther->mMass;

            F32 impulseScalar = -(1.0f + combinedRestitution) * normalVel;
            impulseScalar /= (1.0f / massA + 1.0f / massB);

            this->mVelo.x += info.mNormal.x * (impulseScalar / massA);
            this->mVelo.y += info.mNormal.y * (impulseScalar / massA);

            Vector2 tangent = { -info.mNormal.y, info.mNormal.x };
            F32 tangentVel = relVelX * tangent.x + relVelY * tangent.y;
            F32 combinedFriction = this->mFriction * info.mOther->mFriction;

            F32 frictionScalar = -tangentVel * combinedFriction;
            frictionScalar /= (1.0f / massA + 1.0f / massB);

            this->mVelo.x += tangent.x * (frictionScalar / massA);
            this->mVelo.y += tangent.y * (frictionScalar / massA);

            // push the "other" ... should i - or not
            if (info.mOther->mCollisionType != CollisionType::Static) {
                info.mOther->mVelo.x -= info.mNormal.x * (impulseScalar / massB);
                info.mOther->mVelo.y -= info.mNormal.y * (impulseScalar / massB);

                info.mOther->mVelo.x -= tangent.x * (frictionScalar / massB);
                info.mOther->mVelo.y -= tangent.y * (frictionScalar / massB);
            }

        }
    }

    // else if (mCollisionType == CollisionType::Bounce) {
    //     // bounce
    //
    //     F32 normalVel = this->mVelo.x * info.mNormal.x + this->mVelo.y * info.mNormal.y;
    //
    //     if (normalVel < 0.0f) {
    //         F32 combinedRestitution = this->mRestitution * info.mOther->mRestitution;
    //         combinedRestitution *= info.mOther->mCollisionDamping;
    //
    //         // Impulse:
    //         F32 impulseStrength = -(1.0f + combinedRestitution) * normalVel;
    //
    //         this->mVelo.x += info.mNormal.x * impulseStrength;
    //         this->mVelo.y += info.mNormal.y * impulseStrength;
    //
    //         // Friction
    //         Vector2 tangent = { -info.mNormal.y, info.mNormal.x };
    //         F32 tangentVel = this->mVelo.x * tangent.x + this->mVelo.y * tangent.y;
    //
    //         // combine
    //         F32 combinedFriction = this->mFriction * info.mOther->mFriction;
    //
    //         // reduce speed
    //         this->mVelo.x -= tangent.x * tangentVel * combinedFriction;
    //         this->mVelo.y -= tangent.y * tangentVel * combinedFriction;
    //     }
    // }
}


//-----------------------------------------------------------------------------
// NOTE: collision check here !
void SceneObject2D::onPositionChanged() {
    this->updateWorldBox();

    if ( mCollisionType != CollisionType::Static && mCollisionType != CollisionType::None) {
        Vector<CollisionInfo2D> collsionInfos;
        if (gClientSceneContainer2D.CheckCollide(this, collsionInfos)) {
            // sort by biggest mOverlap
            dQsort( collsionInfos.address(), collsionInfos.size(), sizeof(CollisionInfo2D),
                    SceneContainer2D::compare_CollisionOverlap );

            // Con::warnf("--------- We did collide:");
            for (const auto& info : collsionInfos) {
                if (!::CheckCollisionRecs(this->mWorldBox, info.mOther->mWorldBox)) {
                    continue;
                }
                // debug
                // Con::printf("ID: %d, normal:%.4f, %.4f mCollisionAxis: %.4f, %.4f, mOverlap: %.4f",
                //             info.mOther->getId()
                //             , info.mNormal.x, info.mNormal.y
                //             , info.mCollisionAxis.x, info.mCollisionAxis.y
                //             , info.mOverlap
                // );

                this->solveCollision(info);
            }
        }
    }
}
//-----------------------------------------------------------------------------
// TestScript for layer clamp/MAX_2D_LAYERS:
//  $foo = new SceneObject2D(){ Position = "10 10 0.123456";}; echo($foo.position);
//  $foo.position.z = 5; $foo.refresh(); echo($foo.position);
// NOTE it's designed for future use. moveing object in Container

// TODO move rotation here
// TODO shape type ..(circle)
void SceneObject2D::updateWorldBox() {

    mWorldBox.x = mPosition.x;
    mWorldBox.y = mPosition.y;
    mWorldBox.width = mSize.x;
    mWorldBox.height = mSize.y;


    mWorldBoxHalfSize.x = mSize.x / 2.f;
    mWorldBoxHalfSize.y = mSize.y / 2.f;

    // added for raw rectangle drawing ...
    mWorldPoints.topLeft = { mPosition.x, mPosition.y};
    mWorldPoints.topRight = { mPosition.x + mSize.x, mPosition.y};
    mWorldPoints.bottomLeft = { mPosition.x, mPosition.y + mSize.y};
    mWorldPoints.bottomRight = { mPosition.x + mSize.x, mPosition.y + mSize.y};

    // allow negativ mass
    if (mMass == 0.f) mMass = MIN_MASS;




}


//-----------------------------------------------------------------------------
void SceneObject2D::moveLinear(F32 dt, bool doRefresh) {
    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;
    // this->mPosition.z += this->mVelo.z * dt;

    if (doRefresh) onPositionChanged();
}

//-----------------------------------------------------------------------------
//NOTE gravity, softening, maxSpeed: depends on common object speed
void SceneObject2D::moveGravity(F32 dt, Vector2 gravity, bool doRefresh) {
    this->mVelo.x += (gravity.x / this->mMass) * dt;
    this->mVelo.y += (gravity.y / this->mMass) * dt;
    // this->mVelo.z += (gravity.z / this->mMass) * dt;

    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;
    // this->mPosition.z += this->mVelo.z * dt;

    if (doRefresh) onPositionChanged();
}
//-----------------------------------------------------------------------------
void SceneObject2D::moveOrbital(F32 dt, Vector2 centerPoint, F32 gravity
    , F32 softening, F32 maxSpeed, bool applyMass, bool doRefresh) {
    Vector2 direction;
    direction.x = centerPoint.x - this->mPosition.x;
    direction.y = centerPoint.y - this->mPosition.y;

    F32 distance = ElfMath::mSqrt(direction.x * direction.x + direction.y * direction.y);

    if (distance > 0.0001f) {
        F32 invDistance = 1.0f / distance;
        direction.x *= invDistance;
        direction.y *= invDistance;

        F32 gravityPull = (gravity * 1000.f) / (distance * distance + softening);

        if (applyMass /*&& this->mMass > 0.0001f*/) {
            gravityPull *= this->mMass;
        }

        this->mVelo.x += direction.x * gravityPull * dt;
        this->mVelo.y += direction.y * gravityPull * dt;
    }

    F32 currentSpeed = ElfMath::mSqrt(this->mVelo.x * this->mVelo.x + this->mVelo.y * this->mVelo.y);
    if (currentSpeed > maxSpeed && currentSpeed > 0.0f) {
        F32 invSpeed = maxSpeed / currentSpeed;
        this->mVelo.x *= invSpeed;
        this->mVelo.y *= invSpeed;
    }

    this->mPosition.x += this->mVelo.x * dt;
    this->mPosition.y += this->mVelo.y * dt;

   if (doRefresh) onPositionChanged();
}

//-----------------------------------------------------------------------------
void SceneObject2D::applyRadialImpulse(const Vector2& center, F32 strength, F32 maxDistance) {
    F32 dirX = this->mPosition.x - center.x;
    F32 dirY = this->mPosition.y - center.y;

    F32 distance = ElfMath::mSqrt(dirX * dirX + dirY * dirY);

    // allow negativ mass ..
    if (distance < 0.0001f || ( maxDistance != 0.f &&  distance > maxDistance) /*|| this->mMass <= 0.0001f*/) {
        return;
    }

    F32 normX = dirX / distance;
    F32 normY = dirY / distance;

    F32 deltaV = strength / this->mMass;

    this->mVelo.x += normX * deltaV;
    this->mVelo.y += normY * deltaV;
}
//-----------------------------------------------------------------------------
void SceneObject2D::applyLinearImpulse(Vector2 direction, F32 strength) {
    // if (this->mMass <= 0.0001f) {
    //     return;
    // }

    F32 len = ElfMath::mSqrt(direction.x * direction.x + direction.y * direction.y);
    if (len < 0.0001f) {
        return;
    }

    F32 normX = direction.x / len;
    F32 normY = direction.y / len;

    F32 deltaV = strength / this->mMass;

    this->mVelo.x += normX * deltaV;
    this->mVelo.y += normY * deltaV;
}

//-----------------------------------------------------------------------------
// script interface:
//-----------------------------------------------------------------------------
bool SceneObject2D::castRayLayers(Vector2 pos, F32 minLayer, F32 maxLayer) {
    if (!mVisible || mPosition.z > maxLayer || mPosition.z < minLayer) return false;
    return ::CheckCollisionPointRec(pos, mWorldBox);
}
//-----------------------------------------------------------------------------
bool SceneObject2D::rectCollideLayers(Rectangle rect, F32 minLayer, F32 maxLayer) {
    if (!mVisible || mPosition.z > maxLayer || mPosition.z < minLayer) return false;
    return ::CheckCollisionRecs(rect, mWorldBox);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, setPosition,void, (F32 x, F32 y, F32 z), , "Set the position and refresh the worldbox") {
    object->setPosition({ x,y,z });
;
}

DefineEngineMethod(SceneObject2D, setZ,void, (F32 z), , "het the layer") {
    object->setLayer(z );
}
DefineEngineMethod(SceneObject2D, getZ,F32, (), , "get the layer") {
    return object->mPosition.z;
}
DefineEngineMethod(SceneObject2D, setSize,void, (F32 x, F32 y), ,
                   "Set the size and update the worldbox") {
    object->setSize({x,y});
}
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveLinear, void, (F32 dt, bool doRefresh),(false) ,"Move Linear position/velocity"
    "\nRequire manual refresh!") {
    object->moveLinear(dt, doRefresh);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveGravity, void,
        (F32 dt, F32 gravityX, F32 gravityY/*, F32 gravityZ*/, bool doRefresh),
        (0.f, 9.81f/*, 0.f*/, false) ,
        "Move with gravity acceleration default: 0, 9.81"
        "\nRequire manual refresh!") {
    object->moveGravity(dt, {gravityX, gravityY}, doRefresh);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, moveOrbital2D, void, (F32 dt,
        Vector2 centerPoint, F32 gravity, F32 softening, F32 maxSpeed, bool applyMass, bool doRefresh),
        (10.f, 150.f, 350.f, true, false) ,
        "2D Safe Orbital Movement"
        "\nRequire manual refresh!" ) {
    object->moveOrbital(dt, centerPoint, gravity, softening, maxSpeed, applyMass, doRefresh);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, applyRadialImpulse, void, (F32 centerX, F32 centerY, F32 strength, F32 maxDistance),
(0.f), "Apply a radial impulse, maxDistance == 0 means no distance check "
        ) {
    object->applyRadialImpulse( {centerX, centerY}, strength, maxDistance);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, applyLinearImpulse, void, (F32 dirX, F32 dirY, F32 strength),
                   , "Apply a radial impulse"
) {
    object->applyLinearImpulse({dirX, dirY}, strength);
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, GetWorldBox, Rectangle, (), ,
                   "save the current position"){
    return object->mWorldBox;
}
//-----------------------------------------------------------------------------
DefineEngineMethod(SceneObject2D, beginMove, void, (), ,
                   "save the current position"){
    object->mSavPostion = object->mPosition;
    object->onPositionChanged();
}
DefineEngineMethod(SceneObject2D, endMove, void, (), ,
                   "save the current position"){
    if (object->mSavPostion != object->mPosition)  object->onPositionChanged();
}
}//namespace
