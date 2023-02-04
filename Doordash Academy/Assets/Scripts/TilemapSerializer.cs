#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/* The purpose of this script is to allow us to use very large tilemaps without having very large scene files.
 * It saves the tilemap data in a more space-efficient way in a separate file, instead of saving many objects
 * in the scene. It should be attached to the Grid object that is the parent of the tilemaps.
 */
public class TilemapSerializer : MonoBehaviour
{

    public void ClearTileMaps() {
        foreach(Transform child in gameObject.transform) {
            Tilemap tileMap = child.GetComponent<Tilemap>();
            if (tileMap == null) continue;
            tileMap.ClearAllTiles();
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // load in the tilemap data from save file so that it can be edited
    public void LoadTilemap()
    {
        string path = "TilemapData/" + SceneManager.GetActiveScene().name;
        TextAsset[] JsonFiles = Resources.LoadAll<TextAsset>(path);
        Debug.Log("tried path " + path + ". found "+ JsonFiles.Length);
        Tile[] tiles = Resources.LoadAll<Tile>("Tiles");
        Dictionary<string, Tile> tileCache = new Dictionary<string, Tile>();
        for(int i = 0; i < JsonFiles.Length; i++) {
            Debug.Log("processing file "+ JsonFiles[i].name);
            Tilemap tileMap = GameObject.Find(JsonFiles[i].name).GetComponent<Tilemap>();
            TileMapData mapData = JsonUtility.FromJson<TileMapData>(JsonFiles[i].text);
            int minX = mapData.minX;
            int maxX = mapData.maxX;
            int minY = mapData.minY;
            int maxY = mapData.maxY;
            int minZ = mapData.minZ;
            int maxZ = mapData.maxZ;
            for(int x = minX; x < maxX; x++){
                for(int y = minY; y< maxY; y++){
                    for(int z = minZ; z < maxZ; z++){
                        string neededTile = mapData.data[(x-minX) + (maxX-minX+1) * ((y-minY) + (maxY-minY+1) * z)];
                        if (tileCache.ContainsKey(neededTile)) {
                            tileMap.SetTile(new Vector3Int(x,y,z), tileCache[neededTile]);
                        } else {
                            for(int j = 0; j < tiles.Length; j++) {
                                if (tiles[j].name == neededTile) {
                                    tileMap.SetTile(new Vector3Int(x,y,z), tiles[j]);
                                    tileCache[neededTile] = tiles[j];
                                    j = tiles.Length;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // save the current state of the tilemaps to the save file, overwriting anything there
    public void SaveChanges()
    {
        string path = Path.Combine(Application.dataPath, "Resources");
        path = Path.Combine(path, "TilemapData");
        path = Path.Combine(path, SceneManager.GetActiveScene().name);
        if(!Directory.Exists(path)){
            Directory.CreateDirectory(path); // Create the directory if it doesn't exist
            Debug.Log("Created directory: " + path);
        }
        bool didSave = false;
        foreach(Transform child in gameObject.transform) {
            didSave |= saveTileMap(child.gameObject, path);
        }
        if (didSave) Debug.Log("saved contents of tilemap into TilemapData folder");
        AssetDatabase.Refresh();
    }

    // returns whether the tilemap was actually saved
    private bool saveTileMap(GameObject obj, string path) {
        Tilemap tileMap = obj.GetComponent<Tilemap>();
        if (tileMap == null) return false;

        // Debug.Log("tilemap " + obj.name + " has size " + tileMap.size + " and cellBounds " + tileMap.cellBounds + " and localBounds "+ tileMap.localBounds);
        path = Path.Combine(path, obj.name + ".txt");

        int minX = tileMap.cellBounds.min.x;
        int maxX = tileMap.cellBounds.max.x;
        int minY = tileMap.cellBounds.min.y;
        int maxY = tileMap.cellBounds.max.y;
        int minZ = tileMap.cellBounds.min.z;
        int maxZ = tileMap.cellBounds.max.z;
        string[] data = new string[(maxX-minX + 1)*(maxY-minY + 1)*(maxZ-minZ + 1)];
        bool shouldSave = false;
        for(int x = minX; x < maxX; x++){
            for(int y = minY; y< maxY; y++){
                for(int z = minZ; z < maxZ; z++){
                    TileBase tile = tileMap.GetTile(new Vector3Int(x,y,z));
                    if (tile == null) continue;
                    shouldSave = true;
                    data[(x-minX) + (maxX-minX+1) * ((y-minY) + (maxY-minY+1) * z)] = tile.name;
                }
            }
        }
        if (shouldSave) {
            File.WriteAllText(path, JsonUtility.ToJson(new TileMapData(tileMap, data)));
        }
        return shouldSave;
    }

    public class TileMapData
    {
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;
        public int minZ;
        public int maxZ;
        public string[] data;

        public TileMapData(Tilemap _tm, string[] _data) {
            minX = _tm.cellBounds.min.x;
            maxX = _tm.cellBounds.max.x;
            minY = _tm.cellBounds.min.y;
            maxY = _tm.cellBounds.max.y;
            minZ = _tm.cellBounds.min.z;
            maxZ = _tm.cellBounds.max.z;
            data = _data;
        }
    }
}
#endif