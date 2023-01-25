using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to a child object of the player
public class MinimapComponent : MonoBehaviour
{
    private Transform parentTransform;
    private Transform t;

    void Start()
    {
        t = gameObject.GetComponent<Transform>();
        parentTransform = t.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // counteract the rotation
        float r = parentTransform.localEulerAngles.z % 360f;
        t.localEulerAngles = new Vector3(0.0f, 0.0f, -r);
    }
}
