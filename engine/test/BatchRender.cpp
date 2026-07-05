// #include "elfObjects.h"
#include "console/engineAPI.h"
#include "console/consoleInternal.h"
#include "math/mMathFn.h"
#include "ConsoleTypes.h"

// -------------------------------------------------------------------------------------------
// BatchRender
// this is more a speed test playground than a usable object
// -------------------------------------------------------------------------------------------
namespace ElfRender {


// TODO use a map of StringTableEntry pointer instead building the strings!:
// Dictionary::Entry *getVariableEntry(const char *name)
// {
//     name = prependDollar(name);
//     return gGlobalVars.lookup(StringTable->insert(name));
// }
// ------
// Dictionary::Entry *entry = getVariableEntry(varName);
// return entry ? entry->getFloatValue() : def;
//
// unfortually with the modular BatchPushVec2 it's ait tricky
// or maybe not BatchPushVec2(%i,1, "$b0Z", "$b0W");
// sequence is $b0X, $b0Y, $b0Z, $b0W where 0  is the Vector4 stack (0..3)

//TODO just start porting to Pointers
const String globalVarNames[16] = {
    "$b0X", "$b0Y", "$b0Z", "$b0W"
    ,"$b1X", "$b1Y", "$b1Z", "$b1W"
    ,"$b2X", "$b2Y", "$b2Z", "$b2W"
    ,"$b3X", "$b3Y", "$b3Z", "$b3W"
};
static Dictionary::Entry *entry[16] = {
    nullptr,nullptr,nullptr,nullptr,
    nullptr,nullptr,nullptr,nullptr,
    nullptr,nullptr,nullptr,nullptr,
    nullptr,nullptr,nullptr,nullptr
};
// ..............
class BatchRender : public SimObject {
    typedef SimObject Parent;

public:
    // Flat memory layout holding Raylib Matrix structures
    Vector<Matrix> mElements;

    //WARNING for testing fields only! should be set a functions
    Matrix mCurElement;


    DECLARE_CONOBJECT(BatchRender);

    // Safely resize the internal Torque vector allocation
    void setBufferSize(S32 size) {
        // NOTE: Torque uses setSize() instead of resize() for its Vector

        mElements.setSize(size);
        dMemset(mElements.address(), 0, mElements.size() * sizeof(Matrix));
    }

    static bool _setBufferSize(void* obj,const char* , const char* data) {
        BatchRender* object = static_cast<BatchRender*>(obj);
        if (!object || !data) return false;
        S32 size = dAtoi(data);
        if ( size >= 0 ) {
          object->setBufferSize(size);
          //NOT! return true;
        }
        return false;
    }

    S32 getCount() { return mElements.size();}

    static const char *_getBufferSize(void* obj, const char* data) {
        BatchRender* object = static_cast<BatchRender*>(obj);
        if (!object) return "";
        return Con::getIntArg(object->mElements.size());
    }


    static void initPersistFields() {
        addProtectedField("count", TypeS32, 0, &_setBufferSize,&_getBufferSize, "Set the buffersize (element count).");
        addField("x", TypeF32, Offset(mCurElement.m0, BatchRender), "curent x on the matrix"),
        addField("y", TypeF32, Offset(mCurElement.m1, BatchRender), "curent y on the matrix"),
        addField("z", TypeF32, Offset(mCurElement.m2, BatchRender), "curent z on the matrix"),
        addField("w", TypeF32, Offset(mCurElement.m3, BatchRender), "curent w on the matrix"),
        Parent::initPersistFields();
    }

    // BATCH DRAW 1: Render circles using rectangle coordinates
    void drawCircles(S32 count, Color color) {
        S32 safeCount = getMin(count, mElements.size());
        for (S32 i = 0; i < safeCount; ++i) {
            DrawCircle(mElements[i].m0, mElements[i].m1, mElements[i].m2, color);
        }
    }

    // BATCH DRAW 2: Render solid rectangles directly
    void drawRects(S32 count, Color color) {
        S32 safeCount = getMin(count, mElements.size());
        for (S32 i = 0; i < safeCount; ++i) {
            DrawRectangle( mElements[i].m0, mElements[i].m1, mElements[i].m2, mElements[i].m3, color);
        }
    }

    /**
     * draw all rects
     * fetch color from stack id:3
     */
    void drawColoredRects() {
        S32 count = mElements.size();
        Color color = RAYWHITE;
        for (S32 i = 0; i < count; ++i) {
            color = { (U8) mElements[i].m12 ,(U8) mElements[i].m13,
                      (U8) mElements[i].m14, (U8) mElements[i].m15 };
            DrawRectangle( mElements[i].m0, mElements[i].m1, mElements[i].m2, mElements[i].m3,color);
        }
    }

    void dumpRects() {
        S32 safeCount  = mElements.size();
        for (S32 i = 0; i < safeCount; ++i) {
            Con::printf("#%d %f %f %f %f", i, mElements[i].m0, mElements[i].m1, mElements[i].m2, mElements[i].m3);
        }
    }
    // BATCH DRAW 3: Render lines interpreting components as start/end vectors
    void drawLines(S32 count, Color color) {
        S32 safeCount = getMin(count, mElements.size());
        for (S32 i = 0; i < safeCount; ++i) {
            DrawLine(mElements[i].m0, mElements[i].m1, mElements[i].m2, mElements[i].m3, color);

        }
    }

    /**
     * draw all lines
     * fetch color from stack id:3
     */
    void drawColoredLines() {
        S32 count = mElements.size();
        Color color = RAYWHITE;
        for (S32 i = 0; i < count; ++i) {
            color = { (U8) mElements[i].m12 ,(U8) mElements[i].m13,
                (U8) mElements[i].m14, (U8) mElements[i].m15 };
                DrawLine(mElements[i].m0, mElements[i].m1, mElements[i].m2, mElements[i].m3,color);
        }
    }
};

IMPLEMENT_CONOBJECT(BatchRender);

DefineEngineMethod(BatchRender, push,void, (S32 index), , "push the index to current") {
    if (index < 0 || index > object->mElements.size()) return;
    object->mElements[index] = object->mCurElement;
}
DefineEngineMethod(BatchRender, pull,void, (S32 index), , "push the index to current") {
    if (index < 0 || index > object->mElements.size()) return;
    object->mCurElement = object->mElements[index];
}

// --- Console Method Bindings ---

DefineEngineMethod(BatchRender, setBufferSize, void, (S32 size),, "Resizes the internal buffers safely") {
    object->setBufferSize(size);
}


// FIXME USE STACK HERE too!!
DefineEngineMethod(BatchRender, setVector2, void, (S32 index, F32 x, F32 y),
                   , "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m0 = x;
        object->mElements[index].m1 = y;
    }
}
DefineEngineMethod(BatchRender, setVector2v, void, (S32 index, Vector2 vec),
                   , "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m0 = vec.x;
        object->mElements[index].m1 = vec.y;
    }
}


DefineEngineMethod(BatchRender, setVector3, void, (S32 index, F32 x, F32 y, F32 z),
                   , "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m0 = x;
        object->mElements[index].m1 = y;
        object->mElements[index].m2 = z;
    }
}

DefineEngineMethod(BatchRender, setVector4, void, (S32 index, F32 x, F32 y, F32 w, F32 h),
                   , "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m0 = x;
        object->mElements[index].m1 = y;
        object->mElements[index].m2 = w;
        object->mElements[index].m3 = h;
    }
}

DefineEngineMethod(BatchRender, setVector4_1, void, (S32 index
,  F32 m4, F32 m5, F32 m6, F32 m7),
, "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m4 = m4;
        object->mElements[index].m5 = m5;
        object->mElements[index].m6 = m6;
        object->mElements[index].m7 = m7;
    }
}
DefineEngineMethod(BatchRender, setVector4_2, void, (S32 index
    , F32 m8, F32 m9, F32 m10, F32 m11),
    , "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m8 = m8;
        object->mElements[index].m9 = m9;
        object->mElements[index].m10 = m10;
        object->mElements[index].m11 = m11;
    }
}
DefineEngineMethod(BatchRender, setVector4_3, void, (S32 index
, F32 m12, F32 m13, F32 m14, F32 m15),
, "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m12 = m12;
        object->mElements[index].m13 = m13;
        object->mElements[index].m14 = m14;
        object->mElements[index].m15 = m15;
    }
}


DefineEngineMethod(BatchRender, setVector8, void, (S32 index
, F32 m0, F32 m1, F32 m2, F32 m3, F32 m4, F32 m5, F32 m6, F32 m7),
, "Sets data at a specific index") {
    if (index >= 0 && index < object->mElements.size()) {
        object->mElements[index].m0 = m0;
        object->mElements[index].m1 = m1;
        object->mElements[index].m2 = m2;
        object->mElements[index].m3 = m3;
        object->mElements[index].m4 = m4;
        object->mElements[index].m5 = m5;
        object->mElements[index].m6 = m6;
        object->mElements[index].m7 = m7;
    }
}

DefineEngineMethod(BatchRender, getX, F32, (S32 index),, "Get x position by index") {
    return object->mElements[index].m0;
}

DefineEngineMethod(BatchRender, getY, F32, (S32 index),, "Get y position by index") {
    return object->mElements[index].m1;
}

// Purposefully mapping width to Z-depth for script-side math optimization
DefineEngineMethod(BatchRender, getZ, F32, (S32 index),, "Get z (width) by index") {
    return object->mElements[index].m2;
}

DefineEngineMethod(BatchRender, getW, F32, (S32 index),, "Get width by index") {
    return object->mElements[index].m2;
}

DefineEngineMethod(BatchRender, getH, F32, (S32 index),, "Get height by index") {
    return object->mElements[index].m3;
}

DefineEngineMethod(BatchRender, getVector2, Vector3, (S32 index),, "Get full Vector2 structure by index") {
    return { object->mElements[index].m0, object->mElements[index].m1 };
}

DefineEngineMethod(BatchRender, getVector3, Vector3, (S32 index),, "Get full Vector3 structure by index") {
    return { object->mElements[index].m0, object->mElements[index].m1, object->mElements[index].m2 };
}

DefineEngineMethod(BatchRender, getVector4, Vector4, (S32 index),, "Get Vector4 structure by index") {
   return { object->mElements[index].m0, object->mElements[index].m1, object->mElements[index].m2 , object->mElements[index].m3};
}

DefineEngineMethod(BatchRender, getVector4_1, Vector4, (S32 index),, "Get second Vector4 structure by index") {
    return { object->mElements[index].m4, object->mElements[index].m5, object->mElements[index].m6 , object->mElements[index].m7};
}
DefineEngineMethod(BatchRender, getVector4_2, Vector4, (S32 index),, "Get third Vector4 structure by index") {
    return { object->mElements[index].m8, object->mElements[index].m9, object->mElements[index].m10 , object->mElements[index].m11};
}
DefineEngineMethod(BatchRender, getVector4_3, Vector4, (S32 index),, "Get fourth Vector4 structure by index") {
    return { object->mElements[index].m12, object->mElements[index].m13, object->mElements[index].m14 , object->mElements[index].m15};
}

DefineEngineMethod(BatchRender, getMatrix, Matrix, (S32 index),, "Get the Matrix structure by index") {
    return object->mElements[index];
}


DefineEngineMethod(BatchRender, drawCircles, void, (S32 count, Color color),, "Batch draw circles using first Vector3 x,y,radius") {
    object->drawCircles(count, color);
}

DefineEngineMethod(BatchRender, drawRects, void, (S32 count, Color color),, "Batch draw rectangles using first Vector4 x,y,width,height") {
    object->drawRects(count, color);
}

DefineEngineMethod(BatchRender, drawLines, void, (S32 count, Color color),, "Batch draw lines using first Vector4 x1,y1,x2,y2") {
    object->drawLines(count, color);
}

// -------------------------------------------------------------------------------------

// FIXME look at global batch !!! with stack .. but wait until i did the pointer redesign!
// set named global variables for x, y, z
DefineEngineMethod(BatchRender, getVector2GlobalV, void,
                   (S32 index, String varX, String varY),
                   , "Fetches X, Yto global variables named by parameters") {
    if (index >= 0 && index < object->mElements.size()) {
        if (varX) Con::setFloatVariable(varX, object->mElements[index].m0);
        if (varY) Con::setFloatVariable(varY, object->mElements[index].m1);
    }
}

// this ? not DefineEngineMethod(BatchRender, feedVector2GlobalV, void,


// FIXME look at global batch !!! with stack .. but wait until i did the pointer redesign!
DefineEngineMethod(BatchRender, setVector2GlobalV, void,
                   (S32 index, String varX, String varY),
                   , "Fetches X, Y FROM global variables named by parameters") {
    if (index >= 0 && index < object->mElements.size()) {
        if (varX) object->mElements[index].m0 = Con::getFloatVariable(varX);
        if (varY) object->mElements[index].m1 = Con::getFloatVariable(varY);
    }
}

DefineEngineMethod(BatchRender, getVector3GlobalV, void,
                   (S32 index, String varX, String varY, String varW),
                   , "Fetches X, Y, Z to global variables named by parameters") {
    if (index >= 0 && index < object->mElements.size()) {
        if (varX) Con::setFloatVariable(varX, object->mElements[index].m0);
        if (varY) Con::setFloatVariable(varY, object->mElements[index].m1);
        if (varW) Con::setFloatVariable(varW, object->mElements[index].m2);
    }
}
// -------------------------- Vecor2 Stacks ----------------------------------------
// 8 Stacks from 0..7 x/y
static bool _assignVector2ToGlobal(BatchRender* object, S32 stack, S32 index
        , String varX, String varY)
{
    if (!object) return false;
    if (index >= 0 && index < object->mElements.size()) {

        if (stack == 0) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m0);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m1);
        } else if (stack == 2) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m2);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m3);
        } else if (stack == 3) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m4);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m5);
        } else if (stack == 4) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m6);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m7);
        } else  if (stack == 5) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m8);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m9);
        } else if (stack == 6) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m10);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m11);
        } else  if (stack == 7) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m12);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m13);
        } else if (stack == 8) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m14);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m15);
        } else {
            Con::errorf("getVector2ToGlobal Invalid stack id! (%d)", stack);
            return false;
        }
    } else {
        Con::errorf("getVector2ToGlobal invalid index!! %d max:%d", index,object->mElements.size() );
        return false;
    }
    return true;
}

static bool _assignVector2FromGlobal(BatchRender* object, S32 stack, S32 index
        , String varX, String varY)
{
    if (!object) return false;
    if (index >= 0 && index < object->mElements.size()) {
        if (stack == 0) {
            if (varX) object->mElements[index].m0 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m1 = Con::getFloatVariable(varY);
        } else if (stack == 1) {
            if (varX) object->mElements[index].m2 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m3 = Con::getFloatVariable(varY);
        } else if (stack == 2) {
            if (varX) object->mElements[index].m4 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m5 = Con::getFloatVariable(varY);
        } else if (stack == 3) {
            if (varX) object->mElements[index].m6 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m7 = Con::getFloatVariable(varY);
        } else if (stack == 4) {
            if (varX) object->mElements[index].m8 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m9 = Con::getFloatVariable(varY);
        } else if (stack == 5) {
            if (varX) object->mElements[index].m10= Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m11= Con::getFloatVariable(varY);
        } else if (stack == 6) {
            if (varX) object->mElements[index].m12= Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m13= Con::getFloatVariable(varY);
        } else if (stack == 7) {
            if (varX) object->mElements[index].m14= Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m15= Con::getFloatVariable(varY);
        } else {
            Con::errorf("getVector2ToGlobal Invalid stack id! (%d)", stack);
            return false;
        }
    } else {
        Con::errorf("getVector2ToGlobal invalid index!! %d max:%d", index,object->mElements.size() );
        return false;
    }
    return true;
}
// -------------------------- Vecor4 Stacks ----------------------------------------
static bool _assignVector4ToGlobal(BatchRender* object, S32 stack, S32 index
    , String varX, String varY, String varW, String varH)
{
    if (!object) return false;
    if (index >= 0 && index < object->mElements.size()) {

        if (stack == 0) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m0);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m1);
            if (varW) Con::setFloatVariable(varW, object->mElements[index].m2);
            if (varH) Con::setFloatVariable(varH, object->mElements[index].m3);
        } else if (stack == 1) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m4);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m5);
            if (varW) Con::setFloatVariable(varW, object->mElements[index].m6);
            if (varH) Con::setFloatVariable(varH, object->mElements[index].m7);
        } else  if (stack == 2) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m8);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m9);
            if (varW) Con::setFloatVariable(varW, object->mElements[index].m10);
            if (varH) Con::setFloatVariable(varH, object->mElements[index].m11);
        } else  if (stack == 3) {
            if (varX) Con::setFloatVariable(varX, object->mElements[index].m12);
            if (varY) Con::setFloatVariable(varY, object->mElements[index].m13);
            if (varW) Con::setFloatVariable(varW, object->mElements[index].m14);
            if (varH) Con::setFloatVariable(varH, object->mElements[index].m15);
        } else {
            Con::errorf("getVector4ToGlobal Invalid stack id! (%d)", stack);
            return false;
        }
    } else {
        Con::errorf("getVector4ToGlobal invalid index!! %d max:%d", index,object->mElements.size() );
        return false;
    }
    return true;
}

static bool _assignVector4FromGlobal(BatchRender* object, S32 stack, S32 index
, String varX, String varY, String varW, String varH)
{
     if (!object) return false;
    if (index >= 0 && index < object->mElements.size()) {
        if (stack == 0) {
            if (varX) object->mElements[index].m0 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m1 = Con::getFloatVariable(varY);
            if (varW) object->mElements[index].m2 = Con::getFloatVariable(varW);
            if (varH) object->mElements[index].m3 = Con::getFloatVariable(varH);

        } else if (stack == 1) {
            if (varX) object->mElements[index].m4 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m5 = Con::getFloatVariable(varY);
            if (varW) object->mElements[index].m6 = Con::getFloatVariable(varW);
            if (varH) object->mElements[index].m7 = Con::getFloatVariable(varH);
        } else  if (stack == 2) {
            if (varX) object->mElements[index].m8 = Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m9 = Con::getFloatVariable(varY);
            if (varW) object->mElements[index].m10= Con::getFloatVariable(varW);
            if (varH) object->mElements[index].m11= Con::getFloatVariable(varH);

        } else  if (stack == 3) {
            if (varX) object->mElements[index].m12= Con::getFloatVariable(varX);
            if (varY) object->mElements[index].m13= Con::getFloatVariable(varY);
            if (varW) object->mElements[index].m14= Con::getFloatVariable(varW);
            if (varH) object->mElements[index].m15= Con::getFloatVariable(varH);
        } else {
            Con::errorf("getVector4ToGlobal Invalid stack id! (%d)", stack);
            return false;
        }
    } else {
        Con::errorf("getVector4ToGlobal invalid index!! %d max:%d", index,object->mElements.size() );
        return false;
    }
    return true;
}

// FIXME why did i so many functions?! look are BatchPull/BatchPush
DefineEngineMethod(BatchRender, getVector4GlobalV, void,
        (S32 index, String varX, String varY, String varW, String varH),
            , "Fetches X, Y, W, H to global variables named by parameters") {
    _assignVector4ToGlobal(object, 0,index, varX, varY, varW, varH);
}

DefineEngineMethod(BatchRender, setVector4GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {
     _assignVector4FromGlobal(object, 0,index, varX, varY, varW, varH);
}



DefineEngineMethod(BatchRender, getVector4_1GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {
    _assignVector4ToGlobal(object, 1,index, varX, varY, varW, varH);
}

DefineEngineMethod(BatchRender, setVector4_1GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {
    _assignVector4FromGlobal(object, 1,index, varX, varY, varW, varH);
}

DefineEngineMethod(BatchRender, getVector4_2GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {
    _assignVector4ToGlobal(object, 2,index, varX, varY, varW, varH);
}
DefineEngineMethod(BatchRender, setVector4_2GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {

    _assignVector4FromGlobal(object, 2,index, varX, varY, varW, varH);
}

DefineEngineMethod(BatchRender, getVector4_3GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {
    _assignVector4ToGlobal(object, 3,index, varX, varY, varW, varH);
}

DefineEngineMethod(BatchRender, setVector4_3GlobalV, void,
                   (S32 index, String varX, String varY, String varW, String varH),
                   , "Fetches X, Y, W, H to global variables named by parameters") {

    _assignVector4FromGlobal(object, 3,index, varX, varY, varW, varH);

}

// ================================================================================================
std::unique_ptr<BatchRender> gBatchRender = nullptr;
/*
  Test:
    BatchCreate(10); BatchPull(0,0); echo($b0X SPC  $b0Y SPC  $b0Z SPC  $b0W);
    $b0X = 10; $b0Y = 20; BatchPush(0);
*/

DefineEngineFunction(BatchCreate,void, (S32 count),,"Create or reinit global BatchRender")
{
    if (!gBatchRender) gBatchRender = std::make_unique<BatchRender>();
     gBatchRender->setBufferSize(count);
}

// ------------------ Push / Pull Vector 4 by index and stack
DefineEngineFunction(BatchPush,bool, (S32 index,S32 stack),(0),"push variables $b[stack]x, y, z, w")
{
    if (!gBatchRender || stack < 0 || stack > 3) return false;




    StringBuilder str[4];
    str[0].format("$b%dX", stack);
    str[1].format("$b%dY", stack);
    str[2].format("$b%dZ", stack);
    str[3].format("$b%dW", stack);

    return _assignVector4FromGlobal(gBatchRender.get(), stack, index,
                                  str[0].end(), str[1].end(), str[2].end(), str[3].end()
    );
}
DefineEngineFunction(BatchPull,bool, (S32 index,S32 stack),(0),"setup variables $b[stack]x, y, z, w")
{
    if (!gBatchRender || stack < 0 || stack > 3) return false;

    StringBuilder str[4];
    str[0].format("$b%dX", stack);
    str[1].format("$b%dY", stack);
    str[2].format("$b%dZ", stack);
    str[3].format("$b%dW", stack);

    return _assignVector4ToGlobal(gBatchRender.get(), stack,index,
            str[0].end(), str[1].end(), str[2].end(), str[3].end()
    );
}


// ------------------ Push / Pull Vector 2 by index and stack
DefineEngineFunction(BatchPushVec2,bool, (S32 index,S32 stack, String var1, String var2),,"push Vector2 variables")
{
    if (!gBatchRender || stack < 0 || stack > 7) return false;

    return _assignVector2FromGlobal(gBatchRender.get(), stack, index,
                                    var1,var2
    );
}
DefineEngineFunction(BatchPullVec2,bool, (S32 index,S32 stack, String var1, String var2),,"pull Vector2 variables")
{
    if (!gBatchRender || stack < 0 || stack > 7) return false;

    return _assignVector2ToGlobal(gBatchRender.get(), stack, index,
                                    var1,var2
    );
}

// ------------------ Dump Rects #DEBUG stuff
DefineEngineFunction(BatchDumpRects,bool, (Color color),(DARKPURPLE),"Dump rects from the global BatchRender") {
    if (!gBatchRender) return false;
    gBatchRender->dumpRects();
    return true;
}

// ------------------ Draw Rects
DefineEngineFunction(BatchDrawRects,bool, (Color color),(DARKPURPLE),"Draw rects from the global BatchRender") {
    if (!gBatchRender) return false;
    S32 count = gBatchRender->getCount();
    if (count <= 0) return false;
    gBatchRender->drawRects(count, color);
    return true;
}

DefineEngineFunction(BatchDrawColoredRects,bool, (Color color),(DARKPURPLE),"Draw rects from the global BatchRender 3th stack is used for colors") {
    if (!gBatchRender) return false;

    gBatchRender->drawColoredRects();
    return true;
}

// ------------------ Draw Lines
DefineEngineFunction(BatchDrawLines,bool, (Color color),(DARKPURPLE),"Draw lines from the global BatchRender") {
    if (!gBatchRender) return false;
    S32 count = gBatchRender->getCount();
    if (count <= 0) return false;
    gBatchRender->drawLines(count, color);
    return true;
}

DefineEngineFunction(BatchDrawColoredLines,bool, (Color color),(DARKPURPLE),"Draw lines from the global BatchRender 3th stack is used for colors") {
    if (!gBatchRender) return false;

    gBatchRender->drawColoredLines();
    return true;
}
} //namespace ElfRender

// ==============================================================================

