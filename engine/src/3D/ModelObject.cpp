//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------

#include "console/engineAPI.h"
#include "interface/ConsoleTypes.h"
#include "raylib.h"
#include "raymath.h"
#include "math/mMathRand.h"
#include "interface/elfResource.h"
#include "SceneObject.h"

namespace ElfObjects {


class ModelObject : public SceneObject
{
    typedef SceneObject Parent;
public:
    DECLARE_CONOBJECT(ModelObject);

    S32 mModelId = 0;

    static void initPersistFields() {
        addField("ModelId", TypeS32, Offset(mModelId, ModelObject), "The script ID of the raylib model.");
        Parent::initPersistFields();
    }

    bool onAdd() override {
        if (!Parent::onAdd()) return false;
        refreshWorldBox();
        return true;
    }

    void draw() override;
    void drawTransformed(const Matrix& parentTransform) override;
    void refreshWorldBox() override;
};
//-----------------------------------------------------------------------------
IMPLEMENT_CONOBJECT(ModelObject);
//-----------------------------------------------------------------------------
void ModelObject::refreshWorldBox() {
    if (mModelId <= 0) {
        Parent::refreshWorldBox();
        return;
    }

    Model* baseModel = ElfResource::ModelMap.get(mModelId);
    if (!baseModel || baseModel->meshCount <= 0) {
        Parent::refreshWorldBox();
        return;
    }

    BoundingBox localBox = ::GetModelBoundingBox(*baseModel);
    updateWorldBox(localBox, this->getWorldTransform());

}
//-----------------------------------------------------------------------------
void ModelObject::draw() {
    if (mModelId <= 0) {
        Parent::draw();
        return;
    }

    Model* baseModel = ElfResource::ModelMap.get(mModelId);
    if (!baseModel || baseModel->meshCount <= 0) return;

    Matrix oldTransform = baseModel->transform;
    baseModel->transform = this->getWorldTransform();

    ::DrawModel(*baseModel, Vector3{0.0f, 0.0f, 0.0f}, 1.0f, WHITE);
    baseModel->transform = oldTransform;

    Parent::draw();
}
//-----------------------------------------------------------------------------
void ModelObject::drawTransformed(const Matrix& parentTransform) {
    if (mModelId <= 0) {
        Parent::drawTransformed(parentTransform);
        return;
    }

    Model* baseModel = ElfResource::ModelMap.get(mModelId);
    if (!baseModel || baseModel->meshCount <= 0) return;

    Matrix oldTransform = baseModel->transform;

    Matrix globalTransform = MatrixMultiply(this->getWorldTransform(), parentTransform);
    baseModel->transform = globalTransform;

    ::DrawModel(*baseModel, Vector3{0.0f, 0.0f, 0.0f}, 1.0f, WHITE);
    baseModel->transform = oldTransform;

    Parent::drawTransformed(globalTransform);
}
//-----------------------------------------------------------------------------

DefineEngineMethod(ModelObject, draw, void, (), , "Draws this object instance and all its children polymorphically.") {
    object->draw();
}



// class ModelObject : public SceneObject
// {
//     typedef SceneObject Parent;
// public:
//     DECLARE_CONOBJECT(ModelObject);
//
//     S32 mModelId = 0;
//
//     static void initPersistFields() {
//         addField("ModelId", TypeS32, Offset(mModelId, ModelObject), "The script ID of the raylib model.");
//         Parent::initPersistFields();
//     }
//
//     void draw(bool recursive = false);
// };
//
// IMPLEMENT_CONOBJECT(ModelObject);
// //-----------------------------------------------------------------------------
// void ModelObject::draw(bool recursive) {
//     if (mModelId <= 0) return;
//
//     Model* baseModel = ElfResource::ModelMap.get(mModelId);
//     if (!baseModel || baseModel->meshCount <= 0) return;
//
//     Matrix oldTransform = baseModel->transform;
//
//     Matrix matScale = MatrixScale(mScale.x, mScale.y, mScale.z);
//     Matrix matRotX = MatrixRotateX(mRotation.x * DEG2RAD);
//     Matrix matRotY = MatrixRotateY(mRotation.y * DEG2RAD);
//     Matrix matRotZ = MatrixRotateZ(mRotation.z * DEG2RAD);
//     Matrix matTrans = MatrixTranslate(mPosition.x, mPosition.y, mPosition.z);
//
//     Matrix matRot = MatrixMultiply(MatrixMultiply(matRotX, matRotY), matRotZ);
//     baseModel->transform = MatrixMultiply(MatrixMultiply(matScale, matRot), matTrans);
//
//     ::DrawModel(*baseModel, Vector3{0.0f, 0.0f, 0.0f}, 1.0f, WHITE);
//
//     baseModel->transform = oldTransform;
//
//     // WARNING may end in endless loop.
    // if (recursive) {
    //     lock();
    //     SimSet::iterator itr;
    //     for(itr = begin(); itr != end(); itr++)
    //     {
    //         SimObject *obj = *itr;
    //         if (obj == NULL) continue;
    //         ModelObject* child = dynamic_cast<ModelObject *>(obj);
    //         if (child != nullptr) {
    //             child->draw(true);
    //         }
    //     }
    //     unlock();
    // }
//
// }
// //-----------------------------------------------------------------------------
// //-----------------------------------------------------------------------------
//
// DefineEngineMethod(ModelObject, draw, void, (bool recursive),(false) ,
//     "Draws this object instance using its assigned model."
//     "@param recursive draw it's childs and their childs if any "
// ) {
//     object->draw(recursive);
// }

} //namespace
