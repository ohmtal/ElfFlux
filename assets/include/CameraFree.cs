
//----------------------------------------------------------------------
function CameraFree::update(%this, %dt)
{
    %consoleOpen = $main.consoleGui.isOpen(); //hackfest

    // FIXME IsCursorHidden does NOT work on webbuild
    if (IsMouseButtonPressed(MOUSE_BUTTON_RIGHT))  {
        %this.skipCounter = 5;
        DisableCursor();
    } else if (IsMouseButtonReleased(MOUSE_BUTTON_RIGHT)) EnableCursor();

    %speed = isKeyDown(KEY_LEFT_SHIFT) ? %this.moveSpeed * 2.0 : %this.moveSpeed;
    if (%speed == 0) %this.moveSpeed = 100.0;

    if (!%consoleOpen) {
        if (isKeyDown(KEY_W)) %this.moveForward(%dt * %speed, false);
        if (isKeyDown(KEY_S)) %this.moveForward(-%dt * %speed, false);
        if (isKeyDown(KEY_D)) %this.moveRight(%dt * %speed, false);
        if (isKeyDown(KEY_A)) %this.moveRight(-%dt * %speed, false);
        if (isKeyDown(KEY_E)) %this.moveUp(%dt * %speed);
        if (isKeyDown(KEY_Q)) %this.moveUp(-%dt * %speed);
    }


    // Web build does not like this : if (IsCursorHidden())
    // FIXME Web: jumps around on sometimes
    if (IsMouseButtonDown(MOUSE_BUTTON_RIGHT))
    {

        // === SMOOTH MOUSE ROTATION ===
        if (%this.skipCounter > 0) { //web build need this
            warn("CameraFree: SKIP MouseDelta .... for " SPC %this.skipCounter SPC "more times.");
            %deltaX = %deltaY = 0;
            %this.skipCounter--;
        } else {
            %mouseDelta = GetMouseDelta();
            %deltaX = %mouseDelta.x;
            %deltaY = %mouseDelta.y;
        }




        %sensitivity = 0.0015;

        %panSpeed = %this.panSpeed;
         if (%panSpeed == 0) %this.panSpeed = 30.0;
        %this.targetYaw   = -%deltaX * %panSpeed * %sensitivity;
        %this.targetPitch = -%deltaY * %panSpeed * %sensitivity;

        %smoothFactor = 15.0 * %dt;
        if (%smoothFactor > 1.0) %smoothFactor = 1.0; // Absicherung

        if (%this.targetYaw != 0 || %this.targetPitch != 0) {

            %currentYaw = %this.targetYaw * %smoothFactor;
            %currentPitch = %this.targetPitch * %smoothFactor;

            %this.yaw(%currentYaw, false);
            %this.pitch(%currentPitch, true, false, false);

            %this.targetYaw   -= %currentYaw;
            %this.targetPitch -= %currentPitch;
        }
    }
}
