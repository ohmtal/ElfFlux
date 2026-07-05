//-----------------------------------------------------------------------------
// Copyright (c) 2009/2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#pragma once
// #include "core/fluxBaseObject.h"
// #include "render/fluxRender2D.h"
#include "console/simBase.h"
#include "console/consoleTypes.h"
#include "grid/basicgrid.h"

namespace ElfGrid {

class Grid : public SimSet/*, public FluxBaseObject*/ {
     typedef SimObject Parent;
public:
    BasicGrid mGrid;


    // static void initPersistFields();

    bool onAdd() override;
    void onRemove() override;

    SimObject * createPath(Vector2 start, Vector2 end, const bool smoothPath );


    DECLARE_CONOBJECT(Grid);
};

} //namespace
