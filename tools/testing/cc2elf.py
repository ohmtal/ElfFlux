#!/usr/bin/env python3

# NOTE: this is not meant to do ALL the work for you it help to get started.

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

    # 2. Extract variable declarations to populate the constructor fields
    fields = []
    var_patterns = [
        r'(Vector2)\s+(\w+)\s*=\s*(.*?);',
        r'(Vector3)\s+(\w+)\s*=\s*(.*?);',
        r'(float|double)\s+(\w+)\s*=\s*(.*?);',
        r'(int)\s+(\w+)\s*=\s*(.*?);',
        r'(Color)\s+(\w+)\s*=\s*(.*?);'
    ]

    for pattern in var_patterns:
        for match in re.finditer(pattern, ts):
            c_type, name, value = match.groups()
            ts_type = "TypeF32" if c_type in ["float", "double"] else "TypeS32" if c_type == "int" else f"Type{c_type}"

            # Clean C++ Vector/Color initialization braces/parentheses (e.g. { 10, 20 } -> "10 20")
            value = re.sub(r'[\{\}\(\)]', '', value).replace(',', '')
            if ts_type in ["TypeVector2", "TypeVector3", "TypeColor"] and '"' not in value:
                value = f'"{value.strip()}"'
            fields.append(f'        {ts_type} {name} = {value};')

        ts = re.sub(pattern, '', ts)

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

        # Scope the extracted local fields to %this inside the class methods
        for f in fields:
            f_name = f.split()[1]
            c = re.sub(r'\b' + f_name + r'\b', f'%this.{f_name}', c)

        c = re.sub(r'const\s+\w+\s+', '', c) # Remove C++ const qualifiers
        return c.strip()

    update_clean = clean_syntax(update_body)
    render_clean = clean_syntax(render_body)

    # 5. Assemble the modular TorqueScript object structure for the demo manager
    output = f"function create{class_name}() {{\n"
    output += f"    %obj = new ScriptObject() {{\n"
    output += f"        class = \"{class_name}\";\n"
    output += "\n".join(fields) + "\n"
    output += f"    }};\n    return %obj;\n}}\n\n"

    output += f"function {class_name}::Init(%this) {{\n"
    output += f"    return true;\n}}\n\n"

    output += f"function {class_name}::Update(%this) {{\n"
    output += "    " + update_clean.replace("\n", "\n    ") + "\n}\n\n"

    output += f"function {class_name}::Render(%this) {{\n"
    output += "    " + render_clean.replace("\n", "\n    ") + "\n}"

    return output

if __name__ == "__main__":
    # Ensure a file path argument was passed
    if len(sys.argv) < 2:
        print("Error: No C++ filename provided!", file=sys.stderr)
        print("Usage: python3 cc2elf.py <path_to_raylib_demo.c>", file=sys.stderr)
        sys.exit(1)

    file_path = sys.argv[1]

    if not os.path.exists(file_path):
        print(f"Error: File '{file_path}' not found!", file=sys.stderr)
        sys.exit(1)

    # Derive the class name from the filename in CamelCase style (e.g. core_input_eyes -> CoreInputEyes)
    base_name = os.path.splitext(os.path.basename(file_path))[0]
    class_name = "".join([word.capitalize() for word in base_name.split("_")])

    with open(file_path, "r", encoding="utf-8") as f:
        cpp_content = f.read()

    # Transpile the code and write directly to standard output
    ts_code = transpile_cpp_to_torquescript(cpp_content, class_name)
    print(ts_code)
