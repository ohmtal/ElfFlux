#!/usr/bin/env python3
import sys
import os
import re

def transpile_cpp_to_torquescript(cpp_code, class_name="DemoObject"):
    # 1. Remove C++ specific includes, main function wrapper, and window setup
    ts = cpp_code
    ts = re.sub(r'#include.*?\n', '', ts)
    ts = re.sub(r'int main\s*\(.*?\)\s*\{', '', ts)
    ts = re.sub(r'InitWindow\(.*?\);', '', ts)
    ts = re.sub(r'SetTargetFPS\(.*?\);', '', ts)
    ts = re.sub(r'CloseWindow\(\);', '', ts)
    ts = re.sub(r'return\s+0\s*;', '', ts)

    # 2. Extract variable declarations
    static_fields = []
    dynamic_inits = []

    var_patterns = [
        r'(Vector2)\s+(\w+)\s*=\s*(.*?);',
        r'(Vector3)\s+(\w+)\s*=\s*(.*?);',
        r'(float|double)\s+(\w+)\s*=\s*(.*?);',
        r'(int)\s+(\w+)\s*=\s*(.*?);',
        r'(Color)\s+(\w+)\s*=\s*(.*?);',
        r'(Shader)\s+(\w+)\s*=\s*(.*?);'
    ]

    # Track all found variable names to identify dynamic dependencies
    known_vars = set()
    matches_to_process = []

    for pattern in var_patterns:
        for match in re.finditer(pattern, ts):
            c_type, name, value = match.groups()
            known_vars.add(name)
            matches_to_process.append((c_type, name, value))
        ts = re.sub(pattern, '', ts)

    for c_type, name, value in matches_to_process:
        ts_type = "TypeF32" if c_type in ["float", "double"] else "TypeS32" if c_type == "int" else f"Type{c_type}"

        # Clean float 'f' suffixes (e.g., 2.0f -> 2.0)
        value = re.sub(r'\b([0-9.]+)[fF]\b', r'\1', value)

        # Clean C++ Vector/Color braces (e.g. { 10, 20 } -> "10 20")
        value_clean = re.sub(r'[\{\}\(\)]', '', value).replace(',', '').strip()

        # Determine if the value is a static constant (pure number, string, or basic vector)
        is_static = True
        # If it contains text that isn't a known constant, or math symbols combined with names
        if "(" in value or ")" in value or any(v in value for v in known_vars):
            is_static = False

        if is_static:
            if ts_type in ["TypeVector2", "TypeVector3", "TypeColor"] and '"' not in value_clean:
                value_clean = f'"{value_clean}"'
            static_fields.append(f'        {ts_type} {name} = {value_clean};')
        else:
            # It's dynamic! Put a safe default in the constructor and move calculation to Init()
            default_val = '"0 0"' if "Vector" in ts_type else "0"
            static_fields.append(f'        {ts_type} {name} = {default_val};')

            # Translate common raylib syntax patterns in the init value
            init_val = value.strip()
            init_val = re.sub(r'\b([0-9.]+)[fF]\b', r'\1', init_val)
            # Add missing parentheses to functions if they were stripped
            init_val = re.sub(r'\b(GetMousePosition|GetFrameTime|GetScreenWidth|GetScreenHeight)\b(?!\()', r'\1()', init_val)
            # Fix shader pointer style spacing
            init_val = re.sub(r'GetShaderLocation\s*\(?\s*shader\s*,\s*', 'GetShaderLocation($shader, ', init_val)

            dynamic_inits.append(f'    %this.{name} = {init_val};')

    # 3. Separate the update loop block from the render block
    render_body = ""
    render_match = re.search(r'BeginDrawing\(\);(.*?)EndDrawing\(\);', ts, re.DOTALL)
    if render_match:
        render_body = render_match.group(1)
        ts = ts.replace(render_match.group(0), '')

    update_body = ""
    while_match = re.search(r'while\s*\(!WindowShouldClose\(\)\)\s*\{(.*?)\}', ts, re.DOTALL)
    if while_match:
        update_body = while_match.group(1)

    # 4. Syntax conversion (Math functions and %this. variable scoping)
    def clean_syntax(code):
        c = code
        c = c.replace("atan2(", "mAtan(").replace("cos(", "mCos(").replace("sin(", "mSin(")
        c = c.replace("atan(", "mAtan(")
        c = re.sub(r'\b([0-9.]+)[fF]\b', r'\1', c)

        # Add missing parens to common functions in code bodies
        c = re.sub(r'\b(GetMousePosition|GetFrameTime|GetScreenWidth|GetScreenHeight)\b(?!\()', r'\1()', c)

        # Scope variables to %this
        for name in known_vars:
            c = re.sub(r'\b' + name + r'\b', f'%this.{name}', c)

        c = re.sub(r'const\s+\w+\s+', '', c)
        return c.strip()

    update_clean = clean_syntax(update_body)
    render_clean = clean_syntax(render_body)

    # 5. Assemble the modular TorqueScript object structure
    output = f"function create{class_name}() {{\n"
    output += f"    %obj = new ScriptObject() {{\n"
    output += f"        class = \"{class_name}\";\n"
    output += "\n".join(static_fields) + "\n"
    output += f"    }};\n    return %obj;\n}}\n\n"

    output += f"function {class_name}::Init(%this) {{\n"
    if dynamic_inits:
        output += "\n".join(dynamic_inits) + "\n"
    output += f"    return true;\n}}\n\n"

    output += f"function {class_name}::Update(%this) {{\n"
    output += "    " + update_clean.replace("\n", "\n    ") + "\n}\n\n"

    output += f"function {class_name}::Render(%this) {{\n"
    output += "    " + render_clean.replace("\n", "\n    ") + "\n}"

    return output

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Error: No C++ filename provided!", file=sys.stderr)
        print("Usage: python3 cc2ts.py <path_to_raylib_demo.c>", file=sys.stderr)
        sys.exit(1)

    file_path = sys.argv[1]
    if not os.path.exists(file_path):
        print(f"Error: File '{file_path}' not found!", file=sys.stderr)
        sys.exit(1)

    base_name = os.path.splitext(os.path.basename(file_path))[0]
    class_name = "".join([word.capitalize() for word in base_name.split("_")])

    with open(file_path, "r", encoding="utf-8") as f:
        cpp_content = f.read()

    print(transpile_cpp_to_torquescript(cpp_content, class_name))
