//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// ported from RayTal to raylib-elfscript
//-----------------------------------------------------------------------------
// RayTal Gui
//
// Struct Gui handle gui elements in a way we can get the last states like
// mIsClicked. Every Widget return a boolean usually mIsClicked.
// I only have a few Widgets so far. A bit inspired by ImGui.
// I had started with LazyGui which nearly does the same but to use SameLine
// all Widgets needs to be ported to get the size of each.
// for full control it's possible to combine it with the raygui functions.
//
// Example:
// init:
//      RayTal::Gui gui(100 , 100);
// render:
//      gui.Begin(); // reset cursor and mLastBounds
//      gui.Write("Hello World");
//      //Combine example:
//        gui.setStates(SliderSize); // set the size of the element
//        static float sliderVal = 1.f; //GetMusicVolume(mainMusic);
//        // mLastBounds is set by setStates
//        if (GuiSlider(gui.mLastBounds, nullptr, "Vol.", &sliderVal, 0.f, 1.f)) {
//            SetMusicVolume(*mainMusic,  sliderVal);
//        }
//
//      gui.End(); // OPTIONAL at the moment - does nothing but looks good ;)
//-----------------------------------------------------------------------------
#pragma once

#include "raylib.h"

#include "platform/types.h"
#include "console/engineAPI.h"

//rayTal: #include "TypeDef.h"


namespace RayTal {

    class Gui : public SimObject
    {
        typedef SimObject Parent;

    public:
        DECLARE_CONOBJECT(Gui);

        F32 mLeft = 0.f;
        // F32 mTop  = 0.f;
        Vector2 mCursor = {0.f, 0.f};
        Vector2 mSpacing = { 10.f, 10.f };
        Vector2 mDefaultSize = { 20.f, 20.f };

        bool mSameLine  = false;

        // last states
        bool mIsFocused = false;
        bool mIsPressed = false;
        bool mIsClicked = false;
        Rectangle mLastBounds = {0.f, 0.f, 0.f, 0.f};
        S32 mLastState  = 0;


        static void initPersistFields();



        void Begin(F32 x, F32 y) ;
        void End() {}

        void SameLine( F32 relativeX=0.f );

        F32 getX() { return mCursor.x; };
        F32 getY() { return mCursor.y; };

        // ----- internal function but public for full control
        void resetStates();
        void setStates(const Vector2 size);
        void enhanceCursor();
        // ---------------------------------------------------------------------
        // Button control, returns true when clicked
        bool Button(const Vector2 size, const char *text);
        bool Button(const F32 width, const char *text) { return Button({width, mDefaultSize.y}, text);}
        // Check Box control, returns 1 when state changed
        bool CheckBox(const Vector2 size, const char *text, bool *checked);
        bool CheckBox(const char* text, bool* checked) { return CheckBox(mDefaultSize, text, checked);}
        // Default write
        bool Write(const char *text, int fontSize, Color color = LIGHTGRAY);
        bool Write(const char *text,  Color color = LIGHTGRAY) { return Write(text, mDefaultSize.y, color); }
        // Label
        bool Label(const char *text);
        // Separator
        bool Separator(const Vector2 size, Color color = LIGHTGRAY);
        bool Separator(const F32 width, Color color = LIGHTGRAY) { return Separator( { width, 0.f }, color); }

        // GuiSlider  - return changed
        bool Slider(const Vector2 size, const char *leftText,const char *rightText, F32* value, F32 min, F32 max);
        bool Slider(F32 width, const char *text,  F32* value, F32 min = 0.f, F32 max = 1.f) { return Slider( { width, mDefaultSize.y }, nullptr, text, value, min, max); }



    }; // GUI



}
