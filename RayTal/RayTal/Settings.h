//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//------------------------------------------------------------------------------
// RayTal Settings
//------------------------------------------------------------------------------
#pragma once

#include <string>
#include "Tools.h"
#include "TypeDef.h"

namespace RayTal {
    //--------------------------------------------------------------------------
    struct Settings {
        U32 ScreenWidth = 1152;
        U32 ScreenHeight = 648;
        // not the exact fps since i use integer and round it.
        U32  FpsLimit = 0;
        bool FullScreen       = false;
        bool WindowMaximized  = false;
        // you also can set FpsLimit
        bool EnableVSync      = true;
        bool EnableFSAA       = true;

        bool DisableSound     = false;

        std::string Company = "RayLib Test Company";
        std::string Caption = "RayLib Caption";
        std::string Version = "RayLib Test 0.260505";

        // your window icon (have to be RGBA8 )
        std::string IconFilename = "";

        //pre path for IconFilename and loadTexture
        // base:/ is replaced with your BasePath
        // NOTE: add a tralling slash!

        std::string AssetPath = "base:/assets/";
        std::string SoundPathAppend   = "sound/";
        std::string MusicPathAppend   = "music/";
        std::string TexturePathAppend = "texture/";


        // overwrite Window FLAGS:
        int windowFlagsOverwrite = 0;

        std::string getPrefsPath();
        std::string getSafeCompany();
        std::string getSafeCaption();
        void setFPSLimit( U32 Limit);
        const U32 getFPSLimit() {return FpsLimit;}

        void setFullPath(std::string& path);

        bool getVSync();
        void setVSync(bool value);

        bool getFullScreen();
        void setFullScreen(bool value);

    }; //Settings

} //namespace
