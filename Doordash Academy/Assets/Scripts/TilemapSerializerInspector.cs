#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapSerializer))]
public class TilemapSerializerInspector : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    TilemapSerializer grid = (TilemapSerializer)target;
    if (GUILayout.Button("Load Tilemap (might take a minute)")) {
        grid.LoadTilemap();
    }
    if (GUILayout.Button("Clear Scene for commiting to GitHub")) {
        grid.SaveChanges();
        grid.ClearTileMaps();
    }
  }
}
#endif
