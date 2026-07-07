//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// Inspired by TGE's SceneObject / Container System
//-----------------------------------------------------------------------------
#pragma once

#include "console/engineAPI.h"
#include "console/simSet.h"
#include "core/util/tVector.h"

#include "ConsoleTypes.h"

namespace ElfObjects {

class SceneObject : public SimSet {
    typedef SimSet Parent;
public:
    DECLARE_CONOBJECT(SceneObject);

    Vector3 mPosition = { 0.0f, 0.0f, 0.0f };
    Vector3 mRotation = { 0.0f, 0.0f, 0.0f }; // In degree
    Vector3 mScale = { 1.0f, 1.0f, 1.0f };
    BoundingBox mWorldBox = { 0 };             // raylib AABB

    bool mVisible = true;


    SceneObject();
    ~SceneObject() = default;

    bool onAdd() override;
    void onRemove() override;

    virtual void refreshWorldBox();
    Matrix getWorldTransform() const;
    virtual void draw();
    // for childs which are mounted on parent object
    virtual void drawTransformed(const Matrix& parentTransform);

    virtual RayCollision castRay(Ray ray);

    static void initPersistFields();

    void addObject( SimObject* object ) override;

protected:
    SceneObject* mTransientParent = nullptr; // mount parent only used in rendering and reseted again
   void updateWorldBox(const BoundingBox& localBox, const Matrix& worldTransform);


};

} //namespace
