//-----------------------------------------------------------------------------
// Copyright (c) 2026 Thomas Hühn (XXTH)
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
// Terrain ! :D,
// [ ] TODO add SceneObjectStub  Class
//-----------------------------------------------------------------------------

#include "console/engineAPI.h"
#include "math/mMathRand.h"
#include "console/simSet.h"

#include "raylib.h"
#include "raymath.h"

#include "elfTools.h"
#include "ConsoleTypes.h"
#include "elfResource.h"

#include "SceneObject.h"


namespace ElfObjects {

using namespace ElfMath;

class TerrainObject: public SceneObject
{
    typedef SceneObject Parent;
public:
    DECLARE_CONOBJECT(TerrainObject);
    StringTableEntry mHeightMapFilename;
    Vector3 mSize = { 256.f, 64.f, 256.f};
    Model mModel = { 0 };
    Vector3 mPosition = { 0.f, 0.f, 0.f };

    Vector<F32> mHeightGrid;
    S32 mGridWidth = 0;
    S32 mGridHeight = 0;


    TerrainObject() {
        mHeightMapFilename = StringTable->EmptyString();
    }

    bool onAdd() override;
    void onRemove() override;

    static void initPersistFields()
    {
        addField("HeightMapFilename", TypeString, Offset(mHeightMapFilename, TerrainObject), "You need to call load() after changing it.");
        addField("Size", TypeVector3, Offset(mSize, TerrainObject), "You need to call load() after changing it.");

        addField("Position", TypeVector3, Offset(mPosition, TerrainObject), "Render position of the heightmap");
        Parent::initPersistFields();
    }
    // -----
    F32 getHeight(Vector3 worldPos);
    Vector3 getNormal(Vector3 worldPos);
    RayCollision castRay(Ray ray) override;
    // -----
    S32 getModelId() {return mModelId;}; //model id in resource manager
    // -----
    bool load() { return loadAutoTexture();}
    bool loadAutoTexture();
    bool loadBasic();
    void draw() override;

private:
    S32 mModelId = 0; // model id in resource manager
    void unloadInternal();
    void genHeightGrid(Image* image, Color* heightPixels );
};

IMPLEMENT_CONOBJECT(TerrainObject);
// -----------------------------------------------------------------------------
// generate the heighgrid from the heighmap image and pixels
// called before the image is unloaded.
void TerrainObject::genHeightGrid(Image* image, Color* heightPixels ){
    if (!image || !heightPixels) return ;

    mGridWidth = image->width;
    mGridHeight = image->height;
    mHeightGrid.setSize(mGridWidth * mGridHeight);
    for (int i = 0; i < mGridWidth * mGridHeight; i++) {
        mHeightGrid[i] = (float)heightPixels[i].r / 255.0f;
    }
}
// -----------------------------------------------------------------------------
F32 TerrainObject::getHeight(Vector3 worldPos) {
    if (mHeightGrid.empty() || mGridWidth <= 1 || mGridHeight <= 1) return mPosition.y;

    // worldpos to localpos
    F32 localX = worldPos.x - mPosition.x;
    F32 localZ = worldPos.z - mPosition.z;
    // F32 localX = worldPos.x - mPosition.x + (mSize.x / 2.0f);
    // F32 localZ = worldPos.z - mPosition.z + (mSize.z / 2.0f);


    // to grid
    F32 gridX = (localX / mSize.x) * (mGridWidth - 1);
    F32 gridZ = (localZ / mSize.z) * (mGridHeight - 1);

    // Failsafe: out of terrain
    if (gridX < 0.0f || gridX >= (mGridWidth - 1) || gridZ < 0.0f || gridZ >= (mGridHeight - 1)) {
        return mPosition.y;
    }

    // get corner points
    int x0 = (int)gridX;
    int z0 = (int)gridZ;
    int x1 = x0 + 1;
    int z1 = z0 + 1;

     // normalized local position inside the quad
    F32 dx = gridX - x0;
    F32 dz = gridZ - z0;

    // heightvalues
    F32 h00 = mHeightGrid[z0 * mGridWidth + x0]; // top left
    F32 h10 = mHeightGrid[z0 * mGridWidth + x1]; // top right
    F32 h01 = mHeightGrid[z1 * mGridWidth + x0]; // bottom left
    F32 h11 = mHeightGrid[z1 * mGridWidth + x1]; // bottom right

    F32 interpolatedHeightFactor = 0.0f;

    // linear Interpolation
    if (dx + dz <= 1.0f) {
        // top left  triangle
        interpolatedHeightFactor = h00 + dx * (h10 - h00) + dz * (h01 - h00);
    } else {
        // bottom right triangle
        interpolatedHeightFactor = h11 + (1.0f - dx) * (h01 - h11) + (1.0f - dz) * (h10 - h11);
    }

    // multiply with terrain height
    return mPosition.y + (interpolatedHeightFactor * mSize.y);
}

// -----------------------------------------------------------------------------
void TerrainObject::draw()
{
     if (mModel.meshCount > 0) DrawModel(mModel, mPosition, 1.0f, WHITE);
}

// -----------------------------------------------------------------------------
void TerrainObject::unloadInternal() {
    if (mModel.meshCount > 0) {
        Texture2D terrainTexture = mModel.materials[0].maps[MATERIAL_MAP_ALBEDO].texture;
        if (terrainTexture.id > 0) {
            ::UnloadTexture(terrainTexture);
        }
    }
    if (mModelId > 0) {
        ElfResource::ModelMap.remove(mModelId);
        mModelId = 0;
        mModel = { 0 };
    }
}
// -----------------------------------------------------------------------------
bool TerrainObject::loadAutoTexture() {
    if (!mHeightMapFilename || mHeightMapFilename == StringTable->EmptyString()) {
        Con::errorf("TerrainObject::load: No HeightMapFilename specified.");
        return false;
    }

    Image image = ::LoadImage(mHeightMapFilename);
    if (!::IsImageValid(image)) {
        Con::errorf("TerrainObject::load: Could not load heightmap file: %s", mHeightMapFilename);
        return false;
    }

    Image diffuseImage = ::GenImageColor(image.width, image.height, BLANK);

    Color* heightPixels = ::LoadImageColors(image);
    Color* diffusePixels = ::LoadImageColors(diffuseImage);

    int pixelCount = image.width * image.height;


    for (int i = 0; i < pixelCount; i++) {
        U8 height = heightPixels[i].r; // 0 bis 255

        // Noise-Effekt
        S32 noise = ElfMath::mRandI(-8,8);

        Color baseColor;

        // Zone 1: Beach / Sand
        if (height < 45) {
            baseColor.r = (U8)mClamp(230 + noise, 0, 255);
            baseColor.g = (U8)mClamp(215 + noise, 0, 255);
            baseColor.b = (U8)mClamp(160 + noise, 0, 255);
            baseColor.a = 255;
        }
        // Zone 2: Transition from sand to grass (mixed zone)
        else if (height < 60) {
            F32 factor = (height - 45) / 15.0f;

            baseColor.r = (U8)((1.0f - factor) * 230 + factor * 70);
            baseColor.g = (U8)((1.0f - factor) * 215 + factor * 140);
            baseColor.b = (U8)((1.0f - factor) * 160 + factor * 60);
            baseColor.a = 255;

            baseColor.r = (U8)mClamp(baseColor.r + noise, 0, 255);
            baseColor.g = (U8)mClamp(baseColor.g + noise, 0, 255);
            baseColor.b = (U8)mClamp(baseColor.b + noise, 0, 255);
        }
        // Zone 3: Meadow / Grass
        else if (height < 135) {
            int grassNoise = (rand() % 25) - 12;
            baseColor.r = (U8)mClamp(70 + grassNoise, 0, 255);
            baseColor.g = (U8)mClamp(140 + grassNoise / 2, 0, 255);
            baseColor.b = (U8)mClamp(60 + grassNoise, 0, 255);
            baseColor.a = 255;
        }
        // Zone 4: Transition from grass to rock
        else if (height < 155) {
            F32 factor = (height - 135) / 20.0f;
            baseColor.r = (U8)((1.0f - factor) * 70 + factor * 110);
            baseColor.g = (U8)((1.0f - factor) * 140 + factor * 105);
            baseColor.b = (U8)((1.0f - factor) * 60 + factor * 100);
            baseColor.a = 255;

            baseColor.r = (U8)mClamp(baseColor.r + noise, 0, 255);
            baseColor.g = (U8)mClamp(baseColor.g + noise, 0, 255);
            baseColor.b = (U8)mClamp(baseColor.b + noise, 0, 255);
        }
        // Zone 5: Rock / Mountains
        else if (height < 210) {
            baseColor.r = (U8)mClamp(110 + noise, 0, 255);
            baseColor.g = (U8)mClamp(105 + noise, 0, 255);
            baseColor.b = (U8)mClamp(100 + noise, 0, 255);
            baseColor.a = 255;
        }
        // Zone 6: Snow Peaks
        else {
            int snowNoise = (rand() % 10) - 5;
            baseColor.r = (U8)mClamp(245 + snowNoise, 0, 255);
            baseColor.g = (U8)mClamp(245 + snowNoise, 0, 255);
            baseColor.b = (U8)mClamp(250 + snowNoise, 0, 255);
            baseColor.a = 255;
        }

        diffusePixels[i] = baseColor;
    }


    dMemcpy(diffuseImage.data, diffusePixels, pixelCount * sizeof(Color));

    ::UnloadImageColors(heightPixels);
    ::UnloadImageColors(diffusePixels);

    unloadInternal();

    Mesh mesh = ::GenMeshHeightmap(image, mSize);
    this->genHeightGrid(&image, heightPixels);

    ::UnloadImage(image);

    mModel = ::LoadModelFromMesh(mesh);

    if (mModel.meshCount > 0) {
        mModelId = ElfResource::ModelMap.add(mModel);
    }

    Texture2D terrainTexture = ::LoadTextureFromImage(diffuseImage);
    ::UnloadImage(diffuseImage);

    if (mModel.meshCount > 0) {
        mModel.materials[0].maps[MATERIAL_MAP_ALBEDO].texture = terrainTexture;
        mModel.materials[0].maps[MATERIAL_MAP_ALBEDO].color = WHITE;
    }

    return (mModel.meshCount > 0);
}

// -----------------------------------------------------------------------------
bool TerrainObject::loadBasic() {
    if (!mHeightMapFilename || mHeightMapFilename == StringTable->EmptyString()) {
        Con::errorf("Terrain::load: No HeightMapFilename specified.");
        return false;
    }

    Image image = ::LoadImage(mHeightMapFilename);
    if (!::IsImageValid(image)) {
        Con::errorf("Terrain::load: Could not load heightmap file: %s", mHeightMapFilename);
        return false;
    }

    unloadInternal();

    Vector3 scale = { mSize.x, mSize.y, mSize.z };
    Mesh mesh = ::GenMeshHeightmap(image, scale);
    ::UnloadImage(image);

    mModel = ::LoadModelFromMesh(mesh);

    if (mModel.meshCount > 0) {
         mModelId = ElfResource::ModelMap.add(mModel);
    }

    return (mModel.meshCount > 0);
}

// -----------------------------------------------------------------------------
bool TerrainObject::onAdd() {
    if (!Parent::onAdd()) return false;

    if (mHeightMapFilename && mHeightMapFilename != StringTable->EmptyString()) {
        if (!load()) {
            Con::warnf("Terrain::onAdd: Initial load failed for %s", mHeightMapFilename);
            return false;
        }
    }
    return true;
}

// -----------------------------------------------------------------------------
void TerrainObject::onRemove() {
    unloadInternal();
    Parent::onRemove();
}

// -----------------------------------------------------------------------------
Vector3 TerrainObject::getNormal(Vector3 worldPos) {

    Vector3 defaultNormal = { 0.0f, 1.0f, 0.0f };
    if (mHeightGrid.empty() || mGridWidth <= 1 || mGridHeight <= 1) return defaultNormal;

    F32 localX = worldPos.x - mPosition.x + (mSize.x / 2.0f);
    F32 localZ = worldPos.z - mPosition.z + (mSize.z / 2.0f);

    F32 gridX = (localX / mSize.x) * (mGridWidth - 1);
    F32 gridZ = (localZ / mSize.z) * (mGridHeight - 1);

    if (gridX < 0.0f || gridX >= (mGridWidth - 1) || gridZ < 0.0f || gridZ >= (mGridHeight - 1)) {
        return defaultNormal;
    }

    int x0 = (int)gridX;
    int z0 = (int)gridZ;
    int x1 = x0 + 1;
    int z1 = z0 + 1;

    F32 dx = gridX - x0;
    F32 dz = gridZ - z0;

    // Calculate the real 3D points of the vertices on the terrain To do this,
    // we calculate the grid indices back into world coordinates
    F32 worldX0 = mPosition.x - (mSize.x / 2.0f) + ((float)x0 / (mGridWidth - 1)) * mSize.x;
    F32 worldX1 = mPosition.x - (mSize.x / 2.0f) + ((float)x1 / (mGridWidth - 1)) * mSize.x;
    F32 worldZ0 = mPosition.z - (mSize.z / 2.0f) + ((float)z0 / (mGridHeight - 1)) * mSize.z;
    F32 worldZ1 = mPosition.z - (mSize.z / 2.0f) + ((float)z1 / (mGridHeight - 1)) * mSize.z;

    // Fetching and scaling the 4 height values
    F32 y00 = mPosition.y + (mHeightGrid[z0 * mGridWidth + x0] * mSize.y); // Oben Links
    F32 y10 = mPosition.y + (mHeightGrid[z0 * mGridWidth + x1] * mSize.y); // Oben Rechts
    F32 y01 = mPosition.y + (mHeightGrid[z1 * mGridWidth + x0] * mSize.y); // Unten Links
    F32 y11 = mPosition.y + (mHeightGrid[z1 * mGridWidth + x1] * mSize.y); // Unten Rechts

    Vector3 p00 = { worldX0, y00, worldZ0 };
    Vector3 p10 = { worldX1, y10, worldZ0 };
    Vector3 p01 = { worldX0, y01, worldZ1 };
    Vector3 p11 = { worldX1, y11, worldZ1 };

    Vector3 normal = defaultNormal;

    // Depending on which of the two triangles we are on
    if (dx + dz <= 1.0f) {
        // Oberes/Linkes Dreieck (p00 -> p10 -> p01)
        Vector3 v1 = { p10.x - p00.x, p10.y - p00.y, p10.z - p00.z };
        Vector3 v2 = { p01.x - p00.x, p01.y - p00.y, p01.z - p00.z };
        // Cross product v2 x v1 for raylib CCW array
        normal = Vector3CrossProduct(v2, v1);
    } else {
        // Lower/Right Triangle (p11 -> p01 -> p10)
        Vector3 v1 = { p01.x - p11.x, p01.y - p11.y, p01.z - p11.z };
        Vector3 v2 = { p10.x - p11.x, p10.y - p11.y, p10.z - p11.z };
        // Cross product v1 x v2
        normal = Vector3CrossProduct(v1, v2);
    }

    // normalize
    return Vector3Normalize(normal);
}
// -----------------------------------------------------------------------------
RayCollision TerrainObject::castRay(Ray ray) {
    if (mModel.meshCount <= 0) return RayCollision({0});

    Matrix terrainTransform = MatrixTranslate(mPosition.x, mPosition.y, mPosition.z);
    return ::GetRayCollisionMesh(ray, mModel.meshes[0], terrainTransform);
}

// -----------------------------------------------------------------------------
// -----------------------------------------------------------------------------
DefineEngineMethod(TerrainObject, load, bool, (), , "Load or reload the terrain from the specified HeightMapFilename.") {
    return object->load();
}

DefineEngineMethod(TerrainObject, draw, void, (), , "Draw  heightmap") {
    object->draw();
}

// ElfScript ==> %height = %terrain.getHeight(%playerPos);
DefineEngineMethod(TerrainObject, getHeight, F32, (Vector3 position), ,
                   "Returns the exact terrain height (Y-coordinate) at the given world position.") {
    return object->getHeight(position);
}

DefineEngineMethod(TerrainObject, getNormal, Vector3, (Vector3 position), ,
                   "Returns the surface normal vector at the given world position.") {
    return object->getNormal(position);
}

DefineEngineMethod(TerrainObject, getRayCollision, String, (Ray ray), ,
                   "Performs a raycast collision check against the terrain"
                   "and returns 'X Y Z Nx Ny Nz Dist' or empty string.") {
    return ElfTools::FormatRayCollision( object->castRay(ray));

}

DefineEngineMethod(TerrainObject, getModelId, S32, (), ,
                   "Returns the resource-manager compatible model ID of the terrain's 3D mesh.") {
    return object->getModelId();
}

} //namespace ElfTerrain
