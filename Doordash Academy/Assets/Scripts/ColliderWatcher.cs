using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderWatcher : MonoBehaviour
{

    private LevelManager manager;

    void Start() {
        manager = GameObject.FindObjectOfType<LevelManager>();
    }

    void OnCollisionEnter2D(Collision2D col) {
        //Debug.Log("A collision occurred with relative velocity " + col.relativeVelocity.magnitude);
    }

    void OnTriggerEnter2D(Collider2D other) {
        manager.enteredTrigger(other.gameObject);
    }
}
