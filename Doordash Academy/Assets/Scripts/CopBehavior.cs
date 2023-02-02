using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CopBehavior : VehicleBehavior
{
    public Light2D red;
    public Light2D blue;

    private float NextLightChange;
    private float LightInterval = 0.5f;

    protected override void Start()
    {
        base.Start();
        NextLightChange = Time.time + LightInterval;
        red.enabled = true;
        blue.enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Time.time > NextLightChange) {
            red.enabled = !red.enabled;
            blue.enabled = !blue.enabled;
            NextLightChange = Time.time + LightInterval;
        }
    }
}
