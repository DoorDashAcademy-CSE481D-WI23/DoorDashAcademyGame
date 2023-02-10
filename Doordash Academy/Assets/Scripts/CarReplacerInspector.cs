#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarReplacer))]
public class CarReplacerInspector : Editor
{
  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    CarReplacer replacer = (CarReplacer)target;
    if (GUILayout.Button("replace all cars with prefab instances")) {
        replacer.ReplaceCars();
    }
  }
}
#endif
