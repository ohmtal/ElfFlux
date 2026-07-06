//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// ConsoleGuiObject using modified dk_console
// FIXME port to imgui/ohmflux ImConsole
//-----------------------------------------------------------------------------
#include "console/engineAPI.h"
#include "console/script.h"
#include "raylib.h"
#include "raymath.h"
#include "core/strings/stringUnit.h"

#include "elfResource.h"

#define DK_CONSOLE_IMPLEMENTATION
#include "dk/dk_console.h"

namespace ConsoleGui {

    //XXTH globals hackfest
    Console* gConsolePtr = nullptr;
    Vector<String> gHistory;
    S32 gHistoryNeedle;


    //-----------------------------------------------------------------------------
    static void ConsoleConsumer(/*ConsoleLogEntry::Level*/ U32 level, const char *line)
    {
        if (!gConsolePtr || !gConsolePtr->logs
            || !line || line[0] == 0
        ) return;

        S32 finalLevel = LOG_INFO;
        switch (level)
        {
            case ConsoleLogEntry::Normal: finalLevel = LOG_INFO; break;
            case ConsoleLogEntry::Warning: finalLevel = LOG_WARNING; break;
            case ConsoleLogEntry::Error: finalLevel = LOG_ERROR; break;
            default: finalLevel = LOG_INFO; break;
        }
        U32 unitCount = StringUnit::getUnitCount(line, "\n");

        for (U32 i = 0; i < unitCount; i++) {
            // 1. Hole den aktuellen Slot (und schütze dich vor Array-Überlauf)
            U32 slot = gConsolePtr->log_index % gConsolePtr->logSize;
            gConsolePtr->log_index++;

            // empty curent line
            dMemset(gConsolePtr->logs[slot].text, 0, DKCONSOLE_LINELENGTH);

            // write so line
            StringUnit::getUnit(line, i, "\n", gConsolePtr->logs[slot].text, DKCONSOLE_LINELENGTH);

            // set log level
            gConsolePtr->logs[slot].type = finalLevel;
        }

    }
    //-----------------------------------------------------------------------------
    void  ConsoleHandler(const char* command)
    {
        if (command && command[0] != 0){
            gHistory.push_back(command);
            gHistoryNeedle = gHistory.size(); // - 1;
            Con::evalAutoComplete(command);
        }
    }
    //-----------------------------------------------------------------------------
    // Class
    //-----------------------------------------------------------------------------
    class ConsoleGuiObject : public SimObject
    {
        typedef SimObject Parent;
    public:
        DECLARE_CONOBJECT(ConsoleGuiObject);

         Console mConsole = { .toggle_key = KEY_GRAVE };
         ImUI mImUI;


        static void initPersistFields();
        bool onAdd() override;
        void onRemove() override;

        void pushCommand();
        void update();

    };
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    IMPLEMENT_CONOBJECT(ConsoleGuiObject);
    //-----------------------------------------------------------------------------
    void ConsoleGuiObject::initPersistFields() {
        Parent::initPersistFields();
        addField("key", TypeS32, Offset(mConsole.toggle_key, ConsoleGuiObject), "Shortcut to open console");
        addField("fontSize", TypeF32, Offset(mConsole.fontSize, ConsoleGuiObject), "font size");
        addField("heightDiv", TypeF32, Offset(mConsole.heightDiv, ConsoleGuiObject), "2=half height, 1= full heigth");
    }
    //-----------------------------------------------------------------------------
    bool ConsoleGuiObject::onAdd() {
        if (!Parent::onAdd()) return false;
        if (gConsolePtr) return false;
        gConsolePtr = &mConsole;

        mImUI.theme = &DK_ImUIDefaultTheme;
        mImUI.style = &DK_ImUIDefaultStyle;

        static Font font = GetFontDefault();
        mImUI.font = &font;

        DK_ConsoleInit(gConsolePtr, DKCONSOLE_LOG_SIZE);
        Con::addConsumer(ConsoleConsumer);
        return true;
    }
    //-----------------------------------------------------------------------------
    void ConsoleGuiObject::onRemove() {
        DK_ConsoleShutdown(gConsolePtr);
        gConsolePtr = nullptr;
        Parent::onRemove();
    }
    //-----------------------------------------------------------------------------
    void ConsoleGuiObject::pushCommand() {
        if (!gConsolePtr || gHistory.size() == 0) return;
        if ( gHistoryNeedle >= 0 && gHistoryNeedle < gHistory.size()) {
            // Con::printf("COMMAND SHOULD BE %s", gHistory[gHistoryNeedle].c_str());
            dSprintf(gConsolePtr->ConsoleInputText, DKCONSOLE_LINELENGTH, "%s",  gHistory[gHistoryNeedle].c_str());
            gConsolePtr->cursorPos = gHistory[gHistoryNeedle].length();
        } else {
            dStrcpy(gConsolePtr->ConsoleInputText, "", DKCONSOLE_LINELENGTH);
            gConsolePtr->cursorPos = 0;
        }
    }
    //-----------------------------------------------------------------------------
    void ConsoleGuiObject::update() {
        if (!gConsolePtr) return ;
        DK_ConsoleUpdate(gConsolePtr, &mImUI,ConsoleHandler);
        if (gConsolePtr->is_open) {
            if (IsKeyPressed(KEY_UP)) {
                if (gHistoryNeedle > 0) gHistoryNeedle--;
                    pushCommand();
            } else if (IsKeyPressed(KEY_DOWN)) {
                if (gHistoryNeedle < gHistory.size()) gHistoryNeedle++;
                    pushCommand();
            } else if (IsKeyPressed(KEY_TAB)) {
                bool backward =  (IsKeyDown(KEY_LEFT_SHIFT) || IsKeyDown(KEY_RIGHT_SHIFT));

                gConsolePtr->cursorPos = Con::tabComplete(gConsolePtr->ConsoleInputText
                        , gConsolePtr->cursorPos, DKCONSOLE_LINELENGTH - 1, !backward);


            }
        }
    }
    //-----------------------------------------------------------------------------
    // Console functions
    //-----------------------------------------------------------------------------
    DefineEngineMethod(ConsoleGuiObject, setFont, bool, (S32 fontId), , "set the console font") {
        if (!gConsolePtr) return false;
        Font* font = ElfResource::FontMap.get(fontId);
        if (font) {
            object->mImUI.font = font;
            return true;
        }
        return false;
    }

    //-----------------------------------------------------------------------------
    DefineEngineMethod(ConsoleGuiObject, Update, void, (), , "update the console") {
        object->update();

    }

} //namespace

