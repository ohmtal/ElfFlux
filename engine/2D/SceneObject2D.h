//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// 2D Object
// - position Z is used for Layer
// - FIXME childs of set are not rendered as in SceneObject(3D)
//-----------------------------------------------------------------------------
#pragma once
//-----------------------------------------------------------------------------
#define MAX_2D_LAYERS 100
//-----------------------------------------------------------------------------

#include "console/engineAPI.h"
#include "console/simSet.h"
#include "core/util/tVector.h"

#include "ConsoleTypes.h"

namespace ElfObjects {

struct RectPoints {
    Vector2 topLeft = { 0, 0 };
    Vector2 topRight = { 0, 0 };
    Vector2 bottomLeft = { 0, 0 };
    Vector2 bottomRight = { 0, 0 };
};

struct CollisionInfo2D;

enum class CollisionType {
    None,
    Trigger,
    Kinematic,  // stop / slide
    Bounce,     // ball physics / see also mRestitution and mFriction
    Static
};


class SceneObject2D : public /*SimSet*/ SimObject {
    typedef SimObject Parent;
public:
    DECLARE_CONOBJECT(SceneObject2D);

    Vector3 mPosition = { 0.0f, 0.0f, 0.0f }; // Z is used for layer
    Vector3 mSavPostion = mPosition; // used for BeginMove / EndMove
    Vector2 mVelo = { 0.0f, 0.0f }; // Velocity 2D
    Vector2 mSize = { 32.0f, 32.0f };

    F32 mRestitution = 0.6f; // bouncyness: 0.0 = like a stone ; 1.0 = like a flummy ball
    F32 mFriction    = 0.1f; // 0.0 = like one ice; 1.0 = with handbreak on
    F32 mCollisionDamping = 1.0f; // lower is less more is more Restitution;)
    F32 mMass = 1.f; //mass of the object

    Color mColor = RAYWHITE;
    bool mVisible = true;

    Rectangle mWorldBox = {0};
    Vector2 mWorldBoxHalfSize = {0};
    RectPoints mWorldPoints;

    SceneObject2D();
    ~SceneObject2D() = default;

    bool onAdd() override;
    void onRemove() override;

    void setPosition(const Vector3& pos);
    void setLayer(const F32 z);
    void setSize(const Vector2& size);

    virtual void onPositionChanged(); //this should be called when the position change
    virtual void updateWorldBox();
    // Matrix getWorldTransform() const;

    virtual void draw(const F32& dt) {}

    // used for select or clicked, findObjects
    virtual bool castRayLayers(Vector2 pos, F32 minLayer = 0.f, F32 maxLayer = 1.f);
    virtual bool rectCollideLayers(Rectangle rect, F32 minLayer = 0.f, F32 maxLayer = 1.f);

    static void initPersistFields();


    void moveLinear(F32 dt, bool doRefresh = false);
    void moveGravity(F32 dt, Vector2 gravity, bool doRefresh = false);
    void moveOrbital(F32 dt, Vector2 centerPoint, F32 gravity, F32 softening = 10.f, F32 maxSpeed = 350.f, bool applyMass = true, bool doRefresh = false);

    // Impulse
    void applyRadialImpulse(const Vector2& center, F32 strength, F32 maxDistance);
    void applyLinearImpulse(Vector2 direction, F32 strength);

    // Collision ---------------- abstract ------------------
    CollisionType mCollisionType = CollisionType::None;

    U32 mTypeMask = BIT(1); // bit of my type mask
    U32 mCollisionMask  = 0xFFFFFFFF; // for check what to collide with

    // called when a object other did collide with this ...
    // this is passive
    virtual void onCollision(const  CollisionInfo2D& info) {};

    virtual void solveCollision(const  CollisionInfo2D& info);


private:
    static bool _setFieldPosition( void *object, const char *index, const char *data );
    static bool _setFieldSize( void *object, const char *index, const char *data );

};

} //namespace
