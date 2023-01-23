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
    // r: 3.5 y: 2.85 g: 2.6
    public Light2D south;
    // r: 2.94 y: 2.75 g: 2.56
    public Light2D west;

    public void Start() {
        SetLightColorAndYPos(north, 2.597f, Color.green);

        SetLightColorAndYPos(south, 2.56f, Color.green);

        SetLightColorAndYPos(east, 3.25f, Color.red);

        SetLightColorAndYPos(west, 2.94f, Color.red);
    }

    public void Update() {
        
    }

    private void SetLightColorAndYPos(Light2D lt, float y, Color c) {
        lt.color = c;
        Vector3 pos = lt.transform.localPosition;
        pos.y = y;
        lt.transform.localPosition = pos;
    }
}
