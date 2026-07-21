/*
 * Demo with class
 */

#include "RayTal/Main.h"
#include "RayTal/Gui.h"

#include "Eyes.h"
#include "Lights.h"

#include <string>
#include <math.h>
//------------------------------------------------------------------------------
using namespace RayTal::Tools;

//------------------------------------------------------------------------------
namespace RayTal {

class DemoMain: public Main {
    typedef Main Parent ;

    Texture2D* logoTex;


    Demo::Lights mLights;
    Demo::Eyes mEyes;

    // GUI
    RayTal::Gui gui;
    std::string mConfPathText = "";
    std::string mPrefPathText = "";

public:
    //----------------------------------------------------------------------
    bool Init() override{

        if (!Parent::Init()) return false;

        // Music
        playMusic("fullhouse2026.fms.mp3", 0.75f);

        // Texture
        logoTex = getResourceManager()->getTexture("raylib_32x32.png");


        mLights.Init();
        mEyes.Init();

        //-------
        mConfPathText = TextFormat("Base Path: %s", getBasePath().c_str());
        mPrefPathText = TextFormat("Config Path: %s", getSettings()->getPrefsPath().c_str());


        return true;
    }
private:
    void ShutDown() override {
        mLights.ShutDown();
        Parent::ShutDown();
    }

    void Update(F32 dt) override {
        Parent::Update(dt);

        mLights.Update(dt);
        mEyes.Update(dt);
    }
    // -------------------------------------------------------------------------
    // this override Render without the Main wrapper Begin/End
    void Render() override{
        BeginDrawing();
        ClearBackground(COLOR_SLATEGRAY);


        mLights.Render3D();
        mEyes.Render();



        // logo
        gui.Begin(10,10);
        if (logoTex) DrawTexture(*logoTex, gui.getX(), gui.getY(), WHITE);
        gui.SameLine(40.f);
        gui.Write("RayTal Class Demo", 40, GOLD);
        gui.Write(TextFormat("FPS: %d FT:%f", GetFPS(), GetFrameTime()),20,  RED);

        mLights.RenderGui(gui);


         EndDrawing();
    }

}; //DEMO
} //namespace
//------------------------------------------------------------------------------

int main(void)
{
    RayTal::DemoMain app;
    app.getSettings()->WindowMaximized = true;
    app.getSettings()->IconFilename = "texture:/raylib_32x32.png";

    if (!app.Init()) {
        TraceLog(LOG_ERROR, "%s", "Failed to Initialize!");
        return 1;
    }

    app.Execute();

    return 0;
}
