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

    public TMP_Text tutorialText;
    private int tutorialIndex;
    private string[] tutorialTextList;

    private GameObject[] pickupLocations;
    private GameObject[] dropoffLocations;
    private GameObject[] currentDelivery;
    private bool hasFood;
    private float foodTemp;
    private string goal;
    private float score;

    private int deliveryNumber;

    void Start()
    {
        currentDelivery = new GameObject[2];
        score = 0;
        deliveryNumber = 0;
        tutorialIndex = 0;

        tutorialTextList = new string[] {"Welcome to the DoorDash Academy Tutorial! Use WASD or Arrow Keys to drive around the map. Pickup locations for food will be marked by a blue dot on the minimap. Drive to the first pickup location!",
        "Great! Now that you have picked up the food, the delivery location will be marked by a new blue dot on the minimap. Reach the destination before the temperature bar depletes for a higher payment!",
        "Complete 2 more deliveries to finish the training and don't forget to watch out for traffic! Getting hit by a car hurts...",
        "The interface at the bottom of the screen shows the money you have earned, the temperature bar, and the name of your next destination!",
        "1 more delivery to go!",
        "Press escape to pause the game at any time",
        "You finished the tutorial! Open the pause menu by pressing escape and navigate to the main menu!"};

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
        tutorialText.GetComponent<TMP_Text>().text = tutorialTextList[tutorialIndex];
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
        //currentDelivery[0] = pickupLocations[UnityEngine.Random.Range(0, pickupLocations.Length)];
        currentDelivery[0] = pickupLocations[deliveryNumber % 3];
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[0].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;

        //currentDelivery[1] = dropoffLocations[UnityEngine.Random.Range(0, dropoffLocations.Length)];
        currentDelivery[1] = dropoffLocations[deliveryNumber % 3];
        hasFood = false;
        foodTemp = 100f;

        deliveryNumber++;
        Debug.Log("The current delivery route is from " + currentDelivery[0].name + " to " + currentDelivery[1].name);
    }

    void getFood() {
        updateTutorialIndex();
        Debug.Log("got the food!");
        hasFood = true;
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[1].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;
        FindObjectOfType<AudioManager>().Play("Pickup");

    }

    void deliveryCompleted() {
        updateTutorialIndex();
        Debug.Log("delivery completed!");
        hasFood = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        FindObjectOfType<AudioManager>().Play("Dropoff");
        updateScore();
        if (deliveryNumber >= 3) { // tutorial is done
            displayText.GetComponent<TMP_Text>().text = "Tutorial Complete";
            Time.timeScale = 0.0f;
        } else {
            getNewDeliveryRoute();
        }
    }

    // Calculate score based on the time remaining.
    // Can be modified later for a better score function.
    void updateScore() {
        score +=  MathF.Truncate(TemperatureBar.value * 100);
    }

    void updateTutorialIndex() {
        tutorialIndex++;
        if (tutorialIndex > 6) {
            tutorialIndex = 6;
        }
    }
}
