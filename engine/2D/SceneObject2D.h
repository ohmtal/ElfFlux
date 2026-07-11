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


class SceneObject2D : public /*SimSet*/ SimObject {
    typedef SimObject Parent;
public:
    DECLARE_CONOBJECT(SceneObject2D);

    Vector3 mPosition = { 0.0f, 0.0f, 0.0f }; // Z is used for layer
    Vector2 mSize = { 32.0f, 32.0f };
    Color mColor = RAYWHITE;
    bool mVisible = true;

    Rectangle mWorldBox = {0};
    RectPoints mWorldPoints;

    SceneObject2D();
    ~SceneObject2D() = default;

    bool onAdd() override;
    void onRemove() override;

    void setPosition(const Vector3& pos);
    void setSize(const Vector2& size);
    virtual void refreshWorldBox();
    // Matrix getWorldTransform() const;

    virtual void draw() {}

    virtual bool castRay(Vector2 pos, F32 minLayer = 0.f, F32 maxLayer = 1.f);
    virtual bool rectCollide(Rectangle rect, F32 minLayer = 0.f, F32 maxLayer = 1.f);

    static void initPersistFields();

    // void addObject( SimObject* object ) override;


private:
    static bool _setFieldPosition( void *object, const char *index, const char *data );
    static bool _setFieldSize( void *object, const char *index, const char *data );

};

} //namespace
