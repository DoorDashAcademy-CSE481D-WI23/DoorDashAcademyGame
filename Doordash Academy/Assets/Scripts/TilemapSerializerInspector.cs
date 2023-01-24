using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapSerializer))]
public class TilemapSerializerInspector : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    TilemapSerializer grid = (TilemapSerializer)target;
    if (GUILayout.Button("Load Tilemap")) {
        grid.LoadTilemap();
    }
    if (GUILayout.Button("Save Tilemap")) {
        grid.SaveChanges();
    }
    if (GUILayout.Button("Clear Scene for commiting to GitHub")) {
        grid.SaveChanges();
        grid.ClearTileMaps();
    }
  }
}
