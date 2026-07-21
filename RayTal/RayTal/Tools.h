//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//------------------------------------------------------------------------------
// RayTal::Tools
//------------------------------------------------------------------------------
#pragma once


#include <string>


namespace RayTal::Tools {
    std::string GetUserConfigPath();
    // "SDL_GetPrefPath" equivalent
    std::string GetPrefPath(const char* org, const char* app);
    std::string sanitizeFilenameWithUnderScores(std::string name);
    std::string string_replace_all(std::string str, const std::string& from, const std::string& to);
    std::string getBasePath();
} //namespace
