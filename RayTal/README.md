# RayTal 

C++ Wrapper and Classes using RayLib.

## Demo (callback based)

![Demo Screen](./Screenshot_2026-06-04_01-34-29.png)

## RayTal Classes and Utils

    - Core
        Template Core Class. Does nothing only for inheritance.
        
    - Main 
        The Main Class is for handling: 
        - Init
        - Update
        - Render 
        - Main Loop (Execute) 
        - ShutDown
        - and some resource wrapper
        
        Two different ways to handle it:
        - Using the functional events with lambdas
        - Using inheritance as a derived class
        
    - Settings
        Parameters and Path handling
        
    - ResourceManager
        - Used for 
            - loading resources
            - get resources by fileName
            - cleanup resources on exit
        - Implemented:
            - Sounds
            - Music
            - Textures
    - Gui
        Gui handle gui elements based on raygui in a way we can get the last states like
        focused/pressed/clicked. Every Widget return a boolean usually mIsClicked.
        I only have wrapped a few Widgets so far but you also can use every raygui widget which
        respect it's size with a few extra Lines. 
        Widgets are automaticly placed on next or same line. A bit inspired by ImGui.
        
    - FileHelper
        - Create Directories
        - Webbuild FS (untested - i think)
    
    - Tools
        Tools library. 
        
  
