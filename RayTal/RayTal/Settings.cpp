#include "Settings.h"
#include "raylib.h"

namespace RayTal {

    //--------------------------------------------------------------------------
    std::string Settings::getPrefsPath() {
        static std::string cachedPath = "";
        if (!cachedPath.empty()) return cachedPath;
        cachedPath = Tools::GetPrefPath(getSafeCompany().c_str(), getSafeCaption().c_str());

        return cachedPath;
    }
    //--------------------------------------------------------------------------
    std::string Settings::getSafeCompany() {
        return Tools::sanitizeFilenameWithUnderScores(Company);
    }
    //--------------------------------------------------------------------------
    std::string Settings::getSafeCaption() {
        return Tools::sanitizeFilenameWithUnderScores(Caption);
    }
    //--------------------------------------------------------------------------
    void Settings::setFPSLimit(U32 Limit){
        SetTargetFPS(Limit);
        FpsLimit = Limit;
    }
    //--------------------------------------------------------------------------
    void Settings::setFullPath(std::string& path) {
        if (path.find("assets:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "assets:/", AssetPath);
        }
        if (path.find("sound:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "sound:/", AssetPath + SoundPathAppend);
        }
        if (path.find("music:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "music:/", AssetPath + MusicPathAppend);
        }
        if (path.find("texture:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "texture:/", AssetPath + TexturePathAppend);
        }
        if (path.find("base:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "base:/", Tools::getBasePath());
        }
        if (path.find("pref:/", 0) != std::string::npos) {
            path = Tools::string_replace_all(path, "pref:/", getPrefsPath());
        }
    }
    //--------------------------------------------------------------------------
    bool Settings::getVSync(){
        return IsWindowState(FLAG_VSYNC_HINT);
    }
    void Settings::setVSync(bool value){

        if (getVSync() != value) {
            if ( !value ) ClearWindowState(FLAG_VSYNC_HINT);
            else SetWindowState(FLAG_VSYNC_HINT);
        }

        EnableVSync = value;
    }
    //--------------------------------------------------------------------------
    bool Settings::getFullScreen(){
        return IsWindowState(FLAG_FULLSCREEN_MODE);

    }

    void Settings::setFullScreen(bool value){
        if (getFullScreen() != value) {
            ToggleFullscreen();
        }
        FullScreen = value;
    }


}
