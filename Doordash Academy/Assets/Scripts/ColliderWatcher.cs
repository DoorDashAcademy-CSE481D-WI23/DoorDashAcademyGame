using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderWatcher : MonoBehaviour
{

    private LevelManager manager;
    private Slider healthBar;
    private float totalHealth = 25f;
    private float carDamageMultiplier = 50f;
    private float regenerationRate = 0.1f;  // what ratio of health should go up in one second
    private float secondsUntilRegenerate = 0f;
    private float regenerationPauseOnCollision = 1f;  // how long regeneration should pause when in a collision (seconds)
    private float secondsUntilInvincibilityEnds = 0f;
    private float invincibilityTime = 2f;  // how long you should be invincible after dying (seconds)

    void Start() {
        manager = GameObject.FindObjectOfType<LevelManager>();
        healthBar = GameObject.FindGameObjectWithTag("health-bar").GetComponent<Slider>();
    }

    void Update() {
        if (secondsUntilRegenerate < 0) {
            healthBar.value += regenerationRate * Time.deltaTime;
        } else {
            secondsUntilRegenerate -= Time.deltaTime;
        }
        if (secondsUntilInvincibilityEnds > 0) {
            secondsUntilInvincibilityEnds -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (secondsUntilInvincibilityEnds > 0) {
            return;
        }
        float damageTaken = col.relativeVelocity.magnitude / totalHealth;
        VehicleBehavior car = col.gameObject.GetComponent<VehicleBehavior>();
        if (car != null) {  // we may want to take more damage from cars
            damageTaken += car.velocity * carDamageMultiplier / totalHealth;
        }
        if (damageTaken > 0.1f) {
            healthBar.value -= damageTaken;
            if (healthBar.value < 0.01f) {
                OnDeath();
            }
            secondsUntilRegenerate = regenerationPauseOnCollision;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        manager.SendMessage("enteredTrigger", other.gameObject);
    }

    void OnDeath() {
        Debug.Log("you died :(");
        healthBar.value = 1f;
        secondsUntilInvincibilityEnds = invincibilityTime;
    }
}
