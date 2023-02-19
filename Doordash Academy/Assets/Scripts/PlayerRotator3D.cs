using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator3D : MonoBehaviour
{

    private Transform t, parentTransform;
    public Animator anim;
    private CarController carcontroller;

    void Start()
    {
        t = gameObject.GetComponent<Transform>();
        parentTransform = t.parent;
        carcontroller = parentTransform.gameObject.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        float r = parentTransform.localEulerAngles.z % 360f;
        rotateGivenParentRotation(r);
        anim.speed = 3 * carcontroller.GetVelocity().magnitude / carcontroller.maxSpeed;
    }

    private void rotateGivenParentRotation(float r)
    {
        t.eulerAngles = new Vector3(Mathf.Cos((r * Mathf.PI)/180) * -45, -r, Mathf.Sin((r * Mathf.PI)/180) * 45);
    }
}
