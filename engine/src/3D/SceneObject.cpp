//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// Inspired by TGE's SceneObject / Container System
//-----------------------------------------------------------------------------
#include "SceneObject.h"
#include "SceneContainer.h"

#include "raylib.h"
#include "raymath.h"

//-----------------------------------------------------------------------------
namespace ElfObjects {

IMPLEMENT_CONOBJECT(SceneObject);

//-----------------------------------------------------------------------------
SceneObject::SceneObject() {
}
//-----------------------------------------------------------------------------
void SceneObject::initPersistFields() {
    addField("Position", TypeVector3, Offset(mPosition, SceneObject));
    addField("Rotation", TypeVector3, Offset(mRotation, SceneObject));
    addField("Scale", TypeVector3, Offset(mScale, SceneObject));
    addField("Visible", TypeBool, Offset(mVisible, SceneObject));
    Parent::initPersistFields();
}
//-----------------------------------------------------------------------------

bool SceneObject::onAdd() {
    if (!Parent::onAdd()) return false;

    gClientSceneContainer.registerObject(this);

    refreshWorldBox();
    return true;
}
//-----------------------------------------------------------------------------

void SceneObject::onRemove() {

    gClientSceneContainer.unregisterObject(this);
    Parent::onRemove();
}
//-----------------------------------------------------------------------------

void SceneObject::refreshWorldBox() {
    mWorldBox.min = Vector3{ mPosition.x - 0.5f, mPosition.y - 0.5f, mPosition.z - 0.5f };
    mWorldBox.max = Vector3{ mPosition.x + 0.5f, mPosition.y + 0.5f, mPosition.z + 0.5f };
}
//-----------------------------------------------------------------------------

Matrix SceneObject::getWorldTransform() const {
    Matrix matScale = MatrixScale(mScale.x, mScale.y, mScale.z);
    Matrix matRotX = MatrixRotateX(mRotation.x * DEG2RAD);
    Matrix matRotY = MatrixRotateY(mRotation.y * DEG2RAD);
    Matrix matRotZ = MatrixRotateZ(mRotation.z * DEG2RAD);
    Matrix matTrans = MatrixTranslate(mPosition.x, mPosition.y, mPosition.z);

    Matrix matRot = MatrixMultiply(MatrixMultiply(matRotX, matRotY), matRotZ);
    return MatrixMultiply(MatrixMultiply(matScale, matRot), matTrans);
}
//-----------------------------------------------------------------------------
void SceneObject::draw() {
    lock();
    for (SimSet::iterator itr = begin(); itr != end(); ++itr) {
        SimObject* obj = *itr;
        if (!obj) continue;
        SceneObject* child = dynamic_cast<SceneObject*>(obj);
        if (child) {
            child->drawTransformed(this->getWorldTransform());
        }
    }
    unlock();
}
//-----------------------------------------------------------------------------
void SceneObject::drawTransformed(const Matrix& parentTransform) {
    lock();
    for (SimSet::iterator itr = begin(); itr != end(); ++itr) {
        SimObject* obj = *itr;
        if (!obj) continue;

        SceneObject* child = dynamic_cast<SceneObject*>(obj);
        if (child) {
            Matrix globalTransform = MatrixMultiply(child->getWorldTransform(), parentTransform);
            child->drawTransformed(globalTransform);
        }
    }
    unlock();
}
//-----------------------------------------------------------------------------
void SceneObject::updateWorldBox(const BoundingBox& localBox, const Matrix& worldTransform) {

    Vector3 localMin = { localBox.min.x * mScale.x, localBox.min.y * mScale.y, localBox.min.z * mScale.z };
    Vector3 localMax = { localBox.max.x * mScale.x, localBox.max.y * mScale.y, localBox.max.z * mScale.z };

    mWorldBox.min = ::Vector3Transform(localMin, worldTransform);
    mWorldBox.max = ::Vector3Transform(localMax, worldTransform);
}

//-----------------------------------------------------------------------------
RayCollision SceneObject::castRay(Ray ray) {
    return ::GetRayCollisionBox(ray, mWorldBox);
}

//-----------------------------------------------------------------------------

}//namespace
