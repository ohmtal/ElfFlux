#include "Gui.h"

#define RAYGUI_IMPLEMENTATION
#include "rayspice/raygui_custom.h"

#include "ConsoleTypes.h"



namespace RayTal {
    IMPLEMENT_CONOBJECT(Gui);



    void Gui::initPersistFields() {
        Parent::initPersistFields();
        addField("left", TypeF32, Offset( mLeft, Gui));
        addField("cursor", TypeVector2, Offset(mCursor, Gui));
        addField("spacing", TypeVector2, Offset(mSpacing, Gui));
        addField("defaultSize", TypeVector2, Offset(mDefaultSize, Gui));

        addField("sameLine", TypeBool, Offset(mSameLine, Gui));

        // last states
        addField("isFocused", TypeBool, Offset(mIsFocused, Gui));
        addField("isPressed", TypeBool, Offset(mIsPressed, Gui));
        addField("isClicked", TypeBool, Offset(mIsClicked, Gui));

        addField("lastBounds", TypeRectangle, Offset(mLastBounds, Gui));
        addField("lastState", TypeS32, Offset(mLastState, Gui));

    }
    // -------------------------------------------------------------------------
    void Gui::Begin(F32 x, F32 y) {
        mLeft = x;
        mCursor = Vector2(mLeft, y);
        mLastBounds = {0.f, 0.f, 0.f, 0.f};
    }

    DefineEngineMethod(Gui, Begin,void, (F32 x, F32 y), , ""){
        object->Begin(x,y);
    }
    // -------------------------------------------------------------------------
    void Gui::SameLine( F32 relativeX ) {
        mSameLine = true;
        if (relativeX > 0.f) mCursor.x =  mLeft + relativeX ;
    }

    DefineEngineMethod(Gui, SameLine,void, (F32 relativeX), (0), ""){
        object->SameLine(relativeX);
    }
    // -------------------------------------------------------------------------
    void Gui::enhanceCursor(){
        if ( mSameLine ) {
            mCursor.x += mLastBounds.width + mSpacing.x;
        } else {
            mCursor.y += mLastBounds.height + mSpacing.y;
            mCursor.x = mLeft;
        }
        mSameLine = false;
    }

    DefineEngineMethod(Gui, enhanceCursor,void, (), , ""){
        object->enhanceCursor();
    }
    // -------------------------------------------------------------------------
    void Gui::resetStates(){
        mLastState = guiState;
        mIsFocused = false;
        mIsPressed = false;
        mIsClicked = false;
    }
    DefineEngineMethod(Gui, resetStates,void, (), , ""){
        object->resetStates();
    }
    // -------------------------------------------------------------------------
    void Gui::setStates(const Vector2 size) {
        enhanceCursor();
        resetStates();
        mLastBounds = {mCursor.x, mCursor.y, size.x, size.y};
        if ((mLastState != STATE_DISABLED) && !guiLocked && !guiControlExclusiveMode)
        {

            Vector2 mousePoint = GUI_POINTER_POSITION;

            // Check button state
            if (CheckCollisionPointRec(mousePoint, mLastBounds))
            {
                if (GUI_BUTTON_DOWN) {
                        mLastState = STATE_PRESSED;
                        mIsPressed = true;
                } else {
                    mLastState = STATE_FOCUSED;
                    mIsFocused = true;
                }
                if (GUI_BUTTON_RELEASED) mIsClicked = true;
            }
        }
    }
    DefineEngineMethod(Gui, setStates,void, (F32 width, F32 height), , ""){
        object->setStates({width, height});
    }
    // -------------------------------------------------------------------------
    bool Gui::Button(const Vector2 size, const char* text)
    {
        setStates(size); // check clicked/focused/... setlastBounds

        GuiDrawRectangle(mLastBounds, GuiGetStyle(BUTTON, BORDER_WIDTH),
                            GetColor(GuiGetStyle(BUTTON, BORDER + (mLastState * 3))),
                            GetColor(GuiGetStyle(BUTTON, BASE + (mLastState*3))));
        GuiDrawText(text, GetTextBounds(BUTTON, mLastBounds),
                    GuiGetStyle(BUTTON, TEXT_ALIGNMENT),
                    GetColor(GuiGetStyle(BUTTON, TEXT + (mLastState * 3))));

        if (mIsFocused) GuiTooltip(mLastBounds);


        return mIsClicked;
    }

    DefineEngineMethod(Gui, ButtonV,bool, (Vector2 size, String text ), , ""){
        return object->Button(size, text.c_str());
    }
    // bool Button(const F32 width, const char *text) { return Button({width, mDefaultSize.y}, text);}
    DefineEngineMethod(Gui, Button,bool, (F32 width, String text ), , "") {
        return object->Button(width, text.c_str());
    }
    // -------------------------------------------------------------------------
    bool Gui::CheckBox(const Vector2 size, const char* text, bool* checked)
    {
        bool temp = false;
        if (checked == NULL) checked = &temp;


        Rectangle textBounds = { 0.f };
        F32 textWidth = (float)GuiGetTextWidth(text) + 2;
        // update size - i ignore TEXT_ALIGN_LEFT for checkbox! FIXME?
        Vector2 totalSize = {
            size.x + textWidth + GuiGetStyle(CHECKBOX, TEXT_PADDING),
            size.y
        };
        setStates(totalSize); // check clicked/focused/... setlastBounds

        Rectangle boxBounds = { mCursor.x, mCursor.y, size.x, size.y };

        if (text != NULL)
        {
            textBounds.width = textWidth;
            textBounds.height = (float)GuiGetStyle(DEFAULT, TEXT_SIZE);
            textBounds.x = mCursor.x + size.x + GuiGetStyle(CHECKBOX, TEXT_PADDING);
            textBounds.y = mCursor.y + size.y/2 - GuiGetStyle(DEFAULT, TEXT_SIZE)/2;
            if (GuiGetStyle(CHECKBOX, TEXT_ALIGNMENT) == TEXT_ALIGN_LEFT)
                textBounds.x = mCursor.x - textBounds.width - GuiGetStyle(CHECKBOX, TEXT_PADDING);
        }

        // Draw control
        GuiDrawRectangle(boxBounds,
                        GuiGetStyle(CHECKBOX, BORDER_WIDTH),
                        GetColor(GuiGetStyle(CHECKBOX, BORDER + ( mLastState * 3) )),
                        BLANK);

        if (*checked)
        {
            Rectangle check = { boxBounds.x + GuiGetStyle(CHECKBOX, BORDER_WIDTH) + GuiGetStyle(CHECKBOX, CHECK_PADDING),
                                boxBounds.y + GuiGetStyle(CHECKBOX, BORDER_WIDTH) + GuiGetStyle(CHECKBOX, CHECK_PADDING),
                                boxBounds.width - 2*(GuiGetStyle(CHECKBOX, BORDER_WIDTH) + GuiGetStyle(CHECKBOX, CHECK_PADDING)),
                                boxBounds.height - 2*(GuiGetStyle(CHECKBOX, BORDER_WIDTH) + GuiGetStyle(CHECKBOX, CHECK_PADDING)) };
            GuiDrawRectangle(check, 0, BLANK, GetColor(GuiGetStyle(CHECKBOX, TEXT + mLastState * 3 )));
        }

        GuiDrawText(text, textBounds,
                    (GuiGetStyle(CHECKBOX, TEXT_ALIGNMENT) == TEXT_ALIGN_RIGHT)? TEXT_ALIGN_LEFT : TEXT_ALIGN_RIGHT,
                    GetColor(GuiGetStyle(LABEL, TEXT + ( mLastState * 3 ))));

        if (mIsClicked) *checked = !(*checked);
        return mIsClicked;
    }

    // FIXME  ::: using a ConsoleVariable
    //FIXME engine method! CheckBoxV

    // bool CheckBox(const char* text, bool* checked) { return CheckBox(mDefaultSize, text, checked);}
    DefineEngineMethod(Gui, checkBox,bool, (String text, bool value), , "") {

        return object->CheckBox( text.c_str(), &value);
    }


    DefineEngineMethod(Gui, checkBoxVar,bool, (String text, String variable),
                       , "variable example: $MyValue or %this.object.getId() @ \".enabled\"") {
        StringTable->insert(variable);
        bool value = Con::getBoolVariable(variable.c_str());
        if (object->CheckBox( text.c_str(), &value)) {
            Con::setBoolVariable(variable.c_str(), value);
            return true;
        }
        return false;
    }
    // -------------------------------------------------------------------------
    bool Gui::Write(const char* text, int fontSize, Color color){
        Vector2 size { (F32)MeasureText(text, fontSize), (F32)fontSize};
        setStates(size);
        DrawText(text, (S32)mCursor.x, (S32)mCursor.y, fontSize, color);
        return mIsClicked;
    }


    // bool Write(const char *text,  Color color = LIGHTGRAY) { return Write(text, mDefaultSize.y, color); }
    DefineEngineMethod(Gui, Write,bool, (String text, S32 fontSize, Color color ),(-1, LIGHTGRAY) , "") {
        if (fontSize <= 0 ) fontSize = object->mDefaultSize.y;
        return object->Write( text.c_str(),fontSize, color);
    }
    // -------------------------------------------------------------------------
    bool Gui::Label(const char* text) {
        Vector2 size {
            (float)GuiGetTextWidth(text) + 2,
            (float)GuiGetStyle(LABEL, TEXT_SIZE)
        };
        setStates(size);

        GuiDrawText(text, GetTextBounds(LABEL, mLastBounds),
                    GuiGetStyle(LABEL, TEXT_ALIGNMENT),
                    GetColor(GuiGetStyle(LABEL, TEXT + ( mLastState * 3 ))));
        return mIsClicked;
    }

    DefineEngineMethod(Gui, Label,bool, (String text ), , "") {
        return object->Label( text.c_str());
    }
    // -------------------------------------------------------------------------
    bool Gui::Separator(const Vector2 size, Color color) {
        setStates(size);

        DrawLine(getX(), getY(),  getX() + size.x, getY(), color);
        return mIsClicked;
    }

    DefineEngineMethod(Gui, SeparatorV,bool, (Vector2 size, Color color ), , ""){
        return object->Separator(size, color);
    }
    // bool Separator(const F32 width, Color color = LIGHTGRAY) { return Separator( { width, 0.f }, color); }
    DefineEngineMethod(Gui, Separator,bool, (F32 width, Color color ),(LIGHTGRAY) , ""){
        return object->Separator(width, color);
    }
    // -------------------------------------------------------------------------
    bool Gui::Slider(const Vector2 size, const char *leftText,const char *rightText, F32* value, F32 min, F32 max) {
        setStates(size); // move ...
        return GuiSlider(mLastBounds, leftText, rightText, value, min, max);
    }
    //FIXME Console SliderV/Slider ::: using a ConsoleVariable?

    // bool Slider(F32 width, const char *text,  F32* value, F32 min = 0.f, F32 max = 1.f) { return Slider( { width, mDefaultSize.y }, nullptr, text, value, min, max); }

    // -------------------------------------------------------------------------
    // -------------------------------------------------------------------------
    // FIXME more gui bindings....

}
