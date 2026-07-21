
#include "ResourceManager.h"
#include "Main.h"

namespace RayTal {

    // -------------------------------------------------------------------------
    Music* ResourceManager::getMusic(const std::string fileName, bool noAutoLoad) {
        if (!mInitialized || isBlackListed(fileName)) return nullptr;

        auto it = mMusicMap.find(fileName);
        if (it != mMusicMap.end()) {
            return &it->second;
        }

        if (noAutoLoad) return nullptr;

        std::string lFileName =  "music:/" + fileName;
        mMain->setFullPath(lFileName);
        Music music = LoadMusicStream(lFileName.c_str());
        if (!IsMusicValid(music)) {
            blacklist(fileName);
            UnloadMusicStream(music);
            return nullptr;
        }
        mMusicMap[fileName] = music;


        return &mMusicMap[fileName];
    }
    // -------------------------------------------------------------------------
    Sound* ResourceManager::getSound(const std::string fileName, bool noAutoLoad) {
        if (!mInitialized || isBlackListed(fileName)) return nullptr;


        auto it = mSoundMap.find(fileName);
        if (it != mSoundMap.end()) {
            return &it->second;
        }
        if (noAutoLoad) return nullptr;

        std::string lFileName =  "sound:/" + fileName;
        mMain->setFullPath(lFileName);

        Sound sound = LoadSound(lFileName.c_str());
        if (!IsSoundValid(sound)) {
            blacklist(fileName);
            UnloadSound(sound);
            return nullptr;
        }
        mSoundMap[fileName] = sound;
        return &mSoundMap[fileName];
    }
    // -------------------------------------------------------------------------
    Texture2D* ResourceManager::getTexture(const std::string fileName, bool noAutoLoad) {
        if (!mInitialized || isBlackListed(fileName)) return nullptr;

        auto it = mTextureMap.find(fileName);
        if (it != mTextureMap.end()) {
            return &it->second;
        }

        if (noAutoLoad) return nullptr;

        std::string lFileName =  "texture:/" + fileName;
        mMain->setFullPath(lFileName);
        Image image = LoadImage(lFileName.c_str());
        Texture2D texture = LoadTextureFromImage(image);
        UnloadImage(image);
        mTextureMap[fileName] = texture;
        return &mTextureMap[fileName];
    }
    // -------------------------------------------------------------------------
    void ResourceManager::Update() {
        for (auto& [key, music] : mMusicMap) {
            UpdateMusicStream( music );
        }
    }
    // -------------------------------------------------------------------------
    bool ResourceManager::initialize(Main* main){
        if (main) {
            mMain = main;
            mInitialized = true;
        }
        return mInitialized;
    }
    // -------------------------------------------------------------------------
    void ResourceManager::shutDown() {
        if (mShutDown) return;
        mInitialized = false;
        mShutDown = true;

        for (auto& [key, val] : mSoundMap) {
            UnloadSound(val);
        }

        for (auto& [key, val] : mMusicMap) {
            UnloadMusicStream(val);
        }

        for (auto& [key, val] : mTextureMap) {
            UnloadTexture(val);
        }
    }
    // -------------------------------------------------------------------------
    void ResourceManager::blacklist(const std::string fileName){
        if (!isBlackListed(fileName))
            mBlacklist.push_back(fileName);
    }
    // -------------------------------------------------------------------------
    bool ResourceManager::isBlackListed(const std::string fileName){
        return std::find(mBlacklist.begin(), mBlacklist.end(), fileName) != mBlacklist.end();
    }

}
