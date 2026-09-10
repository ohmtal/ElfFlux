cmake_minimum_required(VERSION 3.24)
# -----------------------------------------------------------------------------
# Project settings:
# -----------------------------------------------------------------------------
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

option(ENABLE_IMGUI "Compile and use ImGui" ON)
# -----------------------------------------------------------------------------

project(elfFlux)
set(PROJECT_SRC
    src/main.cpp
    engine/core/ElfFluxInit.cpp

    engine/3D/ModelObject.cpp
    engine/3D/SceneContainer.cpp
    engine/3D/SceneContainer.h
    engine/3D/SceneObject.cpp
    engine/3D/SceneObject.h
    engine/3D/TerrainObject.cpp

    engine/2D/SceneContainer2D.h
    engine/2D/SceneContainer2D.cpp
    engine/2D/SceneObject2D.h
    engine/2D/SceneObject2D.cpp
    engine/2D/PrimitiveObject2D.cpp
    engine/2D/SpriteObject.cpp

    engine/grid/basicgrid.cpp
    engine/grid/basicgrid.h
    engine/grid/Grid.cpp
    engine/grid/Grid.h
    engine/grid/tBinaryHeap.h

    # engine/gui/ConsoleGuiObject.cpp
    engine/gui/Gui.cpp
    engine/gui/Gui.h

    # -- test stuff can be removed later ....
    engine/test/BatchRender.cpp
    engine/test/starfield.cpp

)

set (ASSET_PATH ${CMAKE_CURRENT_LIST_DIR}/assets@assets)
# -----------------------------------------------------------------------------
if(NOT CMAKE_BUILD_TYPE AND NOT CMAKE_CONFIGURATION_TYPES)
  set(CMAKE_BUILD_TYPE "RelWithDebInfo" CACHE STRING "Choose the type of build." FORCE)
  set_property(CACHE CMAKE_BUILD_TYPE PROPERTY STRINGS "Debug" "Release" "MinSizeRel" "RelWithDebInfo")
endif()

include(FetchContent)

# -----------------------------------------------------------------------------
# Web Configurations
if ("${PLATFORM}" STREQUAL "Web")


    add_link_options(
        "-sUSE_WEBGL2=1"
        "-sALLOW_MEMORY_GROWTH=1"
        "-sSTACK_SIZE=524288"
    )
endif()
# -----------------------------------------------------------------------------
# =============== ElfScript >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
set(ELFSCRIPT_PATH "/opt/ElfScript" CACHE PATH "Path to ElfScript")
set(ELF_MODULE ${ELFSCRIPT_PATH}/ElfScript)
set(ELF_CMAKE ${ELF_MODULE}/ElfScript.cmake)
set(RAYELF_CMAKE ${ELF_MODULE}/addons/raylib/raylib-elfscript.cmake)

if(NOT EXISTS ${ELF_CMAKE})

   set(BUILD_ELFSCRIPT OFF CACHE BOOL "" FORCE)

    FetchContent_Declare(
        elfscript
        GIT_REPOSITORY https://github.com/ohmtal/ElfScript.git
        GIT_TAG        main
    )
    FetchContent_MakeAvailable(elfscript)

  set(ELF_MODULE "${elfscript_SOURCE_DIR}/ElfScript")
  message("ELF_MODULE PATH IS:" ${ELF_MODULE})
  set(ELF_CMAKE ${ELF_MODULE}/ElfScript.cmake)
  set(RAYELF_CMAKE ${ELF_MODULE}/addons/raylib/raylib-elfscript.cmake)

endif()

if(NOT EXISTS ${ELF_CMAKE})
    message(FATAL_ERROR
        "\n ElfScript path is not set or invalid: ${ELFSCRIPT_PATH} "
        "\n Please provide the path using: "
        "\n cmake -B build -DELFSCRIPT_PATH=/path/to/ElfScript"
    )
else()

 set(RAYLIB_FETCH ON)
#  ElfFlux require 6.1-dev
 set(RAYLIB_GIT_MASTER ON)
 set(RAYELF_DEFAULTMAIN OFF)
 set(RAYELF_DKCONSOLE ON)
 set(ELF_ENABLE_CONSOLE_VECTOR ON)
 set(ELF_ENABLE_SLIMOBJECT ON)
 set(ELF_ENABLE_FIELDCACHE ON)
 set(ELF_ENABLE_DOC_EXPORTER ON)
 set(ELF_FILE_EXTENSION "elf" CACHE STRING "ElfScript: File extension")
 set(ELF_APP_NAME "ElfFlux" CACHE STRING "ElfScript: App-Name used in scripts")
 set(ELF_APP_VERSION "800" CACHE STRING "ElfScript: App-Version used in scripts")
 set(ELF_APP_VERSION_STRING "Version 0.8" CACHE STRING "ElfScript: App-Version String used in scripts")


 include(${ELF_CMAKE})
 include(${RAYELF_CMAKE})


  set (ELFSCRIPT_INCLUDES
   ${ELF_MODULE}
   ${ELF_MODULE}/addons/raylib/
  )
endif()
# <<<<<<<<<<<<<<<<<<<<<<<<<<<< ElfScript ========================


# -------- Imgui + rlImgui
if (ENABLE_IMGUI)
    set(IMGGUI_DIR "${CMAKE_CURRENT_LIST_DIR}/lib/imgui")
    set(RLIMGGUI_DIR "${CMAKE_CURRENT_LIST_DIR}/lib/rlImGui")

    set(IMGUI_SRC
        ${IMGGUI_DIR}/imgui.cpp
        ${IMGGUI_DIR}/imgui_draw.cpp
        ${IMGGUI_DIR}/imgui_widgets.cpp
        ${IMGGUI_DIR}/imgui_tables.cpp
        # also demo ?
        # ${IMGGUI_DIR}/imgui_demo.cpp
        # ------
        ${RLIMGGUI_DIR}/rlImGui.cpp
        # -----
        ${ELF_MODULE}/addons/ImGui/bindings_imgui.cpp

    )


endif()


# ------------- Main
add_executable(${PROJECT_NAME}
    ${PROJECT_SRC}
    ${ELFSCRIPT_SRC}
    ${RAYLIB_ELFSCRIPT_SRC}
    ${IMGUI_SRC}
)

target_compile_definitions(${PROJECT_NAME} PUBLIC IMGUI_DEFINE_MATH_OPERATORS)

target_include_directories(${PROJECT_NAME} PRIVATE
    ${CMAKE_CURRENT_LIST_DIR}/src/
    ${CMAKE_CURRENT_LIST_DIR}/engine/
    ${CMAKE_CURRENT_LIST_DIR}/lib/
    ${ELFSCRIPT_INCLUDES}
    ${IMGGUI_DIR}
    ${RLIMGGUI_DIR}
)

set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"

    # -- for windows:
    RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_CURRENT_SOURCE_DIR}"
    RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_CURRENT_SOURCE_DIR}"

)

target_link_libraries(${PROJECT_NAME} PRIVATE
    raylib
)

if (ENABLE_IMGUI)
  target_compile_definitions(${PROJECT_NAME} PUBLIC WITH_IMGUI)
endif()

# Windows - winVolume
if(MSVC)
    target_compile_options(${PROJECT_NAME} PRIVATE "/Zc:wchar_t-")
    target_compile_definitions(${PROJECT_NAME} PRIVATE
        NOMINMAX
        UNICODE
        _UNICODE
    )
endif()


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
