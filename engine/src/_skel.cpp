//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------

#include "console/engineAPI.h"

class EmptyObject : public SimObject
{
    typedef SimObject Parent;
public:
    DECLARE_CONOBJECT(EmptyObject);

    bool onAdd() override;
    void onRemove() override;

    static void initPersistFields();

};
IMPLEMENT_CONOBJECT(EmptyObject);
//--------------

class TestObj: public SimObject
{
    typedef SimObject Parent;
public:
    DECLARE_CONOBJECT(TestObj);
    F32 mX, mY, mZ;
    // StringTableEntry mTestName;

    TestObj() {
        mX = mY = mZ = 0.f;
        // mTestName = StringTable->insert("NoName");
    }

    static void initPersistFields()
    {
        // Parent::initPersistFields();
        addField("x",     TypeF32,     Offset(mX, TestObj));
        addField("y",     TypeF32,     Offset(mY, TestObj));
        addField("z",     TypeF32,     Offset(mZ, TestObj));
        addField("testFloat",     TypeF32,     Offset(mX, TestObj));
        // addField("testName", TypeString, Offset(mTestName, TestObject));

    }

    void setPos(F32 x, F32 y, F32 z) {
        mX = x;
        mY = y;
        mZ = z;
    }
};

IMPLEMENT_CONOBJECT(TestObj);

DefineEngineMethod(TestObj, getPos, String, (), , "get the position") {
    // we do nothing special here
    StringBuilder str;
    str.format("%g %g %g", object->mX, object->mY, object->mZ);
    return Con::getStringArg(str.end());
}

DefineEngineMethod(TestObj, getX, F32, (), , "") {
  return object->mX;
}
DefineEngineMethod(TestObj, getY, F32, (), , "") {
    return object->mY;
}
DefineEngineMethod(TestObj, getZ, F32, (), , "") {
    return object->mZ;
}

DefineEngineMethod(TestObj, walk, void, (F32 x, F32 y, F32 z), , "Set point where to walk to") {
    // we do nothing special here
    object->setPos(x,y,z);
}
