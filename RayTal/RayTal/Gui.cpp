#include "Gui.h"

#define RAYGUI_IMPLEMENTATION
#include "raygui_custom.h"



namespace RayTal {

    void Gui::Begin(F32 x, F32 y) {
        mLeft = x;
        mCursor = Vector2(mLeft, y);
        mLastBounds = {0.f, 0.f, 0.f, 0.f};
    }
    // -------------------------------------------------------------------------
    void Gui::SameLine( F32 relativeX ) {
        mSameLine = true;
        if (relativeX > 0.f) mCursor.x =  mLeft + relativeX ;
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
    // -------------------------------------------------------------------------
    void Gui::resetStates(){
        mLastState = guiState;
        mIsFocused = false;
        mIsPressed = false;
        mIsClicked = false;
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
    // -------------------------------------------------------------------------
    bool Gui::Write(const char* text, int fontSize, Color color){
        Vector2 size { (F32)MeasureText(text, fontSize), (F32)fontSize};
        setStates(size);
        DrawText(text, (S32)mCursor.x, (S32)mCursor.y, fontSize, color);
        return mIsClicked;
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

    // -------------------------------------------------------------------------
    bool Gui::Separator(const Vector2 size, Color color) {
        setStates(size);

        DrawLine(getX(), getY(),  getX() + size.x, getY(), color);
        return mIsClicked;
    }
    // -------------------------------------------------------------------------
    bool Gui::Slider(const Vector2 size, const char *leftText,const char *rightText, F32* value, F32 min, F32 max) {
        setStates(size); // move ...
        return GuiSlider(mLastBounds, leftText, rightText, value, min, max);
    }


}
