#!bin/sh
cmake -S . -B web_debug \
    -DCMAKE_TOOLCHAIN_FILE=/usr/lib/emscripten/cmake/Modules/Platform/Emscripten.cmake \
    -DCMAKE_BUILD_TYPE=Debug -DPLATFORM=Web
cmake --build web_debug -j 24
