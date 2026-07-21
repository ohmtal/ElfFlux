//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#include "raylib.h"
#include "main/engineGlue.h"
#include "console/script.h"
#include "console/engineAPI.h"

#if defined(PLATFORM_WEB)
#include <emscripten/emscripten.h>
#endif

#ifdef WITH_IMGUI
#include "imgui.h"
#include "rlImGui.h"
extern void InitBindings_ImGui();
bool gEnableDockSpace = false;
#endif


String gScriptFile = "assets/main.elf";
bool gShutDownRequest = false;
bool gNoDefaultCalls  = false;
bool gEnableConsole = false;

extern void initEnum();  //elfEnum.cpp
extern void CustomTraceLog(int msgType, const char *text, va_list args); //elfBase.cpp
namespace ElfResource { extern void shutDown(); }
namespace ElfFlux { extern void initEnum(); }


int argParser(int argc, char* argv[]) {

    // pass to script in TGE/OGE3D  it is called Game::!,
    Con::setIntVariable("Main::argc", argc);
    for (S32 i = 0; i < argc; i++)
        Con::setVariable(avar("Main::argv%d", i), argv[i]);

    String argStr;
    // argv[0] is program name
    for (S32 i = 1; i < argc; ++i) {
        if (!argv[i]) continue;
        argStr = argv[i];

        if (argStr.equal("--noloop")) {
            gNoDefaultCalls = true;
            continue;
        }
        if (argStr.equal("--console")) {
            gEnableConsole = true;
            continue;
        }
        // filename test
        if (argStr.equal("--script")) {
            if (i + 1 < argc) {
                gScriptFile= argv[++i];
                dPrintf("Custom Script File: %s\n", gScriptFile.c_str());
            } else {
                dPrintf("--script but no file parameter usage: --script myFile.cs\n");
                return 1;
            }
            continue;
        }


    } //for ...
    return 0;
}


void mainLoop(void*) {
#ifdef WITH_IMGUI
    BeginDrawing();
    rlImGuiBegin();
    if (gEnableDockSpace) ImGui::DockSpaceOverViewport(0, NULL, ImGuiDockNodeFlags_PassthruCentralNode);
    Con::executef("MainLoop");
    rlImGuiEnd();
    EndDrawing();
#else
    Con::executef("MainLoop");
#endif
    static F32 timeAccumulator = 0.0f;
    F32 currentMs = (GetFrameTime() * 1000.0f) + timeAccumulator;
    U32 dtMs = (U32)currentMs;
    timeAccumulator = currentMs - (F32)dtMs;
    engineGlue::process(dtMs);

}


int main(int argc, char* argv[])
{
    argParser(argc, argv);
    engineGlue::init(nullptr, GetApplicationDirectory()); // FIXME command line path => --path
    initEnum();
    ElfFlux::initEnum();
    SetTraceLogCallback(CustomTraceLog);



    if (!Con::executeFile(gScriptFile.c_str())) { //fixme with command line
        Con::errorf("main script not found.");
        return 1;
    }

    if (!gNoDefaultCalls) {


        if (!Con::isFunction("MainLoop")) {
            Con::errorf("MainLoop function is missing!");
            return 1;
        }

        ConsoleValue initResult = Con::executef("MainInit");
        if (initResult.getBool() == false) {
            Con::errorf("init failed");
            return 1;
        }

    #ifdef WITH_IMGUI
        // overwrite script BeginDrawing(); / EndDrawing();
        // with stubs to get ImGui working
        Con::evaluate( R"(
        function BeginDrawing() {}
        function EndDrawing() {}
        )"
        );

        // initialize rlImGui
        rlImGuiSetup(true);
        InitBindings_ImGui(); //ElfScript ImGui bindings

        String iniFileName = Con::getVariable("$IMGUI::IniFilename", "");
        gEnableDockSpace = Con::getBoolVariable("$IMGUI::EnableDockSpace", false);

        if (iniFileName.isEmpty())  ImGui::GetIO().IniFilename = nullptr;
        else ImGui::GetIO().IniFilename = iniFileName.c_str();
        if (gEnableDockSpace) {
            ImGui::GetIO().ConfigFlags |= ImGuiConfigFlags_DockingEnable;
        }

    #endif


    #if defined(PLATFORM_WEB)
        // emscripten_set_main_loop(UpdateDrawFrame, 60, 1);
        emscripten_set_main_loop_arg(mainLoop, nullptr, 0, 1);
        emscripten_set_main_loop_timing(EM_TIMING_RAF, 1); //force RAF
    #else
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            mainLoop(nullptr);
            if (gShutDownRequest) break;
        }
    #endif
        Con::executef("MainShutdown");
    #ifdef WITH_IMGUI
        rlImGuiShutdown();
    #endif
    } // !gNoDefaultCalls


    // -------- finallize
    ElfResource::shutDown();
    engineGlue::shutDown();

    return 0;
}
