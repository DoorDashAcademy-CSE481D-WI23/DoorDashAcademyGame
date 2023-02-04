using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    // public
    public CarController controller;
    public float smoothingFactor = 1.0f;
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
        float currentSize = virtualCamera.m_Lens.OrthographicSize;
        float factor = controller.GetSpeed() / controller.maxSpeed;
        float rawSize = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, factor);
        float deltaTime = Mathf.Clamp(smoothingFactor * Time.deltaTime, 0.0f, 1.0f);
        float tickSize = Mathf.Lerp(currentSize, rawSize, deltaTime); 
        virtualCamera.m_Lens.OrthographicSize = tickSize;
    }
}
