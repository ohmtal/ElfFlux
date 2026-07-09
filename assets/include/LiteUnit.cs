// LiteUnit ... using ModelObject 3D but 2D movment
// FIXME port to c
// -----------------------------------------------------------------------------
function LiteUnit::onAdd(%this) {
    %this.animWalk = %this.getAnimationIndexByName("walk");
    %this.animRun =  %this.getAnimationIndexByName("run");
    %this.animIdle = %this.getAnimationIndexByName("idle");

    //check we have run or walk

    if (%this.animRun == 0) %this.animRun = %this.animWalk;
    if (%this.animWalk == 0) %this.animWalk = %this.animRun;

    %this.moveDestination = ""; //empty!
    %this.moveTolerance = 1.5;
    %this.walkMoveSpeed = 0.08;
    %this.runMoveSpeed = 0.20;
    %this.rotationSpeed = 6.0;

    %this.terrainObject = 0;

    %this.tickObject = new ScriptTickObject() { class = "LiteUnitTicker"; obj = %this;};
}
// -----------------------------------------------------------------------------
function LiteUnit::onRemove(%this) {
    %this.tickObject.delete();
}
// -----------------------------------------------------------------------------
function LiteUnitTicker::onInterpolateTick(%this, %dt) {
    // %dt = 0.625
    // warn("LiteUnitTicker::onInterpolateTick" SPC %this SPC %obj SPC %dt);
    %this.obj.update(%dt);
}
// -----------------------------------------------------------------------------
function LiteUnit::onDestinationReached(%this) {
    warn(%this.getId() SPC "reached destination");
}
// -----------------------------------------------------------------------------
function LiteUnit::WalkTo(%this, %dest, %hurry) {
    %this.moveSpeed = %hurry ? %this.runMoveSpeed : %this.walkMoveSpeed;
    %this.AnimationIndex = %hurry ? %this.animRun : %this.animWalk;
    %this.moveDestination = %dest;
    echo("walk to" SPC %this.moveDestination);
}
// -----------------------------------------------------------------------------
function LiteUnit::update(%this, %dt) {
    // %dt = getFrameTime();
    %destVec = %this.moveDestination; // Vector3
// echo("UPDATE!" SPC %dt);
    // Check if a valid movement destination is set
    if (%destVec !$= "") {
        %currentPos = %this.Position;

        // --- Step 1: Calculate direction and distance to target ---
        %deltaX = %destVec.x - %currentPos.x;
        %deltaY = %destVec.y - %currentPos.y;
        %deltaZ = %destVec.z - %currentPos.z;

        // Calculate horizontal 2D distance for movement logic
        %distance = mSqrt((%deltaX * %deltaX) + (%deltaZ * %deltaZ));

        // Use custom tolerance value or default to 0.1 units
        %tolerance = (%this.moveTolerance !$= "") ? %this.moveTolerance : 1.0;

        // If we are outside the tolerance range, rotate and move
        if (%distance > %tolerance) {

            // --- Step 2: Rotation logic (Raylib standard: Y is Up-Axis) ---
            // Calculate target angle around the Y-axis using horizontal plane (X and Z)
            %targetRadians = mAtan(%deltaX, %deltaZ);
            %targetDegrees = mRadToDeg(%targetRadians);

            // Get current Y-rotation
            %currentRotY = %this.Rotation.y;

            // Normalize angle difference to the shortest path (-180 to 180)
            %angleDiff = %targetDegrees - %currentRotY;
            while (%angleDiff > 180)  { %angleDiff -= 360; }
            while (%angleDiff < -180) { %angleDiff += 360; }

            // Define rotation speed (degrees per second), fallback to 180 if not set
            %rotSpeed = (%this.rotationSpeed !$= "") ? %this.rotationSpeed : 180.0;
            %maxRotation = %rotSpeed * %dt;

            // Clamp rotation step to the maximum allowed speed
            if (mAbs(%angleDiff) > %maxRotation) {
                if (%angleDiff > 0) {
                    %currentRotY += %maxRotation;
                } else {
                    %currentRotY -= %maxRotation;
                }
            } else {
                %currentRotY = %targetDegrees; // Snap to target angle if close enough
            }

            // Apply updated Y-rotation back to the object
            %this.Rotation = %this.Rotation.x SPC %currentRotY SPC %this.Rotation.z;

            // --- Step 3: Movement logic ---
            // Define movement speed (units per second), fallback to 5.0 if not set
            %moveSpeed = (%this.moveSpeed !$= "") ? %this.moveSpeed : 0.5 ;// 5.0;
            %moveStep = %moveSpeed * %dt;

            // Prevent overshooting the destination
            if (%moveStep > %distance) {
                %moveStep = %distance;
            }

            // Calculate normalized movement vector coordinates on the horizontal plane
            %moveX = (%deltaX / %distance) * %moveStep;
            %moveZ = (%deltaZ / %distance) * %moveStep;

            // Apply movement to position (keeping height/Y stable or adapting to terrain)

            %newPos = (%currentPos.x + %moveX) SPC %currentPos.y SPC (%currentPos.z + %moveZ);

            if (%this.terrainObject != 0) {
                %terrH = %this.terrainObject.getHeight(%newPos);
                %newPos.y = %terrH;
            }
            %this.Position = %newPos;

            warn("new pos" SPC %newPos);
        } else {
            //no snap!  %this.Position = %destVec;
            %this.moveDestination = "";
            %this.AnimationIndex = %this.animIdle;
            %this.onDestinationReached();
        }
    }
}

// -----------------------------------------------------------------------------
function LiteUnit::lookAt(%this, %position) {
    // Get the current 3D position vector
    %currentPos = %this.Position;

    // Calculate the direction vector on the horizontal 2D plane (X and Y coordinates)
    %deltaX = %position.x - %currentPos.x;
    %deltaY = %position.y - %currentPos.y;

    // Fallback if the target position matches the current position exactly
    if (%deltaX == 0 && %deltaY == 0) {
        return;
    }

    // Calculate the 2D angle in radians using atan2 (Y, X)
    %radians = mAtan(%deltaY, %deltaX);

    // Convert the angle from radians to degrees
    %degrees = mRadToDeg(%radians);

    // Get the current rotation vector to preserve the other axes
    %currentRot = %this.Rotation;

    // In raylib, the standard Up-axis is the Y-axis.
    // Therefore, rotating a 3D object on a flat 2D plane means rotating around its Y-axis (.y).
    %this.Rotation = %currentRot.x SPC %degrees SPC %currentRot.z;
}

// -----------------------------------------------------------------------------
