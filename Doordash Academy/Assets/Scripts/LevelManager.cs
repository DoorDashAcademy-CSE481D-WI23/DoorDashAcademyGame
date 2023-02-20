using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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
    protected TMP_Text moneyChangeText;
    protected Vector3 moneyChangeTextStartingPosition = Vector3.zero;

    protected int deliveryNumber;

    protected void Start()
    {
        currentDelivery = new GameObject[2];
        score = PlayerPrefs.GetInt("money", 0);
        deliveryNumber = 0;
        moneyChangeText = displayScore.transform.parent.GetChild(3).GetComponent<TMP_Text>();  // sorry, i know this is bad style

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
        AnalyticsManager.LogGetDeliveryRoute(currentDelivery[0].name, currentDelivery[1].name, SceneManager.GetActiveScene().name);
    }

    protected void getFood() {
        hasFood = true;
        currentDelivery[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

        goal = currentDelivery[1].name;
        displayText.GetComponent<TMP_Text>().text = "Go To: " + goal;
        FindObjectOfType<AudioManager>().Play("Pickup");
        AnalyticsManager.LogGetFood(currentDelivery[0].name, currentDelivery[1].name, foodTemp, SceneManager.GetActiveScene().name);
    }

    protected virtual void deliveryCompleted() {
        hasFood = false;
        currentDelivery[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        FindObjectOfType<AudioManager>().Play("Dropoff");
        float scoreEarned = AddScore(TemperatureBar.value * 100);
        getNewDeliveryRoute();
        if (deliveryNumber == 2) {
            completionText.GetComponent<TMP_Text>().text = "You completed " + (deliveryNumber - 1) + " delivery and earned $" + score;
        } else {
            completionText.GetComponent<TMP_Text>().text = "You completed " + (deliveryNumber - 1) + " deliveries and earned $" + score;
        }
        AnalyticsManager.LogDeliveryComplete(currentDelivery[0].name, currentDelivery[1].name, SceneManager.GetActiveScene().name, scoreEarned, foodTemp);
    }

    public float AddScore(float scoreEarned) {
        score +=  MathF.Truncate(scoreEarned);
        StartCoroutine(showMoneyChange(scoreEarned));
        PlayerPrefs.SetInt("money", (int)score);
        PlayerPrefs.Save();
        return scoreEarned;
    }

    public IEnumerator showMoneyChange(float moneyChange) {
        AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);
        Transform t = moneyChangeText.transform;
        if (moneyChangeTextStartingPosition.Equals(Vector3.zero)) {
            moneyChangeTextStartingPosition = moneyChangeText.gameObject.GetComponent<Transform>().position;
        }
        float r,g,b;
        string text;
        if (moneyChange >= 0f) {
            (r,g,b) = (0f,1f,0f);
            text = "+ $" + ((int)moneyChange).ToString();
        } else {
            (r,g,b) = (1f,0f,0f);
            text = "- $" + ((int)moneyChange).ToString();
        }
        moneyChangeText.color = new Color(r,g,b,1);
        moneyChangeText.text = text;

        float time = Time.deltaTime;
        float duration = 2.5f;
        moneyChangeText.gameObject.SetActive(true);
        while (time < duration)
        {
            t.position = moneyChangeTextStartingPosition + curve.Evaluate(time/duration) * (new Vector3(0,100,0));
            moneyChangeText.color = new Color(r,g,b, 1 - curve.Evaluate(time/duration));
            time += Time.deltaTime;
            yield return 0;
        }
        moneyChangeText.gameObject.SetActive(false);
        t.position = moneyChangeTextStartingPosition;
    }
}
