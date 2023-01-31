using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMap : MonoBehaviour
{
    public GameObject mapCamera;
    public GameObject mapBounds;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = mapCamera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Here, the traditional `GetAxis` method will not work
        // since the time scale is 0, use the raw version instead.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        cameraTransform.position += new Vector3(h, v, 0);
    }
}
