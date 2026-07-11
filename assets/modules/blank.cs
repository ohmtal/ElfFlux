function createBlank() {
    %obj = new ScriptObject() {
        class = "Blank";
    };

    return %obj;
}
//----------------------------------------------------------------------
function Blank::onAdd(%this) {
    return true;
}
//----------------------------------------------------------------------
function Blank::OnRemove(%this) {
}
//----------------------------------------------------------------------
function Blank::Render(%this) {
    ClearBackground(WHITE);
}

