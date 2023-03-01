using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    public CarController controller;
    public GameObject cameraTarget;
    
    [Header("Camera Settings")]
    // Controls how fast the camera tries to get to the appropriate zoom level
    public float zoomTimeFactor = 1.0f;
    // Controls how much the camera is allowed to lead relative to player velocity
    public float leadFactor = 1.0f;
    // Ratio of controller max speed that is considered max speed for the camera.
    // Perhaps the largest zoom should occur before the car actually hits max speed.
    public float maxSpeedFactor = 1.0f;
    // Lowest zoom level (speed = 0)
    public float minOrthographicSize = 7.5f;
    // Highest zoom level (speed = max * maxSpeedfactor)
    public float maxOrthographicSize = 15.0f;

    // private
    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize = maxOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = controller.GetVelocity();
        float deltaTime = Mathf.Clamp01(zoomTimeFactor * Time.deltaTime);

        // Update the camera zoom ("orthographic size")
        float speed = velocity.magnitude;
        float factor = Mathf.Clamp01(speed / (controller.maxSpeed * maxSpeedFactor));
        float rawSize = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, factor);
        float tickSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, rawSize, deltaTime); 
        //virtualCamera.m_Lens.OrthographicSize = tickSize;

        // Update the target offset
        Vector3 diff = new Vector3(velocity.x, velocity.y, 0);
        Vector3 position = controller.transform.position + leadFactor * diff;
        cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, position, deltaTime);
    }
}
