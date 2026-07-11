//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// 2D container with layer sorted draw calls, rayCast, boxEmpty and GetBoxObjects
//-----------------------------------------------------------------------------
// NOTE: I use a flat list of Objects (mObjects) at the moment. For many many
//       objects it should be implemented as a Spatial Grid. But the you have
//       the moving to other cells overhead.
//-----------------------------------------------------------------------------
#include "SceneContainer2D.h"
#include "SceneObject2D.h"

namespace ElfObjects {
// -----------------------------------------------------------------------------
SceneContainer2D gClientSceneContainer2D;


S32 QSORT_CALLBACK SceneContainer2D::compare_ObjectLayer( const void* a, const void* b )
{
    const SceneObject2D * cp_a = *(const SceneObject2D**)a;
    const SceneObject2D * cp_b = *(const SceneObject2D**)b;

    if (cp_a->mPosition.z > cp_b->mPosition.z)
        return -1;
    else
        return 1;

}
// called on SceneObject2D::setPosition and SceneContainer2D::registerObject
void SceneContainer2D::sortObjects()
{
    if (!mDoSortObects) return;
    dQsort( mObjects.address(), mObjects.size(), sizeof(SceneObject2D *), SceneContainer2D::compare_ObjectLayer );
}
// -----------------------------------------------------------------------------
void SceneContainer2D::registerObject(SceneObject2D* obj) {
    mObjects.push_back(obj);
    sortObjects();
}

// -----------------------------------------------------------------------------
void SceneContainer2D::unregisterObject(SceneObject2D* obj) {
    for (U32 i = 0; i < mObjects.size(); i++) {
        if (mObjects[i] == obj) {
            mObjects.erase_fast(i);
            break;
        }
    }
}

// -----------------------------------------------------------------------------
SceneObject2D* SceneContainer2D::castRay(Vector2 pos,  F32 minLayer, F32 maxLayer) {
    // objects are sorted from highest layer to lowest so we
    // start at the end of the list
    // hit should be the "closest "
    // for (U32 i = 0; i < mObjects.size(); i++) {
    for (U32 i = mObjects.size() - 1;i >= 0;  i--) {
        SceneObject2D* obj = mObjects[i];
        if (obj->castRay(pos, minLayer,maxLayer)) {
            return obj;

        }
    }
    return nullptr;
}

// -----------------------------------------------------------------------------
void SceneContainer2D::findObjectsInBox(BoundingBox searchBox, Vector<SceneObject2D*>& outResults, bool returnOnlyFirst) {

    Rectangle rect;
    rect.x = searchBox.min.x;
    rect.y = searchBox.min.y;
    rect.width = searchBox.max.x - searchBox.min.x;
    rect.height = searchBox.max.y - searchBox.min.y;

    if (rect.width <= 0.f || rect.height <= 0.f ) {
        Con::errorf("findObjectsInBox: invalid searchBox min is heigher then max!");
        return;
    }

    F32 minLayer = ElfMath::mClamp(searchBox.min.z, 0.f, 1.f);
    F32 maxLayer = ElfMath::mClamp(searchBox.max.z, 0.f, 1.f);
    if (minLayer > maxLayer) {
        F32 dummy = minLayer;
        minLayer = maxLayer;
        maxLayer = dummy;
    }



    for (U32 i = 0; i < mObjects.size(); i++) {
        SceneObject2D* obj = mObjects[i];

        if ( obj->rectCollide(rect, minLayer, maxLayer)) {
            outResults.push_back(obj);
            if (returnOnlyFirst) return;
        }
    }
}
// -----------------------------------------------------------------------------
bool SceneContainer2D::boxEmpty(BoundingBox searchBox) {
    Vector<SceneObject2D*> queryResult;
    findObjectsInBox(searchBox, queryResult, true);
    return queryResult.size() == 0;
}
// -----------------------------------------------------------------------------
// return a SimSet or nullptr if nothing found
SimSet* SceneContainer2D::getBoxObjects(BoundingBox searchBox) {

    Vector<SceneObject2D*> queryResult;
    findObjectsInBox(searchBox, queryResult);

    if (queryResult.size() == 0) return nullptr;

    SimSet* resultSet = new SimSet();

    resultSet->registerObject();

    for (U32 i = 0; i < queryResult.size(); i++) {
        resultSet->addObject(queryResult[i]);
    }

    return resultSet;
}
// -----------------------------------------------------------------------------
void SceneContainer2D::drawObjects() {
    for (U32 i = 0; i < mObjects.size(); i++) {
        if (mObjects[i]->mVisible) mObjects[i]->draw();
    }
}
// -----------------------------------------------------------------------------
DefineEngineFunction(ClientContainer2DDrawObjects, void, (), , "Call draw on all objects") {
    gClientSceneContainer2D.drawObjects();
}
// -----------------------------------------------------------------------------
// ElfScript ==> %hitData = ContainerRayCast(%ray);
DefineEngineFunction( ClientContainer2DRayCast, S32,
                      (Vector2 pos, F32 minLayer, F32 maxLayer), ( 0.f, 1.f),
                      "Casts a ray into the 2D client container and returns the closest hit SceneObject2D id"
                      " closest == lowest layer"
) {
    SceneObject2D* hitObj = gClientSceneContainer2D.castRay(pos,  minLayer, maxLayer);

    if (!hitObj) return 0;

    return hitObj->getId();

}
// -----------------------------------------------------------------------------
// ElfScript ==> %isEmpty = ContainerBoxEmpty("-1 -1 -1", "1 1 1");
DefineEngineFunction( ClientContainer2DBoxEmpty, bool, (Vector3 minBounds, Vector3 maxBounds), ,
                      "Returns true if no SceneObject2D intersects the given bounding box."
) {
    BoundingBox searchBox;
    searchBox.min = minBounds;
    searchBox.max = maxBounds;

    return gClientSceneContainer2D.boxEmpty(searchBox);
}
// -----------------------------------------------------------------------------
// ElfScript ==> %simSetId = ContainerGetBoxObjects("-5 0 -5", "5 10 5");
DefineEngineFunction( ClientContainer2DGetBoxObjects, S32, (Vector3 minBounds, Vector3 maxBounds), ,
                      "Returns a SimSet ID containing all SceneObject2Ds within the specified bounding box.\n"
                      "NOTE: You should delete the returned SimSet when done to avoid leaks!"
) {
    BoundingBox searchBox;
    searchBox.min = minBounds;
    searchBox.max = maxBounds;

    SimSet* resultSet = gClientSceneContainer2D.getBoxObjects(searchBox);
    return resultSet != nullptr ? resultSet->getId() : 0;
}
// -----------------------------------------------------------------------------
DefineEngineFunction( ClientContainer2DReserve, void, (S32 count), ,
                      "reserve objects ... "
) {
    if (count < RESERVED_SCENE_OBJECTS) return; //only if it's more than default
    gClientSceneContainer2D.reserveMore(count);
}
// -----------------------------------------------------------------------------
DefineEngineFunction( ClientContainer2DSetSort, void, (bool doSort), ,
                      "Enable / disable sorting - when adding lots of objects disable it until done"
) {

    bool curValue = gClientSceneContainer2D.mDoSortObects;

    if (doSort == curValue) return;
    if (doSort) {
        gClientSceneContainer2D.mDoSortObects = true;
        gClientSceneContainer2D.sortObjects();
    } else {
        gClientSceneContainer2D.mDoSortObects = false;
    }
}
// -----------------------------------------------------------------------------
DefineEngineFunction( ClientContainer2DListObjects, void, (), ,
                      "For Debug ... use ContainerGetBoxObjects with a big box to get all objects in as a SimSet ")
{
   S32 size = gClientSceneContainer2D.getObjects().size();
   if (size == 0) {
       Con::printf(" ---------- No objects in Container. ----------");
       return ;
   }

   Con::printf(" ---------- Found %d objects in Container. ----------", size);
   Con::printf("%5s %20s %20s %4s", "[ID]", "[NAME]", "[CLASS]", "[Z]");

   for (U32 i = 0; i < gClientSceneContainer2D.getObjects().size(); i++) {
        Con::printf("%5d %20s %20s %.2f",
            gClientSceneContainer2D.getObjects()[i]->getId()
            , gClientSceneContainer2D.getObjects()[i]->getName()
            , gClientSceneContainer2D.getObjects()[i]->getClassName()
            , gClientSceneContainer2D.getObjects()[i]->mPosition.z
         );
   }
   Con::printf(" ---------- ContainerListObjects finished. ----------", size);

}
} //namespace
