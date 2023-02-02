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
    public BoxCollider2D spawnArea = null;

    private float velocity;
    private BoxCollider2D hitbox;
    private LayerMask layerMask;
    private float lastDetectionRange = 0f;
    private float ignoreIntersectionColliders = 0f;

    // Start is called before the first frame update
    void Start()
    {
        velocity = InitialVelocity;
        hitbox = GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask("Actor", "Ignore Collisions");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > ignoreIntersectionColliders) {
            layerMask = LayerMask.GetMask("Actor", "Ignore Collisions");
        }

        RaycastHit2D hit;

        // Cast rays in direction of movement to check for collisions.
        if (DirectionVector == Vector3.left || DirectionVector == Vector3.right) {
            hit = Physics2D.Raycast(hitbox.transform.position + new Vector3(0, hitbox.size.y / 2, 0), DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask);
        } else if (DirectionVector == Vector3.down) {
            hit = Physics2D.Raycast(hitbox.transform.position, DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask);
        } else if (DirectionVector == Vector3.up) {
            hit = Physics2D.Raycast(hitbox.transform.position + (DirectionVector * hitbox.size.x), DirectionVector, Mathf.Max(velocity * DetectionRangeMultiplier, MinimumDetectionRange), layerMask);
        } else {
            throw new System.Exception("Vehicle has invalid direction!");
        }

        if (hit.collider != null) {
            // Move vehicle to beginning of road if needed.
            if (hit.collider.name == "DespawnArea") {
                Vector3 pos = transform.position;
                if (DirectionVector == Vector3.left || DirectionVector == Vector3.right) {
                    pos.x = spawnArea.transform.position.x;
                } else  {
                    pos.y = spawnArea.transform.position.y;
                }
                transform.position = pos;
            } else {
                if (hit.distance - lastDetectionRange < -1) {
                    layerMask = LayerMask.GetMask("Actor");
                    ignoreIntersectionColliders = Time.time + 0.5f;  // Ignore intersection lights for one second.
                }
                ApplyBrake();
                lastDetectionRange = hit.distance;
            }
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
