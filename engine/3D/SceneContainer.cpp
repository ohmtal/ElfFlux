//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// Inspired by TGE's SceneObject / Container System
// NOTE: I use a flat list of Objects (mObjects) at the moment. For many many
//       objects it should be implemented as a Spatial Grid. But the you have
//       the moving to other cells overhead.
//-----------------------------------------------------------------------------
// FIXME:
//  - add deleteAllObjects (like 2D)
//-----------------------------------------------------------------------------
#include "SceneContainer.h"
#include "SceneObject.h"

namespace ElfObjects {
// -----------------------------------------------------------------------------
SceneContainer gClientSceneContainer;
// -----------------------------------------------------------------------------
void SceneContainer::registerObject(SceneObject* obj) {
    mObjects.push_back(obj);
}

// -----------------------------------------------------------------------------
void SceneContainer::unregisterObject(SceneObject* obj) {
    for (U32 i = 0; i < mObjects.size(); i++) {
        if (mObjects[i] == obj) {
            mObjects.erase_fast(i);
            break;
        }
    }
}

// -----------------------------------------------------------------------------
SceneObject* SceneContainer::castRay(Ray ray, RayCollision& outCollision) {
    SceneObject* closestObj = nullptr;
    outCollision.hit = false;
    outCollision.distance = FLT_MAX;

    for (U32 i = 0; i < mObjects.size(); i++) {
        SceneObject* obj = mObjects[i];

        RayCollision col = obj->castRay(ray); // ::GetRayCollisionBox(ray, obj->mWorldBox);

        if (col.hit && col.distance < outCollision.distance) {
            outCollision = col;
            closestObj = obj;
        }
    }
    return closestObj;
}

// -----------------------------------------------------------------------------
void SceneContainer::findObjectsInBox(BoundingBox searchBox, Vector<SceneObject*>& outResults) {
    for (U32 i = 0; i < mObjects.size(); i++) {
        SceneObject* obj = mObjects[i];

        if (::CheckCollisionBoxes(searchBox, obj->mWorldBox)) {
            outResults.push_back(obj);
        }
    }
}
// -----------------------------------------------------------------------------
bool SceneContainer::boxEmpty(BoundingBox searchBox) {
    for (U32 i = 0; i < mObjects.size(); i++) {
        if (::CheckCollisionBoxes(searchBox, mObjects[i]->mWorldBox)) {
            return false;
        }
    }
    return true;
}
// -----------------------------------------------------------------------------
// return a SimSet or nullptr if nothing found
SimSet* SceneContainer::getBoxObjects(BoundingBox searchBox) {

    Vector<SceneObject*> queryResult;
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
void SceneContainer::drawObjects() {
    for (U32 i = 0; i < mObjects.size(); i++) {
        if (mObjects[i]->mVisible) mObjects[i]->draw();
    }
}
// -----------------------------------------------------------------------------
DefineEngineFunction(ClientContainerDrawObjects, void, (), , "Call draw on all objects") {
    gClientSceneContainer.drawObjects();
}
// -----------------------------------------------------------------------------
// ElfScript ==> %hitData = ContainerRayCast(%ray);
DefineEngineFunction( ClientContainerRayCast, const char*, (Ray ray), ,
                      "Casts a ray into the container and returns the closest hit SceneObject and collision info."
) {
    RayCollision collision;
    SceneObject* hitObj = gClientSceneContainer.castRay(ray, collision);

    if (!hitObj || !collision.hit) return "";

    char* ret = Con::getReturnBuffer(128);
    dSprintf(ret, 128, "%d %g %g %g %g %g %g %g",
             hitObj->getId(),
             collision.point.x,  collision.point.y,  collision.point.z,
             collision.normal.x, collision.normal.y, collision.normal.z,
             collision.distance);
    return ret;
}
// -----------------------------------------------------------------------------
// ElfScript ==> %isEmpty = ContainerBoxEmpty("-1 -1 -1", "1 1 1");
DefineEngineFunction( ClientContainerBoxEmpty, bool, (Vector3 minBounds, Vector3 maxBounds), ,
                      "Returns true if no SceneObject intersects the given bounding box."
) {
    BoundingBox searchBox;
    searchBox.min = minBounds;
    searchBox.max = maxBounds;

    return gClientSceneContainer.boxEmpty(searchBox);
}
// -----------------------------------------------------------------------------
// ElfScript ==> %simSetId = ContainerGetBoxObjects("-5 0 -5", "5 10 5");
DefineEngineFunction( ClientContainerGetBoxObjects, S32, (Vector3 minBounds, Vector3 maxBounds), ,
                      "Returns a SimSet ID containing all SceneObjects within the specified bounding box.\n"
                      "NOTE: You should delete the returned SimSet when done to avoid leaks!"
) {
    BoundingBox searchBox;
    searchBox.min = minBounds;
    searchBox.max = maxBounds;

    SimSet* resultSet = gClientSceneContainer.getBoxObjects(searchBox);
    return resultSet != nullptr ? resultSet->getId() : 0;
}

// -----------------------------------------------------------------------------
DefineEngineFunction( ClientContainerListObjects, void, (), ,
                      "For Debug ... use ContainerGetBoxObjects with a big box to get all objects in as a SimSet ")
{
   S32 size = gClientSceneContainer.getObjects().size();
   if (size == 0) {
       Con::printf(" ---------- No objects in Container. ----------");
       return ;
   }

   Con::printf(" ---------- Found %d objects in Container. ----------", size);
   Con::printf("%5s %20s %20s", "[ID]", "[NAME]", "[CLASS]");

   for (U32 i = 0; i < gClientSceneContainer.getObjects().size(); i++) {
        Con::printf("%5d %20s %20s",
            gClientSceneContainer.getObjects()[i]->getId()
            , gClientSceneContainer.getObjects()[i]->getName()
            , gClientSceneContainer.getObjects()[i]->getClassName()
         );
   }
   Con::printf(" ---------- ContainerListObjects finished. ----------", size);

}
} //namespace
