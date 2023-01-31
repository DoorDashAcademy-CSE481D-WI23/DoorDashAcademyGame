#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OnEditorSave : AssetModificationProcessor
{

    private static string[] OnWillSaveAssets(string[] paths)
    {
        TilemapSerializer serializer = GameObject.FindObjectOfType<TilemapSerializer>();
        if (serializer != null) {
            serializer.SaveChanges();
        }

        return paths;
    }

}
#endif
