#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/* This is a one-time use script to replace the car game objects with car prefabs
* To use it, it was placed on a Road that had cars as children, then the corresponding prefab
* was drug into the editor, and then the button in the inspector was clicked.
* I'll keep it around just in case it's useful later
*/
public class CarReplacer : MonoBehaviour
{
    public GameObject prefab;
    public void ReplaceCars() {
        Debug.Log("replacing cars...");
        List<GameObject> oldCars = new List<GameObject>();
        for(int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            if (obj.GetComponent<VehicleBehavior>() != null && obj.GetComponent<CopBehavior>() == null) {
                oldCars.Add(obj);
            }
        }
        for(int i = 0; i < oldCars.Count; i++) {
            GameObject oldcar = oldCars[i];
            Vector3 oldCarPos = oldcar.transform.position;
            GameObject newcar = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            newcar.transform.parent = gameObject.transform;
            newcar.transform.position = oldCarPos;
            DestroyImmediate(oldcar);
        }
    }
}
#endif