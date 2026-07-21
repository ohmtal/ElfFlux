- [ ] Own key/value saver: Separator is always the problem so i do 
  	* KEY(string);VALUE(BASE64 STRING)\n
	- [ ] port base64 from OhmFlux
        - [ ] write SettingsStorage class
	- [ ] port back to Ohmflux ! 

- [ ] Web: 
	* Uncaught RuntimeError: Aborted('requestFullscreen' was not exported. add it to EXPORTED_RUNTIME_METHODS (see the Emscripten FAQ))	
	
```

// ---------- TODO ---------------
/*
 * [X] OnRender
 * [X] OnShutDown
 * [X] OnUpdate
 * [ ] OnEvent :not really a eventsystem implemented ?! << more research on this
 *
 * [X] OnUpdate add fixed call with deltaTime
 *
 * [X] Manager: Sound, Musik and Texture
 *
 * [ ] Like BaseFlux :
 *      [X] setFullPath
 *      [X] bool setWindowIcon
 *      [X] Init << bool InitSDL();
 *      [ ] void shutDown();
 *      [X] bool Execute();
 *      [ ] void TerminateApplication(void);
 *
 *      //----------------------------------------------------------------------
 *      [ ] research how sound objects are handled
 *          NOTE: too bad:  sound and music is handled different ...
 *              => music.looping = true; << use this for one interface !?
 *                 but i guess it's better to handle it different
 *          InitAudioDevice();      // Initialize audio device
 *          Sound fxWav = LoadSound("resources/sound.wav");         // Load WAV audio file
 *          // LoadSoundAlias(Sound source) :: UnloadSoundAlias(Sound alias)
 *          Music music = LoadMusicStream("resources/country.mp3");
 *          ...
 *          SetMusicVolume(music, volume);
 *          SetMusicPan(music, pan);
 *          ...
 *          StopMusicStream(music);
 *          PlayMusicStream(music);
 *          ...
 *          PauseMusicStream(music)
 *          ResumeMusicStream(music);
 *          ...
 *          // normalized value !
 *          timePlayed = GetMusicTimePlayed(music)/GetMusicTimeLength(music);
 *          if (timePlayed > 1.0f) timePlayed = 1.0f;*
 *          ...
 *          PlaySound(fxWav);
 *          PlayMusicStream(music);
 *          ...
 *          UnloadSound(fxWav);
 *          UnloadMusicStream(music);
 *
 *      [ ] bool playMusic(std::string fileName, F32 gain = 1.0f);
 *      [ ] bool stopMusic(std::string fileName  );
 *      [ ] Music* getMusic(std::string fileName )
 *      [ ] bool playSound(std::string fileName, F32 gain = 1.0f);
 *      [ ] bool stopSound(std::string fileName  );
 *      [ ] Sound* getSound(std::string fileName )
 *      [X] research how textures are handled
 *          Image image = LoadImage("resources/raylib_logo.png");
 *          Texture2D texture = LoadTextureFromImage(image);
 *           UnloadImage(image);
 *           ...
 *           DrawTexture(texture, screenWidth/2 - texture.width/2, screenHeight/2 - texture.height/2, WHITE);
 *           ...
 *            UnloadTexture(texture);
 *      [ ] bool renderTexture(std::string fileName , Rectangle srcrect, Rectangle dstrect, .....);
 *          ==> void DrawTexturePro(Texture2D texture, Rectangle source, Rectangle dest, Vector2 origin, F32 rotation, Color tint)
 *
 *      [ ] Texture2D* getTexture(std::string fileName, bool noAutoLoad = false);
 *
 *
 *
 */
 
 ```
