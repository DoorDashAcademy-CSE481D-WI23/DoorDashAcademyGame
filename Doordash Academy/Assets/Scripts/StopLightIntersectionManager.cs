using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StopLightIntersectionManager : MonoBehaviour
{
    // r: 2.97 y: 2.725 g: 2.597
    public Light2D north;
    // r: 3.25 y: 3.06 g: 2.87
    public Light2D east;
    // r: 3.0 y: 2.85 g: 2.6
    public Light2D south;
    // r: 2.94 y: 2.75 g: 2.56
    public Light2D west;

    public float LightChangeInterval = 8f;
    public float YellowLightDuration = 3f;
    private float NextLightChange;
    private bool NorthSouthGreen;
    private bool YellowLights;

    public void Start() {
        SetLightColorAndYPos(north, 2.597f, Color.green);

        SetLightColorAndYPos(south, 2.56f, Color.green);

        SetLightColorAndYPos(east, 3.25f, Color.red);

        SetLightColorAndYPos(west, 2.94f, Color.red);

        NextLightChange = Time.time + LightChangeInterval;
        NorthSouthGreen = true;
        YellowLights = false;
    }

    public void Update() {
        float curTime = Time.time;
        if (curTime > NextLightChange && !YellowLights) {
            SetYellowLights();
        } else if (curTime > NextLightChange + YellowLightDuration) {
            ChangeLights();
        }
    }

    private void SetLightColorAndYPos(Light2D lt, float y, Color c) {
        lt.color = c;
        Vector3 pos = lt.transform.localPosition;
        pos.y = y;
        lt.transform.localPosition = pos;
    }

    private void ChangeLights() {
        if (NorthSouthGreen) {
            SetLightColorAndYPos(north, 2.97f, Color.red);
            SetLightColorAndYPos(south, 3.0f, Color.red);
            SetLightColorAndYPos(east, 2.87f, Color.green);
            SetLightColorAndYPos(west, 2.56f, Color.green);

            NorthSouthGreen = false;
        } else {
            SetLightColorAndYPos(north, 2.597f, Color.green);
            SetLightColorAndYPos(south, 2.56f, Color.green);
            SetLightColorAndYPos(east, 3.25f, Color.red);
            SetLightColorAndYPos(west, 2.94f, Color.red);

            NorthSouthGreen = true;
        }
        NextLightChange = Time.time + LightChangeInterval;
        YellowLights = false;
    }

    private void SetYellowLights() {
        if (NorthSouthGreen) {
            SetLightColorAndYPos(north, 2.725f, Color.yellow);
            SetLightColorAndYPos(south, 2.85f, Color.yellow);
        } else {
            SetLightColorAndYPos(east, 3.06f, Color.yellow);
            SetLightColorAndYPos(west, 2.75f, Color.yellow);
        }
        YellowLights = true;
    }
}
