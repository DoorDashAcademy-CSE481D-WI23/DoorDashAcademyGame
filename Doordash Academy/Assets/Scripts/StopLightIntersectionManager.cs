using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StopLightIntersectionManager : MonoBehaviour
{
    // r: 2.97 y: 2.725 g: 2.597
    private Light2D north;
    // r: 3.25 y: 3.06 g: 2.87
    private Light2D east;
    // r: 3.0 y: 2.85 g: 2.6
    private Light2D south;
    // r: 2.94 y: 2.75 g: 2.56
    private Light2D west;

    private BoxCollider2D NorthCollider;
    private BoxCollider2D EastCollider;
    private BoxCollider2D SouthCollider;
    private BoxCollider2D WestCollider;

    public float LightChangeInterval = 8f;
    public float YellowLightDuration = 3f;
    private float NextLightChange;
    private bool NorthSouthGreen;
    private bool YellowLights;

    public void Start() {
        north = transform.Find("StopLightNorth").Find("Light 2D N").GetComponent<Light2D>();
        NorthCollider = transform.Find("StopLightNorth").Find("NorthCollider").GetComponent<BoxCollider2D>();
        east = transform.Find("StopLightEast").Find("Light 2D E").GetComponent<Light2D>();
        EastCollider = transform.Find("StopLightEast").Find("EastCollider").GetComponent<BoxCollider2D>();
        south = transform.Find("StopLightSouth").Find("Light 2D S").GetComponent<Light2D>();
        SouthCollider = transform.Find("StopLightSouth").Find("SouthCollider").GetComponent<BoxCollider2D>();
        west = transform.Find("StopLightWest").Find("Light 2D W").GetComponent<Light2D>();
        WestCollider = transform.Find("StopLightWest").Find("WestCollider").GetComponent<BoxCollider2D>();


        SetLightColorAndYPos(north, 2.597f, Color.green);
        NorthCollider.enabled = false;

        SetLightColorAndYPos(south, 2.56f, Color.green);
        SouthCollider.enabled = false;

        SetLightColorAndYPos(east, 3.25f, Color.red);
        EastCollider.enabled = true;

        SetLightColorAndYPos(west, 2.94f, Color.red);
        WestCollider.enabled = true;

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
            NorthCollider.enabled = true;
            SetLightColorAndYPos(south, 3.0f, Color.red);
            SouthCollider.enabled = true;
            SetLightColorAndYPos(east, 2.87f, Color.green);
            EastCollider.enabled = false;
            SetLightColorAndYPos(west, 2.56f, Color.green);
            WestCollider.enabled = false;

            NorthSouthGreen = false;
        } else {
            SetLightColorAndYPos(north, 2.597f, Color.green);
            NorthCollider.enabled = false;
            SetLightColorAndYPos(south, 2.56f, Color.green);
            SouthCollider.enabled = false;
            SetLightColorAndYPos(east, 3.25f, Color.red);
            EastCollider.enabled = true;
            SetLightColorAndYPos(west, 2.94f, Color.red);
            WestCollider.enabled = true;

            NorthSouthGreen = true;
        }
        NextLightChange = Time.time + LightChangeInterval;
        YellowLights = false;
    }

    private void SetYellowLights() {
        if (NorthSouthGreen) {
            SetLightColorAndYPos(north, 2.725f, Color.yellow);
            NorthCollider.enabled = true;
            SetLightColorAndYPos(south, 2.85f, Color.yellow);
            SouthCollider.enabled = true;
        } else {
            SetLightColorAndYPos(east, 3.06f, Color.yellow);
            EastCollider.enabled = true;
            SetLightColorAndYPos(west, 2.75f, Color.yellow);
            WestCollider.enabled = true;
        }
        YellowLights = true;
    }
}
