//
// Author: David Kviloria
// ClangFormat: Mozilla
//
// XXTH updated to raylib 6 and much more

#if !defined(DK_CONSOLE_H)
#define DK_CONSOLE_H

#if defined(__cplusplus)
extern "C"
{
#endif

#include <stdlib.h> // malloc

#if defined(DK_CONSOLE_IMPLEMENTATION)
#define DK_UI_IMPLEMENTATION
#include "dk_ui.h"
#else
#include "dk_ui.h"
#endif

#if !defined(LOG_SIZE)
// insane 1080x1080 lines ?! => 1.166.400 Slots x 1024 bytes WTF!
#define DKCONSOLE_LOG_SIZE 3000 //1080 * 1080 // size of log buffer
#endif

#if !defined(DKCONSOLE_LINELENGTH)
#define DKCONSOLE_LINELENGTH 256
#endif

  typedef struct
  {
    char* text;
    int type;
  } Log;

  typedef struct
  {
    int log_index;
    Log* logs;
    Rectangle ui;
    bool is_open;
    int scroll;
    KeyboardKey toggle_key;
    char ConsoleInputText[DKCONSOLE_LINELENGTH];
    float fontSize;
    float heightDiv; // example: 2 =>half height, 1 => full height
    int cursorPos; // the cursor pos in the input widget

    float fontSpacing; // auto calculated
    int logSize;  //WARNING do not change this !
  } Console;

  void DK_ConsoleInit(Console* console, int log_size);

  void DK_ConsoleUpdate(Console* console, ImUI* imui, void (*callback)(const char*));

  void DK_ConsoleShutdown(Console* console);

#if defined(DK_CONSOLE_IMPLEMENTATION)
  void DK_ConsoleInit(Console* console, int log_size)
  {
    console->ui = (Rectangle){ 0.0f, 0.0f, 0.0f, 0.0f };
    console->ui.height = GetScreenHeight();
    console->is_open = false;
    console->log_index = 0;
    console->scroll = 0;
    console->fontSize = 20;
    console->heightDiv = 1;
    console->fontSpacing = 25;
    console->cursorPos = -1;
    console->logSize = log_size;
    console->logs = (Log*)malloc(sizeof(Log) * console->logSize);
    for (int i = 0; i < console->logSize; i++) {
      console->logs[i].text = (char*)malloc(DKCONSOLE_LINELENGTH);
      memset(console->logs[i].text, 0, DKCONSOLE_LINELENGTH);
    }
  }

  void DK_ConsoleUpdate(Console* console, ImUI* imui, void (*callback)(const char*))
  {
    //XXTH  moved up and out of if enter ...
    if (console->log_index >= console->logSize) {
      // console->log_index = 0; //this kill the old log completely

      //better move half up (using torque function -- nu isses auch egal):
      U32 halfSize = console->logSize / 2;
      for (U32 i = 0; i < halfSize; i++) {
        U32 sourceSlot = halfSize + i;
        U32 targetSlot = i;
        // fast copy
        dStrcpy(console->logs[targetSlot].text, console->logs[sourceSlot].text, DKCONSOLE_LINELENGTH);
        console->logs[targetSlot].type = console->logs[sourceSlot].type;
      }
      // now to half size
      console->log_index = halfSize;
    }


    bool key_handled = false;
    if (IsKeyPressed(console->toggle_key) && console->is_open) {
      console->is_open = false;
      key_handled = true;
    }

    static bool focused = false;

    if (console->is_open) {
      console->heightDiv  = Clamp(console->heightDiv, 1.f,5.f); //1/5 is max!
      float maxHeight = GetScreenHeight() / console->heightDiv;
      console->ui.height = Clamp(Lerp(console->ui.height, maxHeight, 0.1f), 0.0f, maxHeight );
      focused = true;
    } else {
      console->ui.height = Lerp(console->ui.height, 0.0f, 0.5f);
    }

    // Console Background
    DrawRectangle(0,
                  0,
                  GetScreenWidth(),
                  console->ui.height,
                  Fade(imui->theme->background, 0.90f /*1.0f*/));

    if (console->is_open) {

// XXTH i dont need this
//      static bool dp_is_open = false;
//      static int dp_selected = 3; //XXTH FIXME should be detected ...3=default
//
// #define DP_OPTIONS_COUNT 7
//
//       static const char* dp_options[DP_OPTIONS_COUNT] = {
//         "Solarized", "Dark", "Light", "Default", "Nord", "Monkai", "White",
//       };
//
//       Vector2 dp_pos = { (float)GetScreenWidth() - 130.0f, 10.f };
//       float dp_width = 120.0f;
//       float dp_height = 20.0f;
//
//       static int dp_status = 0;
//       dp_status = DK_DrawDropdown(imui,
//                                   dp_pos,
//                                   dp_width,
//                                   dp_height,
//                                   dp_options[dp_selected],
//                                   dp_options,
//                                   DP_OPTIONS_COUNT,
//                                   &dp_selected,
//                                   &dp_is_open);
//       if (dp_status) {
//         switch (dp_selected) {
//           case 0:
//             imui->theme = &DK_ImUISolarizedTheme;
//             break;
//           case 1:
//             imui->theme = &DK_ImUIDarkTheme;
//             break;
//           case 2:
//             imui->theme = &DK_ImUILightTheme;
//             break;
//           case 3:
//             imui->theme = &DK_ImUIDefaultTheme;
//             break;
//           case 4:
//             imui->theme = &DK_ImUINordTheme;
//             break;
//           case 5:
//             imui->theme = &DK_ImUIMonokaiTheme;
//             break;
//           case 6:
//             imui->theme = &DK_ImUIWhiteTheme;
//             break;
//         }
//       }
//
// #undef DP_OPTIONS_COUNT

      Rectangle DrawingTextArea = {
        0.0f, 0.0f, (float)GetScreenWidth(), console->ui.height
      };

      int scroll_step = 1;
      int min_scroll_val = 0;

      //XXTH changed to page up/down
      if (focused) {
        if (IsKeyDown(KEY_PAGE_UP)) {
          console->scroll += scroll_step;
        } else if (IsKeyDown(KEY_PAGE_DOWN)) {
          console->scroll -= scroll_step;
        }
      }

      //XXTH FIXME this does not work at all !
      if (CheckCollisionPointRec(GetMousePosition(), DrawingTextArea)) {
        if (GetMouseWheelMove() > 0) {
          console->scroll += scroll_step;
        } else if (GetMouseWheelMove() < 0) {
          console->scroll -= scroll_step;
        }
      }

      console->scroll =
        Clamp(console->scroll, min_scroll_val, console->log_index);

      static int scroll_offset = 0;
      int real_scroll = ((console->log_index - console->scroll) * (int)console->fontSpacing);

      scroll_offset = real_scroll;
      scroll_offset =
        Clamp((float)scroll_offset,
              0.0f,
              (console->log_index * console->fontSpacing) - (console->ui.height - console->fontSpacing));

      // Colors array based on log type
      Color colors[4] = {
        GRAY,              // debug
        ORANGE,            // warning
        RED,               // error
        imui->theme->text, // info
      };
      console->fontSpacing = console->fontSize + 5;

      Vector2 input_pos = { 0.0f, console->ui.height - console->fontSpacing + 1.0f };


      for (int i = 0; i < console->log_index; i++) {
        Vector2 pos = { 10, 0 - scroll_offset + (float)i * console->fontSpacing };
        if (pos.y > input_pos.y)
          break;
        //shadow=>  DrawTextEx(*imui->font, console->logs[i].text, pos + Vector2(1.f,1.f), console->fontSize, 1, DARKGRAY);
        if (console->logs[i].type == LOG_INFO) {
          DrawTextEx(*imui->font, console->logs[i].text, pos, console->fontSize, 1, colors[3]);
        } else if (console->logs[i].type == LOG_WARNING) {
          DrawTextEx(*imui->font, console->logs[i].text, pos, console->fontSize, 1, colors[1]);
        } else if (console->logs[i].type == LOG_ERROR) {
          DrawTextEx(*imui->font, console->logs[i].text, pos, console->fontSize, 1, colors[2]);
        } else if (console->logs[i].type == LOG_DEBUG) {
          DrawTextEx(*imui->font, console->logs[i].text, pos, console->fontSize, 1, colors[0]);
        } else {
          // XXTH default ...
           DrawTextEx(*imui->font, console->logs[i].text, pos, console->fontSize, 1, colors[3]);
        }
      }

      DK_DrawInputField(imui, input_pos, GetScreenWidth(), console->fontSize
        , console->ConsoleInputText, &focused, NULL, console->cursorPos);
      if (IsKeyPressed(KEY_ENTER) || IsKeyPressed(KEY_KP_ENTER)) {
        if (strlen( console->ConsoleInputText ) > 0) {
          // if (console->log_index >= console->logSize) {
          //   console->log_index = 0;
          // }

          if (callback != NULL) {
            callback( console->ConsoleInputText);
          }

          strcpy( console->ConsoleInputText, "");
        }
      }
    } //is_open
    //XXTH moved down so the open key is not pushed on the input
    if (!key_handled && IsKeyPressed(console->toggle_key)) {
      console->is_open = !console->is_open;
    }
  }

  void DK_ConsoleClear(Console* console)
  {
    console->log_index = 0;
    console->scroll = 0;
  }

  void DK_ConsoleShutdown(Console* console) {
    for (int i = 0; i < console->logSize; i++) {
      free(console->logs[i].text);
    }
    free(console->logs);
  }

#endif

#if defined(__cplusplus)
}
#endif

#endif // DK_CONSOLE_H
