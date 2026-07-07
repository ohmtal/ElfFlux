//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// FIXME mount to bone matix still not right ... far away from mountpoint and
//       animation reversed!!!!!!.....
// check https://github.com/raysan5/raylib/blob/master/examples/models/models_bone_socket.c
// TODO on moving objects: refreshWorldBox when position changed!
//-----------------------------------------------------------------------------

#include "console/engineAPI.h"
#include "math/mMathRand.h"

#include "raylib.h"
#include "raymath.h"

#include "elfResource.h"
#include "ConsoleTypes.h"

#include "SceneObject.h"


namespace ElfObjects {


class ModelObject : public SceneObject
{
    typedef SceneObject Parent;
public:
    DECLARE_CONOBJECT(ModelObject);

    S32 mModelId = 0; // ElfResource Id


    S32 mMountBoneIndex = -1; //see also GetModelBoneIndexByName

    S32 mAnimationBlockId = 0; // see also ElfResource::ElfAnimationBlock
    S32 mAnimationIndex = 0; // index of the animation - i set it to default 0 NOT -1
    S32 mAnimationFPS = 25;

    static void initPersistFields() {
        addField("ModelId", TypeS32, Offset(mModelId, ModelObject), "The script ID of the raylib model.");

        addField("MountBoneIndex", TypeS32, Offset(mMountBoneIndex, ModelObject), "If this is a child it can be mounted to a Bone index. See also GetModelBoneIndexByName");

        addGroup("Animation");
        addField("AnimationBlockId", TypeS32, Offset(mAnimationBlockId, ModelObject), "Id of the AnimationBlock");
        addField("AnimationIndex", TypeS32, Offset(mAnimationIndex, ModelObject), "Index of the animation to play. See also GetModelAnimationCount.");

        addField("AnimationFPS", TypeS32, Offset(mAnimationFPS, ModelObject), "Frames per secound when an animation is played");
        endGroup("Animation");
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

    bool playAnimationOnce(S32 animIndex, S32 animFPS, bool force);
private:
    F32 mCurrentAnimFrame = 0.0f;

    S32 mFallbackAnimationIndex = -1; //index of default animation when playonce is used
    S32 mFallbackAnimationFPS = 25;
    bool mAnimationPlayOnce = false; //will fallback to mDefaultAnimationIndex

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
bool ModelObject::playAnimationOnce(S32 animIndex, S32 animFPS, bool force) {
    if (mAnimationPlayOnce && !force) return false;

    if (!mAnimationPlayOnce) {
        // save fallbacks from current only if we have no "playonce" running
        mFallbackAnimationIndex = mAnimationIndex;
        mFallbackAnimationFPS = mAnimationFPS;
    }

    mAnimationPlayOnce = true;
    mAnimationIndex = animIndex;
    mAnimationFPS = animFPS;
    mCurrentAnimFrame = 0.0f;
    return true;
}
//-----------------------------------------------------------------------------
void ModelObject::draw() {
    if (mModelId <= 0) {
        Parent::draw();
        return;
    }

    Model* baseModel = ElfResource::ModelMap.get(mModelId);
    if (!baseModel || baseModel->meshCount <= 0) return;

    // --- ANIMATIONS >>>>>>
    if (mAnimationBlockId > 0 && mAnimationFPS > 0 && mAnimationIndex >= 0) {
        ElfResource::ElfAnimationBlock* block = ElfResource::ModelAnimationMap.get(mAnimationBlockId);

        if (block && block->anims != nullptr && mAnimationIndex < (int)block->count) {
            ModelAnimation currentAnim = block->anims[mAnimationIndex];

            ::UpdateModelAnimation(*baseModel, currentAnim, (S32)mCurrentAnimFrame);

            mCurrentAnimFrame += ::GetFrameTime() * (float)mAnimationFPS;

            if (mCurrentAnimFrame >= (float)currentAnim.keyframeCount) {
                mCurrentAnimFrame = 0.0f;

                if (mAnimationPlayOnce) {
                    // restore previous animation
                    mAnimationIndex = mFallbackAnimationIndex;
                    mAnimationFPS = mFallbackAnimationFPS;
                    mAnimationPlayOnce = false;
                }
            }
        }
    }
    // <<<<<< ANIMATIONS ---



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
    Matrix globalTransform;

    ModelObject* mountParent = nullptr;
    if (mTransientParent) mountParent = dynamic_cast<ModelObject*>(mTransientParent);

    // FIXME check https://github.com/raysan5/raylib/blob/master/examples/models/models_bone_socket.c
    if (mountParent && mMountBoneIndex >= 0) {
        Model* parentModel = ElfResource::ModelMap.get(mountParent->mModelId);

        if (parentModel && mMountBoneIndex < parentModel->skeleton.boneCount) {
            Matrix boneTransform = parentModel->boneMatrices[mMountBoneIndex];
            //--- closest ... lol .. maybe the bone is wrong ?!
            // FIXME can i render the bones ???
            Matrix boneAndParent = MatrixMultiply(boneTransform, parentTransform);
            globalTransform = MatrixMultiply(this->getWorldTransform(), boneAndParent);

            // cant see this ...
            DrawCube(Vector3Transform((Vector3){ 0, 0, 0 }, boneTransform), 10.f, 10.1f, 10.1f, RED);

            //---
            // globalTransform = MatrixMultiply(this->getWorldTransform(), boneTransform);
            //---
            // globalTransform = boneTransform;
        // NOTE attempt 2:
        // if (parentModel && mMountBoneIndex < parentModel->skeleton.boneCount) {
        //     Matrix boneTransform = parentModel->boneMatrices[mMountBoneIndex];
        //
        //     Vector3 m0 = { boneTransform.m0, boneTransform.m1, boneTransform.m2 };
        //     Vector3 m1 = { boneTransform.m4, boneTransform.m5, boneTransform.m6 };
        //     Vector3 m2 = { boneTransform.m8, boneTransform.m9, boneTransform.m10 };
        //
        //     m0 = Vector3Normalize(m0);
        //     m1 = Vector3Normalize(m1);
        //     m2 = Vector3Normalize(m2);
        //
        //     boneTransform.m0 = m0.x; boneTransform.m1 = m0.y; boneTransform.m2 = m0.z;
        //     boneTransform.m4 = m1.x; boneTransform.m5 = m1.y; boneTransform.m6 = m1.z;
        //     boneTransform.m8 = m2.x; boneTransform.m9 = m2.y; boneTransform.m10 = m2.z;
        //     // --------------------------------------------------------
        //
        //     Matrix boneAndWeapon = MatrixMultiply(this->getWorldTransform(), boneTransform);
        //
        //     globalTransform = MatrixMultiply(boneAndWeapon, parentTransform);

        } else {
            globalTransform = MatrixMultiply(this->getWorldTransform(), parentTransform);
        }
    } else {
        globalTransform = MatrixMultiply(this->getWorldTransform(), parentTransform);
    }

    baseModel->transform = globalTransform;
    ::DrawModel(*baseModel, Vector3{0.0f, 0.0f, 0.0f}, 1.0f, WHITE);
    baseModel->transform = oldTransform;

    Parent::drawTransformed(globalTransform);
}

//-----------------------------------------------------------------------------

DefineEngineMethod(ModelObject, draw, void, (), , "Draws this object instance and all its children polymorphically.") {
    object->draw();
}

DefineEngineMethod(ModelObject, playOnce, bool, (S32 animationIndex, S32 animationFPS, bool force), (25,false)
    , "Play a animation once. The current animation will be restored after finished."
      "Force also start the animation if a runOnce is already running.") {
    return object->playAnimationOnce(animationIndex, animationFPS, force);
}



} //namespace
