//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
//NOTE: about 10% slower than raw BatchRender!
//-----------------------------------------------------------------------------
#include "SceneObject2D.h"

#include "raylib.h"
#include <rlgl.h>
// #include "math/mMathFn.h"

namespace ElfObjects {

enum  class Primitive2DType{
   RECTANGLE = 0
   , CIRCLE  // 1 NOTE using height as radius
   , ELLIPSE // 2
   , LINE    // 3
   , TEXT    // 4 NOTE using height as fontsize
   , RAW_RECTANGLE // 5 aded for maximum performance

} ;

class PrimitiveObject2D : public SceneObject2D
{
    typedef SceneObject2D Parent;
public:
    DECLARE_CONOBJECT(PrimitiveObject2D);

    S32 mType = (S32)Primitive2DType::RECTANGLE;
    bool mFilled = true;
    F32 mThick = 1.f; //used by different types
    StringTableEntry mCaption = StringTable->EmptyString(); //used for text

    virtual void draw() override;
    static void initPersistFields();

};
//-----------------------------------------------------------------------------
IMPLEMENT_CONOBJECT(PrimitiveObject2D);
//-----------------------------------------------------------------------------
void PrimitiveObject2D::draw() {

    if (!mVisible) return;

    switch ((Primitive2DType)mType) {
        case Primitive2DType::CIRCLE: {
            F32 halfW =  mWorldBox.width / 2.f;
            F32 halfH =  mWorldBox.height / 2.f;
            if (mFilled) {
                ::DrawCircle(
                    (S32) (mWorldBox.x + halfW)
                    , (S32) (mWorldBox.y + halfH)
                    , halfH
                    , mColor
                );
            } else {
                ::DrawCircleLines(
                    (S32) (mWorldBox.x + halfW)
                    , (S32) (mWorldBox.y + halfH)
                    , halfH
                    , mColor
                );
            }

            break;
        }
            break;
        case Primitive2DType::ELLIPSE: {
            F32 halfW =  mWorldBox.width / 2.f;
            F32 halfH =  mWorldBox.height / 2.f;
             if (mFilled) {
                ::DrawEllipse(
                    (S32) (mWorldBox.x + halfW)
                    , (S32) (mWorldBox.y + halfH)
                    , halfW
                    , halfH
                    , mColor
                );

            } else {
                ::DrawEllipseLines(
                    (S32) (mWorldBox.x + halfW)
                    , (S32) (mWorldBox.y + halfH)
                    , halfW
                    , halfH
                    , mColor
                );
            }
            break;
        }

        case Primitive2DType::LINE: {
            ::DrawLineEx(
                { mWorldBox.x, mWorldBox.y }
                , { (mWorldBox.x + mWorldBox.width), (mWorldBox.y + mWorldBox.height) }
                , mThick
                , mColor
            );
            break;
        }
        case Primitive2DType::TEXT: {
            const char* cap = mCaption;
            if (!cap) break;
            ::DrawText(
                cap
                , (S32) mWorldBox.x
                , (S32) mWorldBox.y
                , (S32) mWorldBox.height
                , mColor
            );
            break;
        }
        case Primitive2DType::RECTANGLE:
        {
            if (mFilled) {
                ::DrawRectangleRec(mWorldBox, mColor);
            }
            else ::DrawRectangleLinesEx(mWorldBox, mThick , mColor);
            break;
        }
        case Primitive2DType::RAW_RECTANGLE: {
            if (mFilled) {
                Texture2D texShapes = GetShapesTexture();
                rlSetTexture(texShapes.id);
                Rectangle shapeRect = GetShapesTextureRectangle();
                rlBegin(RL_QUADS);

                rlNormal3f(0.0f, 0.0f, 1.0f);
                rlColor4ub(mColor.r, mColor.g, mColor.b, mColor.a);

                rlTexCoord2f(shapeRect.x/texShapes.width, shapeRect.y/texShapes.height);
                rlVertex2f(mWorldPoints.topLeft.x, mWorldPoints.topLeft.y);

                rlTexCoord2f(shapeRect.x/texShapes.width, (shapeRect.y + shapeRect.height)/texShapes.height);
                rlVertex2f(mWorldPoints.bottomLeft.x, mWorldPoints.bottomLeft.y);

                rlTexCoord2f((shapeRect.x + shapeRect.width)/texShapes.width, (shapeRect.y + shapeRect.height)/texShapes.height);
                rlVertex2f(mWorldPoints.bottomRight.x, mWorldPoints.bottomRight.y);

                rlTexCoord2f((shapeRect.x + shapeRect.width)/texShapes.width, shapeRect.y/texShapes.height);
                rlVertex2f(mWorldPoints.topRight.x, mWorldPoints.topRight.y);

                rlEnd();

                // is slower ....:
                // rlBegin(RL_TRIANGLES);
                //
                // rlColor4ub(mColor.r, mColor.g, mColor.b, mColor.a);
                //
                // rlVertex2f(mWorldPoints.topLeft.x, mWorldPoints.topLeft.y);
                // rlVertex2f(mWorldPoints.bottomLeft.x, mWorldPoints.bottomLeft.y);
                // rlVertex2f(mWorldPoints.topRight.x, mWorldPoints.topRight.y);
                //
                // rlVertex2f(mWorldPoints.topRight.x, mWorldPoints.topRight.y);
                // rlVertex2f(mWorldPoints.bottomLeft.x, mWorldPoints.bottomLeft.y);
                // rlVertex2f(mWorldPoints.bottomRight.x, mWorldPoints.bottomRight.y);
                // rlEnd();

                // ::DrawRectangleRec(mWorldBox, mColor);
            }
            break;
        }
    }
    Parent::draw();
}
//-----------------------------------------------------------------------------
void PrimitiveObject2D::initPersistFields() {

    addField("filled", TypeBool, Offset(mFilled, PrimitiveObject2D),"Filled or outline");
    addField("thick", TypeF32, Offset(mThick, PrimitiveObject2D),"Thick used by lines and unfilled rectangle");
    addField("type", TypeS32, Offset(mType, PrimitiveObject2D),
                      "drawtype Rectangle = 0, Circle = 1, ELLIPSE = 2, LINE = 3 (Primitive2DType)");
    addField("caption", TypeString, Offset(mCaption, PrimitiveObject2D),"used at type text");

    Parent::initPersistFields();
}


} //namespace
