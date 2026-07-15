//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#include "console/engineAPI.h"
#include "console/consoleExtras.h"
#include "2D/SceneObject2D.h"
namespace ElfFlux {


void initEnum() {
    Con::registerEnumS32<ElfObjects::CollisionType>("CollisionType_");
}


}
