using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBehavior : MonoBehaviour
{

    public float InitialSpeed = 0f;
    public float Acceleration = 1f;
    public float MaxSpeed = 7f;
    public float BrakingPower = 2f;
    // 0: N, 1: E, 2: S, 3: W
    public int direction = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void ApplyGas() {

    }

    private void ApplyBrakes() {

    }
}
