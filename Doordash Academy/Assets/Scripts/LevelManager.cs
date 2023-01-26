using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float temperatureDecay = 0.5f;

    private GameObject[] pickupLocations;
    private GameObject[] dropoffLocations;
    private GameObject[] currentDelivery;
    private bool hasFood;
    private float foodTemp;

    void Start()
    {
        currentDelivery = new GameObject[2];

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
        currentDelivery[0] = pickupLocations[Random.Range(0, pickupLocations.Length)];
        currentDelivery[1] = dropoffLocations[Random.Range(0, dropoffLocations.Length)];
        hasFood = false;
        foodTemp = 100f;
        Debug.Log("The current delivery route is from " + currentDelivery[0].name + " to " + currentDelivery[1].name);
    }

    void getFood() {
        Debug.Log("got the food!");
        hasFood = true;
    }

    void deliveryCompleted() {
        Debug.Log("delivery completed!");
        hasFood = false;
        getNewDeliveryRoute();
    }

}
