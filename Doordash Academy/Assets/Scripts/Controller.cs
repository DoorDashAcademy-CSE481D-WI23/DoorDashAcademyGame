using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Components
    CarController controller;

    // Triggers when script instance is loaded
    void Awake()
    {
        controller = GetComponent<CarController>();
    }

    // On every frame update
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(h, v, 0);
        controller.SetInputVector(inputVector);
    }
}
