using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to a child object of the player that will show the sprite
public class PlayerRotator : MonoBehaviour
{
    // Sprites should represent the object turning uniformly from straight up
    // to straight down, to the right
    public int numAngles = 5;
    public Animator anim;

    private Transform parentTransform;
    private Transform t;
    private SpriteRenderer spriteRenderer;
    private CarController carcontroller;

    // Start is called before the first frame update
    void Start()
    {
        t = gameObject.GetComponent<Transform>();
        parentTransform = t.parent;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        carcontroller = parentTransform.gameObject.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        float r = parentTransform.localEulerAngles.z % 360f;
        setSpriteGivenRotation(r);
        anim.speed = carcontroller.GetVelocity().magnitude / carcontroller.maxSpeed;
    }

    private void setSpriteGivenRotation(float r)
    {
        bool shouldFlipY = false;
        if (r <= 180f) {
            shouldFlipY = true;
            r = 360f - r;
        }
        int spriteNum = Mathf.RoundToInt((r - 360f) / -180f * (numAngles - 1));
        anim.SetInteger("spriteNum", spriteNum);
        t.localEulerAngles = new Vector3(0f, shouldFlipY ? 180f : 0f, -360f + (1.0f * spriteNum / (numAngles - 1) * 180f));
    }
}
