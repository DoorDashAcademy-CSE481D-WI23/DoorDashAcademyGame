using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    // public
    public CarController controller;
    public GameObject cameraTarget;
    public float zoomTimeFactor = 1.0f;
    public float leadFactor = 1.0f;
    public float minOrthographicSize = 7.5f;
    public float maxOrthographicSize = 15.0f;

    // private
    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize = minOrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = controller.GetVelocity();
        float deltaTime = Mathf.Clamp(zoomTimeFactor * Time.deltaTime, 0.0f, 1.0f);

        // Update the camera zoom ("orthographic size")
        float speed = velocity.magnitude;
        float factor = speed / controller.maxSpeed;
        float rawSize = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, factor);
        float tickSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, rawSize, deltaTime); 
        virtualCamera.m_Lens.OrthographicSize = tickSize;

        // Update the target offset
        Vector3 diff = new Vector3(velocity.x, velocity.y, 0);
        Vector3 position = controller.transform.position + leadFactor * diff;
        cameraTarget.transform.position = Vector3.Lerp(cameraTarget.transform.position, position, deltaTime);
    }
}
