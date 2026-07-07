class  WaterPlaneDemo {
  public:
   Script render( string this )...) {}
   Script onRemove( string this )...) {}
   Script onAdd( string this )...) {}
};
class  GridTest {
  public:
   Script newTerrain( string this )...) {}
   Script init( string this )...) {}
   Script render( string this )...) {}
   Script onRemove( string this )...) {}
   Script onAdd( string this )...) {}
};
class  TerrainDemo {
  public:
   Script DropToGround( string this, string obj )...) {}
   Script onMouseLeftClick( string this )...) {}
   Script spawnScriptTree( string this, string worldPos )...) {}
   Script render( string this )...) {}
   Script onRemove( string this )...) {}
   Script onAdd( string this )...) {}
   Script loadKenneyModel( string this, string spawnPoint )...) {}
   Script loadGreenManModel( string this, string spawnPoint )...) {}
};
class  waterPlane {
  public:
   Script update( string this, string dt )...) {}
   Script draw( string this, string dt )...) {}
   Script onRemove( string this )...) {}
   Script onAdd( string this )...) {}
};
class  Sun {
  public:
   Script update( string this, string dt )...) {}
   Script onRemove( string this )...) {}
   Script onAdd( string this )...) {}
};
/*!
@brief Base class for almost all objects involved in the simulation.
@ingroup Console
 */
class  SimObject {
  public:
      /*! Generic callback for when an object is edited */
      void onInspectPostApply( SimObject obj );
      /*! Generic callback for when an object is selected */
      void onSelected( SimObject obj );
      /*! Generic callback for when an object is un-selected */
      void onUnselected( SimObject obj );
      /*! Called when this SimObject is added to the system.
@param ID Unique object ID assigned when created (%this in script).
 */
      void onAdd( SimObjectId ID );
      /*! Called when this SimObject is removed from the system.
@param ID Unique object ID assigned when created (%this in script).
 */
      void onRemove( SimObjectId ID );
   /*! Dump the hierarchy of this object up to RootGroup to the console. */
   void dumpGroupHierarchy() {}
   /*! Test whether the given method is defined on this object.
@param The name of the method.
@return True if the object implements the given method. */
   bool isMethod( string methodName ) {}
   /*! Test whether the object belongs directly or indirectly to the given group.
@param group The SimGroup object.
@return True if the object is a child of the given group or a child of a group that the given group is directly or indirectly a child to. */
   bool isChildOfGroup( SimGroup group ) {}
   /*! Get the name of the class namespace assigned to this object.
@return The name of the 'class' namespace. */
   const char* getClassNamespace() {}
   /*! Get the name of the superclass namespace assigned to this object.
@return The name of the 'superClass' namespace. */
   const char* getSuperClassNamespace() {}
   /*! Assign a class namespace to this object.
@param name The name of the 'class' namespace for this object. */
   void setClassNamespace( string name ) {}
   /*! Assign a superclass namespace to this object.
@param name The name of the 'superClass' namespace for this object. */
   void setSuperClassNamespace( string name ) {}
   /*! Get whether the object has been marked as selected. (in editor)
@return True if the object is currently selected. */
   bool isSelected() {}
   /*! Set whether the object has been marked as selected. (in editor)
@param state True if object is to be marked selected; false if not. */
   void setIsSelected( bool state=true ) {}
   /*! Get whether the object has been marked as expanded. (in editor)
@return True if the object is marked expanded. */
   bool isExpanded() {}
   /*! Set whether the object has been marked as expanded. (in editor)
@param state True if the object is to be marked expanded; false if not. */
   void setIsExpanded( bool state=true ) {}
   /*! Returns the filename the object is attached to.
@return The name of the file the object is associated with; usually the file the object was loaded from. */
   const char* GetFileName() {}
   /*! Sets the object's file name and path
@param fileName The name of the file to associate this object with. */
   void setFilename( string fileName ) {}
   /*! Get the line number at which the object is defined in its file.

@return The line number of the object's definition in script.
@see getFilename() */
   S32 getDeclarationLine() {}
   /*! Copy fields from another object onto this one.  The objects must be of same type. Everything from the object will overwrite what's in this object; extra fields in this object will remain. This includes dynamic fields.
@param fromObject The object from which to copy fields. */
   void assignFieldsFrom( SimObject fromObject ) {}
   /*! Assign a persistent ID to the object if it does not already have one. */
   void assignPersistentId() {}
   /*! Get whether the object will be included in saves.
@return True if the object will be saved; false otherwise. */
   bool getCanSave() {}
   /*! Set whether the object will be included in saves.
@param value If true, the object will be included in saves; if false, it will be excluded. */
   void setCanSave( bool value=true ) {}
   /*! Return true if the object is only used by the editor.
@return True if this object exists only for the sake of editing. */
   bool isEditorOnly() {}
   /*! Set/clear the editor-only flag on this object.
@param value If true, the object is marked as existing only for the editor. */
   void setEditorOnly( bool value=true ) {}
   /*! Get whether this object may be renamed.
@return True if this object can be renamed; false otherwise. */
   bool isNameChangeAllowed() {}
   /*! Set whether this object can be renamed from its first name.
@param value If true, renaming is allowed for this object; if false, trying to change the name of the object will generate a console error. */
   void setNameChangeAllowed( bool value=true ) {}
   /*! Create a copy of this object.
@return An exact duplicate of this object. */
   SimObject* clone() {}
   /*! Create a copy of this object and all its subobjects.
@return An exact duplicate of this object and all objects it references. */
   SimObject* deepClone() {}
   /*! Lock/unlock the object in the editor.
@param value If true, the object will be locked; if false, the object will be unlocked. */
   void setLocked( bool value=true ) {}
   /*! Hide/unhide the object.
@param value If true, the object will be hidden; if false, the object will be unhidden. */
   void setHidden( bool value=true ) {}
   /*! List the methods defined on this object.

Each description is a newline-separated vector with the following elements:
- Minimum number of arguments.
- Maximum number of arguments.
- Prototype string.
- Full script file path (if script method).
- Line number of method definition in script (if script method).

- Documentation string (not including prototype).  This takes up the remainder of the vector.
@return An ArrayObject populated with (name,description) pairs of all methods defined on the object. */
   ArrayObject* dumpMethods() {}
   /*! List the methods defined on this object.

Each description is a newline-separated vector with the following elements:
- method prototype string.
- Documentation string (not including prototype).  This takes up the remainder of the vector.
@return An ArrayObject populated with (name,description) pairs of all methods defined on the object. */
   ArrayObject* getMethodSigs( bool commands=false ) {}
   /*! Dump a description of all fields and methods defined on this object to the console.
@param detailed Whether to print detailed information about members. */
   void dump( bool detailed=false ) {}
   /*! Save out the object to the given file.
@param fileName The name of the file to save to.@param selectedOnly If true, only objects marked as selected will be saved out.
@param preAppendString Text which will be preprended directly to the object serialization.
@param True on success, false on failure. */
   bool save( string fileName, bool selectedOnly=false, string preAppendString="" ) {}
   /*! Set the global name of the object.
@param newName The new global name to assign to the object.
@note If name changing is disallowed on the object, the method will fail with a console error. */
   void setName( string newName ) {}
   /*! Get the global name of the object.
@return The global name assigned to the object. */
   const char* getName() {}
   /*! Get the name of the C++ class which the object is an instance of.
@return The name of the C++ class of the object. */
   const char* getClassName() {}
   /*! Test whether the given field is defined on this object.
@param fieldName The name of the field.
@return True if the object implements the given field. */
   bool isField( string fieldName ) {}
   /*! Return the value of the given field on this object.
@param fieldName The name of the field.  If it includes a field index, the index is parsed out.
@param index Optional parameter to specify the index of an array field separately.
@return The value of the given field or "" if undefined. */
   const char* getFieldValue( string fieldName, int index=-1 ) {}
   /*! Set the value of the given field on this object.
@param fieldName The name of the field to assign to.  If it includes an array index, the index will be parsed out.
@param value The new value to assign to the field.
@param index Optional argument to specify an index for an array field.
@return True. */
   bool setFieldValue( string fieldName, string value, int index=-1 ) {}
   /*! Set the value and console type of the given field on this object.
NOTE: this cant be used with arrays like setFieldValue@param fieldName The name of the field to assign to.  If it includes an array index, the index will be parsed out.
@param type The name of the console type.
@param value The new value to assign to the field.
@return True. */
   bool addTypeField( string fieldName, string type, string value ) {}
   /*! Get the console type code of the given field.
@return The numeric type code for the underlying console type of the given field. */
   const char* getFieldType( string fieldName ) {}
   /*! Set the console type code for the given field.
@param fieldName The name of the dynamic field to change to type for.
@param type The name of the console type.
@note This only works for dynamic fields.  Types of static fields cannot be changed. */
   void setFieldType( string fieldName, string type ) {}
   /*! Dynamically call a method on an object.
@param method Name of method to call.
@param args Zero or more arguments for the method.
@return The result of the method call. */
   string call( string method, string args... )...) {}
   /*! Set the internal name of the object.
@param newInternalName The new internal name for the object. */
   void setInternalName( string newInternalName ) {}
   /*! Get the internal name of the object.
@return The internal name of the object. */
   const char* getInternalName() {}
   /*! Dump the native C++ class hierarchy of this object's C++ class to the console. */
   void dumpClassHierarchy() {}
   /*! Test whether this object is a member of the specified class.
@param className Name of a native C++ class.
@return True if this object is an instance of the given C++ class or any of its super classes. */
   bool isMemberOfClass( string className ) {}
   /*! Test whether the namespace of this object is a direct or indirect child to the given namespace.
@param name The name of a namespace.
@return True if the given namespace name is within the namespace hierarchy of this object. */
   bool isInNamespaceHierarchy( string name ) {}
   /*! Get the underlying unique numeric ID of the object.
@note Object IDs are unique only during single engine runs.
@return The unique numeric ID of the object. */
   S32 getId() {}
   /*! Get the group that this object is contained in.
@note If not assigned to particular SimGroup, an object belongs to RootGroup.
@return The SimGroup object to which the object belongs. */
   SimGroup* getGroup() {}
   /*! Delete and remove the object. */
   void delete() {}
   /*! Delay an invocation of a method.
@param time The number of milliseconds after which to invoke the method.  This is a soft limit.
@param method The method to call.
@param args The arguments with which to call the method.
@return The numeric ID of the created schedule.  Can be used to cancel the call.
 */
   int schedule( float time, string method, string args... )...) {}
   /*! Get the number of dynamic fields defined on the object.
@return The number of dynamic fields defined on the object. */
   S32 getDynamicFieldCount() {}
   /*! Get a value of a dynamic field by index.
@param index The index of the dynamic field.
@return The value of the dynamic field at the given index or "". */
   const char* getDynamicField( int index ) {}
   /*! Get the number of static fields on the object.
@return The number of static fields defined on the object. */
   S32 getFieldCount() {}
   /*! Retrieve the value of a static field by index.
@param index The index of the static field.
@return The value of the static field with the given index or "". */
   const char* getField( int index ) {}
   /*! Dump a description of all fields defined on this object to the console.
@param detailed Whether to print detailed information about members. */
   void dumpFields( bool staticFields=true, bool dynamicFields=true, bool detailed=false ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /*!
   Optional global name of this object.
   
    */
   string name;
   /*!
   Optional Name of object to inherit from as a parent.
   
    */
   string inheritFrom;
   /*!
   Prototype Methods
   
    */
   bool Prototype;
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /*!
   Optional name that may be used to lookup this object within a SimSet.
   
    */
   string internalName;
   /*!
   Group hierarchy parent of the object.
   
    */
   SimObject parentGroup;
   /*!
   Script class of object.
   
    */
   string class;
   /*!
   Script super-class of object.
   
    */
   string superClass;
   /*!
   Script class of object.
   
    */
   string className;
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /*!
   Whether the object is visible.
   
    */
   bool hidden;
   /*!
   Whether the object can be edited.
   
    */
   bool locked;
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /*!
   Whether the object can be saved out. If false, the object is purely transient in nature.
   
    */
   bool canSave;
   /*!
   True if dynamic fields (added at runtime) should be saved. Defaults to true.
   
    */
   bool canSaveDynamicFields;
   /*!
   The universally unique identifier for the object.
   
    */
   pid persistentId;
   /// @}
};
class  Camera3DObject : public SimObject {
  public:
   /*! Update camera position for selected mode (e.g. $CAMERA_FIRST_PERSON, $CAMERA_FREE) */
   void update( int mode ) {}
   /*! Update camera movement/rotation with pro parameters */
   void updatePro( Vector3 movement, Vector3 rotation, float zoom ) {}
   /*! Get a 3D ray trace from a screen position (e.g. mouse) */
   Ray getScreenToWorldRay( Vector2 position ) {}
   /*! Get a 3D ray trace from a screen position within a specific custom viewport size */
   Ray getScreenToWorldRayEx( Vector2 position, int width, int height ) {}
   /*! Get 2D screen space position for a 3D world space position */
   Vector2 getWorldToScreen( Vector3 position ) {}
   /*! Get 2D screen space position for a 3D world space position inside a specific custom viewport size */
   Vector2 getWorldToScreenEx( Vector3 position, int width, int height ) {}
   /*! Get the 3D camera transform matrix (view matrix) */
   Matrix getCameraMatrix() {}
   /*! BeginMode3D - Begins 3D mode with a Camera3DObject */
   void Begin() {}
   /*! EndMode3D - End 3D mode with custom camera */
   void End() {}
   /*! GetCameraForward */
   Vector3 getForwardVector() {}
   /*! GetCameraUp */
   Vector3 getUpVector() {}
   /*! GetCameraRight */
   Vector3 getRightVector() {}
   /*! Moves the camera forward/backward based on distance. If moveInWorldPlane is true, movement is locked to the XZ plane. */
   void moveForward( float distance, bool moveInWorldPlane ) {}
   /*! Moves the camera up/down along its local up vector. */
   void moveUp( float distance ) {}
   /*! Moves the camera right/left based on distance. If moveInWorldPlane is true, movement is locked to the XZ plane. */
   void moveRight( float distance, bool moveInWorldPlane ) {}
   /*! Moves the camera closer or further away from its target point. */
   void moveToTarget( float delta ) {}
   /*! Rotates the camera horizontally (yaw) around its own axis or around the target point (angle in radians). */
   void yaw( float angle, bool rotateAroundTarget ) {}
   /*! Rotates the camera vertically (pitch) with optional view locking (angle in radians). */
   void pitch( float angle, bool lockView, bool rotateAroundTarget, bool rotateUp ) {}
   /*! Tilts the camera (roll) along its view axis (angle in radians). */
   void roll( float angle ) {}
   /*! Returns the camera's view matrix. */
   Matrix getViewMatrix() {}
   /*! Returns the camera's projection matrix based on the given aspect ratio. */
   Matrix getProjectionMatrix( float aspect ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
   /*!
   Camera position in world space
   
    */
   Vector3 position;
   /*!
   Camera target point in world space
   
    */
   Vector3 target;
   /*!
   Camera up vector (defines the orientation)
   
    */
   Vector3 up;
   /*!
   Camera field-of-view aperture in Y (degrees) in perspective, or width in orthographic
   
    */
   float fovy;
   /*!
   Camera projection type (CAMERA_PERSPECTIVE or CAMERA_ORTHOGRAPHIC)
   
    */
   int projection;
};
class  CameraFree : public Camera3DObject {
  public:
   Script update( string this, string dt )...) {}
};
/*!
@brief A script-level OOP object which allows binding of a class, superClass and arguments along with declaration of methods.
ScriptObjects are extrodinarily powerful objects that allow defining of any type of data required. They can optionally have
a class and a superclass defined for added control of multiple ScriptObjects through a simple class definition.
@tsexample
new ScriptObject(Game)
{
   class = "DeathMatchGame";
   superClass = GameCore;
   genre = "Action FPS"; // Note the new, non-Torque variable
};
@endtsexample
@see SimObject
@ingroup Console
 */
class  ScriptObject : public SimObject {
  public:
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  Main : public ScriptObject {
  public:
   Script loop( string this )...) {}
   Script loadModule( string this, string setNewModuleIndex )...) {}
   Script init( string this )...) {}
};
/*!
@brief A collection of SimObjects.
It is often necessary to keep track of an arbitrary set of SimObjects. For instance, Torque's networking code needs to not only keep track of the set of objects which need to be ghosted, but also the set of objects which must <i>always</i> be ghosted. It does this by working with two sets. The first of these is the RootGroup (which is actually a SimGroup) and the second is the GhostAlwaysSet, which contains objects which must always be ghosted to the client.
Some general notes on SimSets:
- Membership is not exclusive. A SimObject may be a member of multiple SimSets.
- A SimSet does not destroy subobjects when it is destroyed.
- A SimSet may hold an arbitrary number of objects.
@ingroup Console
 */
class  SimSet : public SimObject {
  public:
      /*! Called when an object is added to the set.
@param object The object that was added. */
      void onObjectAdded( SimObject object );
      /*! Called when an object is removed from the set.
@param object The object that was removed. */
      void onObjectRemoved( SimObject object );
   /*! Dump a list of all objects contained in the set to the console. */
   void listObjects() {}
   /*! Add the given objects to the set.
@param objects The objects to add to the set. */
   void add( SimObject objects... )...) {}
   /*! Remove the given objects from the set.
@param objects The objects to remove from the set. */
   void remove( SimObject objects... )...) {}
   /*! Remove all objects from the set. */
   void clear() {}
   /*! Delete all objects in the set. */
   void deleteAllObjects() {}
   /*! Delete all objects in the set. */
   void deleteObjects() {}
   /*! Return a random object from the set.
@return A randomly selected object from the set or -1 if the set is empty. */
   SimObject* getRandom() {}
   /*! Call a method on all objects contained in the set.

@param method The name of the method to call.
@param args The arguments to the method.

@note This method recurses into all SimSets that are children to the set.

@see callOnChildrenNoRecurse */
   void callOnChildren( string method, string args... )...) {}
   /*! Call a method on all objects contained in the set.

@param method The name of the method to call.
@param args The arguments to the method.

@note This method does not recurse into child SimSets.

@see callOnChildren */
   void callOnChildrenNoRecurse( string method, string args... )...) {}
   /*! Make sure child1 is ordered right before child2 in the set.
@param child1 The first child.  The object must already be contained in the set.
@param child2 The second child.  The object must already be contained in the set. */
   void reorderChild( SimObject child1, SimObject child2 ) {}
   /*! Get the number of objects contained in the set.
@return The number of objects contained in the set. */
   S32 getCount() {}
   /*! Get the number of direct and indirect child objects contained in the set.
@return The number of objects contained in the set as well as in other sets contained directly or indirectly in the set. */
   S32 getFullCount() {}
   /*! Get the object at the given index.
@param index The object index.
@return The object at the given index or -1 if index is out of range. */
   SimObject* getObject( uint index ) {}
   /*! Return the index of the given object in this set.
@param obj The object for which to return the index.  Must be contained in the set.
@return The index of the object or -1 if the object is not contained in the set. */
   S32 getObjectIndex( SimObject obj ) {}
   /*! Test whether the given object belongs to the set.
@param obj The object.
@return True if the object is contained in the set; false otherwise. */
   bool isMember( SimObject obj ) {}
   /*! Find an object in the set by its internal name.
@param internalName The internal name of the object to look for.
@param searchChildren If true, SimSets contained in the set will be recursively searched for the object.
@return The object with the given internal name or 0 if no match was found.
 */
   SimObject* findObjectByInternalName( string internalName, bool searchChildren=false ) {}
   /*! Make the given object the first object in the set.
@param obj The object to bring to the frontmost position.  Must be contained in the set. */
   void bringToFront( SimObject obj ) {}
   /*! Make the given object the last object in the set.
@param obj The object to bring to the last position.  Must be contained in the set. */
   void pushToBack( SimObject obj ) {}
   /*! Sort the objects in the set using the given comparison function.
@param callbackFunction Name of a function that takes two object arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal. */
   void sort( string callbackFunction ) {}
   /*! Test whether the given object may be added to the set.
@param obj The object to test for potential membership.
@return True if the object may be added to the set, false otherwise. */
   bool acceptsAsChild( SimObject obj ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/// Stub class
/// 
/// @note This is a stub class to ensure a proper class hierarchy. No 
///       information was available for this class.
class  GarbageCollectionSet : public SimSet {
  public:
};
/*!
@brief A collection of SimObjects that are owned by the group.
A SimGroup is a stricter form of SimSet. SimObjects may only be a member of a single SimGroup at a time. The SimGroup will automatically enforce the single-group-membership rule (ie. adding an object to a SimGroup will cause it to be removed from its current SimGroup, if any).
Deleting a SimGroup will also delete all SimObjects in the SimGroup.
@tsexample
// Create a SimGroup for particle emitters
new SimGroup(Emitters)
{
   canSaveDynamicFields = "1";
   new ParticleEmitterNode(CrystalEmmiter) {
      active = "1";
      emitter = "dustEmitter";
      velocity = "1";
      dataBlock = "GenericSmokeEmitterNode";
      position = "-61.6276 2.1142 4.45027";
      rotation = "1 0 0 0";
      scale = "1 1 1";
      canSaveDynamicFields = "1";
   };
   new ParticleEmitterNode(Steam1) {
      active = "1";
      emitter = "SlowSteamEmitter";
      velocity = "1";
      dataBlock = "GenericSmokeEmitterNode";
      position = "-25.0458 1.55289 2.51308";
      rotation = "1 0 0 0";
      scale = "1 1 1";
      canSaveDynamicFields = "1";
   };
};
@endtsexample
@ingroup Console
 */
class  SimGroup : public SimSet {
  public:
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/// Stub class
/// 
/// @note This is a stub class to ensure a proper class hierarchy. No 
///       information was available for this class.
class  RootGroup : public SimGroup {
  public:
};
class  SceneObject : public SimSet {
  public:
   /*! refresh the worldbox, needs to be called when position / scale is changed */
   void refresh() {}
   /*!
    */
   Vector3 position;
   /*!
    */
   Vector3 rotation;
   /*!
    */
   Vector3 Scale;
   /*!
    */
   bool Visible;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  ModelObject : public SceneObject {
  public:
   /*! Draws this object instance and all its children polymorphically. */
   void draw() {}
   /*! Play a animation once. The current animation will be restored after finished.Force also start the animation if a runOnce is already running. */
   bool playOnce( int animationIndex, int animationFPS=25, bool force=false ) {}
   /*!
   The script ID of the raylib model.
   
    */
   int ModelId;
   /*!
   If this is a child it can be mounted to a Bone index. See also GetModelBoneIndexByName
   
    */
   int MountBoneIndex;
   /*! @name Animation
   @{ */
   /*! */
   /*!
   Id of the AnimationBlock
   
    */
   int AnimationBlockId;
   /*!
   Index of the animation to play. See also GetModelAnimationCount.
   
    */
   int AnimationIndex;
   /*!
   Frames per secound when an animation is played
   
    */
   int AnimationFPS;
   /// @}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  TerrainObject : public SceneObject {
  public:
   /*! Load or reload the terrain from the specified HeightMapFilename. */
   bool load() {}
   /*! Draw  heightmap */
   void draw() {}
   /*! Returns the exact terrain height (Y-coordinate) at the given world position. */
   F32 getHeight( Vector3 position ) {}
   /*! Returns the surface normal vector at the given world position. */
   Vector3 getNormal( Vector3 position ) {}
   /*! Performs a raycast collision check against the terrainand returns 'X Y Z Nx Ny Nz Dist' or empty string. */
   String getRayCollision( Ray ray ) {}
   /*! Returns the resource-manager compatible model ID of the terrain's 3D mesh. */
   S32 getModelId() {}
   /*!
   You need to call load() after changing it.
   
    */
   string HeightMapFilename;
   /*!
   You need to call load() after changing it.
   
    */
   Vector3 Size;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  Grid : public SimObject {
  public:
   Script addRandomMud( string this )...) {}
   /*! param: area: x y w h, F32 SquareSize */
   bool init( Rectangle area, float squareSize ) {}
   /*! get count of nodes */
   S32 getNodeCount() {}
   /*! get count of horizontal nodes  */
   S32 getNodeCountX() {}
   /*! get count of vertical nodes  */
   S32 getNodeCountY() {}
   /*! x,y; return top left pos of a node by the world values  */
   Vector2 getPos( float x, float y ) {}
   /*! x,y; return flags  */
   S32 getFlags( float x, float y ) {}
   /*! x,y; return nodeidx x y flags  */
   const char* getNodeByPos( float x, float y ) {}
   /*! x,y; return S32 nodeidx  */
   S32 getNodeIdByPos( float x, float y ) {}
   /*! S32 NodeIndex,  return nodeidx x y z flags  */
   const char* getNode( int nodeIndex ) {}
   /*! x y w h,  return nodeidx nodeidx ..  */
   const char* getNodesByRect( Rectangle area ) {}
   /*! S32 NodeIndex, S32 Direction,  return nodeidx x y flags Directions:1  2  34  X  56  7  8 */
   const char* getNeighbour( int nodeIndex, int direction ) {}
   /*! x,y; set flags  */
   bool setFlags( float x, float y, uint flags ) {}
   /*! x,y, idx[0..9], Value; set int value  */
   bool setIntValue( float x, float y, int idx, int value ) {}
   /*! x,y, U8 weight */
   bool setWeight( float x, float y, int weigth ) {}
   /*! x,y, idx[0..9] */
   S32 getIntValue( float x, float y, int idx ) {}
   /*! x,y, U8 value */
   bool setIntValueByNodeId( int nodeId, int idx, int value ) {}
   /*! nodeId, idx[0..9] */
   S32 getIntValueByNodeId( int nodeId, int idx ) {}
   /*! Display Infos on Console */
   void getinfo( bool listNodes=false ) {}
   /*! findPath (Vector2 start, Vector2 goal, bool smoothPath = false) - Create a path between the two points.Warning smoothPath use line of sight and ignore the weight !!!!! */
   S32 findPath( Vector2 start, Vector2 goal, bool smoothPath=false ) {}
   S32 getWeightByNodeId( int nodeId ) {}
   /*! nodeId, U8 weight */
   bool setWeightByNodeId( int nodeId, int weight ) {}
   /*! return centered rectI pos / extent  */
   Rectangle getNodeRectbyId( int nodeId ) {}
   /*! getPathCost (Vector2 start, Vector2 goal) - Create a path between the two points and Return the lenth of the path */
   S32 getPathCost( Vector2 start, Vector2 goal, bool smoothPath=true ) {}
   /*! Fill the table with all path costs - expensive! */
   void compilePathCosts() {}
   /*! return nodecount of to points to calculated closed path it -1 then it failed */
   S32 getNodeToNodeCosts( Vector2 start, Vector2 goal, bool smoothPath=true ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  ConsoleGuiObject : public SimObject {
  public:
   /*! set the console font */
   bool setFont( int fontId ) {}
   /*! update the console */
   void update() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
   /*!
   Shortcut to open console
   
    */
   int key;
   /*!
   font size
   
    */
   float fontSize;
   /*!
   2=half height, 1= full heigth
   
    */
   float heightDiv;
};
class  Gui : public SimObject {
  public:
   void Begin( float x, float y ) {}
   void SameLine( float relativeX=0 ) {}
   void enhanceCursor() {}
   void resetStates() {}
   void setStates( float width, float height ) {}
   bool ButtonV( Vector2 size, String text ) {}
   bool Button( float width, String text ) {}
   bool checkBox( String text, bool value ) {}
   /*! variable example: $MyValue or %this.object.getId() @ ".enabled" */
   bool checkBoxVar( String text, String variable ) {}
   bool Write( String text, int fontSize=-1, Color color=LIGHTGRAY ) {}
   bool Label( String text ) {}
   bool SeparatorV( Vector2 size, Color color ) {}
   bool Separator( float width, Color color=LIGHTGRAY ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
   /*!
    */
   float left;
   /*!
    */
   Vector2 cursor;
   /*!
    */
   Vector2 spacing;
   /*!
    */
   Vector2 defaultSize;
   /*!
    */
   bool SameLine;
   /*!
    */
   bool isFocused;
   /*!
    */
   bool isPressed;
   /*!
    */
   bool isClicked;
   /*!
    */
   Rectangle lastBounds;
   /*!
    */
   int lastState;
};
class  BatchRender : public SimObject {
  public:
   /*! push the index to current */
   void push( int index ) {}
   /*! push the index to current */
   void pull( int index ) {}
   /*! Resizes the internal buffers safely */
   void setBufferSize( int size ) {}
   /*! Sets data at a specific index */
   void setVector2( int index, float x, float y ) {}
   /*! Sets data at a specific index */
   void setVector2v( int index, Vector2 vec ) {}
   /*! Sets data at a specific index */
   void setVector3( int index, float x, float y, float z ) {}
   /*! Sets data at a specific index */
   void setVector4( int index, float x, float y, float w, float h ) {}
   /*! Sets data at a specific index */
   void setVector4_1( int index, float m4, float m5, float m6, float m7 ) {}
   /*! Sets data at a specific index */
   void setVector4_2( int index, float m8, float m9, float m10, float m11 ) {}
   /*! Sets data at a specific index */
   void setVector4_3( int index, float m12, float m13, float m14, float m15 ) {}
   /*! Sets data at a specific index */
   void setVector8( int index, float m0, float m1, float m2, float m3, float m4, float m5, float m6, float m7 ) {}
   /*! Get x position by index */
   F32 getX( int index ) {}
   /*! Get y position by index */
   F32 getY( int index ) {}
   /*! Get z (width) by index */
   F32 getZ( int index ) {}
   /*! Get width by index */
   F32 getW( int index ) {}
   /*! Get height by index */
   F32 getH( int index ) {}
   /*! Get full Vector2 structure by index */
   Vector3 getVector2( int index ) {}
   /*! Get full Vector3 structure by index */
   Vector3 getVector3( int index ) {}
   /*! Get Vector4 structure by index */
   Vector4 getVector4( int index ) {}
   /*! Get second Vector4 structure by index */
   Vector4 getVector4_1( int index ) {}
   /*! Get third Vector4 structure by index */
   Vector4 getVector4_2( int index ) {}
   /*! Get fourth Vector4 structure by index */
   Vector4 getVector4_3( int index ) {}
   /*! Get the Matrix structure by index */
   Matrix getMatrix( int index ) {}
   /*! Batch draw circles using first Vector3 x,y,radius */
   void drawCircles( int count, Color color ) {}
   /*! Batch draw rectangles using first Vector4 x,y,width,height */
   void drawRects( int count, Color color ) {}
   /*! Batch draw lines using first Vector4 x1,y1,x2,y2 */
   void drawLines( int count, Color color ) {}
   /*! Fetches X, Yto global variables named by parameters */
   void getVector2GlobalV( int index, String varX, String varY ) {}
   /*! Fetches X, Y FROM global variables named by parameters */
   void setVector2GlobalV( int index, String varX, String varY ) {}
   /*! Fetches X, Y, Z to global variables named by parameters */
   void getVector3GlobalV( int index, String varX, String varY, String varW ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void getVector4GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void setVector4GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void getVector4_1GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void setVector4_1GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void getVector4_2GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void setVector4_2GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void getVector4_3GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*! Fetches X, Y, W, H to global variables named by parameters */
   void setVector4_3GlobalV( int index, String varX, String varY, String varW, String varH ) {}
   /*!
   Set the buffersize (element count).
   
    */
   int count;
   /*!
   curent x on the matrix
   
    */
   float x;
   /*!
   curent y on the matrix
   
    */
   float y;
   /*!
   curent z on the matrix
   
    */
   float z;
   /*!
   curent w on the matrix
   
    */
   float w;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  StarField : public SimObject {
  public:
   /*! update and render startfield */
   void loop() {}
   /*!
   Set the mStars count.
   
    */
   int count;
   /*!
   mode 0=lines, 1=rectangles, 2=circles
   
    */
   int mode;
   /*!
   set the mSpeed of the mStars
   
    */
   float speed;
   /*!
   color of the mStars
   
    */
   Color color;
   /*!
   color of the background
   
    */
   Color BackgroundColor;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief Data structure for storing indexed sequences of key/value pairs.
This is a powerful array class providing PHP style arrays in TorqueScript.
The following features are supported:<ul>
<li>array pointers: this allows you to move forwards or backwards through the array as if it was a list, including jumping to the start or end.</li>
<li>sorting: the array can be sorted in either alphabetic or numeric mode, on the key or the value, and in ascending or descending order</li>
<li>add/remove elements: elements can be pushed/popped from the start or end of the array, or can be inserted/erased from anywhere in the middle</li>
<li>removal of duplicates: remove duplicate keys or duplicate values</li>
<li>searching: search the array and return the index of a particular key or value</li>
<li>counting: count the number of instaces of a particular value or key in the array, as well as the total number of elements</li>
<li>advanced features: array append, array crop and array duplicate</li>
</ul>
Array element keys and values can be strings or numbers
 */
class  ArrayObject : public SimObject {
  public:
   /*! Search the array from the current position for the element @param value Array value to search for
@return Index of the first element found, or -1 if none
 */
   S32 getIndexFromValue( string value ) {}
   /*! Search the array from the current position for the key @param value Array key to search for
@return Index of the first element found, or -1 if none
 */
   S32 getIndexFromKey( string key ) {}
   /*! Search the array from the current position for the Key @param value Array key to search for
@return Value of the first element found, or -1 if none
 */
   const char* getValueFromKey( string key ) {}
   /*! Search the array from the current position for the Value @param value Array key to search for
@return Key of the first element found, or -1 if none
 */
   const char* getKeyFromValue( string key ) {}
   /*! Get the value of the array element at the submitted index.
@param index 0-based index of the array element to get
@return The value of the array element at the specified index, or "" if the index is out of range
 */
   const char* getValue( int index ) {}
   /*! Get the key of the array element at the submitted index.
@param index 0-based index of the array element to get
@return The key associated with the array element at the specified index, or "" if the index is out of range
 */
   const char* getKey( int index ) {}
   /*! Set the key at the given index.
@param key New key value
@param index 0-based index of the array element to update
 */
   void setKey( string key, int index ) {}
   /*! Set the value at the given index.
@param value New array element value
@param index 0-based index of the array element to update
 */
   void setValue( string value, int index ) {}
   /*! Get the number of elements in the array. */
   S32 count() {}
   /*! Get the number of times a particular value is found in the array.
@param value Array element value to count
 */
   S32 countValue( string value ) {}
   /*! Get the number of times a particular key is found in the array.
@param key Key value to count
 */
   S32 countKey( string key ) {}
   /*! Adds a new element to the end of an array (same as push_back().
@param key Key for the new element
@param value Value for the new element
 */
   void add( string key, string value="" ) {}
   /*! Adds a new element to the end of an array.
@param key Key for the new element
@param value Value for the new element
 */
   void push_back( string key, string value="" ) {}
   /*! Adds a new element to the front of an array */
   void push_front( string key, string value="" ) {}
   /*! Adds a new element to a specified position in the array.
- @a index = 0 will insert an element at the start of the array (same as push_front()
- @a index = %array.count() will insert an element at the end of the array (same as push_back()

@param key Key for the new element
@param value Value for the new element
@param index 0-based index at which to insert the new element */
   void insert( string key, string value, int index ) {}
   /*! Removes the last element from the array */
   void pop_back() {}
   /*! Removes the first element from the array */
   void pop_front() {}
   /*! Removes an element at a specific position from the array.
@param index 0-based index of the element to remove
 */
   void erase( int index ) {}
   /*! Emptys all elements from an array */
   void empty() {}
   /*! Removes any elements that have duplicated values (leaving the first instance) */
   void uniqueValue() {}
   /*! Removes any elements that have duplicated keys (leaving the first instance) */
   void uniqueKey() {}
   /*! Removes any elements that have duplicated key and value pairs (leaving the first instance) */
   void uniquePair() {}
   /*! Alters array into an exact duplicate of the target array.
@param target ArrayObject to duplicate
 */
   bool duplicate( ArrayObject target ) {}
   /*! Removes elements with matching keys from array.
@param target ArrayObject containing keys to remove from this array
 */
   bool crop( ArrayObject target ) {}
   /*! Appends the target array to the array object.
@param target ArrayObject to append to the end of this array
 */
   bool append( ArrayObject target ) {}
   /*! Alpha sorts the array by value

@param ascending [optional] True for ascending sort, false for descending sort
 */
   void sort( bool ascending=false ) {}
   /*! Alpha sorts the array by value in ascending order */
   void sorta() {}
   /*! Alpha sorts the array by value in descending order */
   void sortd() {}
   /*! Alpha sorts the array by key

@param ascending [optional] True for ascending sort, false for descending sort
 */
   void sortk( bool ascending=false ) {}
   /*! Alpha sorts the array by key in ascending order */
   void sortka() {}
   /*! Alpha sorts the array by key in descending order */
   void sortkd() {}
   /*! Numerically sorts the array by value

@param ascending [optional] True for ascending sort, false for descending sort
 */
   void sortn( bool ascending=false ) {}
   /*! Numerically sorts the array by value in ascending order */
   void sortna() {}
   /*! Numerically sorts the array by value in descending order */
   void sortnd() {}
   /*! Numerically sorts the array by key

@param ascending [optional] True for ascending sort, false for descending sort
 */
   void sortnk( bool ascending=false ) {}
   /*! Numerical sorts the array by key in ascending order */
   void sortnka() {}
   /*! Numerical sorts the array by key in descending order */
   void sortnkd() {}
   /*! Sorts the array by value in ascending order using the given callback function.
@param functionName Name of a function that takes two arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal.

@tsexample
function mySortCallback(%a, %b)
{
   return strcmp( %a.name, %b.name );
}

%array.sortf( "mySortCallback" );
@endtsexample
 */
   void sortf( string functionName ) {}
   /*! Sorts the array by key in ascending order using the given callback function.
@param functionName Name of a function that takes two arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal.@see sortf
 */
   void sortfk( string functionName ) {}
   /*! Sorts the array by value in descending order using the given callback function.
@param functionName Name of a function that takes two arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal.@see sortf
 */
   void sortfd( string functionName ) {}
   /*! Sorts the array by key in descending order using the given callback function.
@param functionName Name of a function that takes two arguments A and B and returns -1 if A is less, 1 if B is less, and 0 if both are equal.@see sortf
 */
   void sortfkd( string functionName ) {}
   /*! Moves array pointer to start of array

@return Returns the new array pointer */
   S32 moveFirst() {}
   /*! Moves array pointer to end of array

@return Returns the new array pointer */
   S32 moveLast() {}
   /*! Moves array pointer to next position

@return Returns the new array pointer, or -1 if already at the end */
   S32 moveNext() {}
   /*! Moves array pointer to prev position

@return Returns the new array pointer, or -1 if already at the start */
   S32 movePrev() {}
   /*! Gets the current pointer index */
   S32 getCurrent() {}
   /*! Sets the current pointer index.
@param index New 0-based pointer index
 */
   void setCurrent( int index ) {}
   /*! Echos the array contents to the console */
   void echo() {}
   /*!
   Makes the keys and values case-sensitive.
By default, comparison of key and value strings will be case-insensitive.
   
    */
   bool caseSensitive;
   /*!
   Helper field which allows you to add new key['keyname'] = value pairs.
   
    */
   caseString key;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief A ScriptObject that responds to tick and frame events.
ScriptTickObject is a ScriptObject that adds callbacks for tick and frame events.  Use setProcessTicks() to enable or disable the onInterpolateTick() and onProcessTick() callbacks.  The callOnAdvanceTime property determines if the onAdvanceTime() callback is called.
@see ScriptObject
@ingroup Console
 */
class  ScriptTickObject : public ScriptObject {
  public:
      /*! This is called every frame, but only if the object is set to process ticks.
@param delta The time delta for this frame.
 */
      void onInterpolateTick( float delta );
      /*! Called once every 32ms if this object is set to process ticks.
 */
      void onProcessTick();
      /*! This is called every frame regardless if the object is set to process ticks, but only if the callOnAdvanceTime property is set to true.
@param timeDelta The time delta for this frame.
@see callOnAdvanceTime
 */
      void onAdvanceTime( float timeDelta );
   /*! @brief Sets this object as either tick processing or not.

@param tick This object's onInterpolateTick() and onProcessTick() callbacks are called if set to true.

 */
   void setProcessTicks( bool tick ) {}
   /*! @brief Is this object wanting to receive tick notifications.

If this object is set to receive tick notifications then its onInterpolateTick() and onProcessTick() callbacks are called.
@return True if object wants tick notifications

 */
   bool isProcessingTicks() {}
   /*!
   Call the onAdvaceTime() callback.
   
    */
   bool callOnAdvanceTime;
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief Essentially a SimGroup, but with onAdd and onRemove script callbacks.
@tsexample
// First container, SimGroup containing a ScriptGroup
new SimGroup(Scenes)
{
  // Subcontainer, ScriptGroup containing variables
  // related to a cut scene and a starting WayPoint
  new ScriptGroup(WelcomeScene)
  {
     class = "Scene";
     pathName = "Pathx";
     description = "A small orc village set in the Hardesty mountains. This town and its surroundings will be used to illustrate some the Torque Game Engine's features.";
     pathTime = "0";
     title = "Welcome to Orc Town";
     new WayPoint(start)
     {
        position = "163.873 -103.82 208.354";
        rotation = "0.136165 -0.0544916 0.989186 44.0527";
        scale = "1 1 1";
        dataBlock = "WayPointMarker";
        team = "0";
     };
  };
};
@endtsexample
@see SimGroup
@ingroup Console
 */
class  ScriptGroup : public SimGroup {
  public:
      /*! Called when this ScriptGroup is added to the system.
@param ID Unique object ID assigned when created (%this in script).
 */
      void onAdd( SimObjectId ID );
      /*! Called when this ScriptObject is removed from the system.
@param ID Unique object ID assigned when created (%this in script).
 */
      void onRemove( SimObjectId ID );
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief A SimSet that can be safely persisted.
Uses SimPersistIDs to reference objects in the set while persisted on disk.  This allows the set to resolve its references no matter whether they are loaded before or after the set is created.
Not intended for game development, for editors or internal use only.
 */
class  SimPersistSet : public SimSet {
  public:
   /*! Try to bind unresolved persistent IDs in the set. */
   void resolvePersistentIds() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief This class is responsible opening, reading, creating, and saving file contents.
FileObject acts as the interface with OS level files.  You create a new FileObject and pass into it a file's path and name.  The FileObject class supports three distinct operations for working with files:
<table border='1' cellpadding='1'><tr><th>Operation</th><th>%FileObject Method</th><th>Description</th></tr><tr><td>Read</td><td>openForRead()</td><td>Open the file for reading</td></tr><tr><td>Write</td><td>openForWrite()</td><td>Open the file for writing to and replace its contents (if any)</td></tr><tr><td>Append</td><td>openForAppend()</td><td>Open the file and start writing at its end</td></tr></table>
Before you may work with a file you need to use one of the three above methods on the FileObject.
@tsexample
// Create a file object for writing
%fileWrite = new FileObject();
// Open a file to write to, if it does not exist it will be created
%result = %fileWrite.OpenForWrite("./test.txt");
if ( %result )
{
   // Write a line to the text files
   %fileWrite.writeLine("READ. READ CODE. CODE");
}
// Close the file when finished
%fileWrite.close();
// Cleanup the file object
%fileWrite.delete();
// Create a file object for reading
%fileRead = new FileObject();
// Open a text file, if it exists
%result = %fileRead.OpenForRead("./test.txt");
if ( %result )
{
   // Read in the first line
   %line = %fileRead.readline();
   // Print the line we just read
   echo(%line);
}
// Close the file when finished
%fileRead.close();
// Cleanup the file object
%fileRead.delete();
@endtsexample
@ingroup FileSystem
 */
class  FileObject : public SimObject {
  public:
   /*! @brief Open a specified file for reading

There is no limit as to what kind of file you can read. Any format and data contained within is accessible, not just text

@param filename Path, name, and extension of file to be read@tsexample
// Create a file object for reading
%fileRead = new FileObject();

// Open a text file, if it exists
%result = %fileRead.OpenForRead("./test.txt");
@endtsexample

@return True if file was successfully opened, false otherwise
 */
   bool openForRead( string filename ) {}
   /*! @brief Open a specified file for writing

There is no limit as to what kind of file you can write. Any format and data is allowable, not just text

@param filename Path, name, and extension of file to write to@tsexample
// Create a file object for writing
%fileWrite = new FileObject();

// Open a file to write to, if it does not exist it will be created
%result = %fileWrite.OpenForWrite("./test.txt");
@endtsexample

@return True if file was successfully opened, false otherwise
 */
   bool openForWrite( string filename ) {}
   /*! @brief Open a specified file for writing, adding data to the end of the file

There is no limit as to what kind of file you can write. Any format and data is allowable, not just text. Unlike openForWrite(), which will erase an existing file if it is opened, openForAppend() preserves data in an existing file and adds to it.

@param filename Path, name, and extension of file to append to@tsexample
// Create a file object for writing
%fileWrite = new FileObject();

// Open a file to write to, if it does not exist it will be created
// If it does exist, whatever we write will be added to the end
%result = %fileWrite.OpenForAppend("./test.txt");
@endtsexample

@return True if file was successfully opened, false otherwise
 */
   bool openForAppend( string filename ) {}
   /*! @brief Determines if the parser for this FileObject has reached the end of the file

@tsexample
// Create a file object for reading
%fileRead = new FileObject();

// Open a text file, if it exists
%fileRead.OpenForRead("./test.txt");

// Keep reading until we reach the end of the file
while( !%fileRead.isEOF() )
{
   %line = %fileRead.readline();
   echo(%line);
}

// Made it to the end
echo("Finished reading file");
@endtsexample

@return True if the parser has reached the end of the file, false otherwise
 */
   bool isEOF() {}
   /*! @brief Read a line from file.

Emphasis on *line*, as in you cannot parse individual characters or chunks of data.  There is no limitation as to what kind of data you can read.

@tsexample
// Create a file object for reading
%fileRead = new FileObject();

// Open a text file, if it exists
%fileRead.OpenForRead("./test.txt");

// Read in the first line
%line = %fileRead.readline();

// Print the line we just read
echo(%line);
@endtsexample

@return String containing the line of data that was just read
 */
   const char* readLine() {}
   /*! @brief Read a line from the file without moving the stream position.

Emphasis on *line*, as in you cannot parse individual characters or chunks of data.  There is no limitation as to what kind of data you can read. Unlike readLine, the parser does not move forward after reading.

@param filename Path, name, and extension of file to be read@tsexample
// Create a file object for reading
%fileRead = new FileObject();

// Open a text file, if it exists
%fileRead.OpenForRead("./test.txt");

// Peek the first line
%line = %fileRead.peekLine();

// Print the line we just peeked
echo(%line);
// If we peek again...
%line = %fileRead.peekLine();

// We will get the same output as the first time
// since the stream did not move forward
echo(%line);
@endtsexample

@return String containing the line of data that was just peeked
 */
   const char* peekLine( int peekOffset=0 ) {}
   /*! @brief Write a line to the file, if it was opened for writing.

There is no limit as to what kind of text you can write. Any format and data is allowable, not just text. Be careful of what you write, as whitespace, current values, and literals will be preserved.

@param text The data we are writing out to file.@tsexample
// Create a file object for writing
%fileWrite = new FileObject();

// Open a file to write to, if it does not exist it will be created
%fileWrite.OpenForWrite("./test.txt");

// Write a line to the text files
%fileWrite.writeLine("READ. READ CODE. CODE");

@endtsexample

@return True if file was successfully opened, false otherwise
 */
   void writeLine( string text ) {}
   /*! @brief Close the file.

It is EXTREMELY important that you call this function when you are finished reading or writing to a file. Failing to do so is not only a bad programming practice, but could result in bad data or corrupt files. Remember: Open, Read/Write, Close, Delete...in that order!

@tsexample
// Create a file object for reading
%fileRead = new FileObject();

// Open a text file, if it exists
%fileRead.OpenForRead("./test.txt");

// Peek the first line
%line = %fileRead.peekLine();

// Print the line we just peeked
echo(%line);
// If we peek again...
%line = %fileRead.peekLine();

// We will get the same output as the first time
// since the stream did not move forward
echo(%line);

// Close the file when finished
%fileWrite.close();

// Cleanup the file object
%fileWrite.delete();
@endtsexample

 */
   void close() {}
   /*! FileObject.writeObject(SimObject, object prepend)@hide */
   void writeObject( string simName, string objName="" ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief Base class for working with streams.
You do not instantiate a StreamObject directly.  Instead, it is used as part of a FileStreamObject and ZipObject to support working with uncompressed and compressed files respectively.@tsexample
// You cannot actually declare a StreamObject
// Instead, use the derived class "FileStreamObject"
%fsObject = FileStreamObject();
@endtsexample
@see FileStreamObject for the derived class usable in script.
@see ZipObject where StreamObject is used to read and write to files within a zip archive.
@ingroup FileSystem
 */
class  StreamObject : public SimObject {
  public:
   /*! @brief Gets a printable string form of the stream's status

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
%fsObject.open("./test.txt", "read");

// Get the status and print it
%status = %fsObject.getStatus();
echo(%status);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return String containing status constant, one of the following:

	OK - Stream is active and no file errors

	IOError - Something went wrong during read or writing the stream

	EOS - End of Stream reached (mostly for reads)

	IllegalCall - An unsupported operation used.  Always w/ accompanied by AssertWarn

  Closed - Tried to operate on a closed stream (or detached filter)

	UnknownError - Catch all for an error of some kind

	Invalid - Entire stream is invalid

 */
   const char* getStatus() {}
   /*! @brief Tests if the stream has reached the end of the file

This is an alternative name for isEOF. Both functions are interchangeable. This simply exists for those familiar with some C++ file I/O standards.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
%fsObject.open("./test.txt", "read");

// Keep reading until we reach the end of the file
while( !%fsObject.isEOS() )
{
   %line = %fsObject.readLine();
   echo(%line);
}
// Made it to the end
echo("Finished reading file");

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return True if the parser has reached the end of the file, false otherwise
@see isEOF() */
   bool isEOS() {}
   /*! @brief Tests if the stream has reached the end of the file

This is an alternative name for isEOS. Both functions are interchangeable. This simply exists for those familiar with some C++ file I/O standards.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
%fsObject.open("./test.txt", "read");

// Keep reading until we reach the end of the file
while( !%fsObject.isEOF() )
{
   %line = %fsObject.readLine();
   echo(%line);
}
// Made it to the end
echo("Finished reading file");

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return True if the parser has reached the end of the file, false otherwise
@see isEOS() */
   bool isEOF() {}
   /*! @brief Gets the position in the stream

The easiest way to visualize this is to think of a cursor in a text file. If you have moved the cursor by five characters, the current position is 5. If you move ahead 10 more characters, the position is now 15. For StreamObject, when you read in the line the position is increased by the number of characters parsed, the null terminator, and a newline.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
// This file contains two lines of text repeated:
// Hello World
// Hello World
%fsObject.open("./test.txt", "read");

// Read in the first line
%line = %fsObject.readLine();

// Get the position of the stream
%position = %fsObject.getPosition();

// Print the current position
// Should be 13, 10 for the words, 1 for the space, 1 for the null terminator, and 1 for the newline
echo(%position);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return Number of bytes which stream has parsed so far, null terminators and newlines are included
@see setPosition() */
   S32 getPosition() {}
   /*! @brief Gets the position in the stream

The easiest way to visualize this is to think of a cursor in a text file. If you have moved the cursor by five characters, the current position is 5. If you move ahead 10 more characters, the position is now 15. For StreamObject, when you read in the line the position is increased by the number of characters parsed, the null terminator, and a newline. Using setPosition allows you to skip to specific points of the file.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
// This file contains the following two lines:
// 11111111111
// Hello World
%fsObject.open("./test.txt", "read");

// Skip ahead by 12, which will bypass the first line entirely
%fsObject.setPosition(12);

// Read in the next line
%line = %fsObject.readLine();

// Print the line just read in, should be "Hello World"
echo(%line);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return Number of bytes which stream has parsed so far, null terminators and newlines are included
@see getPosition() */
   bool setPosition( int newPosition ) {}
   /*! @brief Gets the size of the stream

The size is dependent on the type of stream being used. If it is a file stream, returned value will be the size of the file. If it is a memory stream, it will be the size of the allocated buffer.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
// This file contains the following two lines:
// HelloWorld
// HelloWorld
%fsObject.open("./test.txt", "read");

// Found out how large the file stream is
// Then print it to the console
// Should be 22
%streamSize = %fsObject.getStreamSize();
echo(%streamSize);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return Size of stream, in bytes
 */
   S32 getStreamSize() {}
   /*! @brief Read a line from the stream.

Emphasis on *line*, as in you cannot parse individual characters or chunks of data. There is no limitation as to what kind of data you can read.

@tsexample
// Create a file stream object for reading
// This file contains the following two lines:
// HelloWorld
// HelloWorld
%fsObject = new FileStreamObject();

%fsObject.open("./test.txt", "read");

// Read in the first line
%line = %fsObject.readLine();

// Print the line we just read
echo(%line);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return String containing the line of data that was just read
@see writeLine() */
   const char* readLine() {}
   /*! @brief Write a line to the stream, if it was opened for writing.

There is no limit as to what kind of data you can write. Any format and data is allowable, not just text. Be careful of what you write, as whitespace, current values, and literals will be preserved.

@param line The data we are writing out to file.@tsexample
// Create a file stream
%fsObject = new FileStreamObject();

// Open the file for writing
// If it does not exist, it is created. If it does exist, the file is cleared
%fsObject.open("./test.txt", "write");

// Write a line to the file
%fsObject.writeLine("Hello World");

// Write another line to the file
%fsObject.writeLine("Documentation Rocks!");

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@see readLine() */
   void writeLine( string line ) {}
   /*! @brief Read in a string and place it on the string table.

@param caseSensitive If false then case will not be taken into account when attempting to match the read in string with what is already in the string table.
@return The string that was read from the stream.
@see writeString()@note When working with these particular string reading and writing methods, the stream begins with the length of the string followed by the string itself, and does not include a NULL terminator. */
   String readSTString( bool caseSensitive=false ) {}
   /*! @brief Read a string up to a maximum of 256 characters@return The string that was read from the stream.
@see writeString()@note When working with these particular string reading and writing methods, the stream begins with the length of the string followed by the string itself, and does not include a NULL terminator. */
   String readString() {}
   /*! @brief Read in a string up to the given maximum number of characters.

@param maxLength The maximum number of characters to read in.
@return The string that was read from the stream.
@see writeLongString()@note When working with these particular string reading and writing methods, the stream begins with the length of the string followed by the string itself, and does not include a NULL terminator. */
   String readLongString( int maxLength ) {}
   /*! @brief Write out a string up to the maximum number of characters.

@param maxLength The maximum number of characters that will be written.
@param string The string to write out to the stream.
@see readLongString()@note When working with these particular string reading and writing methods, the stream begins with the length of the string followed by the string itself, and does not include a NULL terminator. */
   void writeLongString( int maxLength, string string ) {}
   /*! @brief Write out a string with a default maximum length of 256 characters.

@param string The string to write out to the stream
@param maxLength The maximum string length to write out with a default of 256 characters.  This value should not be larger than 256 as it is written to the stream as a single byte.
@see readString()@note When working with these particular string reading and writing methods, the stream begins with the length of the string followed by the string itself, and does not include a NULL terminator. */
   void writeString( string string, int maxLength=256 ) {}
   /*! @brief Copy from another StreamObject into this StreamObject

@param other The StreamObject to copy from.
@return True if the copy was successful.
 */
   bool copyFrom( SimObject other ) {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief A wrapper around StreamObject for parsing text and data from files.
FileStreamObject inherits from StreamObject and provides some unique methods for working with strings.  If you're looking for general file handling, you may want to use FileObject.
@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();
// Open a file for reading
%fsObject.open("./test.txt", "read");
// Get the status and print it
%status = %fsObject.getStatus();
echo(%status);
// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample
@see StreamObject for the list of inherited functions variables
@see FileObject for general file handling.
@ingroup FileSystem
 */
class  FileStreamObject : public StreamObject {
  public:
   /*! @brief Open a file for reading, writing, reading and writing, or appending

Using "Read" for the open mode allows you to parse the contents of file, but not making modifications. "Write" will create a new file if it does not exist, or erase the contents of an existing file when opened. Write also allows you to modify the contents of the file.

"ReadWrite" will provide the ability to parse data (read it in) and manipulate data (write it out) interchangeably. Keep in mind the stream can move during each operation. Finally, "WriteAppend" will open a file if it exists, but will not clear the contents. You can write new data starting  at the end of the files existing contents.

@param filename Name of file to open
@param openMode One of "Read", "Write", "ReadWrite" or "WriteAppend"

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
%fsObject.open("./test.txt", "read");

// Get the status and print it
%status = %fsObject.getStatus();
echo(%status);

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@return True if the file was successfully opened, false if something went wrong@see close() */
   bool open( string filename, string openMode ) {}
   /*! @brief Close the file. You can no longer read or write to it unless you open it again.

@tsexample
// Create a file stream object for reading
%fsObject = new FileStreamObject();

// Open a file for reading
%fsObject.open("./test.txt", "read");

// Always remember to close a file stream when finished
%fsObject.close();
@endtsexample

@see open() */
   void close() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
/*!
@brief Base class for messages
Message is the base class for C++ defined messages, and may also be used in script for script defined messages if no C++ subclass is appropriate.
Messages are reference counted and will be automatically deleted when their reference count reaches zero. When you dispatch a message, a reference will be added before the dispatch and freed after the dispatch. This allows for temporary messages with no additional code. If you want to keep the message around, for example to dispatch it to multiple queues, call addReference() before dispatching it and freeReference() when you are done with it. Never delete a Message object directly unless addReference() has not been called or the message has not been dispatched.
Message IDs are pooled similarly to datablocks, with the exception that IDs are reused. If you keep a message for longer than a single dispatch, then you should ensure that you clear any script variables that refer to it after the last freeReference(). If you don't, then it is probable that the object ID will become valid again in the future and could cause hard to track down bugs.
Messages have a unique type to simplify message handling code. For object messages, the type is defined as either the script defined class name or the C++ class name if no script class was defined. The message type may be obtained through the getType() method.
By convention, any data for the message is held in script accessible fields. Messages that need to be handled in C++ as well as script provide the relevant data through persistent fields in a subclass of Message to provide best performance on the C++ side. Script defined messages usually their through dynamic fields, and may be accessed in C++ using the SimObject::getDataField() method.
@ingroup Messaging
 */
class  Message : public SimObject {
  public:
      /*! Script callback when a message is first created and registered.

@tsexample
function Message::onAdd(%this)
{
	// Perform on add code here
}
@endtsexample

 */
      void onAdd();
      /*! Script callback when a message is deleted.

@tsexample
function Message::onRemove(%this)
{
	// Perform on remove code here
}
@endtsexample

 */
      void onRemove();
   /*! Get message type (script class name or C++ class name if no script defined class) */
   const char * getType() {}
   /*! Increment the reference count for this message */
   void addReference() {}
   /*! Decrement the reference count for this message */
   void freeReference() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
};
class  RenderTextureObject : public SimObject {
  public:
   /*! resize (recreate) a render texture */
   bool Resize( int width, int height, bool copyImage=true, Color copyColor=WHITE ) {}
   /*! Begins drawing to render texture */
   void BeginTextureMode() {}
   /*!  End drawing to render texture */
   void EndTextureMode() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
   /*!
   Set the width of the RenderTexture
   
    */
   int width;
   /*!
   Set the width of the RenderTexture
   
    */
   int height;
   /*!
   get the Id of the Texture (readonly )
   
    */
   int Texture;
};
class  Camera2DObject : public SimObject {
  public:
   /*! Get 2D screen space position for a 2D camera world space position */
   Vector2 getWorldToScreen( Vector2 position ) {}
   /*! Get 2D world space position for a 2D camera screen space position (e.g. mouse to world) */
   Vector2 getScreenToWorld( Vector2 position ) {}
   /*! Get the 2D camera transform matrix */
   Matrix getCameraMatrix() {}
   /*! @name Ungrouped
   @{ */
   /*! */
   /// @}
   /*! @name Object
   @{ */
   /*! */
   /// @}
   /*! @name Editing
   @{ */
   /*! */
   /// @}
   /*! @name Persistence
   @{ */
   /*! */
   /// @}
   /*!
   Camera screen offset
   
    */
   Vector2 offset;
   /*!
   Camera world target
   
    */
   Vector2 target;
   /*!
   Camera rotation in degrees
   
    */
   float rotation;
   /*!
   Camera zoom level
   
    */
   float zoom;
};
namespace Global {
   Script createWaterPlaneDemo()...) {}
   Script creategridtest()...) {}
   Script createTerrainDemo()...) {}
   Script createWaterPlane( string type, string sunObject, string cameraObject, string size, string position )...) {}
   Script createSun( string terrainRadiusNeg, string sunModel )...) {}
   Script updateDocu()...) {}
   Script rl()...) {}
   Script MainLoop()...) {}
   Script MainShutDown()...) {}
   Script MainInit()...) {}
   /*! Call draw on all objects */
   void ClientContainerDrawObjects() {}
   /*! Casts a ray into the container and returns the closest hit SceneObject and collision info. */
   const char* ClientContainerRayCast( Ray ray ) {}
   /*! Returns true if no SceneObject intersects the given bounding box. */
   bool ClientContainerBoxEmpty( Vector3 minBounds, Vector3 maxBounds ) {}
   /*! Returns a SimSet ID containing all SceneObjects within the specified bounding box.
NOTE: You should delete the returned SimSet when done to avoid leaks! */
   S32 ClientContainerGetBoxObjects( Vector3 minBounds, Vector3 maxBounds ) {}
   /*! For Debug ... use ContainerGetBoxObjects with a big box to get all objects in as a SimSet  */
   void ClientContainerListObjects() {}
   /*! Create or reinit global BatchRender */
   void BatchCreate( int count ) {}
   /*! push variables $b[stack]x, y, z, w */
   bool BatchPush( int index, int stack=0 ) {}
   /*! setup variables $b[stack]x, y, z, w */
   bool BatchPull( int index, int stack=0 ) {}
   /*! push Vector2 variables */
   bool BatchPushVec2( int index, int stack, String var1, String var2 ) {}
   /*! pull Vector2 variables */
   bool BatchPullVec2( int index, int stack, String var1, String var2 ) {}
   /*! Dump rects from the global BatchRender */
   bool BatchDumpRects( Color color=DARKPURPLE ) {}
   /*! Draw rects from the global BatchRender */
   bool BatchDrawRects( Color color=DARKPURPLE ) {}
   /*! Draw rects from the global BatchRender 3th stack is used for colors */
   bool BatchDrawColoredRects( Color color=DARKPURPLE ) {}
   /*! Draw lines from the global BatchRender */
   bool BatchDrawLines( Color color=DARKPURPLE ) {}
   /*! Draw lines from the global BatchRender 3th stack is used for colors */
   bool BatchDrawColoredLines( Color color=DARKPURPLE ) {}
   /*! @name Clipboard
   
   Miscellaneous functions to control the clipboard and clear the console.
   @{ */
   /*! */
   /*! @brief Get text from the clipboard.

@internal */
   const char* getClipboard() {}
   /*! @brief Set the system clipboard.

@internal */
   bool setClipboard( string text ) {}
   /// @}
   /*! @brief Reset the timestamp to 0 ms.

@ingroup Console */
   void resetTimeStamp() {}
   /*! @brief Get the initialization time in miliseconds.

@internal */
   int getInitTime() {}
   /*! @name ConsoleDoc
   
   Console self-documentation functions. These output psuedo C++ suitable for feeeding through Doxygen or another auto documentation tool.
   @{ */
   /*! */
   /*! @brief Dumps all declared console classes to the console.

@param dumpScript Optional parameter specifying whether or not classes defined in script should be dumped.
@param dumpEngine Optional parameter specifying whether or not classes defined in the engine should be dumped.
@ingroup Logging */
   void dumpConsoleClasses( bool dumpScript=true, bool dumpEngine=true ) {}
   /*! @brief Dumps all declared console functions to the console.
@param dumpScript Optional parameter specifying whether or not functions defined in script should be dumped.
@param dumpEngine Optional parameter specitying whether or not functions defined in the engine should be dumped.
@ingroup Logging */
   void dumpConsoleFunctions( bool dumpScript=true, bool dumpEngine=true ) {}
   /// @}
   /*! Return the integer character code value corresponding to the first character in the given string.
@param chr a (one-character) string.
@return the UTF32 code value for the first character in the given string.
@ingroup Strings */
   int strasc( string chr ) {}
   /*! Format the given value as a string using printf-style formatting.
@param format A printf-style format string.
@param value The value argument matching the given format string.

@tsexample
// Convert the given integer value to a string in a hex notation.
%hex = strformat( "%x", %value );
@endtsexample
@ingroup Strings
@see http://en.wikipedia.org/wiki/Printf */
   const char* strformat( string format, string value ) {}
   /*! Compares two strings using case-<b>sensitive</b> comparison.
@param str1 The first string.
@param str2 The second string.
@return 0 if both strings are equal, a value <0 if the first character different in str1 has a smaller character code value than the character at the same position in str2, and a value >1 otherwise.

@tsexample
if( strcmp( %var, "foobar" ) == 0 )
   echo( "%var is equal to 'foobar'" );
@endtsexample
@see stricmp
@see strnatcmp
@ingroup Strings */
   S32 strcmp( string str1, string str2 ) {}
   /*! Compares two strings using case-<b>insensitive</b> comparison.
@param str1 The first string.
@param str2 The second string.
@return 0 if both strings are equal, a value <0 if the first character different in str1 has a smaller character code value than the character at the same position in str2, and a value >0 otherwise.

@tsexample
if( stricmp( "FOObar", "foobar" ) == 0 )
   echo( "this is always true" );
@endtsexample
@see strcmp
@see strinatcmp
@ingroup Strings */
   S32 stricmp( string str1, string str2 ) {}
   /*! Compares two strings using "natural order" case-<b>sensitive</b> comparison.
Natural order means that rather than solely comparing single character code values, strings are ordered in a natural way.  For example, the string "hello10" is considered greater than the string "hello2" even though the first numeric character in "hello10" actually has a smaller character value than the corresponding character in "hello2".  However, since 10 is greater than 2, strnatcmp will put "hello10" after "hello2".
@param str1 The first string.
@param str2 The second string.

@return 0 if the strings are equal, a value >0 if @a str1 comes after @a str2 in a natural order, and a value <0 if @a str1 comes before @a str2 in a natural order.

@tsexample
// Bubble sort 10 elements of %array using natural order
do
{
   %swapped = false;
   for( %i = 0; %i < 10 - 1; %i ++ )
      if( strnatcmp( %array[ %i ], %array[ %i + 1 ] ) > 0 )
      {
         %temp = %array[ %i ];
         %array[ %i ] = %array[ %i + 1 ];
         %array[ %i + 1 ] = %temp;
         %swapped = true;
      }
}
while( %swapped );
@endtsexample
@see strcmp
@see strinatcmp
@ingroup Strings */
   S32 strnatcmp( string str1, string str2 ) {}
   /*! Compares two strings using "natural order" case-<b>insensitive</b> comparison.
Natural order means that rather than solely comparing single character code values, strings are ordered in a natural way.  For example, the string "hello10" is considered greater than the string "hello2" even though the first numeric character in "hello10" actually has a smaller character value than the corresponding character in "hello2".  However, since 10 is greater than 2, strnatcmp will put "hello10" after "hello2".
@param str1 The first string.
@param str2 The second string.
@return 0 if the strings are equal, a value >0 if @a str1 comes after @a str2 in a natural order, and a value <0 if @a str1 comes before @a str2 in a natural order.

@tsexample

// Bubble sort 10 elements of %array using natural order
do
{
   %swapped = false;
   for( %i = 0; %i < 10 - 1; %i ++ )
      if( strnatcmp( %array[ %i ], %array[ %i + 1 ] ) > 0 )
      {
         %temp = %array[ %i ];
         %array[ %i ] = %array[ %i + 1 ];
         %array[ %i + 1 ] = %temp;
         %swapped = true;
      }
}
while( %swapped );
@endtsexample
@see stricmp
@see strnatcmp
@ingroup Strings */
   S32 strinatcmp( string str1, string str2 ) {}
   /*! Get the length of the given string in bytes.
@note This does <b>not</b> return a true character count for strings with multi-byte characters!
@param str A string.
@return The length of the given string in bytes.
@ingroup Strings */
   S32 strlen( string str ) {}
   /*! Calculate the length of a string in characters, skipping everything between and including first and last.
@param str A string.
@param first First character to look for to skip block of text.
@param last Second character to look for to skip block of text.
@return The length of the given string skipping blocks of text between characters.
@ingroup Strings */
   S32 strlenskip( string str, string first, string last ) {}
   /*! Find the start of @a substring in the given @a string searching from left to right.
@param string The string to search.
@param substring The string to search for.
@return The index into @a string at which the first occurrence of @a substring was found or -1 if @a substring could not be found.

@tsexample
strstr( "abcd", "c" ) // Returns 2.
@endtsexample
@ingroup Strings */
   S32 strstr( string string, string substring ) {}
   /*! Find the start of @a needle in @a haystack searching from left to right beginning at the given offset.
@param haystack The string to search.
@param needle The string to search for.
@return The index at which the first occurrence of @a needle was found in @a haystack or -1 if no match was found.

@tsexample
strpos( "b ab", "b", 1 ) // Returns 3.
@endtsexample
@ingroup Strings */
   S32 strpos( string haystack, string needle, int offset=0 ) {}
   /*! Find the start of @a needle in @a haystack searching from right to left beginning at the given offset.
@param haystack The string to search.
@param needle The string to search for.
@return The index at which the first occurrence of @a needle was found in @a heystack or -1 if no match was found.

@tsexample
strposr( "b ab", "b", 1 ) // Returns 2.
@endtsexample
@ingroup Strings */
   S32 strposr( string haystack, string needle, int offset=0 ) {}
   /*! Remove leading whitespace from the string.
@param str A string.
@return A string that is the same as @a str but with any leading (i.e. leftmost) whitespace removed.

@tsexample
ltrim( "   string  " ); // Returns "string  ".
@endtsexample
@see rtrim
@see trim
@ingroup Strings */
   const char* ltrim( string str ) {}
   /*! Remove trailing whitespace from the string.
@param str A string.
@return A string that is the same as @a str but with any trailing (i.e. rightmost) whitespace removed.

@tsexample
rtrim( "   string  " ); // Returns "   string".
@endtsexample
@see ltrim
@see trim
@ingroup Strings */
   const char* rtrim( string str ) {}
   /*! Remove leading and trailing whitespace from the string.
@param str A string.
@return A string that is the same as @a str but with any leading (i.e. leftmost) and trailing (i.e. rightmost) whitespace removed.

@tsexample
trim( "   string  " ); // Returns "string".
@endtsexample
@ingroup Strings */
   const char* trim( string str ) {}
   /*! Remove all occurrences of characters contained in @a chars from @a str.
@param str The string to filter characters out from.
@param chars A string of characters to filter out from @a str.
@return A version of @a str with all occurrences of characters contained in @a chars filtered out.

@tsexample
stripChars( "teststring", "se" ); // Returns "tttring".@endtsexample
@ingroup Strings */
   const char* stripChars( string str, string chars ) {}
   /*! Sanitizes a string of common name issues, such as:
Starting with numbers, replacing spaces with _, and removing any name un-compliant characters such as .,- etc
@param str The string to sanitize.
@return A version of @a str with all occurrences of invalid characters removed.

@tsexample
cleanString( "123 .-_abc"); // Returns "__abc".@endtsexample
@ingroup Strings */
   const char* sanitizeString( string str ) {}
   /*! Return an all lower-case version of the given string.
@param str A string.
@return A version of @a str with all characters converted to lower-case.

@tsexample
strlwr( "TesT1" ) // Returns "test1"
@endtsexample
@see strupr
@ingroup Strings */
   const char* strlwr( string str ) {}
   /*! Return an all upper-case version of the given string.
@param str A string.
@return A version of @a str with all characters converted to upper-case.

@tsexample
strupr( "TesT1" ) // Returns "TEST1"
@endtsexample
@see strlwr
@ingroup Strings */
   const char* strupr( string str ) {}
   /*! Find the first occurrence of the given character in @a str.
@param str The string to search.
@param chr The character to search for.  Only the first character from the string is taken.
@return The remainder of the input string starting with the given character or the empty string if the character could not be found.

@see strrchr
@ingroup Strings */
   const char* strchr( string str, string chr ) {}
   /*! Find the last occurrence of the given character in @a str.@param str The string to search.
@param chr The character to search for.  Only the first character from the string is taken.
@return The remainder of the input string starting with the given character or the empty string if the character could not be found.

@see strchr
@ingroup Strings */
   const char* strrchr( string str, string chr ) {}
   /*! Replace all occurrences of @a from in @a source with @a to.
@param source The string in which to replace the occurrences of @a from.
@param from The string to replace in @a source.
@param to The string with which to replace occurrences of @from.
@return A string with all occurrences of @a from in @a source replaced by @a to.

@tsexample
strreplace( "aabbccbb", "bb", "ee" ) // Returns "aaeeccee".
@endtsexample
@ingroup Strings */
   const char* strreplace( string source, string from, string to ) {}
   /*! Return a string that repeats @a str @a numTimes number of times delimiting each occurrence with @a delimiter.
@param str The string to repeat multiple times.
@param numTimes The number of times to repeat @a str in the result string.
@param delimiter The string to put between each repetition of @a str.
@return A string containing @a str repeated @a numTimes times.

@tsexample
strrepeat( "a", 5, "b" ) // Returns "ababababa".
@endtsexample
@ingroup Strings */
   const char* strrepeat( string str, int numTimes, string delimiter="" ) {}
   /*! @brief Return a substring of @a str starting at @a start and continuing either through to the end of @a str (if @a numChars is -1) or for @a numChars characters (except if this would exceed the actual source string length).
@param str The string from which to extract a substring.
@param start The offset at which to start copying out characters.
@param numChars Optional argument to specify the number of characters to copy.  If this is -1, all characters up the end of the input string are copied.
@return A string that contains the given portion of the input string.

@tsexample
getSubStr( "foobar", 1, 2 ) // Returns "oo".
@endtsexample

@ingroup Strings */
   const char* getSubStr( string str, int start, int numChars=-1 ) {}
   /*! Match a pattern against a string.
@param pattern The wildcard pattern to match against.  The pattern can include characters, '*' to match any number of characters and '?' to match a single character.
@param str The string which should be matched against @a pattern.
@param caseSensitive If true, characters in the pattern are matched in case-sensitive fashion against this string.  If false, differences in casing are ignored.
@return True if @a str matches the given @a pattern.

@tsexample
strIsMatchExpr( "f?o*R", "foobar" ) // Returns true.
@endtsexample
@see strIsMatchMultipleExpr
@ingroup Strings */
   bool strIsMatchExpr( string pattern, string str, bool caseSensitive=false ) {}
   /*! Match a multiple patterns against a single string.
@param patterns A tab-separated list of patterns.  Each pattern can include charaters, '*' to match any number of characters and '?' to match a single character.  Each of the patterns is tried in turn.
@param str The string which should be matched against @a patterns.
@param caseSensitive If true, characters in the pattern are matched in case-sensitive fashion against this string.  If false, differences in casing are ignored.
@return True if @a str matches any of the given @a patterns.

@tsexample
strIsMatchMultipleExpr( "*.cs *.gui *.mis", "mymission.mis" ) // Returns true.
@endtsexample
@see strIsMatchExpr
@ingroup Strings */
   bool strIsMatchMultipleExpr( string patterns, string str, bool caseSensitive=false ) {}
   /*! Get the numeric suffix of the given input string.
@param str The string from which to read out the numeric suffix.
@return The numeric value of the number suffix of @a str or -1 if @a str has no such suffix.

@tsexample
getTrailingNumber( "test123" ) // Returns '123'.
@endtsexample

@see stripTrailingNumber
@ingroup Strings */
   S32 getTrailingNumber( string str ) {}
   /*! Strip a numeric suffix from the given string.
@param str The string from which to strip its numeric suffix.
@return The string @a str without its number suffix or the original string @a str if it has no such suffix.

@tsexample
stripTrailingNumber( "test123" ) // Returns "test".
@endtsexample

@see getTrailingNumber
@ingroup Strings */
   String stripTrailingNumber( string str ) {}
   /*! Get the first occuring number from @a str.
@param str The string from which to read out the first number.
@return String representation of the number or  if no number.

 */
   String getFirstNumber( string str ) {}
   /*! Test whether the character at the given position is a whitespace character.
Characters such as tab, space, or newline are considered whitespace.
@param str The string to test.
@param index The index of a character in @a str.
@return True if the character at the given index in @a str is a whitespace character; false otherwise.

@see isalnum
@ingroup Strings */
   bool isspace( string str, int index ) {}
   /*! Test whether the character at the given position is an alpha-numeric character.
Alpha-numeric characters are characters that are either alphabetic (a-z, A-Z) or numbers (0-9).
@param str The string to test.
@param index The index of a character in @a str.
@return True if the character at the given index in @a str is an alpha-numeric character; false otherwise.

@see isspace
@ingroup Strings */
   bool isalnum( string str, int index ) {}
   /*! Test whether the given string begins with the given prefix.
@param str The string to test.
@param prefix The potential prefix of @a str.
@param caseSensitive If true, the comparison will be case-sensitive; if false, differences in casing will not be taken into account.
@return True if the first characters in @a str match the complete contents of @a prefix; false otherwise.

@tsexample
startsWith( "TEST123", "test" ) // Returns true.
@endtsexample
@see endsWith
@ingroup Strings */
   bool startsWith( string str, string prefix, bool caseSensitive=false ) {}
   /*! @brief Test whether the given string ends with the given suffix.

@param str The string to test.
@param suffix The potential suffix of @a str.
@param caseSensitive If true, the comparison will be case-sensitive; if false, differences in casing will not be taken into account.
@return True if the last characters in @a str match the complete contents of @a suffix; false otherwise.

@tsexample
startsWith( "TEST123", "123" ) // Returns true.
@endtsexample

@see startsWith
@ingroup Strings */
   bool endsWith( string str, string suffix, bool caseSensitive=false ) {}
   /*! Find the first occurrence of the given character in the given string.
@param str The string to search.
@param chr The character to look for.  Only the first character of this string will be searched for.
@param start The index into @a str at which to start searching for the given character.
@return The index of the first occurrence of @a chr in @a str or -1 if @a str does not contain the given character.

@tsexample
strchrpos( "test", "s" ) // Returns 2.
@endtsexample
@ingroup Strings */
   S32 strchrpos( string str, string chr, int start=0 ) {}
   /*! Find the last occurrence of the given character in the given string.
@param str The string to search.
@param chr The character to look for.  Only the first character of this string will be searched for.
@param start The index into @a str at which to start searching for the given character.
@return The index of the last occurrence of @a chr in @a str or -1 if @a str does not contain the given character.

@tsexample
strrchrpos( "test", "t" ) // Returns 3.
@endtsexample
@ingroup Strings */
   S32 strrchrpos( string str, string chr, int start=0 ) {}
   /*! Parse a Toggle Case word into separate words.
@param str The string to parse.
@return new string space separated.

@tsexample
strToggleCaseToWords( "HelloWorld" ) // Returns "Hello World".
@endtsexample
@ingroup Strings */
   const char* strToggleCaseToWords( string str ) {}
   /*! Returns true if the string is an integer.
@param str The string to test.
@return true if @a str is an integer and false if not

@tsexample
isInt( "13" ) // Returns true.
@endtsexample
@ingroup Strings */
   bool isInt( string str ) {}
   /*! Returns true if the string is a float.
@param str The string to test.
@param sciOk Test for correct scientific notation and accept it (ex. 1.2e+14)@return true if @a str is a float and false if not

@tsexample
isFloat( "13.5" ) // Returns true.
@endtsexample
@ingroup Strings */
   bool isFloat( string str, bool sciOk=false ) {}
   /*! Returns true if the string is a valid port number.
@param str The string to test.
@return true if @a str is a port and false if not

@tsexample
isValidPort( "8080" ) // Returns true.
@endtsexample
@ingroup Strings */
   bool isValidPort( string str ) {}
   /*! Returns true if the string is a valid ip address, excepts localhost.
@param str The string to test.
@return true if @a str is a valid ip address and false if not

@tsexample
isValidIP( "localhost" ) // Returns true.
@endtsexample
@ingroup Strings */
   bool isValidIP( string str ) {}
   /*! [string1, string2, ...]Adds case sensitive strings to the StringTable. */
   void addCaseSensitiveStrings(...) {}
   /*! Extract the word at the given @a index in the whitespace-separated list in @a text.
Words in @a text must be separated by newlines, spaces, and/or tabs.
@param text A whitespace-separated list of words.
@param index The zero-based index of the word to extract.
@return The word at the given index or "" if the index is out of range.

@tsexample
getWord( "a b c", 1 ) // Returns "b"
@endtsexample

@see getWords
@see getWordCount
@see getToken
@see getField
@see getRecord
@ingroup FieldManip */
   const char* getWord( string text, int index ) {}
   /*! Extract a range of words from the given @a startIndex onwards thru @a endIndex.
Words in @a text must be separated by newlines, spaces, and/or tabs.
@param text A whitespace-separated list of words.
@param startIndex The zero-based index of the first word to extract from @a text.
@param endIndex The zero-based index of the last word to extract from @a text.  If this is -1, all words beginning with @a startIndex are extracted from @a text.
@return A string containing the specified range of words from @a text or "" if @a startIndex is out of range or greater than @a endIndex.

@tsexample
getWords( "a b c d", 1, 2, ) // Returns "b c"
@endtsexample

@see getWord
@see getWordCount
@see getTokens
@see getFields
@see getRecords
@ingroup FieldManip */
   const char* getWords( string text, int startIndex, int endIndex=-1 ) {}
   /*! Replace the word in @a text at the given @a index with @a replacement.
Words in @a text must be separated by newlines, spaces, and/or tabs.
@param text A whitespace-separated list of words.
@param index The zero-based index of the word to replace.
@param replacement The string with which to replace the word.
@return A new string with the word at the given @a index replaced by @a replacement or the original string if @a index is out of range.

@tsexample
setWord( "a b c d", 2, "f" ) // Returns "a b f d"
@endtsexample

@see getWord
@see setToken
@see setField
@see setRecord
@ingroup FieldManip */
   const char* setWord( string text, int index, string replacement ) {}
   /*! Remove the word in @a text at the given @a index.
Words in @a text must be separated by newlines, spaces, and/or tabs.
@param text A whitespace-separated list of words.
@param index The zero-based index of the word in @a text.
@return A new string with the word at the given index removed or the original string if @a index is out of range.

@tsexample
removeWord( "a b c d", 2 ) // Returns "a b d"
@endtsexample

@see removeToken
@see removeField
@see removeRecord
@ingroup FieldManip */
   const char* removeWord( string text, int index ) {}
   /*! Return the number of whitespace-separated words in @a text.
Words in @a text must be separated by newlines, spaces, and/or tabs.
@param text A whitespace-separated list of words.
@return The number of whitespace-separated words in @a text.

@tsexample
getWordCount( "a b c d e" ) // Returns 5
@endtsexample

@see getTokenCount
@see getFieldCount
@see getRecordCount
@ingroup FieldManip */
   S32 getWordCount( string text ) {}
   /*! @brief returns month as a word given a number or "" if number is bad@return month as a word given a number or "" if number is bad@ingroup FileSystem */
   String monthNumToStr( int num, bool abbreviate=false ) {}
   /*! @brief returns weekday as a word given a number or "" if number is bad@return weekday as a word given a number or "" if number is bad@ingroup FileSystem */
   String weekdayNumToStr( int num, bool abbreviate=false ) {}
   /*! Extract the field at the given @a index in the newline and/or tab separated list in @a text.
Fields in @a text must be separated by newlines and/or tabs.
@param text A list of fields separated by newlines and/or tabs.
@param index The zero-based index of the field to extract.
@return The field at the given index or "" if the index is out of range.

@tsexample
getField( "a b" TAB "c d" TAB "e f", 1 ) // Returns "c d"
@endtsexample

@see getFields
@see getFieldCount
@see getWord
@see getRecord
@ingroup FieldManip */
   const char* getField( string text, int index ) {}
   /*! Extract a range of fields from the given @a startIndex onwards thru @a endIndex.
Fields in @a text must be separated by newlines and/or tabs.
@param text A list of fields separated by newlines and/or tabs.
@param startIndex The zero-based index of the first field to extract from @a text.
@param endIndex The zero-based index of the last field to extract from @a text.  If this is -1, all fields beginning with @a startIndex are extracted from @a text.
@return A string containing the specified range of fields from @a text or "" if @a startIndex is out of range or greater than @a endIndex.

@tsexample
getFields( "a b" TAB "c d" TAB "e f", 1 ) // Returns "c d" TAB "e f"
@endtsexample

@see getField
@see getFieldCount
@see getWords
@see getRecords
@ingroup FieldManip */
   const char* getFields( string text, int startIndex, int endIndex=-1 ) {}
   /*! Replace the field in @a text at the given @a index with @a replacement.
Fields in @a text must be separated by newlines and/or tabs.
@param text A list of fields separated by newlines and/or tabs.
@param index The zero-based index of the field to replace.
@param replacement The string with which to replace the field.
@return A new string with the field at the given @a index replaced by @a replacement or the original string if @a index is out of range.

@tsexample
setField( "a b" TAB "c d" TAB "e f", 1, "g h" ) // Returns "a b" TAB "g h" TAB "e f"
@endtsexample

@see getField
@see setWord
@see setRecord
@ingroup FieldManip */
   const char* setField( string text, int index, string replacement ) {}
   /*! Remove the field in @a text at the given @a index.
Fields in @a text must be separated by newlines and/or tabs.
@param text A list of fields separated by newlines and/or tabs.
@param index The zero-based index of the field in @a text.
@return A new string with the field at the given index removed or the original string if @a index is out of range.

@tsexample
removeField( "a b" TAB "c d" TAB "e f", 1 ) // Returns "a b" TAB "e f"
@endtsexample

@see removeWord
@see removeRecord
@ingroup FieldManip */
   const char* removeField( string text, int index ) {}
   /*! Return the number of newline and/or tab separated fields in @a text.
@param text A list of fields separated by newlines and/or tabs.
@return The number of newline and/or tab sepearated elements in @a text.

@tsexample
getFieldCount( "a b" TAB "c d" TAB "e f" ) // Returns 3
@endtsexample

@see getWordCount
@see getRecordCount
@ingroup FieldManip */
   S32 getFieldCount( string text ) {}
   /*! Extract the record at the given @a index in the newline-separated list in @a text.
Records in @a text must be separated by newlines.
@param text A list of records separated by newlines.
@param index The zero-based index of the record to extract.
@return The record at the given index or "" if @a index is out of range.

@tsexample
getRecord( "a b" NL "c d" NL "e f", 1 ) // Returns "c d"
@endtsexample

@see getRecords
@see getRecordCount
@see getWord
@see getField
@ingroup FieldManip */
   const char* getRecord( string text, int index ) {}
   /*! Extract a range of records from the given @a startIndex onwards thru @a endIndex.
Records in @a text must be separated by newlines.
@param text A list of records separated by newlines.
@param startIndex The zero-based index of the first record to extract from @a text.
@param endIndex The zero-based index of the last record to extract from @a text.  If this is -1, all records beginning with @a startIndex are extracted from @a text.
@return A string containing the specified range of records from @a text or "" if @a startIndex is out of range or greater than @a endIndex.

@tsexample
getRecords( "a b" NL "c d" NL "e f", 1 ) // Returns "c d" NL "e f"
@endtsexample

@see getRecord
@see getRecordCount
@see getWords
@see getFields
@ingroup FieldManip */
   const char* getRecords( string text, int startIndex, int endIndex=-1 ) {}
   /*! Replace the record in @a text at the given @a index with @a replacement.
Records in @a text must be separated by newlines.
@param text A list of records separated by newlines.
@param index The zero-based index of the record to replace.
@param replacement The string with which to replace the record.
@return A new string with the record at the given @a index replaced by @a replacement or the original string if @a index is out of range.

@tsexample
setRecord( "a b" NL "c d" NL "e f", 1, "g h" ) // Returns "a b" NL "g h" NL "e f"
@endtsexample

@see getRecord
@see setWord
@see setField
@ingroup FieldManip */
   const char* setRecord( string text, int index, string replacement ) {}
   /*! Remove the record in @a text at the given @a index.
Records in @a text must be separated by newlines.
@param text A list of records separated by newlines.
@param index The zero-based index of the record in @a text.
@return A new string with the record at the given @a index removed or the original string if @a index is out of range.

@tsexample
removeRecord( "a b" NL "c d" NL "e f", 1 ) // Returns "a b" NL "e f"
@endtsexample

@see removeWord
@see removeField
@ingroup FieldManip */
   const char* removeRecord( string text, int index ) {}
   /*! Return the number of newline-separated records in @a text.
@param text A list of records separated by newlines.
@return The number of newline-sepearated elements in @a text.

@tsexample
getRecordCount( "a b" NL "c d" NL "e f" ) // Returns 3
@endtsexample

@see getWordCount
@see getFieldCount
@ingroup FieldManip */
   S32 getRecordCount( string text ) {}
   /*! Return the first word in @a text.
@param text A list of words separated by newlines, spaces, and/or tabs.
@return The word at index 0 in @a text or "" if @a text is empty.

@note This is equal to 
@tsexample_nopar
getWord( text, 0 )
@endtsexample

@see getWord
@ingroup FieldManip */
   const char* firstWord( string text ) {}
   /*! Return all but the first word in @a text.
@param text A list of words separated by newlines, spaces, and/or tabs.
@return @a text with the first word removed.

@note This is equal to 
@tsexample_nopar
getWords( text, 1 )
@endtsexample

@see getWords
@ingroup FieldManip */
   const char* restWords( string text ) {}
   /*! Tokenize a string using a set of delimiting characters.
This function first skips all leading charaters in @a str that are contained in @a delimiters. From that position, it then scans for the next character in @a str that is contained in @a delimiters and stores all characters from the starting position up to the first delimiter in a variable in the current scope called @a token.  Finally, it skips all characters in @a delimiters after the token and then returns the remaining string contents in @a str.

To scan out all tokens in a string, call this function repeatedly by passing the result it returns each time as the new @a str until the function returns "".

@param str A string.
@param token The name of the variable in which to store the current token.  This variable is set in the scope in which nextToken is called.
@param delimiters A string of characters.  Each character is considered a delimiter.
@return The remainder of @a str after the token has been parsed out or "" if no more tokens were found in @a str.

@tsexample
// Prints:
// a
// b
// c
%str = "a   b c";
while ( %str !$= "" )
{
   // First time, stores "a" in the variable %token and sets %str to "b c".
   %str = nextToken( %str, "token", " " );
   echo( %token );
}
@endtsexample

@ingroup Strings */
   const char* nextToken( string str1, string token, string delim ) {}
   /*! Extract the substring at the given @a index in the @a delimiters separated list in @a text.
@param text A @a delimiters list of substrings.
@param delimiters Character or characters that separate the list of substrings in @a text.
@param index The zero-based index of the substring to extract.
@return The substring at the given index or "" if the index is out of range.

@tsexample
getToken( "a b c d", " ", 2 ) // Returns "c"
@endtsexample

@see getTokens
@see getTokenCount
@see getWord
@see getField
@see getRecord
@ingroup FieldManip */
   const char* getToken( string text, string delimiters, int index ) {}
   /*! Extract a range of substrings separated by @a delimiters at the given @a startIndex onwards thru @a endIndex.
@param text A @a delimiters list of substrings.
@param delimiters Character or characters that separate the list of substrings in @a text.
@param startIndex The zero-based index of the first substring to extract from @a text.
@param endIndex The zero-based index of the last substring to extract from @a text.  If this is -1, all words beginning with @a startIndex are extracted from @a text.
@return A string containing the specified range of substrings from @a text or "" if @a startIndex is out of range or greater than @a endIndex.

@tsexample
getTokens( "a b c d", " ", 1, 2, ) // Returns "b c"
@endtsexample

@see getToken
@see getTokenCount
@see getWords
@see getFields
@see getRecords
@ingroup FieldManip */
   const char* getTokens( string text, string delimiters, int startIndex, int endIndex=-1 ) {}
   /*! Replace the substring in @a text separated by @a delimiters at the given @a index with @a replacement.
@param text A @a delimiters list of substrings.
@param delimiters Character or characters that separate the list of substrings in @a text.
@param index The zero-based index of the substring to replace.
@param replacement The string with which to replace the substring.
@return A new string with the substring at the given @a index replaced by @a replacement or the original string if @a index is out of range.

@tsexample
setToken( "a b c d", " ", 2, "f" ) // Returns "a b f d"
@endtsexample

@see getToken
@see setWord
@see setField
@see setRecord
@ingroup FieldManip */
   const char* setToken( string text, string delimiters, int index, string replacement ) {}
   /*! Remove the substring in @a text separated by @a delimiters at the given @a index.
@param text A @a delimiters list of substrings.
@param delimiters Character or characters that separate the list of substrings in @a text.
@param index The zero-based index of the word in @a text.
@return A new string with the substring at the given index removed or the original string if @a index is out of range.

@tsexample
removeToken( "a b c d", " ", 2 ) // Returns "a b d"
@endtsexample

@see removeWord
@see removeField
@see removeRecord
@ingroup FieldManip */
   const char* removeToken( string text, string delimiters, int index ) {}
   /*! Return the number of @a delimiters substrings in @a text.
@param text A @a delimiters list of substrings.
@param delimiters Character or characters that separate the list of substrings in @a text.
@return The number of @a delimiters substrings in @a text.

@tsexample
getTokenCount( "a b c d e", " " ) // Returns 5
@endtsexample

@see getWordCount
@see getFieldCount
@see getRecordCount
@ingroup FieldManip */
   S32 getTokenCount( string text, string delimiters ) {}
   /*! @brief Returns the string from a tag string.

Should only be used within the context of a function that receives a tagged string, and is not meant to be used outside of this context.  Use getTaggedString() to convert a tagged string ID back into a regular string at any time.

@tsexample
// From scripts/client/message. cs
function clientCmdChatMessage(%sender, %voice, %pitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10)
{
   onChatMessage(detag(%msgString), %voice, %pitch);
}
@endtsexample

@see \ref syntaxDataTypes under Tagged %Strings
@see getTag()
@see getTaggedString()
@ingroup Networking */
   const char* detag( string str ) {}
   /*! @brief Extracts the tag from a tagged string

Should only be used within the context of a function that receives a tagged string, and is not meant to be used outside of this context.

@param textTagString The tagged string to extract.
@returns The tag ID of the string.
@see \ref syntaxDataTypes under Tagged %Strings
@see detag()
@ingroup Networking */
   const char* getTag( string textTagString ) {}
   /*! @brief Logs a message to the console.

Concatenates all given arguments to a single string and prints the string to the console. A newline is added automatically after the text.

@param message Any number of string arguments.

@ingroup Logging */
   void echo( string message... )...) {}
   /*! @brief Logs a warning message to the console.

Concatenates all given arguments to a single string and prints the string to the console as a warning message (in the in-game console, these will show up using a turquoise font by default). A newline is added automatically after the text.

@param message Any number of string arguments.

@ingroup Logging */
   void warn( string message... )...) {}
   /*! @brief Logs an error message to the console.

Concatenates all given arguments to a single string and prints the string to the console as an error message (in the in-game console, these will show up using a red font by default). A newline is added automatically after the text.

@param message Any number of string arguments.

@ingroup Logging */
   void error( string message... )...) {}
   /*! @brief Logs the value of the given variable to the console.

Prints a string of the form "<variableName> = <variable value>" to the console.

@param variableName Name of the local or global variable to print.

@tsexample
%var = 1;
debugv( "%var" ); // Prints "%var = 1"
@endtsexample

@ingroup Debugging */
   void debugv( string variableName ) {}
   /*! @brief Replace all characters in @a text that need to be escaped for the string to be a valid string literal with their respective escape sequences.

All characters in @a text that cannot appear in a string literal will be replaced by an escape sequence (\\n, \\t, etc).

The primary use of this function is for converting strings suitable for being passed as string literals to the TorqueScript compiler.

@param text A string
@return A duplicate of the text parameter with all unescaped characters that cannot appear in string literals replaced by their respective escape sequences.

@tsxample
expandEscape( "str" NL "ing" ) // Returns "str\ning".
@endtsxample

@see collapseEscape
@ingroup Strings */
   const char* expandEscape( string text ) {}
   /*! Replace all escape sequences in @a text with their respective character codes.

This function replaces all escape sequences (\\n, \\t, etc) in the given string with the respective characters they represent.

The primary use of this function is for converting strings from their literal form into their compiled/translated form, as is normally done by the TorqueScript compiler.

@param text A string.
@return A duplicate of @a text with all escape sequences replaced by their respective character codes.

@tsexample
// Print:
//
//    str
//    ing
//
// to the console.  Note how the backslash in the string must be escaped here
// in order to prevent the TorqueScript compiler from collapsing the escape
// sequence in the resulting string.
echo( collapseEscape( "str\ning" ) );
@endtsexample

@see expandEscape

@ingroup Strings */
   const char* collapseEscape( string text ) {}
   /*! Shut down the engine and exit its process.
This function cleanly uninitializes the engine and then exits back to the system with a process exit status indicating a clean exit.

@see quitWithErrorMessage

@ingroup Platform */
   void quit() {}
   void realQuit() {}
   /*! Display an error message box showing the given @a message and then shut down the engine and exit its process.
This function cleanly uninitialized the engine and then exits back to the system with a process exit status indicating an error.

@param message The message to log to the console and show in an error message box.
@param status  The status code to return to the OS.

@see quit

@ingroup Platform */
   void quitWithErrorMessage( string message, int status=0 ) {}
   /*! Shut down the engine and exit its process.
This function cleanly uninitializes the engine and then exits back to the system with a given return status code.

@param status The status code to return to the OS.

@see quitWithErrorMessage

@ingroup Platform */
   void quitWithStatus( int status=0 ) {}
   /*! Count the number of bits that are set in the given 32 bit integer.
@param v An integer value.

@return The number of bits that are set in @a v.

@ingroup Utilities */
   S32 countBits( int v ) {}
   /*! Generate a new universally unique identifier (UUID).

@return A newly generated UUID.

@ingroup Utilities */
   Torque::UUID generateUUID() {}
   /*! Apply the given arguments to the specified global function and return the result of the call.

@param functionName The name of the function to call.  This function must be in the global namespace, i.e. you cannot call a function in a namespace through #call.  Use eval() for that.
@return The result of the function call.

@tsexample
function myFunction( %arg )
{
  return ( %arg SPC "World!" );
}

echo( call( "myFunction", "Hello" ) ); // Prints "Hello World!" to the console.
@endtsexample

@ingroup Scripting */
   string call( string functionName, string args... )...) {}
   /*! Get the absolute path to the file in which the compiled code for the given script file will be stored.
@param scriptFileName %Path to the .cs script file.
@return The absolute path to the .dso file for the given script file.

@note The compiler will store newly compiled DSOs in the prefs path but pre-existing DSOs will be loaded from the current paths.

@see compile
@see getPrefsPath
@ingroup Scripting */
   const char* getDSOPath( string scriptFileName ) {}
   /*! Compile a file to bytecode.

This function will read the TorqueScript code in the specified file, compile it to internal bytecode, and, if DSO generation is enabled or @a overrideNoDDSO is true, will store the compiled code in a .dso file in the current DSO path mirrorring the path of @a fileName.

@param fileName Path to the file to compile to bytecode.
@param overrideNoDSO If true, force generation of DSOs even if the engine is compiled to not generate write compiled code to DSO files.

@return True if the file was successfully compiled, false if not.

@note The definitions contained in the given file will not be made available and no code will actually be executed.  Use exec() for that.

@see getDSOPath
@see exec
@ingroup Scripting */
   bool compile( string fileName, bool overrideNoDSO=false ) {}
   /*! Execute the given script file.
@param fileName Path to the file to execute
@param noCalls Deprecated
@param journalScript Deprecated
@return True if the script was successfully executed, false if not.

@tsexample
// Execute the init.cs script file found in the same directory as the current script file.
exec( "./init.cs" );
@endtsexample

@see compile
@see eval
@ingroup Scripting */
   bool exec( string fileName, bool noCalls=false, bool journalScript=false ) {}
   /*! eval(consoleString) */
   const char* eval( string consoleString, bool echo=false ) {}
   /*! @brief Returns the value of the named variable or an empty string if not found.

@varName Name of the variable to search for
@return Value contained by varName, "" if the variable does not exist
@ingroup Scripting */
   const char* getVariable( string varName ) {}
   /*! @brief Sets the value of the named variable.

@param varName Name of the variable to locate
@param value New value of the variable
@return True if variable was successfully found and set
@ingroup Scripting */
   void setVariable( string varName, string value ) {}
   /*! @brief Determines if a function exists or not

@param funcName String containing name of the function
@return True if the function exists, false if not
@ingroup Scripting */
   bool isFunction( string funcName ) {}
   /*! @brief Provides the name of the package the function belongs to

@param funcName String containing name of the function
@return The name of the function's package
@ingroup Packages */
   const char* getFunctionPackage( string funcName ) {}
   /*! @brief Determines if a class/namespace method exists

@param namespace Class or namespace, such as Player
@param method Name of the function to search for
@return True if the method exists, false if not
@ingroup Scripting
 */
   bool isMethod( string nameSpace, string method ) {}
   /*! @brief Provides the name of the package the method belongs to

@param namespace Class or namespace, such as Player
@param method Name of the funciton to search for
@return The name of the method's package
@ingroup Packages */
   const char* getMethodPackage( string nameSpace, string method ) {}
   /*! @brief Determines if a variable exists and contains a value
@param varName Name of the variable to search for
@return True if the variable was defined in script, false if not
@tsexample
isDefined( "$myVar" );
@endtsexample

@ingroup Scripting */
   bool isDefined( string varName, string varValue="" ) {}
   /*! @brief Attempts to extract a mod directory from path. Returns empty string on failure.

@param File path of mod folder
@note This is no longer relevant in Torque 3D (which does not use mod folders), should be deprecated
@internal */
   const char * getModNameFromPath( string path ) {}
   /*! @brief Pushes the current $instantGroup on a stack and sets it to the given value (or clears it).

@note Currently only used for editors
@ingroup Editors
@internal */
   void pushInstantGroup( String group="" ) {}
   /*! @brief Pop and restore the last setting of $instantGroup off the stack.

@note Currently only used for editors

@ingroup Editors
@internal */
   void popInstantGroup() {}
   /*! @note Appears to be useless in Torque 3D, should be deprecated
@internal */
   const char * getPrefsPath( string relativeFileName="" ) {}
   /*! @brief Manually execute a special script file that contains game or editor preferences

@param relativeFileName Name and path to file from project folder
@param noCalls Deprecated
@param journalScript Deprecated
@return True if script was successfully executed
@note Appears to be useless in Torque 3D, should be deprecated
@ingroup Scripting */
   bool execPrefs( string relativeFileName, bool noCalls=false, bool journalScript=false ) {}
   /*! Write out the definitions of all global variables matching the given name @a pattern.
If @a fileName is not "", the variable definitions are written to the specified file.  Otherwise the definitions will be printed to the console.

The output are valid TorqueScript statements that can be executed to restore the global variable values.

@param pattern A global variable name pattern.  Must begin with '$'.
@param filename %Path of the file to which to write the definitions or "" to write the definitions to the console.
@param append If true and @a fileName is not "", then the definitions are appended to the specified file. Otherwise existing contents of the file (if any) will be overwritten.

@tsexample
// Write out all preference variables to a prefs.cs file.
export( "$prefs::*", "prefs.cs" );
@endtsexample

@ingroup Scripting */
   void export( string pattern, string filename="", bool append=false ) {}
   /*! Undefine all global variables matching the given name @a pattern.
@param pattern A global variable name pattern.  Must begin with '$'.
@tsexample
// Define a global variable in the "My" namespace.
$My::Variable = "value";

// Undefine all variable in the "My" namespace.
deleteVariables( "$My::*" );
@endtsexample

@see strIsMatchExpr
@ingroup Scripting */
   void deleteVariables( string pattern ) {}
   /*! Enable or disable tracing in the script code VM.

When enabled, the script code runtime will trace the invocation and returns from all functions that are called and log them to the console. This is helpful in observing the flow of the script program.

@param enable New setting for script trace execution, on by default.
@ingroup Debugging */
   void trace( bool enable=true ) {}
   /*! Drops the engine into the native C++ debugger.

This function triggers a debug break and drops the process into the IDE's debugger.  If the process is not running with a debugger attached it will generate a runtime error on most platforms.

@note This function is not available in shipping builds.@ingroup Debugging */
   void debug() {}
   /*! Test whether the engine has been compiled with TORQUE_PLAYER.

@return True if this is a playback only build; false otherwise.

@ingroup Platform */
   bool isPlayerBuild() {}
   /*! Test whether the engine has been compiled with TORQUE_SHIPPING, i.e. in a form meant for final release.

@return True if this is a shipping build; false otherwise.

@ingroup Platform */
   bool isShippingBuild() {}
   /*! Test whether the engine has been compiled with TORQUE_DEBUG, i.e. if it includes debugging functionality.

@return True if this is a debug build; false otherwise.

@ingroup Platform */
   bool isDebugBuild() {}
   /*! Test whether the engine has been compiled with TORQUE_TOOLS, i.e. if it includes tool-related functionality.

@return True if this is a tool build; false otherwise.

@ingroup Platform */
   bool isToolBuild() {}
   /*! generate a hash from a string. foramt is (string, casesensitive). defaults to true */
   S32 getStringHash( string _inString="", bool _sensitive=true ) {}
   /*! Gets datetime string.

@return YYYY-mm-DD_hh-MM-ss formatted date time string. */
   const char* getTimestamp() {}
   /*! @brief Prints the scripting call stack to the console log.

Used to trace functions called from within functions. Can help discover what functions were called (and not yet exited) before the current point in scripts.

@ingroup Debugging */
   void backtrace() {}
   /*! @brief Returns true if the identifier is the name of a declared package.

@ingroup Packages
 */
   bool isPackage( String identifier ) {}
   /*! @brief Activates an existing package.

The activation occurs by updating the namespace linkage of existing functions and methods. If the package is already activated the function does nothing.
@ingroup Packages
 */
   void activatePackage( String packageName ) {}
   /*! @brief Deactivates a previously activated package.

The package is deactivated by removing its namespace linkages to any function or method. If there are any packages above this one in the stack they are deactivated as well. If the package is not on the stack this function does nothing.
@ingroup Packages
 */
   void deactivatePackage( String packageName ) {}
   /*! @brief Returns a space delimited list of the active packages in stack order.

@ingroup Packages
 */
   const char* getPackageList() {}
   /*! @brief Returns true if the passed identifier is the name of a declared class.

@ingroup Console */
   bool isClass( string identifier ) {}
   /*! @brief Returns true if the class is derived from the super class.

If either class doesn't exist this returns false.
@param className The class name.
@param superClassName The super class to look for.
@ingroup Console */
   bool isMemberOfClass( string className, string superClassName ) {}
   /*! @brief Returns the description string for the named class.

@param className The name of the class.
@return The class description in string format.
@ingroup Console */
   const char* getDescriptionOfClass( string className ) {}
   /*! @brief Returns the category of the given class.

@param className The name of the class.
@ingroup Console */
   const char* getCategoryOfClass( string className ) {}
   /*! @brief Returns a list of classes that derive from the named class.

If the named class is omitted this dumps all the classes.
@param className The optional base class name.
@return A tab delimited list of classes.
@ingroup Editors
@internal */
   const char* enumerateConsoleClasses( string className="" ) {}
   /*! @brief Provide a list of classes that belong to the given category.

@param category The category name.
@return A tab delimited list of classes.
@ingroup Editors
@internal */
   const char* enumerateConsoleClassesByCategory( String category ) {}
   /*! @brief Dumps network statistics for each class to the console.

The returned <i>avg</i>, <i>min</i> and <i>max</i> values are in bits sent per update.  The <i>num</i> value is the total number of events collected.
@note This method only works when TORQUE_NET_STATS is defined in torqueConfig.h.
@ingroup Networking
 */
   void dumpNetStats() {}
   /*! @brief Determines the memory consumption of a class or object.

@param objectOrClass The object or class being measured.
@return Returns the total size of an object in bytes.
@ingroup Debugging
 */
   S32 sizeof( string objectOrClass ) {}
   /*! @brief Links childNS to parentNS.

Links childNS to parentNS, or nothing if parentNS is NULL.
Will unlink the namespace from previous namespace if a parent already exists.
@internal
 */
   bool linkNamespaces( String childNSName, String parentNSName ) {}
   /*! Dumps the engine scripting documentation to the specified file overwriting any existing content.
@param outputFile The relative or absolute output file path and name.
@return Returns true if successful.
@ingroup Console */
   bool dumpEngineDocs( string outputFile ) {}
   /*! @brief Grabs the full path of a specified file

@param filename Name of the local file to locate
@return String containing the full filepath on disk
@ingroup FileSystem */
   const char* expandFilename( string filename ) {}
   /*! @brief Retrofits a filepath that uses old Torque style

@return String containing filepath with new formatting
@ingroup FileSystem */
   const char* expandOldFilename( string filename ) {}
   /*! @name SimFunctions
   
   Functions relating to Sim.
   @{ */
   /*! */
   /*! nameToID(object) */
   S32 nameToID( string objectName ) {}
   /*! isObject(object) */
   bool isObject( string objectName ) {}
   /*! cancel(eventId) */
   void cancel( int eventId ) {}
   /*! cancelAll(objectId): cancel pending events on the specified object.  Events will be automatically cancelled if object is deleted. */
   void cancelAll( string objectId ) {}
   /*! isEventPending(%scheduleId); */
   bool isEventPending( int scheduleId ) {}
   /*! getEventTimeLeft(scheduleId) Get the time left in ms until this event will trigger. */
   S32 getEventTimeLeft( int scheduleId ) {}
   /*! getScheduleDuration(%scheduleId); */
   S32 getScheduleDuration( int scheduleId ) {}
   /*! getTimeSinceStart(%scheduleId); */
   S32 getTimeSinceStart( int scheduleId ) {}
   /*! schedule(time, refobject|0, command, <arg1...argN>) */
   int schedule(...) {}
   /*! @brief Returns a unique unused SimObject name based on a given base name.

@baseName Name to conver to a unique string if another instance exists
@note Currently only used by editors
@ingroup Editors
@internal */
   const char* getUniqueName( string baseName ) {}
   /*! @brief Returns a unique unused internal name within the SimSet/Group based on a given base name.

@note Currently only used by editors
@ingroup Editors
@internal */
   const char* getUniqueInternalName( string baseName, string setString, bool searchChildren ) {}
   /*! @brief Return true if the given name makes for a valid object name.

@param name Name of object
@return True if name is allowed, false if denied (usually because it starts with a number, _, or invalid character@ingroup Console */
   bool isValidObjectName( string name ) {}
   /// @}
   /*! List the methods defined on this object.

Each description is a newline-separated vector with the following elements:
- method prototype string.
- Documentation string (not including prototype).  This takes up the remainder of the vector.
@return An ArrayObject populated with (name,description) pairs of all methods defined on the object. */
   ArrayObject* getMethodSigsNS( StringTableEntry className, bool commands=false ) {}
   /*! Get the number of static fields on the name space.
@return The number of static fields defined on the object. */
   S32 getFieldCountNS( StringTableEntry className ) {}
   /*! Retrieve the value of a static field by index.
@param index The index of the static field.
@return The value of the static field with the given index or "". */
   const char* getFieldNS( StringTableEntry className, int index ) {}
   /*! Returns the inheritance hierarchy for a given class. */
   const char* getClassHierarchy( string name ) {}
   /*! @brief Serialize the object to a file.

@param object The object to serialize.
@param filename The file name and path.
@ingroup Console
 */
   bool saveObject( SimObject  object, string filename ) {}
   /*! @brief Loads a serialized object from a file.

@param Name and path to text file containing the object
@ingroup Console
 */
   SimObject* loadObject( string filename ) {}
   /*! Dumps the script constants table to the console. Using filter for keys if not empty. */
   void dumpScriptConstants( String filter="" ) {}
   /*! @brief Dumps information about String memory usage

@ingroup Debugging
@ingroup Strings
 */
   void dumpStringMemStats() {}
   /*! @brief Returns the OS temporary directory, "C:/Users/Mich/AppData/Local/Temp" for example

@note This can be useful to adhering to OS standards and practices, but not really used in Torque 3D right now.
@note Be very careful when getting into OS level File I/O.@return String containing path to OS temp directory
@note This is legacy function brought over from TGB, and does not appear to have much use. Possibly deprecate?
@ingroup FileSystem
@internal */
   const char * getTemporaryDirectory() {}
   /*! @brief Creates a name and extension for a potential temporary file

This does not create the actual file. It simply creates a random name for a file that does not exist.

@note This is legacy function brought over from TGB, and does not appear to have much use. Possibly deprecate?
@ingroup FileSystem
@internal */
   const char * getTemporaryFileName() {}
   /*! getUserDataDirectory() */
   const char * getUserDataDirectory() {}
   /*! getUserHomeDirectory() */
   const char * getUserHomeDirectory() {}
   /*! setMainDotCsDir() */
   void setMainDotCsDir( string path ) {}
   /*! Fatal Script Assertion */
   void Assert( bool condition, string message ) {}
   /*! getRandom interger value params min,max */
   S32 getRandom( int min, int max=0 ) {}
   /*! getRandom float value params min,max */
   F32 getRandomF( float min, float max=0.f ) {}
   /*! Round v down to the nearest integer.
@param v Number to convert to integer.@returns Number converted to integer.@ingroup Math */
   S32 mFloor( float v ) {}
   /*! Round v to the nth decimal place or the nearest whole number by default.@param v Value to roundn@return The rounded value as a S32.@ingroup Math */
   S32 mRound( float v ) {}
   /*! Round v to the nth decimal place or the nearest whole number by default.@param v Value to roundn@param n Number of decimal places to round to, 0 by defaultn@return The rounded value as a S32.@ingroup Math */
   F32 mRoundColour( float v, int n=0 ) {}
   /*! Round v up to the nearest integer.
@param v Number to convert to integer.@returns Number converted to integer.@ingroup Math */
   S32 mCeil( float v ) {}
   /*! Formats the specified number to the given number of decimal places.
@param v Number to format.@param precision Number of decimal places to format to (1-9).@returns Number formatted to the specified number of decimal places.@ingroup Math */
   const char* mFloatLength( float v, uint precision ) {}
   /*! Calculate absolute value of specified value.
@param v Input Value.@returns Absolute value of specified value.@ingroup Math */
   F32 mAbs( float v ) {}
   /*! Calculate the remainder of v/d.
@param v Input Value.@param d Divisor Value.@returns The remainder of v/d.@ingroup Math */
   F32 mFMod( float v, float d ) {}
   /*! Calculate the square-root of v.
@param v Input Value.@returns The square-root of the input value.@ingroup Math */
   F32 mSqrt( float v ) {}
   /*! Calculate b raised to the p-th power.
@param v Input Value.@param p Power to raise value by.@returns v raised to the p-th power.@ingroup Math */
   F32 mPow( float v, float p ) {}
   /*! Calculate the natural logarithm of v.
@param v Input Value.@returns The natural logarithm of the input value.@ingroup Math */
   F32 mLog( float v ) {}
   /*! Calculate the sine of v.
@param v Input Value (in radians).@returns The sine of the input value.@ingroup Math */
   F32 mSin( float v ) {}
   /*! Calculate the cosine of v.
@param v Input Value (in radians).@returns The cosine of the input value.@ingroup Math */
   F32 mCos( float v ) {}
   /*! Calculate the tangent of v.
@param v Input Value (in radians).@returns The tangent of the input value.@ingroup Math */
   F32 mTan( float v ) {}
   /*! Calculate the arc-sine of v.
@param v Input Value (in radians).@returns The arc-sine of the input value.@ingroup Math */
   F32 mAsin( float v ) {}
   /*! Calculate the arc-cosine of v.
@param v Input Value (in radians).@returns The arc-cosine of the input value.@ingroup Math */
   F32 mAcos( float v ) {}
   /*! Calculate the arc-tangent (slope) of a line defined by rise and run.
@param rise of line.@param run of line.@returns The arc-tangent (slope) of a line defined by rise and run.@ingroup Math */
   F32 mAtan( float rise, float run ) {}
   /*! Convert specified radians into degrees.
@param radians Input Value (in radians).@returns The specified radians value converted to degrees.@ingroup Math */
   F32 mRadToDeg( float radians ) {}
   /*! Convert specified degrees into radians.
@param degrees Input Value (in degrees).@returns The specified degrees value converted to radians.@ingroup Math */
   F32 mDegToRad( float degrees ) {}
   /*! Clamp the specified integer value between two bounds.
@param S32 v Input value.@param S32 min Minimum Bound.@param S32 max Maximum Bound.@returns The specified value clamped to the specified bounds.@ingroup Math */
   S32 mClamp( int v, int min, int max ) {}
   /*! Clamp the specified integer value between two bounds.
@param S32 v Input value.@param S32 min Minimum Bound.@param S32 max Maximum Bound.@returns The specified value clamped to the specified bounds.@ingroup Math */
   S32 mClampI( int v, int min, int max ) {}
   /*! ClampF the specified float value between two bounds.
@param F32 v Input value.@param F32 min Minimum Bound.@param F32 max Maximum Bound.@returns The specified value clamped to the specified bounds.@ingroup Math */
   F32 mClampF( float v, float min, float max ) {}
   /*! Clamp the specified value between 0 and 1 (inclusive).
@param v Input value.@returns The specified value clamped between 0 and 1 (inclusive).@ingroup Math */
   F32 mSaturate( float v ) {}
   /*! Wrap the specified value between two bounds.
@param v Input value.@param min Minimum Bound.@param max Maximum Bound.@returns The specified value wrapped to the specified bounds.@ingroup Math */
   F32 mWrapF( float v, float min, float max ) {}
   /*! Wrap the specified value between two bounds.
@param v Input value.@param min Minimum Bound.@param max Maximum Bound.@returns The specified value wrapped to the specified bounds.@ingroup Math */
   S32 mWrap( int v, int min, int max ) {}
   /*! Calculate the greater of two specified numbers.
@param v1 Input value.@param v2 Input value.@returns The greater value of the two specified values.@ingroup Math */
   F32 getMax( float v1, float v2 ) {}
   /*! Calculate the lesser of two specified numbers.
@param v1 Input value.@param v2 Input value.@returns The lesser value of the two specified values.@ingroup Math */
   F32 getMin( float v1, float v2 ) {}
   /*! Calculate linearly interpolated value between two specified numbers using specified normalized time.
@param v1 Interpolate From Input value.@param v2 Interpolate To Input value.@param time Normalized time used to interpolate values (0-1).@returns The interpolated value between the two specified values at normalized time t.@ingroup Math */
   F32 mLerp( float v1, float v2, float time ) {}
   /*! Return the value of PI (half-circle in radians).
@returns The value of PI.@ingroup Math */
   F32 mPi() {}
   /*! Return the value of 2*PI (full-circle in radians).
@returns The value of 2*PI.@ingroup Math */
   F32 m2Pi() {}
   /*! Returns whether the value is an exact power of two.
@param v Input value.@returns Whether the specified value is an exact power of two.@ingroup Math */
   bool mIsPow2( int v ) {}
   /*! @brief Export all declared console classes to the console.

@param dumpScript Optional parameter specifying whether or not classes defined in script should be dumped.
@param dumpEngine Optional parameter specifying whether or not classes defined in the engine should be dumped.
@ingroup Docu */
   bool exportConsoleDocu( String fileName, bool forDoxyGen=false, bool append=false, bool dumpScript=true, bool dumpEngine=true ) {}
   /*! Initialize window and OpenGL context */
   void InitWindow( int width, int height, String title ) {}
   /*! Close window and unload OpenGL context */
   void CloseWindow() {}
   /*! Check if application should close (KEY_ESCAPE pressed or windows close icon clicked) */
   bool WindowShouldClose() {}
   /*! Check if window has been initialized successfully */
   bool IsWindowReady() {}
   /*! Check if window is currently fullscreen */
   bool IsWindowFullscreen() {}
   /*! Check if window is currently hidden */
   bool IsWindowHidden() {}
   /*! Check if window is currently minimized */
   bool IsWindowMinimized() {}
   /*! Check if window is currently maximized */
   bool IsWindowMaximized() {}
   /*! Check if window is currently focused */
   bool IsWindowFocused() {}
   /*! Check if window has been resized last frame */
   bool IsWindowResized() {}
   /*! Check if one specific window flag is enabled */
   bool IsWindowState( unsigned int flag ) {}
   /*! Set window configuration state using flags */
   void SetWindowState( unsigned int flags ) {}
   /*! Clear window configuration state flags */
   void ClearWindowState( unsigned int flags ) {}
   /*! Toggle window state: fullscreen/windowed, resizes monitor to match window resolution */
   void ToggleFullscreen() {}
   /*! Toggle window state: borderless windowed, resizes window to match monitor resolution */
   void ToggleBorderlessWindowed() {}
   /*! Set window state: maximized, if resizable */
   void MaximizeWindow() {}
   /*! Set window state: minimized, if resizable */
   void MinimizeWindow() {}
   /*! Restore window from being minimized/maximized */
   void RestoreWindow() {}
   /*! Set icon for window (single image, RGBA 32bit) */
   void SetWindowIcon( int imageId ) {}
   /*! Set icon for window (multiple images, RGBA 32bit) */
   void SetWindowIcons( Vector<S32> imageIds, int count ) {}
   /*! Set title for window */
   void SetWindowTitle( string title ) {}
   /*! Set window position on screen */
   void SetWindowPosition( int x, int y ) {}
   /*! Set monitor for the current window */
   void SetWindowMonitor( int monitor ) {}
   /*! Set window minimum dimensions (for FLAG_WINDOW_RESIZABLE) */
   void SetWindowMinSize( int width, int height ) {}
   /*! Set window maximum dimensions (for FLAG_WINDOW_RESIZABLE) */
   void SetWindowMaxSize( int width, int height ) {}
   /*! Set window dimensions */
   void SetWindowSize( int width, int height ) {}
   /*! Set window opacity [0.0f..1.0f] */
   void SetWindowOpacity( float opacity ) {}
   /*! Set window focused */
   void SetWindowFocused() {}
   /*! Get current screen width */
   int GetScreenWidth() {}
   /*! Get current screen height */
   int GetScreenHeight() {}
   /*! Get current render width (it considers HiDPI) */
   int GetRenderWidth() {}
   /*! Get current render height (it considers HiDPI) */
   int GetRenderHeight() {}
   /*! Get number of connected monitors */
   int GetMonitorCount() {}
   /*! Get current monitor where window is placed */
   int GetCurrentMonitor() {}
   /*! Get specified monitor position */
   Vector2 GetMonitorPosition( int monitor ) {}
   /*! Get specified monitor width (current video mode used by monitor) */
   int GetMonitorWidth( int monitor ) {}
   /*! Get specified monitor height (current video mode used by monitor) */
   int GetMonitorHeight( int monitor ) {}
   /*! Get specified monitor physical width in millimetres */
   int GetMonitorPhysicalWidth( int monitor ) {}
   /*! Get specified monitor physical height in millimetres */
   int GetMonitorPhysicalHeight( int monitor ) {}
   /*! Get specified monitor refresh rate */
   int GetMonitorRefreshRate( int monitor ) {}
   /*! Get window position XY on monitor */
   Vector2 GetWindowPosition() {}
   /*! Get window scale DPI factor */
   Vector2 GetWindowScaleDPI() {}
   /*! Get the human-readable, UTF-8 encoded name of the specified monitor */
   const char * GetMonitorName( int monitor ) {}
   /*! Set clipboard text content */
   void SetClipboardText( string text ) {}
   /*! Get clipboard text content */
   const char * GetClipboardText() {}
   /*! Get clipboard image content */
   S32 GetClipboardImage() {}
   /*! Enable waiting for events on EndDrawing(), no automatic event polling */
   void EnableEventWaiting() {}
   /*! Disable waiting for events on EndDrawing(), automatic events polling */
   void DisableEventWaiting() {}
   /*! Show cursor */
   void ShowCursor() {}
   /*! Hide cursor */
   void HideCursor() {}
   /*! Check if cursor is not visible */
   bool IsCursorHidden() {}
   /*! Enable cursor (unlock cursor) */
   void EnableCursor() {}
   /*! Disable cursor (lock cursor) */
   void DisableCursor() {}
   /*! Check if cursor is on the screen */
   bool IsCursorOnScreen() {}
   /*! Clear background (framebuffer) to color */
   void ClearBackground( Color color=RAYWHITE ) {}
   /*! Begin canvas (framebuffer) drawing */
   void BeginDrawing() {}
   /*!  End canvas (framebuffer) drawing and swap buffers (double buffering) */
   void EndDrawing() {}
   /*! Begins drawing to render texture */
   void BeginTextureMode( int renderTextureId ) {}
   /*!  End drawing to render texture */
   void EndTextureMode() {}
   /*! Begin blending mode (e.g. BLEND_ALPHA, BLEND_ADDITIVE, BLEND_MULTIPLIED) */
   void BeginBlendMode( int mode ) {}
   /*! End blending mode (reset to default: alpha blending) */
   void EndBlendMode() {}
   /*! Begin scissor mode (define screen area for following drawing) */
   void BeginScissorMode( int x, int y, int width, int height ) {}
   /*! End scissor mode */
   void EndScissorMode() {}
   /*! Set target FPS (maximum) */
   void SetTargetFPS( int fps ) {}
   /*! Get time in seconds for last frame drawn (delta time) */
   F32 GetFrameTime() {}
   /*! Get elapsed time in seconds since InitWindow() */
   F64 GetTime() {}
   /*! Get current FPS */
   S32 GetFPS() {}
   /*! Swap back buffer with front buffer (screen drawing) */
   void SwapScreenBuffer() {}
   /*! Register all input events */
   void PollInputEvents() {}
   /*! Wait for some time (halt program execution) */
   void WaitTime( double seconds ) {}
   /*! Set the seed for the random number generator */
   void SetRandomSeed( unsigned int seed ) {}
   /*! Get a random value between min and max (both included) */
   int GetRandomValue( int min, int max ) {}
   /*! Generate a list of random values. Unline LoadRandomSequence they are NOT uniqe #elfScript custom function. */
   Vector<S32> GetRandomValues( uint count, int min, int max ) {}
   /*! Load random values sequence, no values repeated, unload automaticly done.  */
   Vector<S32> LoadRandomSequence( uint count, int min, int max ) {}
   /*! Takes a screenshot of current screen (filename extension defines format) */
   void TakeScreenshot( string fileName ) {}
   /*! Set up init configuration flags (view FLAGS) */
   void SetConfigFlags( unsigned int flags ) {}
   /*! Open URL with default system browser (if available) */
   void OpenURL( string url ) {}
   /*! Set the current threshold (minimum) raylib log level (e.g. 2 = LOG_TRACE, 3 = LOG_DEBUG, 4 = LOG_INFO...) */
   void SetTraceLogLevel( int logLevel ) {}
   /*! Check if a key has been pressed once */
   bool IsKeyPressed( int key ) {}
   /*! Check if a key has been pressed again */
   bool IsKeyPressedRepeat( int key ) {}
   /*! Check if a key is being pressed */
   bool IsKeyDown( int key ) {}
   /*! Check if a key has been released once */
   bool IsKeyReleased( int key ) {}
   /*! Check if a key is NOT being pressed */
   bool IsKeyUp( int key ) {}
   /*! Get key pressed (keycode), call it multiple times for keys queued, returns 0 when the queue is empty */
   int GetKeyPressed(  void ) {}
   /*! Get char pressed (unicode), call it multiple times for chars queued, returns 0 when the queue is empty */
   int GetCharPressed(  void ) {}
   /*! Set a custom key to exit program (default is ESC) */
   void SetExitKey( int key ) {}
   /*! Check if a gamepad is available */
   bool IsGamepadAvailable( int gamepad ) {}
   /*! Get gamepad internal name id */
   String GetGamepadName( int gamepad ) {}
   /*! Check if a gamepad button has been pressed once */
   bool IsGamepadButtonPressed( int gamepad, int button ) {}
   /*! Check if a gamepad button is being pressed */
   bool IsGamepadButtonDown( int gamepad, int button ) {}
   /*! Check if a gamepad button has been released once */
   bool IsGamepadButtonReleased( int gamepad, int button ) {}
   /*! Check if a gamepad button is NOT being pressed */
   bool IsGamepadButtonUp( int gamepad, int button ) {}
   /*! Get the last gamepad button pressed */
   S32 GetGamepadButtonPressed() {}
   /*! Get axis count for a gamepad */
   S32 GetGamepadAxisCount( int gamepad ) {}
   /*! Get movement value for a gamepad axis */
   F32 GetGamepadAxisMovement( int gamepad, int axis ) {}
   /*! Set internal gamepad mappings (SDL_GameControllerDB) */
   S32 SetGamepadMappings( string mappings ) {}
   /*! Set gamepad vibration for both motors (duration in seconds) */
   void SetGamepadVibration( int gamepad, float leftMotor, float rightMotor, float duration ) {}
   /*! Check if a mouse button has been pressed once */
   bool IsMouseButtonPressed( int button ) {}
   /*! Check if a mouse button is being pressed */
   bool IsMouseButtonDown( int button ) {}
   /*! Check if a mouse button has been released once */
   bool IsMouseButtonReleased( int button ) {}
   /*!  Check if a mouse button is NOT being pressed */
   bool IsMouseButtonUp( int button ) {}
   /*!  Get mouse position X */
   S32 GetMouseX() {}
   /*!  Get mouse position Y */
   S32 GetMouseY() {}
   /*!  Get mouse position XY */
   Vector2 GetMousePosition() {}
   /*!  Get mouse delta XY */
   Vector2 GetMouseDelta() {}
   /*! Set mouse position XY on the screen */
   void SetMousePosition( int x, int y ) {}
   /*! Set mouse offset (useful for custom rendering resolution scaling) */
   void SetMouseOffset( int offsetX, int offsetY ) {}
   /*! Set mouse scaling for its coordinates (useful for custom rendering resolution scaling) */
   void SetMouseScale( float scaleX, float scaleY ) {}
   /*!  Get mouse wheel movement for X or Y, whichever is larger */
   F32 GetMouseWheelMove() {}
   /*! Get mouse wheel movement for both X and Y as a Vector2 */
   Vector2 GetMouseWheelMoveV() {}
   /*! Set mouse cursor icon type (e.g. MOUSE_CURSOR_IBEAM, MOUSE_CURSOR_POINT) */
   void SetMouseCursor( int cursor ) {}
   /*! Get touch position X for touch point 0 (relative to screen size) */
   int GetTouchX() {}
   /*! Get touch position Y for touch point 0 (relative to screen size) */
   int GetTouchY() {}
   /*! Get touch position XY for a touch point index (relative to screen size) */
   Vector2 GetTouchPosition( int index ) {}
   /*! Get touch point identifier for given index */
   int GetTouchPointId( int index ) {}
   /*! Get number of active touch points */
   int GetTouchPointCount() {}
   /*! Enable a set of gestures using flags (bitmask) */
   void SetGesturesEnabled( int flags ) {}
   /*! Check if a specific gesture has been detected */
   bool IsGestureDetected( int gesture ) {}
   /*! Get latest detected gesture type */
   int GetGestureDetected() {}
   /*! Get gesture hold time in seconds */
   float GetGestureHoldDuration() {}
   /*! Get gesture drag vector as Vector2 */
   Vector2 GetGestureDragVector() {}
   /*! Get gesture drag angle in degrees */
   float GetGestureDragAngle() {}
   /*! Get gesture pinch delta vector as Vector2 */
   Vector2 GetGesturePinchVector() {}
   /*! Get gesture pinch angle in degrees */
   float GetGesturePinchAngle() {}
   /*! Clear all resources. */
   void ClearAllResources() {}
   /*! Load image from file into CPU memory (RAM) and return the ImageID */
   S32 LoadImage( String fileName ) {}
   /*! Unload image from CPU memory (RAM) */
   void UnloadImage( int imageId ) {}
   /*! Load image from RAW file data */
   S32 LoadImageRaw( String fileName, int width, int height, int format, int headerSize ) {}
   /*! Load image sequence from file. Outputs frames to console. */
   S32 LoadImageAnim( String fileName ) {}
   /*! Load image from GPU texture data */
   S32 LoadImageFromTexture( int textureId ) {}
   /*! Load image from screen buffer (screenshot) */
   S32 LoadImageFromScreen() {}
   /*! Check if an image is valid */
   bool IsImageValid( int imageId ) {}
   /*! Export image data to file, returns true on success */
   bool ExportImage( int imageId, String fileName ) {}
   /*! Export image as code file defining an array of bytes, returns true on success */
   bool ExportImageAsCode( int imageId, String fileName ) {}
   /*! Generate image: plain color */
   S32 GenImageColor( int width, int height, Color color ) {}
   /*! Generate image: linear gradient, direction in degrees [0..360], 0=Vertical gradient */
   S32 GenImageGradientLinear( int width, int height, int direction, Color start, Color end ) {}
   /*! Generate image: radial gradient */
   S32 GenImageGradientRadial( int width, int height, float density, Color inner, Color outer ) {}
   /*! Generate image: square gradient */
   S32 GenImageGradientSquare( int width, int height, float density, Color inner, Color outer ) {}
   /*! Generate image: checked */
   S32 GenImageChecked( int width, int height, int checksX, int checksY, Color col1, Color col2 ) {}
   /*! Generate image: white noise */
   S32 GenImageWhiteNoise( int width, int height, float factor ) {}
   /*! Generate image: perlin noise */
   S32 GenImagePerlinNoise( int width, int height, int offsetX, int offsetY, float scale ) {}
   /*! Generate image: cellular algorithm, bigger tileSize means bigger cells */
   S32 GenImageCellular( int width, int height, int tileSize ) {}
   /*! Generate image: grayscale image from text data */
   S32 GenImageText( int width, int height, String text ) {}
   /*! Create an image duplicate */
   S32 ImageCopy( int imageId ) {}
   /*! Create an image from another image piece */
   S32 ImageFromImage( int imageId, Rectangle rec ) {}
   /*! Create an image from a selected channel of another image (GRAYSCALE) */
   S32 ImageFromChannel( int imageId, int selectedChannel ) {}
   /*! Create an image from text (default font) */
   S32 ImageText( String text, int fontSize, Color color ) {}
   /*! Create an image from text (custom sprite font) */
   S32 ImageTextEx( int fontId, String text, float fontSize, float spacing, Color tint ) {}
   /*! Convert image data to desired format */
   void ImageFormat( int imageId, int newFormat ) {}
   /*! Convert image to POT (power-of-two) */
   void ImageToPOT( int imageId, Color fill ) {}
   /*! Crop an image to a defined rectangle */
   void ImageCrop( int imageId, Rectangle crop ) {}
   /*! Crop image depending on alpha value */
   void ImageAlphaCrop( int imageId, float threshold ) {}
   /*! Clear alpha channel to desired color */
   void ImageAlphaClear( int imageId, Color color, float threshold ) {}
   /*! Apply alpha mask to image */
   void ImageAlphaMask( int imageId, int alphaMaskId ) {}
   /*! Premultiply alpha channel */
   void ImageAlphaPremultiply( int imageId ) {}
   /*! Apply Gaussian blur using a box blur approximation */
   void ImageBlurGaussian( int imageId, int blurSize ) {}
   /*! Resize image (Bicubic scaling algorithm) */
   void ImageResize( int imageId, int newWidth, int newHeight ) {}
   /*! Resize image (Nearest-Neighbor scaling algorithm) */
   void ImageResizeNN( int imageId, int newWidth, int newHeight ) {}
   /*! Resize canvas and fill with color */
   void ImageResizeCanvas( int imageId, int newWidth, int newHeight, int offsetX, int offsetY, Color fill ) {}
   /*! Compute all mipmap levels for a provided image */
   void ImageMipmaps( int imageId ) {}
   /*! Dither image data to 16bpp or lower */
   void ImageDither( int imageId, int rBpp, int gBpp, int bBpp, int aBpp ) {}
   /*! Flip image vertically */
   void ImageFlipVertical( int imageId ) {}
   /*! Flip image horizontally */
   void ImageFlipHorizontal( int imageId ) {}
   /*! Rotate image by input angle in degrees (-359 to 359) */
   void ImageRotate( int imageId, int degrees ) {}
   /*! Rotate image clockwise 90deg */
   void ImageRotateCW( int imageId ) {}
   /*! Rotate image counter-clockwise 90deg */
   void ImageRotateCCW( int imageId ) {}
   /*! Modify image color: tint */
   void ImageColorTint( int imageId, Color color ) {}
   /*! Modify image color: invert */
   void ImageColorInvert( int imageId ) {}
   /*! Modify image color: grayscale */
   void ImageColorGrayscale( int imageId ) {}
   /*! Modify image color: contrast (-100 to 100) */
   void ImageColorContrast( int imageId, int contrast ) {}
   /*! Modify image color: brightness (-255 to 255) */
   void ImageColorBrightness( int imageId, int brightness ) {}
   /*! Modify image color: replace color */
   void ImageColorReplace( int imageId, Color color, Color replace ) {}
   /*! Load color data from image by id as a Color array (RGBA - 32bit) and return the colorsId */
   S32 LoadImageColors( int imageId ) {}
   /*! Get a color from Colors* loaded with LoadImageColors */
   Color GetImageColorsColor( int colorsId, int index ) {}
   /*! Unload color data loaded with LoadImageColors() */
   void UnloadImageColors( int colorsId ) {}
   /*! Get image alpha border rectangle */
   Rectangle GetImageAlphaBorder( int imageId, float threshold ) {}
   /*! Get image pixel color at (x, y) position */
   Color GetImageColor( int imageId, int x, int y ) {}
   /*! Clear image background with given color */
   void ImageClearBackground( int dstId, Color color ) {}
   /*! Draw pixel within an image */
   void ImageDrawPixel( int dstId, int posX, int posY, Color color ) {}
   /*! Draw pixel within an image (Vector version) */
   void ImageDrawPixelV( int dstId, Vector2 position, Color color ) {}
   /*! Draw line within an image */
   void ImageDrawLine( int dstId, int startPosX, int startPosY, int endPosX, int endPosY, Color color ) {}
   /*! Draw line within an image (Vector version) */
   void ImageDrawLineV( int dstId, Vector2 start, Vector2 end, Color color ) {}
   /*! Draw a line defining thickness within an image */
   void ImageDrawLineEx( int dstId, Vector2 start, Vector2 end, int thick, Color color ) {}
   /*! Draw a filled circle within an image */
   void ImageDrawCircle( int dstId, int centerX, int centerY, int radius, Color color ) {}
   /*! Draw a filled circle within an image (Vector version) */
   void ImageDrawCircleV( int dstId, Vector2 center, int radius, Color color ) {}
   /*! Draw circle outline within an image */
   void ImageDrawCircleLines( int dstId, int centerX, int centerY, int radius, Color color ) {}
   /*! Draw circle outline within an image (Vector version) */
   void ImageDrawCircleLinesV( int dstId, Vector2 center, int radius, Color color ) {}
   /*! Draw rectangle within an image */
   void ImageDrawRectangle( int dstId, int posX, int posY, int width, int height, Color color ) {}
   /*! Draw rectangle within an image (Vector version) */
   void ImageDrawRectangleV( int dstId, Vector2 position, Vector2 size, Color color ) {}
   /*! Draw rectangle within an image */
   void ImageDrawRectangleRec( int dstId, Rectangle rec, Color color ) {}
   /*! Draw rectangle lines within an image */
   void ImageDrawRectangleLines( int dstId, int posX, int posY, int width, int height, Color color ) {}
   /*! Draw rectangle lines within an image with extended parameters */
   void ImageDrawRectangleLinesEx( int dstId, Rectangle rec, int thick, Color color ) {}
   /*! Draw triangle within an image */
   void ImageDrawTriangle( int dstId, Vector2 v1, Vector2 v2, Vector2 v3, Color color ) {}
   /*! Draw triangle with interpolated colors within an image */
   void ImageDrawTriangleGradient( int dstId, Vector2 v1, Vector2 v2, Vector2 v3, Color c1, Color c2, Color c3 ) {}
   /*! Draw triangle outline within an image */
   void ImageDrawTriangleLines( int dstId, Vector2 v1, Vector2 v2, Vector2 v3, Color color ) {}
   /*! Draw a triangle fan defined by points within an image (pointsValues format: 'x1 y1 x2 y2 ...') */
   void ImageDrawTriangleFan( int dstId, Vector<F32> pointValues, int pointCount, Color color ) {}
   /*! Draw a triangle fan defined by points within an image (pointsValues format: 'x1 y1 x2 y2 ...') */
   void ImageDrawTriangleStrip( int dstId, Vector<F32> pointValues, int pointCount, Color color ) {}
   /*! Draw a source image within a destination image (tint applied to source) */
   void ImageDraw( int dstId, int srcId, Rectangle srcRec, Rectangle dstRec, Color tint ) {}
   /*! Draw text (using default font) within an image (destination) */
   void ImageDrawText( int dstId, String text, int posX, int posY, int fontSize, Color color ) {}
   /*! Draw text (custom sprite font) within an image (destination) */
   void ImageDrawTextEx( int dstId, int fontId, String text, Vector2 position, float fontSize, float spacing, Color tint ) {}
   /*! Load texture from file into GPU memory (VRAM) and return the TextureID */
   S32 LoadTexture( String fileName ) {}
   /*! Unload texture from GPU memory (VRAM) */
   void UnloadTexture( int textureId ) {}
   /*! Load texture from image data and return the TextureID */
   S32 LoadTextureFromImage( int imageId ) {}
   /*! Load cubemap from image, multiple image cubemap layouts supported */
   S32 LoadTextureCubemap( int imageId, int layout ) {}
   /*! Check if a texture is valid (loaded in GPU) */
   bool IsTextureValid( int textureId ) {}
   /*! get the texture size int width int height */
   String getTextureSize( int textureId ) {}
   /*! get the texture width  */
   S32 getTextureWidth( int textureId ) {}
   /*! get the texture height  */
   S32 getTextureHeight( int textureId ) {}
   /*! Update GPU texture with data from an Image ID */
   void UpdateTexture( int textureId, int imageId ) {}
   /*! Update GPU texture rectangle with data from an Image ID */
   void UpdateTextureRec( int textureId, Rectangle rec, int imageId ) {}
   /*! Generate GPU mipmaps for a texture */
   void GenTextureMipmaps( int textureId ) {}
   /*! Set texture scaling filter mode (e.g., TEXTURE_FILTER_POINT, TEXTURE_FILTER_BILINEAR) */
   void SetTextureFilter( int textureId, int filter ) {}
   /*! Set texture wrapping mode (e.g., TEXTURE_WRAP_REPEAT, TEXTURE_WRAP_CLAMP) */
   void SetTextureWrap( int textureId, int wrap ) {}
   /*! Draw a Texture2D using its TextureID */
   void DrawTexture( int textureId, int posX, int posY, Color tint=WHITE ) {}
   /*! Draw a Texture2D with position defined as Vector2 */
   void DrawTextureV( int textureId, Vector2 position, Color tint=WHITE ) {}
   /*! Draw a Texture2D with extended parameters (rotation, scale, tint) */
   void DrawTextureEx( int textureId, Vector2 position, float rotation, float scale, Color tint=WHITE ) {}
   /*! Draw a part of a texture defined by a rectangle */
   void DrawTextureRec( int textureId, Rectangle source, Vector2 position, Color tint=WHITE ) {}
   /*! Draw a part of a texture defined by a rectangle with 'pro' parameters */
   void DrawTexturePro( int textureId, Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color tint=WHITE ) {}
   /*! Draws a texture using 9-patch / N-patch scaling to prevent UI distortion. */
   void DrawTextureNPatch( int textureId, Rectangle sourceRec, int left, int top, int right, int bottom, int layout, Rectangle destRec, Vector2 origin, float rotation, Color tint=WHITE ) {}
   /*! Check if two colors are equal */
   bool ColorIsEqual( Color col1, Color col2 ) {}
   /*! Get color with alpha applied, alpha goes from 0.0f to 1.0f */
   Color Fade( Color color, float alpha ) {}
   /*! Get hexadecimal value for a Color (0xRRGGBBAA) */
   int ColorToInt( Color color ) {}
   /*! Get Color normalized as float [0..1] */
   Vector4 ColorNormalize( Color color ) {}
   /*! Get Color from normalized values [0..1] */
   Color ColorFromNormalized( Vector4 normalized ) {}
   /*! Get HSV values for a Color, hue [0..360], saturation/value [0..1] */
   Vector3 ColorToHSV( Color color ) {}
   /*! Get a Color from HSV values, hue [0..360], saturation/value [0..1] */
   Color ColorFromHSV( float hue, float saturation, float value ) {}
   /*! Get color multiplied with another color */
   Color ColorTint( Color color, Color tint ) {}
   /*! Get color with brightness correction, brightness factor goes from -1.0f to 1.0f */
   Color ColorBrightness( Color color, float factor ) {}
   /*! Get color with contrast correction, contrast values between -1.0f and 1.0f */
   Color ColorContrast( Color color, float contrast ) {}
   /*! Get color with alpha applied, alpha goes from 0.0f to 1.0f */
   Color ColorAlpha( Color color, float alpha ) {}
   /*! Get src alpha-blended into dst color with tint */
   Color ColorAlphaBlend( Color dst, Color src, Color tint ) {}
   /*! Get color lerp interpolation between two colors, factor [0.0f..1.0f] */
   Color ColorLerp( Color color1, Color color2, float factor ) {}
   /*! Get Color structure from hexadecimal value (e.g. 0xFF0000FF) */
   Color GetColor( int hexValue ) {}
   /*! Get pixel data size in bytes for certain format */
   int GetPixelDataSize( int width, int height, int format ) {}
   /*! Get the default Raylib Font and return the FontID */
   S32 GetFontDefault() {}
   /*! Load font from file into GPU memory (VRAM) and return the FontID */
   S32 LoadFont( String fileName ) {}
   /*! Load font from file with extended parameters. Pass 0 for codepointCount to use default set. */
   S32 LoadFontEx( String fileName, int fontSize, Vector<S32> codepointValues, int codepointCount ) {}
   /*! Load font from Image data and return the FontID */
   S32 LoadFontFromImage( int imageId, Color key, int firstChar ) {}
   /*! Unload font from GPU memory (VRAM) */
   void UnloadFont( int fontId ) {}
   /*! Export font data as a C code file (.h), returns true on success */
   bool ExportFontAsCode( int fontId, String fileName ) {}
   /*! Draw current FPS */
   void DrawFPS( int posX, int posY ) {}
   /*! Draw text (using default font */
   void DrawText( string text, int posX, int posY, int fontSize=20, Color color=RAYWHITE, bool doShadow=false, Color shadowColor=DARKGRAY ) {}
   /*! Draw text using font and additional parameters, fontId 0 means default font */
   void DrawTextEx( int fontId, string text, Vector2 position, float fontSize=20.f, float spacing=1.f, Color tint=RAYWHITE, bool doShadow=false, Color shadowColor=DARKGRAY ) {}
   /*! Draw text using Font and pro parameters (rotation) */
   void DrawTextPro( int fontId, string text, Vector2 position, Vector2 origin, float rotation, float fontSize, float spacing, Color tint=RAYWHITE ) {}
   /*!  Draw one character (codepoint) */
   void DrawTextCodepoint( int fontId, int codepoint, Vector2 position, float fontSize, Color tint=RAYWHITE ) {}
   /*!  Draw one character (codepoint) */
   void DrawTextCodepoints( int fontId, Vector<S32> codepoints, int codepointCount, Vector2 position, float fontSize, float spacing, Color tint=RAYWHITE ) {}
   /*! Set vertical line spacing when drawing with line-breaks */
   void SetTextLineSpacing( int spacing ) {}
   /*! Measure string width in pixels for the default font */
   int MeasureText( String text, int fontSize ) {}
   /*! Measure string width and height as Vector2 for a specific Font */
   Vector2 MeasureTextEx( int fontId, String text, float fontSize, float spacing ) {}
   /*! Measure string size for an array of codepoints */
   Vector2 MeasureTextCodepoints( int fontId, Vector<S32> codepointValues, int length, float fontSize, float spacing ) {}
   /*! Get glyph index position in font for a codepoint */
   int GetGlyphIndex( int fontId, int codepoint ) {}
   /*! Get glyph rectangle in font atlas for a codepoint */
   Rectangle GetGlyphAtlasRec( int fontId, int codepoint ) {}
   /*! Get glyph info (offsetX, offsetY, advanceX) for a codepoint */
   String GetGlyphInfo( int fontId, int codepoint ) {}
   /*! Begins 2D mode with a Camera2DObject object */
   void BeginMode2D( int cameraId ) {}
   /*! End 2D mode with custom camera */
   void EndMode2D() {}
   /*! Begins 3D mode with a Camera3DObject */
   void BeginMode3D( int cameraId ) {}
   /*! End 3D mode with custom camera */
   void EndMode3D() {}
   /*! Set texture and source rectangle to be used on shapes drawing */
   void SetShapesTexture( int textureId, Rectangle source ) {}
   /*! Get texture ID currently used for shapes drawing */
   S32 GetShapesTexture() {}
   /*! Get texture source rectangle that is used for shapes drawing */
   Rectangle GetShapesTextureRectangle() {}
   /*! Draw a pixel using geometry [Can be slow, use with care] */
   void DrawPixel( int posX, int posY, Color color=RAYWHITE ) {}
   /*! Draw a pixel using geometry (Vector version) [Can be slow, use with care] */
   void DrawPixelV( Vector2 position, Color color=RAYWHITE ) {}
   /*! Draw a line */
   void DrawLine( int startPosX, int startPosY, int endPosX, int endPosY, Color color=RAYWHITE ) {}
   /*! Draw a line (using gl lines) */
   void DrawLineV( Vector2 startPos, Vector2 endPos, Color color=RAYWHITE ) {}
   /*! Draw a line (using triangles/quads) */
   void DrawLineEx( Vector2 startPos, Vector2 endPos, float thick, Color color=RAYWHITE ) {}
   /*! Draw lines sequence (using gl lines) */
   void DrawLineStrip( Vector<F32> pointValues, int pointCount, Color color=RAYWHITE ) {}
   /*! Draw line segment cubic-bezier in-out interpolation */
   void DrawLineBezier( Vector2 startPos, Vector2 endPos, float thick, Color color=RAYWHITE ) {}
   /*! Draw a dashed line */
   void DrawLineDashed( Vector2 startPos, Vector2 endPos, int dashSize, int spaceSize, Color color=RAYWHITE ) {}
   /*! Draw a color-filled circle */
   void DrawCircle( int centerX, int centerY, float radius, Color color=RAYWHITE ) {}
   /*! Draw a filled circle within an image (Vector version) */
   void DrawCircleV( Vector2 center, float radius, Color color=RAYWHITE ) {}
   /*! Draw a gradient-filled circle */
   void DrawCircleGradient( Vector2 center, float radius, Color inner, Color outer ) {}
   /*! Draw a piece of a circle */
   void DrawCircleSector( Vector2 center, float radius, float startAngle, float endAngle, int segments, Color color=RAYWHITE ) {}
   /*! Draw circle sector outline */
   void DrawCircleSectorLines( Vector2 center, float radius, float startAngle, float endAngle, int segments, Color color=RAYWHITE ) {}
   /*! Draw circle outline */
   void DrawCircleLines( int centerX, int centerY, float radius, Color color=RAYWHITE ) {}
   /*! Draw circle outline (Vector version) */
   void DrawCircleLinesV( Vector2 center, float radius, Color color=RAYWHITE ) {}
   /*! Draw ellipse */
   void DrawEllipse( int centerX, int centerY, float radiusH, float radiusV, Color color=RAYWHITE ) {}
   /*! Draw ellipse (Vector version) */
   void DrawEllipseV( Vector2 center, float radiusH, float radiusV, Color color=RAYWHITE ) {}
   /*! Draw ellipse outline */
   void DrawEllipseLines( int centerX, int centerY, float radiusH, float radiusV, Color color=RAYWHITE ) {}
   /*! Draw ellipse outline (Vector version) */
   void DrawEllipseLinesV( Vector2 center, float radiusH, float radiusV, Color color=RAYWHITE ) {}
   /*! Draw ring */
   void DrawRing( Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, Color color=RAYWHITE ) {}
   /*! Draw ring outline */
   void DrawRingLines( Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, int segments, Color color=RAYWHITE ) {}
   /*! Draw a color-filled rectangle */
   void DrawRectangle( int posX, int posY, int width, int height, Color color=RAYWHITE ) {}
   /*! Draw a color-filled rectangle (Vector version) */
   void DrawRectangleV( Vector2 position, Vector2 size, Color color=RAYWHITE ) {}
   /*! Draw a color-filled rectangle */
   void DrawRectangleRec( Rectangle rec, Color color=RAYWHITE ) {}
   /*! Draw a color-filled rectangle with pro parameters */
   void DrawRectanglePro( Rectangle rec, Vector2 origin, float rotation, Color color=RAYWHITE ) {}
   /*! Draw a vertical-gradient-filled rectangle */
   void DrawRectangleGradientV( int posX, int posY, int width, int height, Color top, Color bottom ) {}
   /*! Draw a horizontal-gradient-filled rectangle */
   void DrawRectangleGradientH( int posX, int posY, int width, int height, Color left, Color right ) {}
   /*! Draw a gradient-filled rectangle with custom vertex colors */
   void DrawRectangleGradientEx( Rectangle rec, Color topLeft, Color bottomLeft, Color bottomRight, Color topRight ) {}
   /*! Draw rectangle outline */
   void DrawRectangleLines( int posX, int posY, int width, int height, Color color=RAYWHITE ) {}
   /*! Draw rectangle outline with extended parameters */
   void DrawRectangleLinesEx( Rectangle rec, float lineThick, Color color=RAYWHITE ) {}
   /*! Draw rectangle with rounded edges */
   void DrawRectangleRounded( Rectangle rec, float roundness, int segments, Color color=RAYWHITE ) {}
   /*! Draw rectangle lines with rounded edges */
   void DrawRectangleRoundedLines( Rectangle rec, float roundness, int segments, Color color=RAYWHITE ) {}
   /*! Draw rectangle lines with rounded edges outline */
   void DrawRectangleRoundedLinesEx( Rectangle rec, float roundness, int segments, float lineThick, Color color=RAYWHITE ) {}
   /*! Draw a color-filled triangle (vertex in counter-clockwise order!) */
   void DrawTriangle( Vector2 v1, Vector2 v2, Vector2 v3, Color color=RAYWHITE ) {}
   /*! Draw triangle with interpolated colors (vertex in counter-clockwise order!) */
   void DrawTriangleGradient( Vector2 v1, Vector2 v2, Vector2 v3, Color c1, Color c2, Color c3 ) {}
   /*! Draw triangle outline (vertex in counter-clockwise order!) */
   void DrawTriangleLines( Vector2 v1, Vector2 v2, Vector2 v3, Color color=RAYWHITE ) {}
   /*! Draw a triangle fan defined by points (first vertex is the center) */
   void DrawTriangleFan( Vector<F32> pointValues, int pointCount, Color color=RAYWHITE ) {}
   /*! Draw a triangle strip defined by points */
   void DrawTriangleStrip( Vector<F32> pointValues, int pointCount, Color color=RAYWHITE ) {}
   /*! Draw a polygon of n sides */
   void DrawPoly( Vector2 center, int sides, float radius, float rotation, Color color=RAYWHITE ) {}
   /*! Draw a polygon outline of n sides */
   void DrawPolyLines( Vector2 center, int sides, float radius, float rotation, Color color=RAYWHITE ) {}
   /*! Draw a polygon outline of n sides with extended parameters */
   void DrawPolyLinesEx( Vector2 center, int sides, float radius, float rotation, float lineThick, Color color=RAYWHITE ) {}
   /*! Draw spline: Linear, minimum 2 points */
   void DrawSplineLinear( Vector<F32> pointValues, int pointCount, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline: B-Spline, minimum 4 points */
   void DrawSplineBasis( Vector<F32> pointValues, int pointCount, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline: Catmull-Rom, minimum 4 points */
   void DrawSplineCatmullRom( Vector<F32> pointValues, int pointCount, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline: Quadratic Bezier, minimum 3 points (1 control point): [p1, c2, p3, c4...] */
   void DrawSplineBezierQuadratic( Vector<F32> pointValues, int pointCount, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline: Cubic Bezier, minimum 4 points (2 control points): [p1, c2, c3, p4, c5, c6...] */
   void DrawSplineBezierCubic( Vector<F32> pointValues, int pointCount, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline segment: Linear, 2 points */
   void DrawSplineSegmentLinear( Vector2 p1, Vector2 p2, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline segment: B-Spline, 4 points */
   void DrawSplineSegmentBasis( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline segment: Catmull-Rom, 4 points */
   void DrawSplineSegmentCatmullRom( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline segment: Quadratic Bezier, 2 points, 1 control point */
   void DrawSplineSegmentBezierQuadratic( Vector2 p1, Vector2 c2, Vector2 p3, float thick, Color color=RAYWHITE ) {}
   /*! Draw spline segment: Cubic Bezier, 2 points, 2 control points */
   void DrawSplineSegmentBezierCubic( Vector2 p1, Vector2 c2, Vector2 c3, Vector2 p4, float thick, Color color=RAYWHITE ) {}
   /*! Get (evaluate) spline point: Linear */
   Vector2 GetSplinePointLinear( Vector2 startPos, Vector2 endPos, float t ) {}
   /*! Get (evaluate) spline point: B-Spline */
   Vector2 GetSplinePointBasis( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t ) {}
   /*! Get (evaluate) spline point: Catmull-Rom */
   Vector2 GetSplinePointCatmullRom( Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t ) {}
   /*! Get (evaluate) spline point: Quadratic Bezier */
   Vector2 GetSplinePointBezierQuadratic( Vector2 p1, Vector2 c2, Vector2 p3, float t ) {}
   /*! Get (evaluate) spline point: Cubic Bezier */
   Vector2 GetSplinePointBezierCubic( Vector2 p1, Vector2 c2, Vector2 c3, Vector2 p4, float t ) {}
   /*! Check collision between two rectangles */
   bool CheckCollisionRecs( Rectangle rec1, Rectangle rec2 ) {}
   /*! Check collision between two circles */
   bool CheckCollisionCircles( Vector2 center1, float radius1, Vector2 center2, float radius2 ) {}
   /*! Check collision between circle and rectangle */
   bool CheckCollisionCircleRec( Vector2 center, float radius, Rectangle rec ) {}
   /*! Check if circle collides with a line created between two points [p1] and [p2] */
   bool CheckCollisionCircleLine( Vector2 center, float radius, Vector2 p1, Vector2 p2 ) {}
   /*! Check if point is inside rectangle */
   bool CheckCollisionPointRec( Vector2 point, Rectangle rec ) {}
   /*! Check if point is inside circle */
   bool CheckCollisionPointCircle( Vector2 point, Vector2 center, float radius ) {}
   /*! Check if point is inside a triangle */
   bool CheckCollisionPointTriangle( Vector2 point, Vector2 p1, Vector2 p2, Vector2 p3 ) {}
   /*! Check if point belongs to line created between two points [p1] and [p2] with defined margin in pixels [threshold] */
   bool CheckCollisionPointLine( Vector2 point, Vector2 p1, Vector2 p2, int threshold ) {}
   /*! Check if point is within a polygon described by array of vertices */
   bool CheckCollisionPointPoly( Vector2 point, Vector<F32> pointValues, int pointCount ) {}
   /*! Check the collision between two lines. Returns 'x y' string if colliding, or empty string if not. */
   Vector2 CheckCollisionLines( Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2 ) {}
   /*! Get collision rectangle for two rectangles collision */
   Rectangle GetCollisionRec( Rectangle rec1, Rectangle rec2 ) {}
   /*! Draw a line in 3D world space */
   void DrawLine3D( Vector3 startPos, Vector3 endPos, Color color ) {}
   /*! Draw a point in 3D space, actually a small line */
   void DrawPoint3D( Vector3 position, Color color ) {}
   /*! Draw a circle in 3D world space */
   void DrawCircle3D( Vector3 center, float radius, Vector3 rotationAxis, float rotationAngle, Color color ) {}
   /*! Draw a color-filled triangle (vertex in counter-clockwise order!) */
   void DrawTriangle3D( Vector3 v1, Vector3 v2, Vector3 v3, Color color ) {}
   /*! Draw a triangle strip defined by points */
   void DrawTriangleStrip3D( Vector<F32> pointValues, int pointCount, Color color ) {}
   /*! Draw cube */
   void DrawCube( Vector3 position, float width, float height, float length, Color color ) {}
   /*! Draw cube (Vector version) */
   void DrawCubeV( Vector3 position, Vector3 size, Color color ) {}
   /*! Draw cube wires */
   void DrawCubeWires( Vector3 position, float width, float height, float length, Color color ) {}
   /*! Draw cube wires (Vector version) */
   void DrawCubeWiresV( Vector3 position, Vector3 size, Color color ) {}
   /*! Draw sphere */
   void DrawSphere( Vector3 centerPos, float radius, Color color ) {}
   /*! Draw sphere with extended parameters */
   void DrawSphereEx( Vector3 centerPos, float radius, int rings, int slices, Color color ) {}
   /*! Draw sphere wires */
   void DrawSphereWires( Vector3 centerPos, float radius, int rings, int slices, Color color ) {}
   /*! Draw a cylinder/cone */
   void DrawCylinder( Vector3 position, float radiusTop, float radiusBottom, float height, int slices, Color color ) {}
   /*! Draw a cylinder with base at startPos and top at endPos */
   void DrawCylinderEx( Vector3 startPos, Vector3 endPos, float startRadius, float endRadius, int sides, Color color ) {}
   /*! Draw a cylinder/cone wires */
   void DrawCylinderWires( Vector3 position, float radiusTop, float radiusBottom, float height, int slices, Color color ) {}
   /*! Draw a cylinder wires with base at startPos and top at endPos */
   void DrawCylinderWiresEx( Vector3 startPos, Vector3 endPos, float startRadius, float endRadius, int slices, Color color ) {}
   /*! Draw a capsule with the center of its sphere caps at startPos and endPos */
   void DrawCapsule( Vector3 startPos, Vector3 endPos, float radius, int rings, int slices, Color color ) {}
   /*! Draw capsule wireframe with the center of its sphere caps at startPos and endPos */
   void DrawCapsuleWires( Vector3 startPos, Vector3 endPos, float radius, int rings, int slices, Color color ) {}
   /*! Draw a plane XZ */
   void DrawPlane( Vector3 centerPos, Vector2 size, Color color ) {}
   /*! Draw a ray line using the TypeRay format */
   void DrawRay( Ray ray, Color color ) {}
   /*! Draw a grid (centered at (0, 0, 0) */
   void DrawGrid( int slices, float spacing ) {}
   /*! Load model from files (meshes and materials) and return ModelID */
   S32 LoadModel( String fileName ) {}
   /*! Load model from generated mesh (default material) and return ModelID */
   S32 LoadModelFromMesh( int meshId ) {}
   /*! Gets the material count from a model by 
GetModelMapTexture(modelId, [mapMap=MATERIAL_MAP_DIFFUSE]) */
   S32 GetModelMaterialCount( int modelId, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! Gets the texture ID from a model material map.
GetModelMapTexture(modelId, [matIndex=0], [mapMap=MATERIAL_MAP_DIFFUSE]) */
   S32 GetModelMapTexture( int modelId, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! set a texture for a model material map like model.materials[0].maps[MATERIAL_MAP_DIFFUSE].texture = texture;SetModelMapTexture($model, $texture) << matIndex default 0,  matMap default MATERIAL_MAP_DIFFUSE  */
   bool SetModelMapTexture( int modelId, int textureId, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! Gets the color from a model material map.
GetModelMapColor(modelId, [matIndex=0], [mapMap=MATERIAL_MAP_DIFFUSE]) */
   Color GetModelMapColor( int modelId, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! set a color for a modal material map like model.materials[0].maps[MATERIAL_MAP_DIFFUSE].color = BLUE;SetModelMapColor($model, BLUE) << matIndex default 0,  matMap default MATERIAL_MAP_DIFFUSE  */
   bool SetModelMapColor( int modelId, Color color, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! Gets the float value from a model material map.
GetModelMapValue(modelId, [matIndex=0], [mapMap=MATERIAL_MAP_DIFFUSE]) */
   F32 GetModelMapValue( int modelId, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! Sets the float value for a model material map (e.g., roughness or metalness factor).
SetModelMapValue(modelId, value [, matIndex=0, mapMap=MATERIAL_MAP_DIFFUSE]) */
   bool SetModelMapValue( int modelId, float value, int matIndex=0, int mapMap=(S32)MATERIAL_MAP_DIFFUSE ) {}
   /*! Gets the shader ID from a model material.
GetModelShader(modelId [, matIndex=0]) */
   S32 GetModelShader( int modelId, int matIndex=0 ) {}
   /*! Sets a shader for a model material like model.materials[matIndex].shader = shader;
SetModelShader(modelId, shaderId [, matIndex=0]) */
   bool SetModelShader( int modelId, int shaderId, int matIndex=0 ) {}
   /*! Check if a model is valid (loaded in GPU, VAO/VBOs) */
   bool IsModelValid( int modelId ) {}
   /*! Unload model (including meshes) from memory (RAM and/or VRAM) */
   void UnloadModel( int modelId ) {}
   /*! Compute model bounding box limits (considers all meshes) */
   BoundingBox GetModelBoundingBox( int modelId ) {}
   /*! Draw a 3D model with position, scale and tint color */
   void DrawModel( int modelId, Vector3 position, float scale, Color tint=WHITE ) {}
   /*! Draw a 3D model with extended transform parameters */
   void DrawModelEx( int modelId, Vector3 position, Vector3 rotationAxis, float rotationAngle, Vector3 scale, Color tint=WHITE ) {}
   /*! Draw a 3D model wireframe */
   void DrawModelWires( int modelId, Vector3 position, float scale, Color tint=WHITE ) {}
   /*! Draw a 3D model wireframe with extended transform parameters */
   void DrawModelWiresEx( int modelId, Vector3 position, Vector3 rotationAxis, float rotationAngle, Vector3 scale, Color tint=WHITE ) {}
   /*! Draw a 3D bounding box wireframe */
   void DrawBoundingBox( BoundingBox box, Color color ) {}
   /*! Draw a 2D billboard texture in 3D space facing the camera */
   void DrawBillboard( int cameraId, int textureId, Vector3 position, float scale, Color tint=WHITE ) {}
   /*! Draw a part of a 2D billboard texture defined by a source rectangle */
   void DrawBillboardRec( int cameraId, int textureId, Rectangle source, Vector3 position, Vector2 size, Color tint=WHITE ) {}
   /*! Draw a billboard texture with pro transform parameters */
   void DrawBillboardPro( int cameraId, int textureId, Rectangle source, Vector3 position, Vector3 up, Vector2 size, Vector2 origin, float rotation, Color tint=WHITE ) {}
   /*! Upload mesh vertex data to GPU (VRAM) */
   void UploadMesh( int meshId, bool dynamic ) {}
   /*! Update mesh vertex data in GPU for a specific buffer index using a float list */
   void UpdateMeshBuffer( int meshId, int index, Vector<F32> dataValues, int dataSize, int offset ) {}
   /*! Unload mesh data from CPU and GPU memory */
   void UnloadMesh( int meshId ) {}
   /*! Returns a 3D identity matrix */
   Matrix MatrixIdentity() {}
   /*! Returns a translation matrix */
   Matrix MatrixTranslate( float x, float y, float z ) {}
   /*! Returns a rotation matrix from XYZ angles (in radians) */
   Matrix MatrixRotateXYZ( Vector3 angleRadians ) {}
   /*! Multiplies two matrices together */
   Matrix MatrixMultiply( Matrix left, Matrix right ) {}
   /*! Draw a 3d mesh with material and transform matrix */
   void DrawMesh( int meshId, int materialId, Matrix transform ) {}
   /*! Compute mesh bounding box limits */
   BoundingBox GetMeshBoundingBox( int meshId ) {}
   /*! Compute mesh tangents for normal-mapping */
   void GenMeshTangents( int meshId ) {}
   /*! Export mesh data to file (.obj), returns true on success */
   bool ExportMesh( int meshId, String fileName ) {}
   /*! Export mesh as code file (.h) defining vertex attribute arrays */
   bool ExportMeshAsCode( int meshId, String fileName ) {}
   /*! Generate polygonal mesh and return MeshID */
   S32 GenMeshPoly( int sides, float radius ) {}
   /*! Generate plane mesh (with subdivisions) and return MeshID */
   S32 GenMeshPlane( float width, float length, int resX, int resZ ) {}
   /*! Generate cuboid mesh and return MeshID */
   S32 GenMeshCube( float width, float height, float length ) {}
   /*! Generate sphere mesh (standard sphere) and return MeshID */
   S32 GenMeshSphere( float radius, int rings, int slices ) {}
   /*! Generate half-sphere mesh (no bottom cap) and return MeshID */
   S32 GenMeshHemiSphere( float radius, int rings, int slices ) {}
   /*! Generate cylinder mesh and return MeshID */
   S32 GenMeshCylinder( float radius, float height, int slices ) {}
   /*! Generate cone/pyramid mesh and return MeshID */
   S32 GenMeshCone( float radius, float height, int slices ) {}
   /*! Generate torus mesh and return MeshID */
   S32 GenMeshTorus( float radius, float size, int radSeg, int sides ) {}
   /*! Generate trefoil knot mesh and return MeshID */
   S32 GenMeshKnot( float radius, float size, int radSeg, int sides ) {}
   /*! Generate heightmap mesh from image data and return MeshID */
   S32 GenMeshHeightmap( int imageId, Vector3 size ) {}
   /*! Generate cubes-based map mesh from image data and return MeshID */
   S32 GenMeshCubicmap( int imageId, Vector3 cubeSize ) {}
   /*! Check if a material is valid */
   bool IsMaterialValid( int materialId ) {}
   /*! Unload material from GPU memory (VRAM) */
   void UnloadMaterial( int materialId ) {}
   /*! Set texture for a material map type (e.g. MATERIAL_MAP_DIFFUSE) */
   void SetMaterialTexture( int materialId, int mapType, int textureId ) {}
   /*! Set material for a specific mesh index inside a model */
   void SetModelMeshMaterial( int modelId, int meshId, int materialId ) {}
   /*! Check collision between two spheres */
   bool CheckCollisionSpheres( Vector3 center1, float radius1, Vector3 center2, float radius2 ) {}
   /*! Check collision between two bounding boxes */
   bool CheckCollisionBoxes( BoundingBox box1, BoundingBox box2 ) {}
   /*! Check collision between box and sphere */
   bool CheckCollisionBoxSphere( BoundingBox box, Vector3 center, float radius ) {}
   /*! Returns hit info string 'px py pz nx ny nz dist' or empty string */
   const char* GetRayCollisionSphere( Ray ray, Vector3 center, float radius ) {}
   /*! Returns hit info string 'px py pz nx ny nz dist' or empty string */
   const char* GetRayCollisionBox( Ray ray, BoundingBox box ) {}
   /*! Returns hit info string from 3D mesh collision */
   const char* GetRayCollisionMesh( Ray ray, int meshId, Matrix transform ) {}
   /*! Returns hit info string from 3D triangle collision */
   const char* GetRayCollisionTriangle( Ray ray, Vector3 p1, Vector3 p2, Vector3 p3 ) {}
   /*! Returns hit info string from 3D quad collision */
   const char* GetRayCollisionQuad( Ray ray, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4 ) {}
   /*! Load model animations from file and return an AnimationBlockID */
   S32 LoadModelAnimations( String fileName ) {}
   /*! Update model animation pose for a specific frame index */
   void UpdateModelAnimation( int modelId, int animBlockId, int animIndex, int frame ) {}
   /*! Get the FrameCount used by UpdateModelAnimation for a animaton */
   S32 GetModelAnimationFrameCount( int animBlockId, int animIndex ) {}
   /*! Get the boneIndex by queryname. used for mounting. Return -1 if not found. */
   S32 GetModelBoneIndexByName( int modelId, String queryname, bool caseSensitive=true ) {}
   /*! Unload model animation array data from memory */
   void UnloadModelAnimations( int animBlockId ) {}
   /*! Get total number of animations inside an AnimationBlock */
   int GetModelAnimationCount( int animBlockId ) {}
   /*! Generates a 3D cube model from dimensions and returns its modelId. */
   S32 GenModelCube( float width, float height, float length ) {}
   /*! Generates a 3D sphere model and returns its modelId. */
   S32 GenModelSphere( float radius, int rings, int slices ) {}
   /*! Generates a 3D cylinder model and returns its modelId. */
   S32 GenModelCylinder( float radius, float height, int slices ) {}
   /*! Initialize audio device and context */
   void InitAudioDevice() {}
   /*! Close the audio device and context */
   void CloseAudioDevice() {}
   /*! Check if audio device has been initialized successfully */
   bool IsAudioDeviceReady() {}
   /*! Set master volume (listener), value between 0.0 and 1.0 */
   void SetMasterVolume( float volume ) {}
   /*! Get master volume (listener) */
   float GetMasterVolume() {}
   /*! Load wave data from file and return WaveID */
   S32 LoadWave( String fileName ) {}
   /*! Check if wave data is valid */
   bool IsWaveValid( int waveId ) {}
   /*! Load sound from file and return SoundID */
   S32 LoadSound( String fileName ) {}
   /*! Load sound from wave data and return SoundID */
   S32 LoadSoundFromWave( int waveId ) {}
   /*! Load sound alias (shares sample data but can play independently) and return a new SoundID */
   S32 LoadSoundAlias( int sourceSoundId ) {}
   /*! Check if a sound is valid */
   bool IsSoundValid( int soundId ) {}
   /*! Unload wave data from memory */
   void UnloadWave( int waveId ) {}
   /*! Unload sound from memory */
   void UnloadSound( int soundId ) {}
   /*! Unload sound alias */
   void UnloadSoundAlias( int aliasSoundId ) {}
   /*! Export wave data to file (.wav), returns true on success */
   bool ExportWave( int waveId, String fileName ) {}
   /*! Export wave sample data to code (.h), returns true on success */
   bool ExportWaveAsCode( int waveId, String fileName ) {}
   /*! Play a sound using its SoundID */
   void PlaySound( int soundId ) {}
   /*! Stop playing a sound */
   void StopSound( int soundId ) {}
   /*! Pause a sound */
   void PauseSound( int soundId ) {}
   /*! Resume a paused sound */
   void ResumeSound( int soundId ) {}
   /*! Check if a sound is currently playing */
   bool IsSoundPlaying( int soundId ) {}
   /*! Set volume for a sound (0.0 to 1.0) */
   void SetSoundVolume( int soundId, float volume ) {}
   /*! Set pitch/speed for a sound (1.0 is base) */
   void SetSoundPitch( int soundId, float pitch ) {}
   /*! Set pan for a sound (-1.0 left, 0.0 center, 1.0 right) */
   void SetSoundPan( int soundId, float pan ) {}
   /*! Copy a wave to a new wave and return WaveID */
   S32 WaveCopy( int waveId ) {}
   /*! Crop a wave to defined frames range */
   void WaveCrop( int waveId, int initFrame, int finalFrame ) {}
   /*! Convert wave data to desired format */
   void WaveFormat( int waveId, int sampleRate, int sampleSize, int channels ) {}
   /*! Load music stream from file and return MusicID */
   S32 LoadMusicStream( String fileName ) {}
   /*! Check if a music stream is valid */
   bool IsMusicValid( int musicId ) {}
   /*! Unload music stream from VRAM and RAM */
   void UnloadMusicStream( int musicId ) {}
   /*! Start playing a music stream */
   void PlayMusicStream( int musicId ) {}
   /*! Check if music is currently playing */
   bool IsMusicStreamPlaying( int musicId ) {}
   /*! Update buffers for music streaming (MUST be called every frame!) */
   void UpdateMusicStream( int musicId ) {}
   /*! Stop music playing */
   void StopMusicStream( int musicId ) {}
   /*! Pause music playing */
   void PauseMusicStream( int musicId ) {}
   /*! Resume playing paused music */
   void ResumeMusicStream( int musicId ) {}
   /*! Seek music to a specific position in seconds */
   void SeekMusicStream( int musicId, float position ) {}
   /*! Set volume for music stream (0.0 to 1.0) */
   void SetMusicVolume( int musicId, float volume ) {}
   /*! Set pitch/speed for music stream */
   void SetMusicPitch( int musicId, float pitch ) {}
   /*! Set pan for music stream (-1.0 left, 0.0 center, 1.0 right) */
   void SetMusicPan( int musicId, float pan ) {}
   /*! Get total music time length in seconds */
   float GetMusicTimeLength( int musicId ) {}
   /*! Get current music time played in seconds */
   float GetMusicTimePlayed( int musicId ) {}
   /*! Set default buffer size for new audio streams (in samples) */
   void SetAudioStreamBufferSizeDefault( int size ) {}
   /*! Convert Color to float values */
   Vector4 ColorToVector4( Color col ) {}
   /*! Convert Color to float values */
   Color Vector4ToColor( Vector4 vec ) {}
   /*! Begin custom shader drawing using a ShaderID */
   void BeginShaderMode( int shaderId ) {}
   /*! End custom shader drawing and reset to the default Raylib shader */
   void EndShaderMode() {}
   /*! Load shader from files and return ShaderID */
   S32 LoadShader( String vsFileName, String fsFileName ) {}
   /*! Load shader from code strings and return ShaderID */
   S32 LoadShaderFromMemory( String vsCode, String fsCode ) {}
   /*! Check if a shader is valid (loaded on GPU) */
   bool IsShaderValid( int shaderId ) {}
   /*! Get shader uniform location index */
   int GetShaderLocation( int shaderId, String uniformName ) {}
   /*! set shader uniform location by index */
   bool SetShaderLocation( int shaderId, int shaderLocationIndex, int shaderLocation ) {}
   /*! set shader uniform location by uniformName */
   bool SetShaderLocationByUniformName( int shaderId, int shaderLocationIndex, String uniformName ) {}
   /*! Get shader uniform location from shader locs  */
   S32 GetShaderLocsLocation( int shaderId, int shaderLocationIndex ) {}
   /*! Get shader attribute location index */
   int GetShaderLocationAttrib( int shaderId, String attribName ) {}
   /*! Sets a shader uniform value parsed from a space-separated string based on uniformType. */
   bool SetShaderValue( int shaderId, int locIndex, String valueStr, int uniformType ) {}
   /*! Set shader uniform array/vector using a flat float list */
   void SetShaderValueV( int shaderId, int locIndex, Vector<F32> dataValues, int uniformType, int count ) {}
   /*! Set shader uniform value for a 4x4 matrix */
   void SetShaderValueMatrix( int shaderId, int locIndex, Matrix mat ) {}
   /*! Set shader uniform value and bind a Texture2D ID */
   void SetShaderValueTexture( int shaderId, int locIndex, int textureId ) {}
   /*! Unload shader from GPU memory (VRAM) */
   void UnloadShader( int shaderId ) {}
   /*! Load text data from file and return it as a string */
   const char* LoadFileText( String fileName ) {}
   /*! Save text data to a file, returns true on success */
   bool SaveFileText( String fileName, String text ) {}
   /*! Check if file exists */
   bool FileExists( String fileName ) {}
   /*! Check if a directory path exists */
   bool DirectoryExists( String dirPath ) {}
   /*! Replace text in an existing file. Returns count of replacements. */
   int FileTextReplace( String fileName, String search, String replacement ) {}
   /*! Find text in existing file, returns character index or -1 if not found */
   int FileTextFindIndex( String fileName, String search ) {}
   /*! Rename a file. Returns true on success. */
   bool FileRename( String fileName, String fileRename ) {}
   /*! Delete a file. Returns true on success. */
   bool FileRemove( String fileName ) {}
   /*! Copy a file from one path to another. Returns true on success. */
   bool FileCopy( String srcPath, String dstPath ) {}
   /*! Move a file from one directory to another. Returns true on success. */
   bool FileMove( String srcPath, String dstPath ) {}
   /*! Get file length in bytes */
   int GetFileLength( String fileName ) {}
   /*! Get extension for a filename string (includes dot: '.png') */
   const char* GetFileExtension( String fileName ) {}
   /*! Get filename for a path string */
   const char* GetFileName( String filePath ) {}
   /*! Get filename string without extension */
   const char* GetFileNameWithoutExt( String filePath ) {}
   /*! Create directories (including full path requested), returns true on success */
   bool MakeDirectory( String dirPath ) {}
   /*! Get the total file count in a directory */
   int GetDirectoryFileCount( String dirPath ) {}
   /*! Get file count with extension filtering and recursive directory scan. Use 'DIR' in filter to include directories. */
   int GetDirectoryFileCountEx( String basePath, String filter, bool scanSubdirs ) {}
   /*! Adds two Vector3 values together. */
   Vector3 Vector3Add( Vector3 v1, Vector3 v2 ) {}
   /*! Subtracts v2 from v1. */
   Vector3 Vector3Subtract( Vector3 v1, Vector3 v2 ) {}
   /*! Scales a Vector3 by a float multiplier. */
   Vector3 Vector3Scale( Vector3 v, float scale ) {}
   /*! Normalizes a Vector3 to a length of 1.0. */
   Vector3 Vector3Normalize( Vector3 v ) {}
   /*! Returns the length of a Vector3. */
   F32 Vector3Length( Vector3 v ) {}
   /*! Returns the distance between two Vector3 points. */
   F32 Vector3Distance( Vector3 v1, Vector3 v2 ) {}
};
