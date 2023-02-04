using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private CarController controller;

    // Triggers when script instance is loaded
    void Awake()
    {
        controller = GetComponent<CarController>();
    }

    // On every frame update
    void Update()
    {
        // Update underlying car
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        controller.SetInputVector(new Vector3(h, v, 0));
    }
}
