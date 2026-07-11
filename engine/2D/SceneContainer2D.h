//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// SceneObject2D / Container System
//
// Together with SceneObject2D it handle layers and can be used for drawing
//
//-----------------------------------------------------------------------------
//TEST SCRIPT:
/*
$foo = new SceneObject2D() { position = "10 10 0";};
$bar = new SceneObject2D() { position = "10 10 0.1";};
ClientContainer2DListObjects();
echo(ClientContainer2DRayCast("11 11"));
$foo.setPosition("10 10 0.2");   //shift foo's layer down
echo(ClientContainer2DRayCast("11 11"));
echo(ClientContainer2DBoxEmpty("1 1 0", "20 20 1")); // should be 0
echo(ClientContainer2DBoxEmpty("100 100 0", "200 200 1"));// should be 1
$boxObjects = ClientContainer2DGetBoxObjects( "1 1 0", "20 20 1");
$boxObjects.listObjects(); // should list $foo and $bar (id's)
$boxObjects.delete();
*/
//-----------------------------------------------------------------------------
#pragma once

#ifndef RESERVED_SCENE_OBJECTS
#define RESERVED_SCENE_OBJECTS 250
#endif

#include "console/engineAPI.h"
#include "console/simSet.h"
#include "core/util/tVector.h"

#include "ConsoleTypes.h"

namespace ElfObjects {

class SceneObject2D;

class SceneContainer2D {
private:
    Vector<SceneObject2D*> mObjects;
    bool mLocked = false;

public:
    SceneContainer2D() {
        mObjects.reserve(RESERVED_SCENE_OBJECTS);
    }
    void registerObject(SceneObject2D* obj);
    void unregisterObject(SceneObject2D* obj);

    SceneObject2D* castRay(Vector2 pos,   F32 minLayer = 0.f, F32 maxLayer = 1.f);


    void findObjectsInBox(BoundingBox searchBox, Vector<SceneObject2D*>& outResults, bool returnOnlyFirst = false);

    bool boxEmpty(BoundingBox searchBox);
    SimSet* getBoxObjects(BoundingBox searchBox);

    const Vector<SceneObject2D*> getObjects() { return  mObjects; }

    void drawObjects(); //for future use to implement shadows and reflection

    bool mDoSortObects = true;
    void sortObjects(); // resort objects for layer drawing

    void reserveMore(S32 count) {
        mObjects.reserve(count);
    }

    void deleteAllObjects() ;

private:
    static S32 QSORT_CALLBACK compare_ObjectLayer( const void* a, const void* b );
};

// global Container
extern SceneContainer2D gClientSceneContainer2D;

} // namespace
