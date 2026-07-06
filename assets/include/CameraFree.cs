
//----------------------------------------------------------------------
function CameraFree::update(%this, %dt)
{
    if (IsMouseButtonPressed(MOUSE_BUTTON_RIGHT))  DisableCursor();
    else if (IsMouseButtonReleased(MOUSE_BUTTON_RIGHT)) EnableCursor();

    %speed = isKeyDown(KEY_LEFT_SHIFT) ? %this.moveSpeed * 2.0 : %this.moveSpeed;
    if (%speed == 0) %this.moveSpeed = 100.0;

    if (isKeyDown(KEY_W)) %this.moveForward(%dt * %speed, false);
    if (isKeyDown(KEY_S)) %this.moveForward(-%dt * %speed, false);
    if (isKeyDown(KEY_D)) %this.moveRight(%dt * %speed, false);
    if (isKeyDown(KEY_A)) %this.moveRight(-%dt * %speed, false);
    if (isKeyDown(KEY_E)) %this.moveUp(%dt * %speed);
    if (isKeyDown(KEY_Q)) %this.moveUp(-%dt * %speed);

    if (IsCursorHidden())
    {

        // === SMOOTH MOUSE ROTATION ===
        %mouseDelta = GetMouseDelta();
        %deltaX = %mouseDelta.x;
        %deltaY = %mouseDelta.y;

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
