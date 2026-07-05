function createBlank() {
    %obj = new ScriptObject() {
        class = "Blank";
    };

    return %obj;
}
//----------------------------------------------------------------------
function Blank::onAdd(%this) {
    SetTargetFPS(60); //FPS TEST only calling module
    return true;
}
//----------------------------------------------------------------------
function Blank::OnRemove(%this) {
    SetTargetFPS(60);
}
//----------------------------------------------------------------------
function Blank::Render(%this) {
    ClearBackground(WHITE);
}

