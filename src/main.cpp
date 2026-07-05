//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#include "raylib.h"
#include "main/engineGlue.h"
#include "console/script.h"
#include "console/engineAPI.h"


String gScriptFile = "assets/main.cs";
bool gShutDownRequest = false;
bool gNoDefaultCalls  = false;
bool gEnableConsole = false;

extern void initEnum();  //elfEnum.cpp
extern void CustomTraceLog(int msgType, const char *text, va_list args); //elfBase.cpp
namespace ElfResource { extern void shutDown(); }


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

int main(int argc, char* argv[])
{
    argParser(argc, argv);
    engineGlue::init(nullptr, GetApplicationDirectory()); // FIXME command line path => --path
    initEnum();
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


        // --------- advance time for scheduler this should be placed in the main loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            Con::executef("MainLoop");
            engineGlue::process(GetFrameTime());
            if (gShutDownRequest) break;
        }

        Con::executef("MainShutdown");
    } // !gNoDefaultCalls


    // -------- finallize
    ElfResource::shutDown();
    engineGlue::shutDown();

    return 0;
}
