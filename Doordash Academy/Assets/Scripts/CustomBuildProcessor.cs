#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This runs when the project is being built. It opens each scene, looks for a tilemap serializer,
 * and if it finds one it loads the tilemap. This way our automated builds will make the game with the
 * map even if the tilemaps are cleared in the repo to save on space.
 */
class CustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            Scene currScene = EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i));
            TilemapSerializer serializer = GameObject.FindObjectOfType<TilemapSerializer>();
            if (serializer != null) {
                serializer.LoadTilemap();
                EditorSceneManager.SaveScene(currScene);
            }
            EditorSceneManager.CloseScene(currScene, false);
        }
    }
}
#endif