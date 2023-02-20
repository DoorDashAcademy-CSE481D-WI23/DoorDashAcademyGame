using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColliderWatcher : MonoBehaviour
{
    private LevelManager manager;
    private Slider healthBar;

    private System.DateTime time0;

    private float totalHealth = 50f;
    private float carDamageMultiplier = 25f;
    private float regenerationRate = 0.1f;  // what ratio of health should go up in one second
    private float secondsUntilRegenerate = 0f;
    private float regenerationPauseOnCollision = 1f;  // how long regeneration should pause when in a collision (seconds)
    private float secondsUntilInvincibilityEnds = 0f;
    private float invincibilityTime = 2f;  // how long you should be invincible after dying (seconds)
    private float damageFinancialPenalty = 25.0f;


    void Start() {
        manager = GameObject.FindObjectOfType<LevelManager>();
        healthBar = GameObject.FindGameObjectWithTag("health-bar").GetComponent<Slider>();
        time0 = System.DateTime.UtcNow;
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
            // TODO: play take damage sound
            healthBar.value -= damageTaken;
            if (healthBar.value < 0.01f) {
                OnDeath();
            }
            secondsUntilRegenerate = regenerationPauseOnCollision;

            // analytics
            AnalyticsManager.LogCollision(SceneManager.GetActiveScene().name,
                                          transform.position.x,
                                          transform.position.y,
                                          damageTaken,
                                          (System.DateTime.UtcNow - time0).Milliseconds);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        manager.SendMessage("enteredTrigger", other.gameObject);
    }

    void OnDeath() {
        manager.AddScore(-damageFinancialPenalty);
        // TODO: play sad sound
        healthBar.value = 1f;
        secondsUntilInvincibilityEnds = invincibilityTime;
        AnalyticsManager.LogDeath(SceneManager.GetActiveScene().name,
                                  (System.DateTime.UtcNow - time0).Milliseconds);
    }
}
