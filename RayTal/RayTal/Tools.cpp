//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// RayTal Tools
//-----------------------------------------------------------------------------
#include "Tools.h"


// For GetUserConfigPath <<
#if defined(__ANDROID__)
#include <android/native_activity.h>
#endif

#include "raylib.h" //<< for GetApplicationDirectory

namespace RayTal::Tools {

    std::string GetUserConfigPath() {
        char path[512] = { 0 };

        #if defined(_WIN32)
        const char* appData = getenv("APPDATA");
        if (appData) snprintf(path, sizeof(path), "%s", appData);

        #elif defined(__ANDROID__)
        snprintf(path, sizeof(path), "%s", GetAndroidAppDirectory());

        #elif defined(__EMSCRIPTEN__)
        snprintf(path, sizeof(path), "%s",  "/home/web_user");

        #elif defined(__unix__)
        const char* home = getenv("HOME");
        if (home) snprintf(path, sizeof(path), "%s/.local/share", home);

        #elif defined(__APPLE__)
        const char* home = getenv("HOME");
        if (home) snprintf(path, sizeof(path), "%s/Library/Application Support", home);
        #endif

        return std::string(path);
    }

    std::string GetPrefPath(const char* org, const char* app){
        static std::string cachedPath = "";
        if (!cachedPath.empty()) return cachedPath;
        std::string base = GetUserConfigPath();
        if (base.empty()) return std::string();
        cachedPath =  base + "/" + org + "/" + app;
        return cachedPath;
    }

    std::string sanitizeFilenameWithUnderScores(std::string name)
    {
        std::string result;
        for (unsigned char c : name) {
            if (std::isalnum(c)) {
                result += c;
            } else if (std::isspace(c)) {
                result += '_';
            }
            // Special characters (like '.') are ignored/dropped here
        }
        return result;
    }

    std::string string_replace_all(std::string str, const std::string& from, const std::string& to) {
        size_t start_pos = 0;
        while ((start_pos = str.find(from, start_pos)) != std::string::npos) {
            str.replace(start_pos, from.length(), to);
            start_pos += to.length();
        }
        return str;
    }

    std::string getBasePath() {
        static std::string cachedPath = "";
        if (!cachedPath.empty()) return cachedPath;

        const char* rawPath = GetApplicationDirectory();
        if (rawPath) {
            cachedPath = rawPath;
        }
        return cachedPath;
    }
}
