function createSun(%terrainRadiusNeg, %sunModel) {

    %sun = new ScriptObject() {
        class = "Sun";
        sunDirection = "-0.5 -1.0 -0.2";
        model = %sunModel;
        terrainRadiusNeg = %terrainRadiusNeg;

        sunspeed = 5.0;
    };
    return %sun;
}
//----------------------------------------------------------------------
function Sun::onAdd(%this) {

    // ---- Sun shader
    //

    %this.sunShader = LoadShader(
        "assets/shaders/custom/sun.vert",
        "assets/shaders/custom/sun.frag"
    );
    if (%this.sunShader == 0) return false;
    echo("** Loader shaderId:" SPC %this.sunShader);
    %this.dirLoc   = GetShaderLocation(%this.sunShader, "sunDirection");
    %this.colorLoc = GetShaderLocation(%this.sunShader, "sunColor");

    SetShaderValue(%this.sunShader, %this.dirLoc, Vector3Normalize(%this.sunDirection), 2);
    SetShaderValue(%this.sunShader, %this.colorLoc, "1.0 0.95 0.85", 3); //normalized

    // ---- Visual Sun
    // scale against terrain radius
    %scale = mAbs(%this.terrainRadiusNeg) / 1024 * 3;
    %this.visualSun = new ModelObject() {
        Position = "0 0 0";
        ModelId = %this.model;
        Scale = %scale SPC %scale SPC %scale;
    };

    // ---- Day night
    %this.sunTime = 0.0; // 0.0 .. 360.0



    return true;
}
//----------------------------------------------------------------------
function Sun::OnRemove(%this) {
    %this.visualSun.delete();
    UnloadShader(%this.sunShader);
}
//----------------------------------------------------------------------
function Sun::update(%this, %dt)
{
    %this.sunTime += %dt * %this.sunSpeed;
    if (%this.sunTime > 360.0) %this.sunTime -= 360.0;

    %rad = %this.sunTime * 0.0174532925; // DEG2RAD

    %sunX = mCos(%rad);
    %sunY = mSin(%rad);
    %sunZ = -0.2;

    %lightDirection = %sunX SPC (-%sunY) SPC %sunZ;
    %this.sunDirection = Vector3Normalize(%lightDirection);

    SetShaderValue(%this.sunShader, %this.dirLoc, %this.sunDirection, 2);

    %camPos = %this.camera.position;
    %sunOffset = Vector3Scale(%this.sunDirection, %this.terrainRadiusNeg);
    %this.visualSun.position = Vector3Add(%camPos, %sunOffset);

    if (%sunY > 0.0)
    {
        %blend = %sunY;
        if (%blend > 1.0) %blend = 1.0;

        %r = mClamp((1.0 - %blend) * 200 + %blend * 60, 0, 255);
        %g = mClamp((1.0 - %blend) * 120 + %blend * 140, 0, 255);
        %b = mClamp((1.0 - %blend) * 80 + %blend * 230, 0, 255);

        %this.skyColor = %r SPC %g SPC %b;

        SetShaderValue(%this.sunShader, %this.colorLoc, "1.0 0.95 0.85", 3);
    }
    else
    {
        %blend = mAbs(%sunY) * 4.0;
        if (%blend > 1.0) %blend = 1.0;

        %r = mClamp((1.0 - %blend) * 60, 0, 255);
        %g = mClamp((1.0 - %blend) * 140 + %blend * 10, 0, 255);
        %b = mClamp((1.0 - %blend) * 230 + %blend * 25, 0, 255);

        %this.skyColor = %r SPC %g SPC %b;

        SetShaderValue(%this.sunShader, %this.colorLoc, "0.1 0.12 0.2", 3);
    }
}

