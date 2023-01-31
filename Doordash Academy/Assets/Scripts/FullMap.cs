using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMap : MonoBehaviour
{
    public GameObject mapCamera;
    public Rect cameraBounds;

    private Transform cameraTransform;
    private float xMin, xMax, yMin, yMax;
    

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = mapCamera.GetComponent<Transform>();
        xMin = cameraBounds.xMin;
        xMax = cameraBounds.xMax;
        yMin = cameraBounds.yMin;
        yMax = cameraBounds.yMax;
    }

    // Update is called once per frame
    void Update()
    {
        // Here, the traditional `GetAxis` method will not work
        // since the time scale is 0, use the raw version instead.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        float x = Mathf.Clamp(cameraTransform.position.x + h, xMin, xMax);
        float y = Mathf.Clamp(cameraTransform.position.y + v, yMin, yMax);
        cameraTransform.position = new Vector3(x, y, cameraTransform.position.z);
    }
}
