//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// RayTal ResourceManager
//-----------------------------------------------------------------------------
#pragma once
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_map>

#include "raylib.h"

namespace RayTal {
    class Main; //fwd
    class ResourceManager {
    protected:
        Main* mMain = nullptr;
    private:
        std::vector<std::string> mBlacklist; //blacklisted filename

        std::unordered_map<std::string, Sound> mSoundMap;
        std::unordered_map<std::string, Music> mMusicMap;
        std::unordered_map<std::string, Texture2D> mTextureMap;

        bool mInitialized = false;
        bool mShutDown = false;

    public:
        ResourceManager(Main* main = nullptr) {
           initialize(main);
        }

        bool initialize(Main* main = nullptr);
        void shutDown();

        ~ResourceManager() {
            shutDown();
        }
        bool isBlackListed(const std::string fileName);
        void blacklist(const std::string fileName);

        void Update();

        Music* getMusic(const std::string fileName , bool noAutoLoad = false );
        Sound* getSound(const std::string fileName, bool noAutoLoad = false );
        Texture2D* getTexture(const std::string fileName, bool noAutoLoad = false);

    }; //ResourceManager

}; //namespace
