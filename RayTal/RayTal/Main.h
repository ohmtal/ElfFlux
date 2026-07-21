//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//------------------------------------------------------------------------------
// RayTal Main
//------------------------------------------------------------------------------
#pragma once

#include "raylib.h"
#include "raymath.h"

#include "Core.h"
#include "TypeDef.h"
#include "Settings.h"
#include "ResourceManager.h"

#include <functional>
#include <memory> // unique_ptr


namespace RayTal {

    #define COLOR_SLATEGRAY   CLITERAL(Color){ 20, 20, 20, 255 }     // Darkest Gray


    // ---------------- Main Class
    class Main :public Core {
    protected:
        Settings mSettings;
        ResourceManager mResourceManager;

    public:
        /**
         * a list of Objects based on Core
         * automated calls: Render, Update and ShutDown
         * NOTE: untested! and maybe need some sorting
         */
        std::vector<std::unique_ptr<Core>> mCoreObjects;

        Settings* getSettings() { return &mSettings; }
        ResourceManager* getResourceManager() { return &mResourceManager; }

        // ---------- Events ----------
        /**
         * Event OnInit - called after base init is done
         */
        std::function<bool()> OnInit = nullptr;
        /**
         * Event OnRender - wrap begin/end drawing and clear
         */
        std::function<void()> OnRender = nullptr;
        /**
         * Event OnUpdate - called at a fixed deltaTime
         */
        std::function<void(const F32)> OnUpdate = nullptr;
        /**
         * Event OnShutDown - called on shutdown system before
         *                    method ShutDown is called.
         */
        std::function<void()> OnShutDown = nullptr;

        // ---------- Methods ----------
        /**
         * Init - create window and apply settings
         */
        bool Init() override;
        /**
         * update call at a fixed delta time (60fps)
         */
        void Update(F32 dt) override;
        /**
         * render call
         */
        void Render() override;
        /**
         * Shutdown - unload objects
         */
        void ShutDown() override;

        /**
         * Main object loop
         */
        void Execute();
        void IterateFrame();
        // ---------- Tool Functions ----------
        /**
         * setFullPath
         * replace a path with full path (see also Settings and Tools)
         */
        void setFullPath(std::string& path){ getSettings()->setFullPath(path); }


        // ---------- Resources ----------
        // NOTE: The resource Manager uses maps to identfy the Objects by short Filename
        //       based on the Path Settings. The Objects unload it's content on ResourceManager
        //       shutDown.

        // wrapper to play Music or Sound
        bool playMusic(const char * musicFileName, F32 volume = 1.f);
        bool stopMusic(const char * musicFileName);
        bool playSound( const char* soundFileName );

        // wrapper for getting a Resource Object Pointer
        Music* getMusic(const char * musicFileName) { return getResourceManager()->getMusic(musicFileName);}
        Sound* getSound(const char* soundFileName) { return getResourceManager()->getSound(soundFileName); }
        Texture* getTexture(const char* fileName) { return getResourceManager()->getTexture(fileName); }

    }; //class

} ; //namespace
