//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// Inspired by TGE's SceneObject / Container System
//-----------------------------------------------------------------------------
#pragma once

#include "interface/ConsoleTypes.h"

#include "console/engineAPI.h"
#include "console/simSet.h"
#include "core/util/tVector.h"

namespace ElfObjects {

class SceneObject;

class SceneContainer {
private:
    Vector<SceneObject*> mObjects;

public:
    void registerObject(SceneObject* obj);
    void unregisterObject(SceneObject* obj);
    SceneObject* castRay(Ray ray, RayCollision& outCollision);
    void findObjectsInBox(BoundingBox searchBox, Vector<SceneObject*>& outResults);

    bool boxEmpty(BoundingBox searchBox);
    SimSet* getBoxObjects(BoundingBox searchBox);

    const Vector<SceneObject*> getObjects() { return  mObjects; }

    void drawObjects(); //for future use to implement shadows
};

// global Container
extern SceneContainer gClientSceneContainer;

} // namespace
