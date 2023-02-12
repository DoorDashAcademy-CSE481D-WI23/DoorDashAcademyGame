using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public TMP_Text displayText;
    public TMP_Text displayScore;
    public TMP_Text completionText;
    public float temperatureDecay = 1.5f;
    public Slider TemperatureBar;


    protected GameObject[] pickupLocations;
    protected GameObject[] dropoffLocations;
    protected GameObject[] currentDelivery;
    protected bool hasFood;
    protected float foodTemp;
    protected string goal;
    protected float score;

    protected int deliveryNumber;

    protected void Start()
    {
        currentDelivery = new GameObject[2];
        score = PlayerPrefs.GetInt("money", 0);
        deliveryNumber = 0;

        GameObject PickupLocationsParent = GameObject.Find("Pickup Locations");
        if (PickupLocationsParent == null) Debug.Log("Error: the scene should have a \"Pickup Locations\" object");
        int numPickupLocations = PickupLocationsParent.transform.childCount;
        pickupLocations = new GameObject[numPickupLocations];
        for (int i = 0; i < numPickupLocations; i++){
            pickupLocations[i] = PickupLocationsParent.transform.GetChild(i).gameObject;
        }

        GameObject DropoffLocationsParent = GameObject.Find("Dropoff Locations");
        if (DropoffLocationsParent == null) Debug.Log("Error: the scene should have a \"Dropoff Locations\" object");
        int numDropoffLocations = DropoffLocationsParent.transform.childCount;
        dropoffLocations = new GameObject[numDropoffLocations];
        for (int i = 0; i < numDropoffLocations; i++){
            dropoffLocations[i] = DropoffLocationsParent.transform.GetChild(i).gameObject;
        }

        getNewDeliveryRoute();
    }

    virtual protected void Update()
    {
        foodTemp -= temperatureDecay * Time.deltaTime;
        TemperatureBar.value = foodTemp / 100f;
        displayScore.GetComponent<TMP_Text>().text = "$ " + score;
    }

    public virtual void enteredTrigger(GameObject triggerObj)
    {
        GameObject currentObjective = currentDelivery[hasFood ? 1 : 0];
        if (triggerObj == currentObjective) {
            if (hasFood) {
                deliveryCompleted();
            } else {
                getFood();
            }
        }
    }

    void getNewDeliveryRoute() {
        int index = UnityEngine.Random.Range(0, pickupLocations.Length);
        currentDelivery[0] = pickupLocations[index];
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[0].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;

        currentDelivery[1] = dropoffLocations[UnityEngine.Random.Range(0, dropoffLocations.Length)];
        hasFood = false;
        foodTemp = 100f;

        deliveryNumber++;
        Debug.Log("Delivery Number: " + deliveryNumber);
        AnalyticsManager.LogGetDeliveryRoute(currentDelivery[0].name, currentDelivery[1].name);
    }

    protected void getFood() {
        hasFood = true;
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[1].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;
        FindObjectOfType<AudioManager>().Play("Pickup");
        AnalyticsManager.LogGetFood();
    }

    protected virtual void deliveryCompleted() {
        hasFood = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        FindObjectOfType<AudioManager>().Play("Dropoff");
        float scoreEarned = updateScore();
        getNewDeliveryRoute();
        if (deliveryNumber == 2) {
            completionText.GetComponent<TMP_Text>().text = "You completed " + (deliveryNumber - 1) + " delivery and earned $" + score;
        } else {
            completionText.GetComponent<TMP_Text>().text = "You completed " + (deliveryNumber - 1) + " deliveries and earned $" + score;
        }
        AnalyticsManager.LogDeliveryComplete(scoreEarned);
    }

    // Calculate score based on the time remaining.
    // Can be modified later for a better score function.
    float updateScore() {
        float scoreEarned = TemperatureBar.value * 100;
        score +=  MathF.Truncate(scoreEarned);
        PlayerPrefs.SetInt("money", (int)score);
        PlayerPrefs.Save();
        return scoreEarned;
    }
}
