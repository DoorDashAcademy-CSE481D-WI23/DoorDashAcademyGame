using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBehavior : MonoBehaviour
{
    public float InitialVelocity = 0f;
    public float MaxSpeed = 7f;
    public float AccelerationPower = 1f;
    public float BrakingPower = 2f;
    public Vector3 DirectionVector = Vector3.left;
    public float DetectionRangeMultiplier = 30f;
    public float MinimumDetectionRange = 2f;
    public float hitboxOffsetY = 0.08f;
    public float hitboxOffsetX = 0f;

    private float velocity;
    private BoxCollider2D hitbox;
    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        velocity = InitialVelocity;
        hitbox = GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask("Player", "Blocking", "Actor", "Ignore Collisions");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hit = false;

        // Cast multiple rays in direction of movement to check for collisions. 
        if (DirectionVector == Vector3.left || DirectionVector == Vector3.right) {
            hit = (Physics2D.Raycast(hitbox.transform.position, DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask).collider != null ||
                   Physics2D.Raycast(hitbox.transform.position + new Vector3(0, hitbox.size.y + hitboxOffsetY, 0), DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask).collider != null);
        } else if (DirectionVector == Vector3.up || DirectionVector == Vector3.down) {
            hit = (Physics2D.Raycast(hitbox.transform.position, DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask).collider != null ||
                   Physics2D.Raycast(hitbox.transform.position + new Vector3(hitbox.size.x + hitboxOffsetX, 0, 0), DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask).collider != null);
        }

        if (hit) {
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
