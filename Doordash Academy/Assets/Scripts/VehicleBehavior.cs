using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBehavior : MonoBehaviour
{
    public float InitialVelocity = 0f;
    public float MaxSpeed = 7f;
    public float AccelerationPower = 1f;
    public float BrakingPower = 2f;
    public Vector3 DirectionVector = new Vector3(-1, 0, 0);
    public float DetectionRangeMultiplier = 30f;
    public float MinimumDetectionRange = 2f;

    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = InitialVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(velocity);
         // TODO: Currently the ray is cast from the pivot point (bottom y position) of the sprite.
         // Another ray needs to be cast from the other side of the sprite to better detect player.
         if (Physics2D.Raycast(transform.position, DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), LayerMask.GetMask("Player", "Blocking", "Actor"))) {
            ApplyBrake();
         } else {
            ApplyGas();
         }
    }

    private void ApplyGas() {
        velocity = Mathf.Clamp(velocity + (AccelerationPower * Time.deltaTime), 0, MaxSpeed);
        transform.Translate(DirectionVector * velocity);
    }

    private void ApplyBrake() {
        velocity = Mathf.Clamp(velocity - (BrakingPower * Time.deltaTime), 0, MaxSpeed);
        transform.Translate(DirectionVector * velocity);
    }
}
