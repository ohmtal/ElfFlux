cmake_minimum_required(VERSION 3.24)
# -----------------------------------------------------------------------------
# Project settings:

project(RayTalDemo)
set(PROJECT_SRC
    # src/main_class.cpp
    src/main_callbacks.cpp
)

set (ASSET_PATH ${CMAKE_CURRENT_LIST_DIR}/assets@assets)
# -----------------------------------------------------------------------------
if(NOT CMAKE_BUILD_TYPE AND NOT CMAKE_CONFIGURATION_TYPES)
  set(CMAKE_BUILD_TYPE "Debug" CACHE STRING "Choose the type of build." FORCE)
  set_property(CACHE CMAKE_BUILD_TYPE PROPERTY STRINGS "Debug" "Release" "MinSizeRel" "RelWithDebInfo")
endif()
# -----------------------------------------------------------------------------


# WEB Example compile (arch)
	# cmake -S . -B web_debug \
	# 	-DCMAKE_TOOLCHAIN_FILE=/usr/lib/emscripten/cmake/Modules/Platform/Emscripten.cmake \
	# 	-DCMAKE_BUILD_TYPE=Debug \
	# 	-DPLATFORM=Web
	# cmake --build web_debug -j 24
# Web Run: (bash)
#  source /etc/profile.d/emscripten.sh
#  emrun RayTalDemo.html

# -----------------------------------------------------------------------------
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# include(FetchContent)
# ------------- RayTal
set(RAYTAL_DIR "${CMAKE_CURRENT_LIST_DIR}/../" CACHE PATH "Path to RayTal")

#  add RAYTAL_SRC
include("${RAYTAL_DIR}/RayTal.cmake")


# ------------- RayLib
# set(PLATFORM "SDL" CACHE STRING "raylib platform" FORCE)
set(RAYLIB_DIR "${RAYTAL_DIR}/lib/raylib" CACHE PATH "Path to RayLib")
message( STATUS "\n Set Raylib to: " ${RAYLIB_DIR} )

if(NOT EXISTS "${RAYLIB_DIR}/CMakeLists.txt")
  message(FATAL_ERROR
    "\n" ${RAYLIB_DIR}
    "\n ********** RayLib not found! *************"
  )
else()

  add_subdirectory("${RAYLIB_DIR}" "${CMAKE_BINARY_DIR}/raylib_build")

  # target_compile_definitions( raylib PUBLIC PLATFORM_DESKTOP_SDL)

endif()



# ------------- Main
add_executable(${PROJECT_NAME}
    ${RAYTAL_SRC}
    ${PROJECT_SRC}

)

target_include_directories(${PROJECT_NAME} PRIVATE
    ${RAYTAL_DIR}
    ${RAYLIB_DIR}/src
    ${RAYTAL_DIR}/lib/rayspice/
)


target_compile_definitions(${PROJECT_NAME} PUBLIC IMGUI_DEFINE_MATH_OPERATORS)



set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
)

target_link_libraries(${PROJECT_NAME} PRIVATE
    raylib
)


# Web Configurations
if ("${PLATFORM}" STREQUAL "Web")

    set_target_properties(${PROJECT_NAME} PROPERTIES SUFFIX ".html")
    target_link_options(${PROJECT_NAME} PUBLIC -sUSE_GLFW=3 PUBLIC --preload-file ${ASSET_PATH} PRIVATE "-lidbfs.js")


    # ---- Add minshell
    set(TARGET_NAME ${PROJECT_NAME})
    set(CUSTOM_SHELL_FILE "${RAYLIB_DIR}/src/minshell.html")
    if(EXISTS "${CUSTOM_SHELL_FILE}")
        target_link_options(${TARGET_NAME} PRIVATE "--shell-file" "${CUSTOM_SHELL_FILE}")
        set_target_properties(${TARGET_NAME} PROPERTIES LINK_DEPENDS "${CUSTOM_SHELL_FILE}")
    endif()

endif()
