//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
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

    ModelAnimation* getCurrentAnimation();
    S32 getAnimationCount();
    F32 getCurrentAnimFrame() { return mCurrentAnimFrame; }
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
S32 ModelObject::getAnimationCount() {
    if (mAnimationBlockId > 0) {
         ElfResource::ElfAnimationBlock* block = ElfResource::ModelAnimationMap.get(mAnimationBlockId);
         if (!block || block->anims == nullptr) return 0;
         return block->count;
    }
    return 0;
}
//-----------------------------------------------------------------------------
ModelAnimation* ModelObject::getCurrentAnimation() {
    if (mAnimationBlockId > 0 && mAnimationFPS > 0 && mAnimationIndex >= 0) {
        ElfResource::ElfAnimationBlock* block = ElfResource::ModelAnimationMap.get(mAnimationBlockId);

        if (block && block->anims != nullptr && mAnimationIndex < block->count) {
            return &block->anims[mAnimationIndex];
        }
    }
    return nullptr;

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
    ModelAnimation* currentAnim = getCurrentAnimation();
    if (currentAnim != nullptr) {

        ::UpdateModelAnimation(*baseModel, *currentAnim, (S32)mCurrentAnimFrame);
        mCurrentAnimFrame += ::GetFrameTime() * (float)mAnimationFPS;

        if (mCurrentAnimFrame >= (float)currentAnim->keyframeCount) {
            mCurrentAnimFrame = 0.0f;

            if (mAnimationPlayOnce) {
                // restore previous animation
                mAnimationIndex = mFallbackAnimationIndex;
                mAnimationFPS = mFallbackAnimationFPS;
                mAnimationPlayOnce = false;
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


    if (mountParent && mMountBoneIndex >= 0) {
        bool mountTransFormHandled = false;
        Model* parentModel = ElfResource::ModelMap.get(mountParent->mModelId);

        if (parentModel && mMountBoneIndex < parentModel->skeleton.boneCount) {
            ModelAnimation* curAnim = mountParent->getCurrentAnimation();
            if (curAnim) {
                S32 curFrame = (S32)mountParent->getCurrentAnimFrame();
                Transform *transform = &curAnim->keyframePoses[curFrame][mMountBoneIndex];
                Quaternion inRotation = parentModel->skeleton.bindPose[mMountBoneIndex].rotation;
                Quaternion outRotation = transform->rotation;
                // Calculate socket rotation (angle between bone in initial pose and same bone in current animation frame)
                Quaternion rotate = QuaternionMultiply(outRotation, QuaternionInvert(inRotation));
                Matrix matrixTransform = QuaternionToMatrix(rotate);
                // Translate socket to its position in the current animation
                matrixTransform = MatrixMultiply(matrixTransform, MatrixTranslate(transform->translation.x, transform->translation.y, transform->translation.z));
                // Transform the socket using the transform of the character (angle and translate)
                // but also respect the own model values:
                Matrix boneAndParent = MatrixMultiply(matrixTransform, parentTransform);
                globalTransform = MatrixMultiply(this->getWorldTransform(), boneAndParent);

                mountTransFormHandled = true;

            }
        }


        if (!mountTransFormHandled) {
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

DefineEngineMethod(ModelObject, getAnimationCount, S32, (), , "Get the count of the current animations") {
    return object->getAnimationCount();
}

DefineEngineMethod(ModelObject, getAnimationName, String, (), , "Get the name of the current animations") {
    ModelAnimation* anim = object->getCurrentAnimation();
    if (anim) {
        return anim->name;
    }
    return "";
}


DefineEngineMethod(ModelObject, playOnce, bool, (S32 animationIndex, S32 animationFPS, bool force), (25,false)
    , "Play a animation once. The current animation will be restored after finished."
      "Force also start the animation if a runOnce is already running.") {
    return object->playAnimationOnce(animationIndex, animationFPS, force);
}



} //namespace
