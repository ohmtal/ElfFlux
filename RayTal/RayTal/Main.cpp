#include "Main.h"
#include "ResourceManager.h"

#if defined(PLATFORM_WEB)
#include <emscripten/emscripten.h>

//-------------------------------------------------------------------------------
// webGL main loop:
//-------------------------------------------------------------------------------
// This wrapper bridges the C-style callback to your C++ class instance
void emscripten_loop_wrapper(void* arg)
{
    RayTal::Main* app = static_cast<RayTal::Main*>(arg);
    app->IterateFrame();
}
#endif



namespace RayTal {

    const F32 gFixedStep = 1.0f / 60.0f;
    static F32 gAccumulator = 0.0f;

    //--------------------------------------------------------------------------
    bool Main::Init() {
        int windowFlags = 0;
        if (mSettings.windowFlagsOverwrite) windowFlags = mSettings.windowFlagsOverwrite;
        else {
            if (mSettings.FullScreen) windowFlags |=  FLAG_FULLSCREEN_MODE;
            else {
                windowFlags |= FLAG_WINDOW_RESIZABLE;
                if (mSettings.WindowMaximized) windowFlags |= FLAG_WINDOW_MAXIMIZED;
            }
            if (mSettings.EnableVSync) windowFlags |= FLAG_VSYNC_HINT;
            if (mSettings.EnableFSAA) windowFlags |= FLAG_MSAA_4X_HINT;
        }

        SetConfigFlags(windowFlags);
        if (mSettings.FpsLimit) SetTargetFPS(mSettings.FpsLimit);
        InitWindow(mSettings.ScreenWidth, mSettings.ScreenHeight, mSettings.Caption.c_str());
        if (!IsWindowReady()) return false;

        setFullPath( getSettings()->IconFilename );

        Image imgLogo = LoadImage( getSettings()->IconFilename.c_str() );
        SetWindowIcon(imgLogo);
        // can i unload ?  - seams so
        UnloadImage(imgLogo);

        if (!mResourceManager.initialize(this)) {
            return false;
        }

        if (!getSettings()->DisableSound) {
            InitAudioDevice();
        }

        if (OnInit) {
            if (!OnInit())
                return false;
        }


        return true;
    }
    //--------------------------------------------------------------------------
    void Main::ShutDown() {
        for (auto& obj: mCoreObjects) {
            obj->ShutDown();
        }
        getResourceManager()->shutDown();
    }
    //--------------------------------------------------------------------------
    void Main::Update(F32 dt) {
        if (OnUpdate) OnUpdate(dt);
        getResourceManager()->Update();

        for (auto& obj: mCoreObjects) {
            obj->Update(dt);
        }
    }
    //--------------------------------------------------------------------------
    void Main::Render() {
        BeginDrawing();
        ClearBackground(COLOR_SLATEGRAY);
        if (OnRender) OnRender();
        for (auto& obj: mCoreObjects) {
            obj->Render();
        }
        EndDrawing();
    }
    //--------------------------------------------------------------------------
    void Main::IterateFrame() {

            gAccumulator += GetFrameTime();
            while (gAccumulator >= gFixedStep) {

                this->Update(gFixedStep);
                gAccumulator -= gFixedStep;
            }

            this->Render();
    }
    void Main::Execute() {


    #if defined(PLATFORM_WEB)
        // emscripten_set_main_loop(UpdateDrawFrame, 60, 1);
        emscripten_set_main_loop_arg(emscripten_loop_wrapper, this, 0, 1);
        emscripten_set_main_loop_timing(EM_TIMING_RAF, 1); //force RAF

    #else
            while (!WindowShouldClose())
            {
                IterateFrame();
            }
    #endif


        if (OnShutDown) OnShutDown();
        ShutDown();
        CloseWindow();
    }
    //--------------------------------------------------------------------------
    bool Main::playMusic(const char* musicFileName, F32 volume){
        Music* music = getResourceManager()->getMusic(musicFileName);
        if (!music) return false;
        SetMusicVolume(*music, volume);
        PlayMusicStream(*music);
        return true;
    }
    bool Main::stopMusic(const char * musicFileName) {
        Music* music = getResourceManager()->getMusic(musicFileName);
        if (!music) return false;
        StopMusicStream(*music);
        return true;
    }
    //--------------------------------------------------------------------------
    bool Main::playSound(const char* soundFileName){
        Sound* sound = getResourceManager()->getSound(soundFileName);
        if (!sound) return false;
        PlaySound(*sound);
        return true;
    }
} // namespace

