using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarController : MonoBehaviour
{
    // State accessible from editor
    [Header("Car Settings")]
    public float accelerationFactor = 1.0f;
    public float maxTurnFactor = 1.0f;
    public float driftFactor = 1.0f;
    public float frictionFactor = 1.0f;
    public float maxSpeed = 1.0f;
    public float adjustedTurnFactor;

    // Constants
    const float TurnSpeedFactor = 8.0f;
    const float DragTimeFactor = 3.0f;
    const float ReverseFactor = 0.5f;

    // Local state
    float accelerationInput = 0.0f;
    float steeringInput = 0.0f;
    float rotationAngle = 0.0f;
    float velocityUp = 0.0f;

    // Components
    Rigidbody2D carRigidbody2D;

    // Triggers when script instance is loaded
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Triggers before first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(3,8);
    }

    // On every frame update
    void Update()
    {

    }

    // Frame-independent update
    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplyOrthogonalDrag();
        ApplySteering();
    }

    void ApplyEngineForce()
    {

        // Compute "forward" velocity
        velocityUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        // Check if user wants to go beyond max speed (forward).
        if (velocityUp > maxSpeed && accelerationInput > 0)
            return;

        // Check if user wants to go beyond max speed (backward).
        if (velocityUp < -maxSpeed * ReverseFactor && accelerationInput < 0)
            return;

        // Check if user wants to go beyond max speed (while turning)
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationFactor > 0)
            return;

        // Apply forward (or backward) drag when there is no user acceleration
        if (accelerationInput != 1 || accelerationInput != -1)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag,
                                             frictionFactor,
                                             Time.fixedDeltaTime * DragTimeFactor);
        else
            carRigidbody2D.drag = 0;

        // Create a force to push the vehicle forward
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        // Rate of rotation is proportional to speed
        float speedFactor = carRigidbody2D.velocity.magnitude / TurnSpeedFactor;
        speedFactor = Mathf.Clamp01(speedFactor);

        // Controls should flip when reversing
        steeringInput = (velocityUp < 0.0f) ? -steeringInput : steeringInput;
        // TODO: How? Need to correctly compute "forward" velocity

        adjustedTurnFactor = maxTurnFactor - ((maxTurnFactor - 1)/maxSpeed)*carRigidbody2D.velocity.magnitude;

        // Update the rotation angle based on input
        rotationAngle -= (steeringInput * adjustedTurnFactor * speedFactor);
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void ApplyOrthogonalDrag()
    {
        // Applies drag to lateral movement to prevent "drifting"
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    // Public API

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetSpeed()
    {
        return carRigidbody2D.velocity.magnitude;
    }
}
