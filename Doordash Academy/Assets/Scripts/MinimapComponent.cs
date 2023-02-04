using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to a child object of the player
public class MinimapComponent : MonoBehaviour
{
    public float zoomTransitionDuration = 0.15f;
    private  AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
    private Transform parentTransform;
    private Transform t;
    private Vector3 zoomedOutPosition = new Vector3(26.5f, 33.1f, -139.17f);
    private float zoomedInZ;
    private Transform minimapTransform;
    private bool isZoomedOut = false;

    void Start()
    {
        t = gameObject.GetComponent<Transform>();
        parentTransform = t.parent;
        zoomedInZ = t.position.z;
        minimapTransform = GameObject.Find("Minimap Frame").transform;
    }

    // Update is called once per frame
    void Update()
    {
        t.rotation = Quaternion.identity;

        if (Input.GetKeyDown(KeyCode.M))
            ToggleZoom();
    }

    public void ToggleZoom() {
        if (isZoomedOut) {
            // we need to zoom in
            t.parent = parentTransform;
            StartCoroutine(moveTo(new Vector3(parentTransform.position.x, parentTransform.position.y, zoomedInZ), new Vector3(1,1,1)));
            isZoomedOut = false;
        } else {
            // we need to zoom out
            t.parent = null;
            t.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            StartCoroutine(moveTo(zoomedOutPosition, new Vector3(2,2,1)));
            isZoomedOut = true;
        }
    }

    IEnumerator moveTo(Vector3 cameraPosition, Vector3 minimapScale) {
        Vector3 startingPosition = t.position;
        Vector3 startingScale = minimapTransform.localScale;
        float time = Time.deltaTime;
        while (time < zoomTransitionDuration)
        {
            t.position = startingPosition + curve.Evaluate(time/zoomTransitionDuration) * (cameraPosition-startingPosition);
            minimapTransform.localScale = startingScale + curve.Evaluate(time/zoomTransitionDuration) * (minimapScale - startingScale);
            time += Time.deltaTime;
            yield return 0;
        }
        t.position = cameraPosition;
        minimapTransform.localScale = minimapScale;
    }
}
