# Initial translator from c++ to TorqueScript object
import re

def transpile_cpp_to_torquescript(cpp_code, class_name="DemoObject"):
    # cleanup  C++
    ts = cpp_code
    ts = re.sub(r'#include.*?\n', '', ts)
    ts = re.sub(r'int main\s*\(.*?\)\s*\{', '', ts)
    ts = re.sub(r'InitWindow\(.*?\);', '', ts)
    ts = re.sub(r'SetTargetFPS\(.*?\);', '', ts)
    ts = re.sub(r'CloseWindow\(\);', '', ts)
    ts = re.sub(r'return\s+0\s*;', '', ts)

    # variables for  createObject
    fields = []
    # looking for types
    var_patterns = [
        r'(Vector2)\s+(\w+)\s*=\s*(.*?);',
        r'(float|double)\s+(\w+)\s*=\s*(.*?);',
        r'(int)\s+(\w+)\s*=\s*(.*?);',
        r'(Color)\s+(\w+)\s*=\s*(.*?);'
    ]

    # isolate local variables
    for pattern in var_patterns:
        for match in re.finditer(pattern, ts):
            c_type, name, value = match.groups()
            # Mappe C++ Typen auf deine Torque-Konsolentypen
            ts_type = "TypeF32" if c_type in ["float", "double"] else "TypeS32" if c_type == "int" else f"Type{c_type}"
            # Vektoren konvertieren von {x, y} oder Vector2(x, y) zu "x y"
            value = re.sub(r'[\{\}\(\)]', '', value).replace(',', '')
            if ts_type in ["TypeVector2", "TypeVector3", "TypeColor"] and '"' not in value:
                value = f'"{value.strip()}"'
            fields.append(f'        {ts_type} {name} = {value};')

        # Entferne die Deklarationen aus dem restlichen Code-Body
        ts = re.sub(pattern, '', ts)

    # isolate
    render_body = ""
    render_match = re.search(r'BeginDrawing\(\);(.*?)EndDrawing\(\);', ts, re.DOTALL)
    if render_match:
        render_body = render_match.group(1)
        ts = ts.replace(render_match.group(0), '')

    # Der verbleibende Code in der while-Schleife ist das Update
    update_body = ""
    while_match = re.search(r'while\s*\(!WindowShouldClose\(\)\)\s*\{(.*?)\}', ts, re.DOTALL)
    if while_match:
        update_body = while_match.group(1)


    def clean_syntax(code):
        c = code
        # math renameing
        c = c.replace("atan2(", "mAtan(").replace("cos(", "mCos(").replace("sin(", "mSin(")
        c = c.replace("atan(", "mAtan(")
        # adding %this
        for f in fields:
            f_name = f.split()[1]
            c = re.sub(r'\b' + f_name + r'\b', f'%this.{f_name}', c)

        # (using Swizzling for positions)
        c = re.sub(r'const\s+\w+\s+', '', c) # remove const
        return c.strip()

    update_clean = clean_syntax(update_body)
    render_clean = clean_syntax(render_body)

    # put them together
    output = f"function create{class_name}() {{\n"
    output += f"    %obj = new ScriptObject() {{\n"
    output += f"        class = \"{class_name}\";\n"
    output += "\n".join(fields) + "\n"
    output += f"    }};\n    return %obj;\n}}\n\n"

    output += f"function {class_name}::Init(%this) {{\n"
    output += f"    return true;\n}}\n\n"

    output += f"function {class_name}::Update(%this) {{\n"
    # ident
    output += "    " + update_clean.replace("\n", "\n    ") + "\n}\n\n"

    output += f"function {class_name}::Render(%this) {{\n"
    output += "    " + render_clean.replace("\n", "\n    ") + "\n}"

    return output
