using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderWatcher : MonoBehaviour
{

    private TutorialLevelManager manager;

    void Start() {
        manager = GameObject.FindObjectOfType<TutorialLevelManager>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        //Debug.Log("A collision occurred with relative velocity " + col.relativeVelocity.magnitude);
    }

    void OnTriggerEnter2D(Collider2D other) {
        manager.enteredTrigger(other.gameObject);
    }
}
