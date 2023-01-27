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
    public float temperatureDecay = 1.5f;
    public Slider TemperatureBar;

    private GameObject[] pickupLocations;
    private GameObject[] dropoffLocations;
    private GameObject[] currentDelivery;
    private bool hasFood;
    private float foodTemp;
    private string goal;
    private float score;

    void Start()
    {
        currentDelivery = new GameObject[2];
        score = 0;

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

    void Update()
    {
        foodTemp -= temperatureDecay * Time.deltaTime;
        TemperatureBar.value = foodTemp / 100f;
        displayScore.GetComponent<TMP_Text>().text = "$ " + score;
    }

    public void enteredTrigger(GameObject triggerObj)
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
        currentDelivery[0] = pickupLocations[UnityEngine.Random.Range(0, pickupLocations.Length)];
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[0].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;

        currentDelivery[1] = dropoffLocations[UnityEngine.Random.Range(0, dropoffLocations.Length)];
        hasFood = false;
        foodTemp = 100f;
        Debug.Log("The current delivery route is from " + currentDelivery[0].name + " to " + currentDelivery[1].name);
    }

    void getFood() {
        Debug.Log("got the food!");
        hasFood = true;
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[1].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;
        FindObjectOfType<AudioManager>().Play("Pickup");

    }

    void deliveryCompleted() {
        Debug.Log("delivery completed!");
        hasFood = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        FindObjectOfType<AudioManager>().Play("Dropoff");
        updateScore();
        getNewDeliveryRoute();
    }

    // Calculate score based on the time remaining.
    // Can be modified later for a better score function.
    void updateScore() {
        score +=  MathF.Truncate(TemperatureBar.value * 100);
    }
}
